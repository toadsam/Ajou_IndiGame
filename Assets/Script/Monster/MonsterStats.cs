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
        Debug.Log("������ �Ա�");
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

        Destroy(gameObject); // ���� ������Ʈ ����
        if (healthSlider != null)
        {
            Destroy(healthSlider.gameObject); // ü�¹� ����
        }
    }
}
