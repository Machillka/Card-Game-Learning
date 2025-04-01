using UnityEngine;
using UnityEditor;

// [CreateAssetMenu(fileName = "Effect", menuName = "Card Effect")]
public abstract class Effect : ScriptableObject
{
    public int value;
    public EffectTargetType targetType;

    public abstract void Excute(CharacterBase from, CharacterBase target);
}
