using UnityEngine;
using System.Collections;



public class Dome_Controller : MonoBehaviour
{
    public bool IsDomeCoro; // set in PLAYER CONTROLLER
    public bool IsDomeDoorActive;
    public GameObject DomeDoor1;
    public Coroutine Dome_Coro;

    public void Update()
    {
        if (IsDomeCoro && Dome_Coro == null)
        {
            Dome_Coro = StartCoroutine(Dome_Coroutine());
        }
    }
    public IEnumerator Dome_Coroutine()
    {
        Debug.Log("DOME CORO STARTED");
        if (IsDomeDoorActive == true)
        {
            DomeDoor1.SetActive(false);
        }
        else if (IsDomeDoorActive == false) 
        {
            DomeDoor1.SetActive(true);
        }
            yield return null;
            Debug.Log("DOME CORO ENDED");
            Dome_Coro = null;
            IsDomeCoro = false;
        

    }
}
