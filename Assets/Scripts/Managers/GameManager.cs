using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> aliveEnemyList = new List<Enemy>();

    [Header("事件广播")]
    public ObjectEventSO gameOverEvent;
    public ObjectEventSO gameWinEvent;

    /// <summary>
    /// 更新地图布局数据的事件监听函数
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayoutData(object value)
    {
        if (value is not Vector2Int roomVector)
        {
            return;
        }

        if (mapLayout.mapRoomDataList.Count == 0)
        {
            return;
        }

        var currentRoom = mapLayout.mapRoomDataList.Find(room => room.column == roomVector.x && room.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;

        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(room => room.column == roomVector.x);
        foreach (var room in sameColumnRooms)
        {
            if (room.line != roomVector.y)
            {
                room.roomState = RoomState.Locked;
            }
        }

        foreach (var linkedRoom in currentRoom.linkTo)
        {
            var room = mapLayout.mapRoomDataList.Find(r => r.column == linkedRoom.x && r.line == linkedRoom.y);
            room.roomState = RoomState.Attainable;
        }

        aliveEnemyList.Clear();
    }

    public void OnCharacterDeathEvent(object character)
    {
        if (character is Player)
        {
            // 失败
            // gameOverEvent.RaiseEvent(null, this);
            StartCoroutine(EventDelayAction(gameOverEvent, 1.5f));
        }

        if (character is Boss)
        {
            StartCoroutine(EventDelayAction(gameOverEvent, 1.5f));
        }
        else if (character is Enemy)
        {
            aliveEnemyList.Remove(character as Enemy);

            if (aliveEnemyList.Count == 0)
            {
                // Win
                // gameWinEvent.RaiseEvent(null, this);
                StartCoroutine(EventDelayAction(gameWinEvent, 1.5f));
            }
        }
    }

    public void OnRoomLoadEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO, float deltaTime)
    {
        yield return new WaitForSeconds(deltaTime);
        eventSO.RaiseEvent(null, this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
