using UnityEngine;
using UnityEngine.UI; // UI 슬라이더를 위해 추가

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

    [Header("UI")]
    public Slider healthSlider; // 체력바 슬라이더 참조

    private void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하는지 확인
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 자신을 파괴
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        // 체력 슬라이더 초기화
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
        UpdateHealthBar(); // 체력바 업데이트
    }

    // 체력 회복
    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.FloorToInt(healthRegenRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    // 마나 회복
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate * Time.deltaTime);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        }
    }

    // 체력 감소 함수 (몬스터의 공격을 받을 때 호출)
    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - defense, 0);
        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar(); // 체력이 감소할 때 체력바 업데이트

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 체력바 업데이트
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
