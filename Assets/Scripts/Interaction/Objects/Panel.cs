using Photon.Pun;

public class Panel : Interactable
{
    private const string initialPromptText = "Touch Panel";
    private const string inProgressText = "Touching Panel..";
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Panel() : base(initialPromptText, inProgressText, timeToComplete, teamMembersRequired)
    {
    }

    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        if (minimalTeamMembersCount <= 2)
        {
            TeamMembersRequired = 1;
        } else
        {
            System.Random random = new System.Random();
            TeamMembersRequired = random.Next(2, 3);
        }
    }
}
