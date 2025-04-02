using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    // 设置血量
    public int maxHP;
    public IntVariable hp;

    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }

    public bool isDead = false;

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHP;
        hp.currentValue = maxHP;
        CurrentHP = MaxHP;
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHP > damage)
        {
            CurrentHP -= damage;
        }
        else
        {
            CurrentHP = 0;
            isDead = true;
        }
    }
}
