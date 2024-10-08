using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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

    private void Start()
    {
        // 초기값 설정
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    private void Update()
    {
        // 체력과 마나 자동 재생
        RegenerateHealth();
        RegenerateMana();
    }

    // 체력 회복
    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += Mathf.FloorToInt(healthRegenRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 최대 체력을 넘지 않도록 제한
        }
    }

    // 마나 회복
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate * Time.deltaTime);
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // 최대 마나를 넘지 않도록 제한
        }
    }

    // 체력 감소 함수 (몬스터의 공격을 받을 때 호출)
    public void TakeDamage(int damage)
    {
        // 방어력 적용하여 받은 데미지 감소
        int finalDamage = Mathf.Max(damage - defense, 0); // 방어력만큼 데미지 감소, 최소값은 0
        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력이 음수로 내려가지 않도록 제한

        if (currentHealth <= 0)
        {
            Die(); // 플레이어가 죽으면 호출되는 함수
        }
    }

    // 마나 소모 함수 (스킬 사용 시 호출)
    public void UseMana(int amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana); // 마나가 음수로 내려가지 않도록 제한
    }

    // 힘 증가 함수 (아이템이나 스킬을 통해 힘을 얻을 때)
    public void IncreaseStrength(int amount)
    {
        strength += amount;
    }

    // 방어력 증가 함수
    public void IncreaseDefense(int amount)
    {
        defense += amount;
    }

    // 플레이어가 죽었을 때 호출되는 함수
    private void Die()
    {
        Debug.Log("Player has died.");
        // 여기서 게임 오버 처리 등을 추가할 수 있습니다.
    }
}
