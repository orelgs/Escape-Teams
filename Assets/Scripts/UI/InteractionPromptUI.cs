using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private GameObject interactionNotStartedPanel;
    [SerializeField] private GameObject interactionInProgressPanel;
    [SerializeField] private TextMeshProUGUI promptActionText;
    [SerializeField] private TextMeshProUGUI teamMembersRequiredText;
    [SerializeField] private TextMeshProUGUI team1MembersText;
    [SerializeField] private TextMeshProUGUI team2MembersText;
    [SerializeField] private TextMeshProUGUI timeToCompleteText;
    [SerializeField] private TextMeshProUGUI currentlyInteractingTeamText;
    [SerializeField] private GameObject pressToCancelActionText;
    public bool IsDisplayed { get; private set; } = false;

    private void Start()
    {
        promptPanel.SetActive(false);
        interactionNotStartedPanel.SetActive(false);
        interactionInProgressPanel.SetActive(false);
        pressToCancelActionText.SetActive(false);
    }

    public void SetUp(IInteractable interactable, bool isInteracting)
    {
        IsDisplayed = true;
        promptPanel.SetActive(true);
        promptActionText.text = interactable.CurrentActionText;
        pressToCancelActionText.SetActive(isInteracting);

        if (interactable.InteractionStatus == InteractionStatus.NotStarted)
        {
            SetUpInteractionNotStartedPanel(interactable);
        }
        else if (interactable.InteractionStatus == InteractionStatus.InProgress)
        {
            SetUpInteractionInProgressPanel(interactable);
        }
        else
        {
            Close();
        }
    }

    private void SetUpInteractionNotStartedPanel(IInteractable interactable)
    {
        interactionNotStartedPanel.SetActive(true);
        interactionInProgressPanel.SetActive(false);
        teamMembersRequiredText.text = interactable.TeamMembersRequired.ToString();
        team1MembersText.text = interactable.Team1Members.ToString();
        team2MembersText.text = interactable.Team2Members.ToString();
        timeToCompleteText.text = interactable.TimeToComplete.ToString();
    }

    private void SetUpInteractionInProgressPanel(IInteractable interactable)
    {
        interactionInProgressPanel.SetActive(true);
        interactionNotStartedPanel.SetActive(false);
        currentlyInteractingTeamText.text = interactable.Team1Members > interactable.Team2Members ? "Team 1" : "Team 2";
    }

    public void Close()
    {
        IsDisplayed = false;
        promptPanel.SetActive(false);
        interactionNotStartedPanel.SetActive(false);
        interactionInProgressPanel.SetActive(false);
        pressToCancelActionText.SetActive(false);
    }
}
