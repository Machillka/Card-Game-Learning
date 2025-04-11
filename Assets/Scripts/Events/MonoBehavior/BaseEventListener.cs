using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;          // 监听的事件
    public UnityEvent<T> response;          // 响应方法 ( 可能有多个 )

    // 绑定事件响应的方法
    private void OnEnable()
    {
        // 如果事件存在
        if (eventSO != null)
        {
            // 那么在事件发生的时候, 触发响应方法 ( 绑定上去 )
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
