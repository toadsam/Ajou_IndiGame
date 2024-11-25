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
    public int strength = 10; // 힘
    public int defense = 5;    // 방어력

    [Header("Regeneration Rates")]
    public float healthRegenRate = 1f; // 체력 재생률 (초당 회복)
    public float manaRegenRate = 2f;   // 마나 재생률 (초당 회복)

    [Header("Level and Experience")]
    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;

    [Header("Currency")]
    public int gold = 500; // 플레이어의 초기 재화

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
        Debug.Log($"경험치 획득: {amount} / 총 경험치: {experience}");

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel *= 2; // 다음 레벨로 가는 데 필요한 경험치 2배 증가

        //maxHealth += 10;
        //currentHealth = maxHealth;
        //strength += 2;
        //defense += 1;
        InGameSkillManager.instance.LevelUp();

        Debug.Log($"레벨 업! 현재 레벨: {level}, 다음 레벨까지 필요 경험치: {experienceToNextLevel}");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }

    // 재화 소비 메서드
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log($"재화 {amount} 사용. 남은 재화: {gold}");
            return true;
        }
        Debug.Log("재화가 부족합니다.");
        return false;
    }

    // 재화 획득 메서드
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"재화 {amount} 획득. 총 재화: {gold}");
    }
}
