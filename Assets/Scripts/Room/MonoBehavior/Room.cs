using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Room : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int column;
    public int line;
    public float scaleFactor = 1.1f;
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

    /// <summary>
    /// 鼠标移动到房间上的悬浮动画
    /// </summary>
    public void OnHoverEnterAnimation()
    {
        transform.DOScale(Vector3.one * scaleFactor, 0.2f);
    }

    /// <summary>
    /// 鼠标移出房间的悬浮动画
    /// </summary>
    public void OnHoverExitAnimation()
    {
        if (roomState == RoomState.Locked)
        {
            return;
        }

        transform.DOScale(Vector3.one, 0.2f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovering");
        OnHoverEnterAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Hover Exit");
        OnHoverExitAnimation();
    }

}