public class Lever : Interactable
{
    private const string initialPromptText = "Pull Lever";
    private const string inProgressText = "Pulling lever..";

    public Lever(float timeToComplete = 2f, int teamMembersRequired = 1) : base(initialPromptText, inProgressText, timeToComplete, teamMembersRequired)
    {
    }
}
