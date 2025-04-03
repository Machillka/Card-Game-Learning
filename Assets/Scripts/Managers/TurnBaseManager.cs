using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;

    public bool battleEnd = true;

    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("事件广播")]
    public ObjectEventSO playerTurnBegin;
    public ObjectEventSO enemyTurnBegin;
    public ObjectEventSO enemyTurnEnd;

    private void Update()
    {
        if (battleEnd)
        {
            return;
        }

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;

            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0f;
                //TODO: 敌人回合结束
                EnemyTurnEnd();
                isPlayerTurn = true;
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;

            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                //TODO: 玩家回合结束 敌人回合开始
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("Game Start")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0f;
    }

    public void PlayerTurnBegin()
    {
        playerTurnBegin.RaiseEvent(null, this);
    }

    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBegin.RaiseEvent(null, this);
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEnd.RaiseEvent(null, this);
    }
}
