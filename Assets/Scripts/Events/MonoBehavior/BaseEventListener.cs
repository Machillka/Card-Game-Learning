using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;          // 监听的事件
    public UnityEvent<T> response;          // 事件响应

    private void OnEnable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OneventRaised;
        }
    }

    public void OnDisable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised -= OneventRaised;
        }
    }

    private void OneventRaised(T value)
    {
        response.Invoke(value);
    }
}
