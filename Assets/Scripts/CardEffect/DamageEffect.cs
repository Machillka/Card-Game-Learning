using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/Damage Effect")]
public class DamageEffect : Effect
{
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        if (target == null)
            return;
        switch (targetType)
        {
            case EffectTargetType.Target:
                //TODO: 计算伤害
                var damage = (int)math.round(value * from.baseStrength);
                target.TakeDamage(damage);
                break;
            case EffectTargetType.All:
                //TODO: 实现特定几个敌人的攻击
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}
