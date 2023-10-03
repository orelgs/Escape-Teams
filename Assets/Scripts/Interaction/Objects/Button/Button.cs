public class Button : Interactable
{
    private const string initialPromptText = "Push Button";
    private const string inProgressText = "Pushing button..";

    public Button(float timeToComplete = 2f, int teamMembersRequired = 1) : base(initialPromptText, inProgressText, timeToComplete, teamMembersRequired)
    {
    }
}
