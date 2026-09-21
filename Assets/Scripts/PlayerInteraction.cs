using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float playerReach = 3f;
    [SerializeField] private Camera playerCamera; // Drag your camera here in Inspector or let Start auto-find it

    private Interactable currentInteractable;

    void Start()
    {
        // Automatically fetch Main Camera if not manually assigned in Inspector
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Lock and hide cursor for center-screen raycasting
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckInteraction();

        // Trigger interaction on key press
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        if (playerCamera == null) return;

        RaycastHit hit;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // Perform raycast
        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                // Check collider or parent for the Interactable script
                Interactable newInteractable = hit.collider.GetComponentInParent<Interactable>();

                if (newInteractable != null && newInteractable.enabled)
                {
                    if (currentInteractable != newInteractable)
                    {
                        DisableCurrentInteractable();
                        SetNewCurrentInteractable(newInteractable);
                    }
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.OnHover();
    }

    void DisableCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnHoverExit();
            currentInteractable = null;
        }
    }
}
