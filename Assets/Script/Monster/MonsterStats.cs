using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthSlider;

    public int experienceReward = 50;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("데미지 입기");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Monster has died.");
        PlayerStats.instance.GainExperience(experienceReward);

        Destroy(gameObject); // 몬스터 오브젝트 제거
        if (healthSlider != null)
        {
            Destroy(healthSlider.gameObject); // 체력바 제거
        }
    }
}
