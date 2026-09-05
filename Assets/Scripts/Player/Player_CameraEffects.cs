using UnityEngine;

public class Player_CameraEffects : MonoBehaviour
{
    private Camera camera;

    private PlayerController Player_Controller;
    private void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        if (Player_Controller == null)
        {
            Player_Controller = GetComponent<PlayerController>();
        }
    }
}
