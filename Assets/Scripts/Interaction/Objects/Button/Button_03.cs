using Photon.Pun;
using UnityEngine;

public class Button_03 : Button
{
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Button_03() : base(timeToComplete, teamMembersRequired)
    {
    }

    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        TeamMembersRequired = minimalTeamMembersCount >= 2 ? 2 : 1;
    }
}
