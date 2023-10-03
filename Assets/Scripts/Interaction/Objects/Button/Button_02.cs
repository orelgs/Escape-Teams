public class Button_02 : Button
{
    private const float timeToComplete = 4f;
    private const int teamMembersRequired = 1;

    public Button_02() : base(timeToComplete, teamMembersRequired)
    {
        TeamMembersRequired = 1;
    }
}
