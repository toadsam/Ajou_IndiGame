using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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

    private void Start()
    {
        // �ʱⰪ ����
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        // ü�°� ���� �ڵ� ���
        RegenerateHealth();
        RegenerateMana();
    }

    // ü�� ȸ��
    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.FloorToInt(healthRegenRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // �ִ� ü���� ���� �ʵ��� ����
        }
    }

    // ���� ȸ��
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate * Time.deltaTime);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // �ִ� ������ ���� �ʵ��� ����
        }
    }

    // ü�� ���� �Լ� (������ ������ ���� �� ȣ��)
    public void TakeDamage(int damage)
    {
        // ���� �����Ͽ� ���� ������ ����
        int finalDamage = Mathf.Max(damage - defense, 0); // ���¸�ŭ ������ ����, �ּҰ��� 0
        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü���� ������ �������� �ʵ��� ����

        if (currentHealth <= 0)
        {
            Die(); // �÷��̾ ������ ȣ��Ǵ� �Լ�
        }
    }

    // ���� �Ҹ� �Լ� (��ų ��� �� ȣ��)
    public void UseMana(int amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana); // ������ ������ �������� �ʵ��� ����
    }

    // �� ���� �Լ� (�������̳� ��ų�� ���� ���� ���� ��)
    public void IncreaseStrength(int amount)
    {
        strength += amount;
    }

    // ���� ���� �Լ�
    public void IncreaseDefense(int amount)
    {
        defense += amount;
    }

    // �÷��̾ �׾��� �� ȣ��Ǵ� �Լ�
    private void Die()
    {
        Debug.Log("Player has died.");
        // ���⼭ ���� ���� ó�� ���� �߰��� �� �ֽ��ϴ�.
    }
}
