using UnityEngine;

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

    [Header("Level and Experience")]
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        RegenerateHealth();
        RegenerateMana();
    }

    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.FloorToInt(healthRegenRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate * Time.deltaTime);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - defense, 0);
        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        Debug.Log($"����ġ ȹ��: {amount} / �� ����ġ: {experience}");

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel *= 2; // ���� ������ ���� �� �ʿ��� ����ġ 2�� ����

        //maxHealth += 10;
        //currentHealth = maxHealth;
        //strength += 2;
        //defense += 1;
        InGameSkillManager.instance.LevelUp();

        Debug.Log($"���� ��! ���� ����: {level}, ���� �������� �ʿ� ����ġ: {experienceToNextLevel}");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
