using System.Collections;
using UnityEngine;
using TMPro;

public class IntercomManager : MonoBehaviour
{
    [Header("625095_wwstudioswastaken_radio_static_01")]
public AudioSource radioAudioSource;
public AudioClip radioStaticClip;




    [Header("Intercom UI")]
    public GameObject intercomPanel;
    public TMP_Text dialogueText;

    [Header("Intercom Audio")]
    public AudioSource audioSource;
    public AudioClip staticSound;

    [Header("Settings")]
    public float messageDuration = 6f;

    private Coroutine currentMessage;

    private void Start()
    {
        if (intercomPanel != null)
        {
            intercomPanel.SetActive(false);
        }
    }

    public void PlayMessage(string message)
    {
        if (currentMessage != null)
        {
            StopCoroutine(currentMessage);
        }

        currentMessage = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        if (intercomPanel != null)
        {
            intercomPanel.SetActive(true);
        }

        if (dialogueText != null)
        {
            dialogueText.text = message;
        }

        if (audioSource != null && staticSound != null)
        {
            audioSource.PlayOneShot(staticSound);
        }

        yield return new WaitForSeconds(messageDuration);

        if (intercomPanel != null)
        {
            intercomPanel.SetActive(false);
        }

        currentMessage = null;
    }
    public void PlayRadioStatic()
{
    if (radioAudioSource != null && radioStaticClip != null)
    {
        radioAudioSource.PlayOneShot(radioStaticClip);
    }
    else
    {
        Debug.LogWarning("Radio Audio Source or Static Clip is missing!");
    }
}
}








