using UnityEngine;
using Photon.Pun;
using WebSocketSharp;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Globalization;
using System;
using UnityEngine.UIElements;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public static PhotonLauncher Instance;

    [SerializeField] private TMP_InputField createRoomInputField;
    [SerializeField] private TMP_InputField joinPrivateRoomInputField;
    [SerializeField] private UnityEngine.UI.Toggle createPrivateRoomToggle;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private Transform roomListContent;
    [SerializeField] private Transform playerListContentTeam1;
    private List<string> team1List = new List<string>();
    private List<string> team2List = new List<string>();
    [SerializeField] private Transform playerListContentTeam2;

    [SerializeField] private GameObject roomListItemPrefab;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private GameObject startGameButton;
    private static Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private int numOfPlayersInTeam = 10;

    private void Awake()
    {
        Instance = this;

        if (!Application.isMobilePlatform)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Start()
    {
        MenuManager.Instance.OpenMenu("Loading");
        Debug.Log("Connecting to Master");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        cachedRoomList.Clear();
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Main");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if (createRoomInputField.text.IsNullOrEmpty()) return;

        MenuManager.Instance.OpenMenu("Loading");
        PhotonNetwork.CreateRoom(createRoomInputField.text, new RoomOptions() { IsVisible = !createPrivateRoomToggle.isOn });
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        MenuManager.Instance.OpenMenu("Loading");
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }

    public void JoinRoom()
    {
        if (joinPrivateRoomInputField.text.IsNullOrEmpty()) return;

        MenuManager.Instance.OpenMenu("Loading");
        PhotonNetwork.JoinRoom(joinPrivateRoomInputField.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorMessageText.text = "Room Join Failed:\n" + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("InRoom");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerNameText.text = PhotonNetwork.LocalPlayer.NickName;


        DeleteList(playerListContentTeam1);
        DeleteList(playerListContentTeam2);

        if (!PhotonNetwork.IsMasterClient)
        {
            updateListForNotMasterPlayers();
        }
        if (team1List.Count < team2List.Count)
        {
            team1List.Add(PhotonNetwork.LocalPlayer.NickName);
            writePlayerOnList(playerListContentTeam1);
        }
        else
        {
            team2List.Add(PhotonNetwork.LocalPlayer.NickName);
            writePlayerOnList(playerListContentTeam2);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    private void updateListForNotMasterPlayers()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("team2List") ||
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("team1List"))
        {
            UpdateList(team1List, (string[])PhotonNetwork.CurrentRoom.CustomProperties["team1List"]);
            UpdateList(team2List, (string[])PhotonNetwork.CurrentRoom.CustomProperties["team2List"]);
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (ValidateProperties(propertiesThatChanged))
        {
            DeleteList(playerListContentTeam1);
            DeleteList(playerListContentTeam2);
            UpdateList(team1List, (string[])(propertiesThatChanged["team1List"]));
            UpdateList(team2List, (string[])(propertiesThatChanged["team2List"]));

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (team1List.Contains(player.NickName))
                {
                    Instantiate(playerListItemPrefab, playerListContentTeam1).GetComponent<PlayerListItem>().SetUp(player);
                }
                else if (team2List.Contains(player.NickName))
                {
                    Instantiate(playerListItemPrefab, playerListContentTeam2).GetComponent<PlayerListItem>().SetUp(player);
                }
            }
        }
    }

    private bool ValidateProperties(Hashtable propertiesThatChanged)
    {
        bool isTeam1Player, isTeam2Player;

        isTeam1Player = propertiesThatChanged.ContainsKey("team1List");
        isTeam2Player = propertiesThatChanged.ContainsKey("team2List");

        return isTeam1Player || isTeam2Player;
    }

    private void UpdateList(List<string> oldTeamList, string[] newTeamList)
    {
        oldTeamList.Clear();
        foreach (string player in newTeamList)
        {
            oldTeamList.Add(player);
        }
    }

    private void writePlayerOnList(Transform team)
    {
        Instantiate(playerListItemPrefab, team).GetComponent<PlayerListItem>().SetUp(PhotonNetwork.LocalPlayer);
        Hashtable hash = new Hashtable();
        hash.Add("team1List", team1List.ToArray());
        hash.Add("team2List", team2List.ToArray());
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    private void DeleteList(Transform playerListContentTeam)
    {
        foreach (Transform trans in playerListContentTeam)
        {
            Destroy(trans.gameObject);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorMessageText.text = "Room Creation Failed:\n" + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    public void LeaveRoom()
    {
        CleanLists();
        MenuManager.Instance.OpenMenu("Loading");
        PhotonNetwork.LeaveRoom();
    }

    public void ResetRoomsList()
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList || !room.IsVisible || !room.IsOpen)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name);
                }
            }
            else
            {
                cachedRoomList[room.Name] = room;
            }
        }

        string roomsList = "";

        foreach (RoomInfo room in roomList)
        {
            roomsList += $"{room.Name}, ";
        }

        string cachedRoomsList = "";

        foreach (string room in cachedRoomList.Keys)
        {
            cachedRoomsList += $"{room}, ";
        }

        foreach (KeyValuePair<string, RoomInfo> entry in cachedRoomList)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(entry.Value);
        }
    }

    public void OnClickTeam1()
    {
        changeGroup(playerListContentTeam2, playerListContentTeam1);
    }

    public void OnClickTeam2()
    {
        changeGroup(playerListContentTeam1, playerListContentTeam2);
    }

    private void changeGroup(Transform currentGroup, Transform goToGroup)
    {
        List<string> teamList = currentGroup == playerListContentTeam1 ? team1List : team2List;
        List<string> teamListDes = currentGroup == playerListContentTeam1 ? team2List : team1List;

        if (teamList.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            teamList.Remove(PhotonNetwork.LocalPlayer.NickName);
            teamListDes.Add(PhotonNetwork.LocalPlayer.NickName);
            writePlayerOnList(goToGroup);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContentTeam1).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    private void CleanLists()
    {
        team1List.Clear();
        team2List.Clear();
    }

    private void SplitIntoGroups()
    {
        Vector3[] team1Positions = new Vector3[numOfPlayersInTeam];
        Vector3[] team2Positions = new Vector3[numOfPlayersInTeam];

        InitializePositions(team1Positions, team2Positions);
        Hashtable hash = new Hashtable();
        InitializeHash(team1Positions, hash, "team1", team1List);
        InitializeHash(team2Positions, hash, "team2", team2List);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    private void InitializeHash(Vector3[] teamPositions, Hashtable hash, string team, List<string> teamListPlayer)
    {
        for (int i = 0; i < teamListPlayer.Count; i++)
        {
            string[] playerInfo = new string[] { team, teamPositions[i].x.ToString(), teamPositions[i].y.ToString(), teamPositions[i].z.ToString() };
            hash.Add(teamListPlayer[i], playerInfo);
        }
    }

    private void InitializePositions(Vector3[] team1Positions, Vector3[] team2Positions)
    {
        // Team 1 positions
        team1Positions[0] = new Vector3(3.55f, 0.42f, -37.17f);
        team1Positions[1] = new Vector3(8.81f, 0.42f, -37.31f);
        team1Positions[2] = new Vector3(6.33f, 0.42f, -40.7f);
        team1Positions[3] = new Vector3(3.55f, 0.42f, -39.43f);
        team1Positions[4] = new Vector3(6.33f, 0.42f, -36.0f);
        team1Positions[5] = new Vector3(8.81f, 0.42f, -39.56f);
        team1Positions[6] = new Vector3(6.33f, 0.42f, -38.61f);
        team1Positions[7] = new Vector3(8.81f, 0.42f, -34.35f);
        team1Positions[8] = new Vector3(3.55f, 0.42f, -34.68f);
        team1Positions[9] = new Vector3(6.33f, 0.42f, -33.05f);

        // Team 2 positions
        team2Positions[0] = new Vector3(-0.39f, 0.42f, -35.1f);
        team2Positions[1] = new Vector3(-2.48f, 0.42f, -36.41f);
        team2Positions[2] = new Vector3(-4.54f, 0.42f, -37.33f);
        team2Positions[3] = new Vector3(-2.37f, 0.42f, -33.59f);
        team2Positions[4] = new Vector3(-4.54f, 0.42f, -34.85f);
        team2Positions[5] = new Vector3(-4.54f, 0.42f, -31.91f);
        team2Positions[6] = new Vector3(-0.36f, 0.42f, -32.16f);
        team2Positions[7] = new Vector3(-0.39f, 0.42f, -38.41f);
        team2Positions[8] = new Vector3(-4.54f, 0.42f, -39.89f);
        team2Positions[9] = new Vector3(-2.18f, 0.42f, -39.43f);
    }

    public void StartGame()
    {
        SplitIntoGroups();
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel((int)SceneIndex.GameScene);
    }
}