using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int iHealthMax = 100;
    [SerializeField] private int iHealthMin = 1;
    [SerializeField] private int iCurrentHealth = 100;

    public Slider HealthSlider;
    private void HealthGain()
    {
        iCurrentHealth++;
    }
    public void TakeDamage()
    {
        HealthSlider.value--;
        if (HealthSlider.value == HealthSlider.minValue)
        {
            Die();
        }
       /* iCurrentHealth--;
        if (iCurrentHealth <= iHealthMin)
        {
            Die();
        } */
    }
    public void Die()
    {
        Debug.Log("MAN IM DEAD");
        SceneManager.LoadScene("Death");
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy_Bullet"))
            {
            TakeDamage();
            //iCurrentHealth = Mathf.Clamp(iCurrentHealth, iHealthMin, iHealthMax);
        }

    }
}
