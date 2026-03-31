using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 5f;
    public float baseAttackSpeed = 1f;
    public float baseCritChance = 0.05f;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float attackSpeed;

    private float moveSpeedMultiplier = 1f;
    private float attackSpeedMultiplier = 1f;
    private float critChanceMultiplier = 1f;

    public int baseDamage = 10;
    [Range(0f, 1f)] public float critChance = 0.2f;
    public float critMultiplier = 2f;

    public int CalculateFinalDamage()
    {
        int damage = baseDamage;
        if (Random.value < critChance)
        {
            damage = Mathf.RoundToInt(baseDamage * critMultiplier);
            Debug.Log("[PlayerStats] CRITICAL STRIKE!");
        }
        return damage;
    }

    private void Awake()
    {
        ResetStats();
    }

    public void ApplyUpgrade(PlayerUpgradeData data)
    {
        switch (data.statType)
        {
            case StatType.MoveSpeed:
                if (data.valueType == UpgradeValueType.Flat)
                    moveSpeed += data.value;
                else
                {
                    moveSpeedMultiplier += data.value;
                    moveSpeed = baseMoveSpeed * moveSpeedMultiplier;
                }
                break;

            case StatType.AttackSpeed:
                if (data.valueType == UpgradeValueType.Flat)
                    attackSpeed += data.value;
                else
                {
                    attackSpeedMultiplier += data.value;
                    attackSpeed = baseAttackSpeed * attackSpeedMultiplier;
                }
                break;

            case StatType.CritChance:
                if (data.valueType == UpgradeValueType.Flat)
                    critChance += data.value;
                else
                {
                    critChanceMultiplier += data.value;
                    critChance = baseCritChance * critChanceMultiplier;
                }
                break;
            case StatType.Health:
                var player = Object.FindFirstObjectByType<PlayerController>();
                if (player != null)
                    ApplyHealthUpgrade(player, data);
                break;
        }

        Debug.Log($"Applied {data.upgradeName}: {data.value} {data.valueType} {data.statType}");
    }

    public void ResetStats()
    {
        moveSpeedMultiplier = 1f;
        attackSpeedMultiplier = 1f;
        critChanceMultiplier = 1f;

        moveSpeed = baseMoveSpeed;
        attackSpeed = baseAttackSpeed;
        critChance = baseCritChance;
    }

    public int CalculateDamage(int baseDamage)
    {
        bool isCrit = UnityEngine.Random.value < critChance;
        int finalDamage = isCrit ? baseDamage * 2 : baseDamage;

        if (isCrit)
        {
            Debug.Log($"[PlayerStats] Critical strike rolled! Base={baseDamage}, Final={finalDamage}");
        }

        return finalDamage;
    }

    public void ApplyHealthUpgrade(PlayerController player, PlayerUpgradeData data)
    {
        if (data.valueType == UpgradeValueType.Flat)
        {
            player.SetHealth(player.GetCurrentHealth() + (int)data.value);
        }
        else
        {
            player.maxHealth = Mathf.RoundToInt(player.maxHealth * (1f + data.value));
            player.SetHealth(player.maxHealth); // reset to new max
        }

        Debug.Log($"Applied {data.upgradeName}: {data.value} {data.valueType} Health");
    }
}
