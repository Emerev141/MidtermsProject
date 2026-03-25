using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseMoveSpeed = 5f;
    public float baseAttackSpeed = 1f;
    public float baseCritChance = 0.05f;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float critChance;

    private float moveSpeedMultiplier = 1f;
    private float attackSpeedMultiplier = 1f;
    private float critChanceMultiplier = 1f;

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
}
