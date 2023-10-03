using Photon.Pun;

public class Lever_01 : Lever
{
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Lever_01() : base(timeToComplete, teamMembersRequired)
    {
    }

    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        TeamMembersRequired = minimalTeamMembersCount >= 3 ? minimalTeamMembersCount / 3 + 1 : minimalTeamMembersCount;
    }
}
