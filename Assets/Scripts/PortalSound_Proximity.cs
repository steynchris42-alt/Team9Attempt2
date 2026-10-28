using UnityEngine;

public class PortalSound_Proximity : MonoBehaviour
{
    private float DisToPlayer;
    public Transform player;
    public AudioSource PortalSound;
    private void Awake()
    {
        PortalSound.mute = true;

    }
    void Update()
    {
        DisToPlayer = Vector3.Distance(transform.position, player.position);
            if(DisToPlayer <= 70.0f)
        {
            PortalSound.mute = false;
        }
        else if (DisToPlayer > 15.0f)
        {
            PortalSound.mute = true;
        }
    }
}
