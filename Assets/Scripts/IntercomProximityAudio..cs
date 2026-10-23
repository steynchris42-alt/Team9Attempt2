
using UnityEngine;

public class IntercomProximityAudio : MonoBehaviour
{
    public AudioSource radioAudioSource;
    public AudioClip radioStaticClip;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            if (radioAudioSource != null && radioStaticClip != null)
            {
                radioAudioSource.PlayOneShot(radioStaticClip);
                hasPlayed = true;
            }
            else
            {
                Debug.LogWarning("Assign the radio Audio Source and Static Clip!");
            }
        }
    }
}

