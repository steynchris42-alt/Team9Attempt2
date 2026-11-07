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
    public void HealthGain()
    {
        Mathf.Clamp(HealthSlider.value+=20, iHealthMin, iHealthMax);
    }
    public void TakeDamage()
    {
        HealthSlider.value--;
        if (HealthSlider.value == HealthSlider.minValue)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("MAN IM DEAD");
        SceneManager.LoadScene("Death_proto3_Scene");
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
