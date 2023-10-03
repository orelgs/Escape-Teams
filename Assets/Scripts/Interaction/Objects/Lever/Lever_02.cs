using Photon.Pun;

public class Lever_02 : Lever
{
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Lever_02() : base(timeToComplete, teamMembersRequired)
    {
    }
    
    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        TeamMembersRequired = minimalTeamMembersCount >= 2 ? 2 : 1;
    }
}
