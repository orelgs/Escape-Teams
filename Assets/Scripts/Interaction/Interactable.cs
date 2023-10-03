using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public abstract class Interactable : MonoBehaviourPunCallbacks, IInteractable
{
    protected string InitialPromptText { get; set; }

    protected string InProgressText { get; set; }

    protected string CurrentActionText { get; set; }
    string IInteractable.CurrentActionText => CurrentActionText;

    protected float TimeToComplete { get; set; }
    float IInteractable.TimeToComplete => TimeToComplete;

    protected int TeamMembersRequired { get; set; }
    int IInteractable.TeamMembersRequired => TeamMembersRequired;

    [SerializeField] private GameEvent onObjectInteractionStarted;
    private GameEvent OnObjectInteractionStarted => onObjectInteractionStarted;

    [SerializeField] private GameEvent onObjectInteractionEnded;
    private GameEvent OnObjectInteractionEnded => onObjectInteractionEnded;

    protected InteractionStatus InteractionStatus { get; set; } = InteractionStatus.NotStarted;
    InteractionStatus IInteractable.InteractionStatus => InteractionStatus;

    protected DateTime? TimeStarted { get; set; } = null;

    private List<string> CurrentInteractors { get; set; } = new List<string>();

    private List<string> Team1Interactors { get; set; } = new List<string>();
    public int Team1Members => Team1Interactors.Count;

    private List<string> Team2Interactors { get; set; } = new List<string>();
    public int Team2Members => Team2Interactors.Count;

    private float progress = 0;
    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value;
        }
    }

    private Animator animator;
    private string animatorTriggerName;

    public Interactable(string initialPromptText = "Interact",
                        string inProgressText = "Interacting..",
                        float timeToComplete = 5f,
                        int teamMembersRequired = 1)
    {
        CurrentActionText = InitialPromptText = initialPromptText;
        InProgressText = inProgressText;
        TimeToComplete = timeToComplete;
        TeamMembersRequired = teamMembersRequired;
    }

    private void Awake()
    {
        // Assets.Interactable interactable = GetComponentInChildren<Assets.Interactable>();

        // animator = interactable.GetAnimator();
        // animatorTriggerName = interactable.GetAnimationTriggerName();
    }

    private void Update()
    {
        if (InteractionStatus == InteractionStatus.NotStarted)
        {
            if (CanStartInteraction())
            {
                StartInteraction();
            }
        }
        else if (InteractionStatus == InteractionStatus.InProgress)
        {
            if (CanStartInteraction())
            {
                UpdateProgress();
            }
            else
            {
                OnInteractionCancelled();
            }
        }
    }

    public bool Interact(Interactor interactor)
    {
        if (!CanInteract(interactor)) return false;

        AddInteractor(interactor);

        return true;
    }

    private bool CanStartInteraction()
    {
        return Team1Interactors.Count >= TeamMembersRequired || Team2Interactors.Count >= TeamMembersRequired;
    }

    private void StartInteraction()
    {
        List<string> teamToAdd = null;

        if (Team1Interactors.Count >= TeamMembersRequired)
        {
            teamToAdd = Team1Interactors;
        }
        else if (Team2Interactors.Count >= TeamMembersRequired)
        {
            teamToAdd = Team2Interactors;
        }

        if (teamToAdd is not null)
        {
            for (int i = 0; i < TeamMembersRequired; i++)
            {
                CurrentInteractors.Add(teamToAdd[i]);
            }

            TimeStarted = DateTime.Now;
            CurrentActionText = InProgressText;
            InteractionStatus = InteractionStatus.InProgress;
            OnObjectInteractionStarted.Raise(this, CurrentInteractors);
            Animate();
        }
    }

    private void Animate()
    {
        Assets.Interactable interactable = GetComponentInChildren<Assets.Interactable>();

        animator = interactable.GetAnimator();
        animatorTriggerName = interactable.GetAnimationTriggerName();
        animator.SetTrigger(animatorTriggerName);
    }

    private void AddInteractor(Interactor interactor)
    {
        PlayerTeam playerTeam = interactor.PlayerStats.PlayerTeam;

        if (playerTeam == PlayerTeam.Team1)
        {
            if (Team1Interactors.Contains(PhotonNetwork.LocalPlayer.NickName)) return;

            Team1Interactors.Add(PhotonNetwork.LocalPlayer.NickName);
            SyncInteractorsListOverPhoton("Team1Interactors", Team1Interactors);
        }
        else
        {
            if (Team2Interactors.Contains(PhotonNetwork.LocalPlayer.NickName)) return;

            Team2Interactors.Add(PhotonNetwork.LocalPlayer.NickName);
            SyncInteractorsListOverPhoton("Team2Interactors", Team2Interactors);
        }
    }

    public bool CanInteract(Interactor interactor)
    {
        return InteractionStatus == InteractionStatus.NotStarted;
    }

    protected void UpdateProgress()
    {
        int timePassedMilliseconds = (int)(DateTime.Now - TimeStarted.Value).TotalMilliseconds;
        int timeRequiredMilliseconds = (int)(TimeToComplete * 1000);

        if (timeRequiredMilliseconds - timePassedMilliseconds > 0)
        {
            Progress = (float)timePassedMilliseconds / timeRequiredMilliseconds;
        }
        else
        {
            OnInteractionSuccessful();
        }
    }

    private void ResetObject()
    {
        Progress = 0;
        CurrentActionText = InitialPromptText;
        CurrentInteractors.Clear();
        Team1Interactors.Clear();
        Team2Interactors.Clear();
    }

    private void OnInteractionEnded()
    {
        ResetObject();
        OnObjectInteractionEnded.Raise(this, InteractionStatus);
    }

    public void CancelInteraction(Interactor interactor)
    {
        PlayerTeam playerTeam = interactor.PlayerStats.PlayerTeam;

        if (CurrentInteractors.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            CurrentInteractors.Remove(PhotonNetwork.LocalPlayer.NickName);
            SyncInteractorsListOverPhoton("CurrentInteractors", CurrentInteractors);
        }

        if (playerTeam == PlayerTeam.Team1 && Team1Interactors.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            Team1Interactors.Remove(PhotonNetwork.LocalPlayer.NickName);
            SyncInteractorsListOverPhoton("Team1Interactors", Team1Interactors);
        }
        else if (playerTeam == PlayerTeam.Team2 && Team2Interactors.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            Team2Interactors.Remove(PhotonNetwork.LocalPlayer.NickName);
            SyncInteractorsListOverPhoton("Team2Interactors", Team2Interactors);
        }
    }

    private void OnInteractionCancelled()
    {
        InteractionStatus = InteractionStatus.NotStarted;
        OnInteractionEnded();
        SyncInteractorsListOverPhoton("CurrentInteractors", CurrentInteractors);
        SyncInteractorsListOverPhoton("Team1Interactors", Team1Interactors);
        SyncInteractorsListOverPhoton("Team2Interactors", Team2Interactors);
    }

    protected void OnInteractionSuccessful()
    {
        if (Team1Interactors.Count >= TeamMembersRequired)
        {
            InteractionStatus = InteractionStatus.Team1Finished;
        }
        else
        {
            InteractionStatus = InteractionStatus.Team2Finished;
        }

        OnInteractionEnded();
    }

    private void SyncInteractorsListOverPhoton(string listPropertyName, List<string> interactorsList)
    {
        // if (!PV.IsMine) return;

        Hashtable hash = new Hashtable
        {
            { gameObject.name, new object[] {listPropertyName, interactorsList.ToArray() } }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey(gameObject.name)) return;

        object[] objects = (object[])changedProps[gameObject.name];
        string listName = (string)(objects[0]);
        string[] newList = (string[])(objects[1]);

        if (listName == "CurrentInteractors")
        {
            UpdateList(CurrentInteractors, newList);
        }

        if (listName == "Team1Interactors")
        {
            UpdateList(Team1Interactors, newList);
        }

        if (listName == "Team2Interactors")
        {
            UpdateList(Team2Interactors, newList);
        }
    }

    private void UpdateList(List<string> oldInteractorsList, string[] newInteractorsList)
    {
        oldInteractorsList.Clear();
        foreach (string interactor in newInteractorsList)
        {
            oldInteractorsList.Add(interactor);
        }
    }
}