using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureController : MonoBehaviour, IPointerDownHandler
{
    public ObjectEventSO gameWinEvent;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameWinEvent.RaiseEvent(null, this);
    }
}
