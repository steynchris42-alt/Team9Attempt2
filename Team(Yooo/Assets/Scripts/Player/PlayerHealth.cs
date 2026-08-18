using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
[SerializeField]private int iHealthMax = 100;
[SerializeField]private int iHealthMin = 1;
[SerializeField]private int iCurrentHealth = 100;
private void HealthGain()
    {
        iCurrentHealth++;
    }
 public void TakeDamage()
    {
      iCurrentHealth = Mathf.Clamp(iCurrentHealth, iHealthMax, iHealthMin);
        iCurrentHealth--;
    }
private void Die()
    {
        Debug.Log("Womp womp you died");
    }
}
