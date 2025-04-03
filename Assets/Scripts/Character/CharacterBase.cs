using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    // 设置血量
    public int maxHP;

    //TODO: 重置 defense 和 hp 的使用逻辑 MaxHp 其实没有必要 款今后Intv类型设置成private更好, 使用属性器来调用
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;

    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }

    public bool isDead = false;

    protected Animator animator;
    public GameObject buffAFX;
    public GameObject debuffAFX;

    // Strength
    public float baseStrength = 1f;
    private float strengthEffect = 0.5f;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHP;
        hp.currentValue = maxHP;
        // hp.SetValue(maxHP);
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;
        ResetDefense();
    }

    public void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue);
        currentDamage = currentDamage >= 0 ? currentDamage : 0;

        var currentDefense = (damage - defense.currentValue);
        currentDefense = currentDefense >= 0 ? 0 : currentDefense;

        defense.SetValue(currentDefense);

        if (CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
        }
        else
        {
            CurrentHP = 0;
            isDead = true;
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = CurrentHP >= MaxHP ? MaxHP : CurrentHP;
        buffAFX.SetActive(true);
    }

    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            buffAFX.SetActive(true);
        }
        else
        {
            float newStrength = baseStrength - strengthEffect;
            baseStrength = Mathf.Max(newStrength, 0f);
            debuffAFX.SetActive(true);
        }

        var currentRount = buffRound.currentValue + round;

        if (baseStrength == 1)
        {
            buffRound.SetValue(0);
        }
        else
        {
            buffRound.SetValue(currentRount);
        }
    }

    public void UpdateStrengthRoung()
    {
        var currentRount = buffRound.currentValue - 1;

        if (currentRount <= 0)
        {
            currentRount = 0;
            baseStrength = 1f;
        }

        buffRound.SetValue(currentRount);
    }
}
