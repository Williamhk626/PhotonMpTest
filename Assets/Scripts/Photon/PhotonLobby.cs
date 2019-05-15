using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public GameObject battleButton;
    public GameObject cancelButton;


    public static PhotonLobby lobby;
    void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player connected to Master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);
    }

    public void OnBattleButtonClick()
    {
        Debug.Log("Battle button was clicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed joining random room. No open rooms available.");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a room.");
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room: " + randomRoomName, roomOps);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room succesfully.");
        base.OnJoinedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel button was clicked");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
