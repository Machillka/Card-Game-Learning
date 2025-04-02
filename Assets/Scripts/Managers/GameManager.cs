using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

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
    }
}
