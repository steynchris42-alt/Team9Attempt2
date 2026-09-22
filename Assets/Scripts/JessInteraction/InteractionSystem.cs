using UnityEngine;

public interface IInteractable
{
    void Interact();
    void OnHover();
    void OnHoverExit();
}

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Camera cam;
    public float distance = 3f;
    public Color HoverColor = Color.green;

    private IInteractable current;
    private Renderer r;
    private Color originalColor;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance) &&
            hit.collider.TryGetComponent(out IInteractable i))
        {
            if (current != i)
            {
                Clear();

                current = i;
                current.OnHover();

                r = hit.collider.GetComponent<Renderer>();

                if (r != null)
                {
                    originalColor = r.material.color;
                    r.material.color = HoverColor;
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                current.Interact();
            }
        }
        else
        {
            Clear();
        }
    }

    void Clear()
    {
        if (current != null)
        {
            current.OnHoverExit();
        }

        if (r != null)
        {
            r.material.color = originalColor;
        }

        current = null;
        r = null;
    }
}
