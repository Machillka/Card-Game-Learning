using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public void OnLoadRoomEvent(object data)
    {
        Debug.Log("OnLoadRoomEvent");
        if (data is RoomDataSO)
        {
            RoomDataSO currentRoom = (RoomDataSO)data;
            Debug.Log("Loading room: " + currentRoom.roomType);
        }
    }
}
