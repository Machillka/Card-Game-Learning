using System.Collections;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;

    public int CurrentMana { get => playerMana.currentValue; set => playerMana.SetValue(value); }

    public ObjectEventSO playerTurnEnd;

    //TODO: 此处改为在 Mana 改变的事件中使用监听来发送广播
    protected override void Update()
    {
        if (CurrentMana <= 0)
        {
            StartCoroutine(playerTurnEnd.DelayRaiseEvent(null, this, 0.7f));
            // StartCoroutine(DelayPlayerTurnEnd());
        }
    }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentHP = maxHP;
    }

    public void NewTurn()
    {
        CurrentMana = maxMana;
        //TODO: 重置其他状态
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;

        if (CurrentMana <= 0)
            CurrentMana = 0;
    }

    public void NewGame()
    {
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = 0;
        NewTurn();
    }

    // IEnumerator DelayPlayerTurnEnd()
    // {
    //     yield return new WaitForSeconds(1.5f);
    //     playerTurnEnd.RaiseEvent(null, this);
    // }
}
