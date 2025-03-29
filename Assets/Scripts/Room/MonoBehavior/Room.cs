using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkTo = new List<Vector2Int>();    // 与之相连的房间坐标 (利用column和line来确定房间位置)

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// <description>
    /// 定义点击房间行为，安卓触摸屏等点击同样适用
    /// </summary>
    private void OnMouseDown()
    {
        if (roomState == RoomState.Locked)
        {
            return;
        }

        loadRoomEvent.RaiseEvent(this, this);
        // Debug.Log("Room clicked: " + roomData.roomType);
    }

    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomData, RoomState roomState)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
        this.roomState = roomState;

        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 1f),
            RoomState.Attainable => Color.white,
            _ => spriteRenderer.color
        };
    }
}
