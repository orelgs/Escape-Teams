using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TaskManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameEvent onTaskCompleted;
    [SerializeField] private GameEvent onAllClientsFinished;
    [SerializeField] private GameEvent onTeam1ProgressChanged;
    [SerializeField] private GameEvent onTeam2ProgressChanged;
    [SerializeField] private int tasksRequired;
    private int team1TasksCompleted = 0;
    private int team2TasksCompleted = 0;
    private int clientsFinishedGameAmount = 0;
    private bool isRunning = true;
    private bool isMasterRunning = true;
    private PlayerTeam winningTeam;

    private void Awake()
    {
        tasksRequired = PhotonNetwork.CurrentRoom.PlayerCount >= 4 ? PhotonNetwork.CurrentRoom.PlayerCount + 1 : 5;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && isMasterRunning)
        {
            CheckForGameFinished();
        }

        if (!isRunning) return;

        if (team1TasksCompleted == tasksRequired)
        {
            SyncGameFinishedOverPhoton();
            isRunning = false;
            winningTeam = PlayerTeam.Team1;
        }
        else if (team2TasksCompleted == tasksRequired)
        {
            SyncGameFinishedOverPhoton();
            isRunning = false;
            winningTeam = PlayerTeam.Team2;
        }
    }

    private void SyncGameFinishedOverPhoton()
    {
        Hashtable hash = new Hashtable
        {
            { "GameFinished", PhotonNetwork.LocalPlayer.NickName }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (!changedProps.ContainsKey("GameFinished")) return;

        clientsFinishedGameAmount++;
    }

    private void CheckForGameFinished()
    {
        if (clientsFinishedGameAmount != PhotonNetwork.CurrentRoom.PlayerCount) return;

        onAllClientsFinished.Raise(this, winningTeam);
        isMasterRunning = false;
    }

    public void CompleteTask(Component sender, object data)
    {
        if (data is InteractionStatus interactionStatus)
        {
            if (interactionStatus == InteractionStatus.Team1Finished)
            {
                team1TasksCompleted++;
            }
            else if (interactionStatus == InteractionStatus.Team2Finished)
            {
                team2TasksCompleted++;
            }

            onTaskCompleted.Raise(this, new TasksStatusData(tasksRequired, team1TasksCompleted, team2TasksCompleted));
            onTeam1ProgressChanged.Raise(this, (float)team1TasksCompleted / tasksRequired);
            onTeam2ProgressChanged.Raise(this, (float)team2TasksCompleted / tasksRequired);
        }
    }
}
