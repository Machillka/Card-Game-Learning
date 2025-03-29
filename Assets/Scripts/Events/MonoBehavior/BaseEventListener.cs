using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;
    public UnityEvent<T> response;

    public void OnEnble()
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
