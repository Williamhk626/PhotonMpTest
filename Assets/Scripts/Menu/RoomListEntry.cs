using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListEntry : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI PlayerNumbersText;
    public Button JoinRoomButton;

    private string roomName;

    public void Start()
    {
        JoinRoomButton.onClick.AddListener(() =>
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            PhotonNetwork.JoinRoom(roomName);
        });
    }

    public void Initialize(string name, byte players, byte maxPlayers)
    {
        roomName = name;

        RoomNameText.text = roomName;
        PlayerNumbersText.text = $"{players} / {maxPlayers}";
    }
}
