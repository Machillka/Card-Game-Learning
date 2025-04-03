using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCard Effect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;

    public override void Excute(CharacterBase from, CharacterBase target)
    {
        // 实现抽卡效果
        drawCardEvent.RaiseEvent(value, this);
    }
}
