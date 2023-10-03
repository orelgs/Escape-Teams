using Photon.Pun;

public class Detonator : Interactable
{
    private const string initialPromptText = "Push Handle";
    private const string inProgressText = "Pushing Handle..";
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Detonator() : base(initialPromptText, inProgressText, timeToComplete, teamMembersRequired)
    {
    }

    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        TeamMembersRequired = minimalTeamMembersCount;    
    }
}
