using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsSceneManager : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] private GameEvent onTeamWinAnnounced;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        string winningTeam = (string)info.photonView.InstantiationData[0];

        if (winningTeam == "Team1")
        {
            onTeamWinAnnounced.Raise(this, PlayerTeam.Team1);
        }
        else if (winningTeam == "Team2")
        {
            onTeamWinAnnounced.Raise(this, PlayerTeam.Team2);
        }
    }
}
