using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    public GameObject playerObj;

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

    public void OnRoomLoadEvent(object obj)
    {
        Room currentRoom = obj as Room;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.Shop:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                break;
        }
    }

    public void StopTurnBaseManager(object obj)
    {
        battleEnd = true;
        playerObj.SetActive(false);
    }

    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
    }
}
