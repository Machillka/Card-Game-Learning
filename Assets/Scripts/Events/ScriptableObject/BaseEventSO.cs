using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;              // 事件描述

    public UnityAction<T> OnEventRaised;    // 定义事件
    public string lastSender;               // 事件的最后一个发送者（最后触发该事件的对象）

    public void RaiseEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }

    public IEnumerator DelayRaiseEvent(T value, object sender, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Delay end!");
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }


}