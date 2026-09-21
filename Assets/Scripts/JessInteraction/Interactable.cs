using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    // Outline outline; // Commented out to fix CS0246 error
    public string message = "[E] Interact"; // Changed: Added default message text

    public UnityEvent onInteraction;

    // Start is called before the first frame update
    void Start()
    {
        // outline = GetComponent<Outline>();
        // if (outline != null)
        // {
        //     outline.enabled = false;
        // }
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    // Helper method to change the message text from another script or UnityEvent
    public void SetMessage(string newMessage)
    {
        message = newMessage;
    }

    public void OnHover()
    {
        EnableOutline();
    }

    public void OnHoverExit()
    {
        DisableOutline();
    }

    public void DisableOutline()
    {
        // if (outline != null)
        //     outline.enabled = false;
    }

    public void EnableOutline()
    {
        // if (outline != null)
        //     outline.enabled = true;
    }
}
