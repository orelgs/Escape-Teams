using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance = null;

    private bool isPlayerInGame = false;
    private int initialPlayerCount;
    private PlayerTeam winningTeam;

    private PhotonView PV;

    private void Awake()
    {
        if (Instance is not null)
        {
            if (Instance.PV.Owner == PhotonNetwork.LocalPlayer)
            {
                PhotonNetwork.Destroy(Instance.PV);
            }
            else
            {
                Instance.DestroySelf();
            }
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
        PV = GetComponent<PhotonView>();
        if (PV.ViewID == 0)
        {
            PV.ViewID = 999;
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        CheckPlayerLeftRoom();
    }

    private void CheckPlayerLeftRoom()
    {
        if (!isPlayerInGame) return;
        if (initialPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount) return;

        PV.ViewID = 0;
        PhotonNetwork.LoadLevel((int)SceneIndex.MainScene);
        isPlayerInGame = false;
    }

    public void OnLeaveGameButtonClicked(Component sender, object data)
    {
        PV.ViewID = 0;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene((int)SceneIndex.MainScene);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
    }

    private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == (int)SceneIndex.GameScene)
        {
            initialPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            isPlayerInGame = true;

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity); // Might be a problem here with mobile

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameSceneManager"), Vector3.zero, Quaternion.identity);
            }
        }
        else if (scene.buildIndex == (int)SceneIndex.ResultsScene)
        {
            isPlayerInGame = false;

            if (PhotonNetwork.IsMasterClient)
            {
                GameObject resultsSceneManager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ResultsSceneManager"), Vector3.zero, Quaternion.identity, data: new object[] { winningTeam.ToString() });
            }
        }
    }

    public void OnAllClientsFinished(Component sender, object data)
    {
        if (data is PlayerTeam winningTeam)
        {
            this.winningTeam = winningTeam;
        }
    }
}
