using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffet", menuName = "Card Effect/Strength Effect")]
public class StrengthEffect : Effect
{
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupStrength(value, true);
                break;
            case EffectTargetType.Target:
                //TODO: 计算伤害
                target.SetupStrength(value, false);
                break;
            case EffectTargetType.All:
                break;
        }
    }
}
