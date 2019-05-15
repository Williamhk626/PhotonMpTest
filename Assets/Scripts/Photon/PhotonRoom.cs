using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room info
    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    // Player info

    private Player[] photonPlayers;
    public int playersInRoom;
    public int myNumbersInRoom;

    public int playerInGame;

    // Delayed Start

    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    void Awake()
    {
        // Setup singleton
        if (PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        // subscribe to functions
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;

    }

    // Start is called before the first frame update
    void Start()
    {
        // set private variables
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;

    }

    // Update is called once per frame
    void Update()
    {
        // for delayed start & count down to start
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
                if (!isGameLoaded)
                {
                    if (readyToStart)
                    {
                        atMaxPlayers -= Time.deltaTime;
                        lessThanMaxPlayers = atMaxPlayers;
                        timeToStart = atMaxPlayers;
                    }
                    else if (readyToCount)
                    {
                        lessThanMaxPlayers -= Time.deltaTime;
                        timeToStart = lessThanMaxPlayers;
                    }
                    Debug.Log("Time to start" + timeToStart);
                    if (timeToStart <= 0)
                    {
                        StartGame();
                    }
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        // sets player data when joining the room
        base.OnJoinedRoom();
        Debug.Log("Joined room succesfully.");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumbersInRoom = playersInRoom;
        

        // for delayed start only
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            Debug.Log($"Players in room out of max players possible ({playersInRoom}:{MultiplayerSetting.multiplayerSetting.maxPlayers})");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            Debug.Log($"Displayes players in room out of max players possible ({playersInRoom}:{MultiplayerSetting.multiplayerSetting.maxPlayers})");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
    void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.multiplayerScene);
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // called when multiplayer scene is loaded
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSetting.multiplayerSetting.multiplayerScene)
        {
            isGameLoaded = true;
            
            if (MultiplayerSetting.multiplayerSetting.delayStart) // for delayed start
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else // for non delayed start
            {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}
