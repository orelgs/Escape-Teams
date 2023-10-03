public interface IInteractable
{
    string CurrentActionText { get; }
    InteractionStatus InteractionStatus { get; }
    int TeamMembersRequired { get; }
    int Team1Members { get; }
    int Team2Members { get; }
    float TimeToComplete { get; }
    float Progress { get; }

    bool Interact(Interactor interactor);
    void CancelInteraction(Interactor interactor);
}
