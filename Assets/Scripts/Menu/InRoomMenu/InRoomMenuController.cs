using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class InRoomMenuController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button startGameButton;
    [SerializeField] private TMP_Text errorMessageText;
    [SerializeField] private GameObject readyToStartText;
    [SerializeField] private Transform playerListContentTeam1;
    [SerializeField] private Transform playerListContentTeam2;

    // Update is called once per frame
    void Update()
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

        CheckForStartGamePossibility();
    }

    private void CheckForStartGamePossibility()
    {
        int totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        int team1Players = playerListContentTeam1.childCount;
        int team2Players = playerListContentTeam2.childCount;
        bool isStartGamePossible = true;
        string errorMessage = "";

        if (totalPlayers < 2)
        {
            isStartGamePossible = false;
            errorMessage = "Not enough players to start the game";
        }
        else if (Math.Abs(team1Players - team2Players) > 1)
        {
            isStartGamePossible = false;
            errorMessage = "Cannot have more than 1 player gap between teams";
        }

        startGameButton.interactable = isStartGamePossible;
        errorMessageText.text = errorMessage;
        readyToStartText.SetActive(isStartGamePossible);
    }
}
