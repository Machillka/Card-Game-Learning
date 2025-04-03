using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffet", menuName = "Card Effect/Defense Effect")]
public class DefenseEffect : Effect
{
    public override void Excute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            from.UpdateDefense(value);
        }

        if (targetType == EffectTargetType.Target)
        {
            target.UpdateDefense(value);
        }
    }
}
