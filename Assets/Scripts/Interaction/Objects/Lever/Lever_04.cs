public class Lever_04 : Lever
{
    private const float timeToComplete = 4f;
    private const int teamMembersRequired = 1;

    public Lever_04() : base(timeToComplete, teamMembersRequired)
    {
        TeamMembersRequired = 1;
    }
}
