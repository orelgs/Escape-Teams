using Photon.Pun;

public class Lever_03 : Lever
{
    private const float timeToComplete = 2f;
    private const int teamMembersRequired = 1;

    public Lever_03() : base(timeToComplete, teamMembersRequired)
    {

    }
    
    private void Awake() 
    {
        int minimalTeamMembersCount = PhotonNetwork.CurrentRoom.PlayerCount / 2;

        if (minimalTeamMembersCount <= 2)
        {
            TeamMembersRequired = 1;
        } else if (minimalTeamMembersCount == 3)
        {
            TeamMembersRequired = 3;
        } else
        {
            System.Random random = new System.Random();
            TeamMembersRequired = random.Next(2, 4);
        }
    }
}
