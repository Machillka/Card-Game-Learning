using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/Heal Effect")]
public class HealEffect : Effect
{
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            from.HealHealth(value);
        }

        if (targetType == EffectTargetType.Target)
        {
            target.HealHealth(value);
        }
    }
}
