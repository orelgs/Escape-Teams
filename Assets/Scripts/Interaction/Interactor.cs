using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    [SerializeField] private PromptProgressBarUI promptProgressBarUI;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;

    private IInteractable closestInteractable;
    private IInteractable currentlyInteractingWith = null;

    private bool IsInteracting { get; set; } = false;

    private PlayerController playerController;
    public PlayerStats PlayerStats { get; private set; }

    private PhotonView PV;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        PlayerStats = GetComponent<PlayerStats>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;

        CheckForInteractionPromptUpdate();

        if (!IsInteracting)
        {
            TryFindInteractables();
        }
    }

    private void CheckForInteractionPromptUpdate()
    {
        if (closestInteractable is null && currentlyInteractingWith is null)
        {
            if (interactionPromptUI.IsDisplayed)
            {
                interactionPromptUI.Close();
                promptProgressBarUI.IncrementProgress(0);
            }
        }
        else
        {
            IInteractable interactable = currentlyInteractingWith is not null ? currentlyInteractingWith : closestInteractable;

            interactionPromptUI.SetUp(interactable, IsInteracting);
            promptProgressBarUI.IncrementProgress(interactable.Progress);
        }
    }

    public void AttemptInteraction(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (!IsInteracting)
        {
            if (closestInteractable is not null)
            {
                IsInteracting = closestInteractable.Interact(this);

                if (IsInteracting)
                {
                    currentlyInteractingWith = closestInteractable;
                }

                playerController.SetMove(!IsInteracting);
            }
        }
        else
        {
            currentlyInteractingWith.CancelInteraction(this);
            IsInteracting = false;
            currentlyInteractingWith = null;
            playerController.SetMove(!IsInteracting);
        }
    }

    private void TryFindInteractables()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

        if (numFound > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider is not null && !collider.IsDestroyed())
                {
                    IInteractable interactable = collider.GetComponent<IInteractable>();

                    if (interactable is not null)
                    {
                        closestInteractable = interactable;
                        break;
                    }
                }
            }
        }
        else
        {
            if (closestInteractable is not null)
            {
                closestInteractable = null;
            }
        }
    }

    public void InteractionStarted(Component sender, object data)
    {
        if (data is List<string> interactors && sender is IInteractable interactable)
        {
            if (!interactors.Contains(PhotonNetwork.LocalPlayer.NickName) && IsInteracting && currentlyInteractingWith == interactable)
            {
                IsInteracting = false;
                currentlyInteractingWith = null;
                playerController.SetMove(!IsInteracting);
            }
        }
    }

    public void InteractionEnded(Component sender, object data)
    {
        if (sender is IInteractable interactable && IsInteracting && currentlyInteractingWith == interactable)
        {
            IsInteracting = false;
            currentlyInteractingWith = null;
            playerController.SetMove(!IsInteracting);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
