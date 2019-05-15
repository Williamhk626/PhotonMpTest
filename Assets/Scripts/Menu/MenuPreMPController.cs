using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MenuPreMPController : MonoBehaviourPunCallbacks
{
    #region Fields
    [Header("Connect Panel")]
    public GameObject ConnectPanel;
    public InputField NameInputField;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public TMP_InputField RoomNameInput;
    public Slider MaxPlayersSlider;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject insideRoomPanel;

    [Header("Loading Panel")]
    public GameObject loadingPanel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;

    #endregion

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    #region Button clicks

    public void onClickConnect()
    {
        if (NameInputField.text == string.Empty)
            return;

        loadingPanel.SetActive(true);

        PhotonNetwork.LocalPlayer.NickName = NameInputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void onClickCreateRoom()
    {
        SetActivePanel(CreateRoomPanel.name);
    }

    public void onclickJoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void onClickCreateNewRoom()
    {
        string roomName = RoomNameInput.text;
        if (roomName == string.Empty)
            return;

        byte.TryParse(MaxPlayersSlider.value.ToString(), out byte maxPlayers);

        SetActivePanel(loadingPanel.name);
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };
        PhotonNetwork.CreateRoom(roomName, options);
    }
    public void onClickRoomList()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        SetActivePanel(RoomListPanel.name);
    }
    public void onClickBack()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
    #endregion

    #region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log($"{NameInputField.text} connected to master");
        SetActivePanel(SelectionPanel.name);
    }
    public override void OnJoinedRoom()
    {
        SetActivePanel(insideRoomPanel.name);
        Debug.Log("Joined Room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Trying to create a room.");
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room: " + randomRoomName, roomOps);
    }


    #endregion

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
        roomListEntries.Clear();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }

    void SetActivePanel(string panelName)
    {
        ConnectPanel.SetActive(panelName.Equals(ConnectPanel.name));
        loadingPanel.SetActive(panelName.Equals(loadingPanel.name));
        SelectionPanel.SetActive(panelName.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(panelName.Equals(CreateRoomPanel.name));
        RoomListPanel.SetActive(panelName.Equals(RoomListPanel.name));
    }
}
