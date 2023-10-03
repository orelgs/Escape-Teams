using Photon.Pun;

public class Wheel : Interactable
{
    private const string initialPromptText = "Wheel";
    private const string inProgressText = "Wheeling..";
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Wheel() : base(initialPromptText, inProgressText, timeToComplete, teamMembersRequired)
    {
    }
    
    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        TeamMembersRequired = minimalTeamMembersCount >= 4 ? minimalTeamMembersCount / 2 : minimalTeamMembersCount;
    }
}
