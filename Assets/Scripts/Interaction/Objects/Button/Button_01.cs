public class Button_01 : Button
{
    private const float timeToComplete = 4f;
    private const int teamMembersRequired = 1;

    public Button_01() : base(timeToComplete, teamMembersRequired)
    {
        TeamMembersRequired = 1;
    }
}
