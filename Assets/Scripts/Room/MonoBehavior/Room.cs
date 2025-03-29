using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        SetupRoom(0, 0, roomData);
    }

    /// <summary>
    /// <description>
    /// 定义点击房间行为，安卓触摸屏等点击同样适用
    /// </summary>
    private void OnMouseDown()
    {
        Debug.Log("Room clicked: " + roomData.roomType);
        loadRoomEvent.RaiseEvent(roomData, this);
    }

    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
    }
}
