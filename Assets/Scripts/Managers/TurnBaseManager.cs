using TMPro;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    private Timer _enemyTurnDurationTimer;
    private Timer _playerTurnDelayTimer;

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

    private void Awake()
    {
        _enemyTurnDurationTimer = new Timer(2.5f, EnemyTurnEnd);
        _playerTurnDelayTimer = new Timer(0.7f, PlayerTurnBegin);
    }

    //TODO[x] 修改回合转化逻辑
    private void Update()
    {
        if (battleEnd)
        {
            return;
        }

        if (isEnemyTurn)
        {
            _enemyTurnDurationTimer.UpdateTimer(Time.deltaTime);
        }

        if (isPlayerTurn)
        {
            _playerTurnDelayTimer.UpdateTimer(Time.deltaTime);
        }

        // // 到敌人回合, 直接开始 然后经过一段时间之后结束敌人回合
        // // 此种写法, 如果还没有到时间就结束的话, 虽然会导致计时不准确, 但是依旧会继续计时, 并且到了 duration 后 reset（因为在自己的update中重复执行）
        // if (isEnemyTurn)
        // {
        //     timeCounter += Time.deltaTime;

        //     if (timeCounter >= enemyTurnDuration)
        //     {
        //         timeCounter = 0f;
        //         EnemyTurnEnd();
        //         isPlayerTurn = true;
        //     }
        // }

        // // 到了玩家回合 先进行一个 delay 然后进入玩家回合
        // if (isPlayerTurn)
        // {
        //     timeCounter += Time.deltaTime;

        //     if (timeCounter >= playerTurnDuration)
        //     {
        //         timeCounter = 0f;
        //         PlayerTurnBegin();
        //         isPlayerTurn = false;
        //     }
        // }
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
        isPlayerTurn = false;

        // 重置计时器
        _playerTurnDelayTimer.ResetTime();
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
        isPlayerTurn = true;

        // 重置计时器
        _enemyTurnDurationTimer.ResetTime();
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
                playerObj.GetComponent<PlayerAnimation>().SetSleepAnimation();
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

        // 进入新游戏重置计时器, 计时准确
        _playerTurnDelayTimer.ResetTime();
        _enemyTurnDurationTimer.ResetTime();
    }
}
