using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;

    public int CurrentMana { get => playerMana.currentValue; set => playerMana.SetValue(value); }

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
        Debug.Log($"Newgame, HP = {CurrentHP}");
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = 0;
        NewTurn();
    }
}
