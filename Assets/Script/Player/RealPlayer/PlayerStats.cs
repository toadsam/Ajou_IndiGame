using UnityEngine;
using UnityEngine.UI; // UI �����̴��� ���� �߰�

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Health and Mana")]
    public int maxHealth = 100;
    public int currentHealth;

    public int maxMana = 50;
    public int currentMana;

    [Header("Attributes")]
    public int strength = 10; // ��
    public int defense = 5;    // ����

    [Header("Regeneration Rates")]
    public float healthRegenRate = 1f; // ü�� ����� (�ʴ� ȸ��)
    public float manaRegenRate = 2f;   // ���� ����� (�ʴ� ȸ��)

    [Header("UI")]
    public Slider healthSlider; // ü�¹� �����̴� ����

    private void Awake()
    {
        // �̱��� �ν��Ͻ��� �̹� �����ϴ��� Ȯ��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ڽ��� �ı�
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        // ü�� �����̴� �ʱ�ȭ
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Update()
    {
        RegenerateHealth();
        RegenerateMana();
        UpdateHealthBar(); // ü�¹� ������Ʈ
    }

    // ü�� ȸ��
    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.FloorToInt(healthRegenRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    // ���� ȸ��
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate * Time.deltaTime);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }
    }

    // ü�� ���� �Լ� (������ ������ ���� �� ȣ��)
    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - defense, 0);
        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar(); // ü���� ������ �� ü�¹� ������Ʈ

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ü�¹� ������Ʈ
    private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
