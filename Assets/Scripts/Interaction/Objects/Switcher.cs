public class Switcher : Interactable
{
    private const string initialPromptText = "Switch On";
    private const string inProgressText = "Switching On..";
    private const float timeToComplete = 4f;
    private const int teamMembersRequired = 1;

    public Switcher() : base(initialPromptText, inProgressText, timeToComplete, teamMembersRequired)
    {
        TeamMembersRequired = 1;
    }
}
