using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyActionDataSO", menuName = "Enemy/EnemyActionDataSO")]
public class EnemyActionDataSO : ScriptableObject
{
    public List<EnemyAction> actions;
}

[System.Serializable]
public struct EnemyAction
{
    public Sprite sprite;
    public Effect effect;
}