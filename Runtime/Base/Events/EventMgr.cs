using System;
using System.Collections.Generic;
using UnityEngine;
 namespace Saber.Base
{
    public static class EventMgr 
    {


        /// <summary>
        /// 事件字典
        /// </summary>
        private static Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();
        //事件的队列
        private static Queue<QueuedEvent> eventQueue = new Queue<QueuedEvent>();

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="listener"></param>
        public static void StartListening(string eventName, Action<object> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] += listener;
            }
            else
            {
                eventDictionary.Add(eventName, listener);
            }

            // 如果队列中有待处理的事件，立即触发它们
            while (eventQueue.Count > 0 && eventQueue.Peek().EventName == eventName)
            {
                QueuedEvent queuedEvent = eventQueue.Dequeue();
                TriggerEvent(queuedEvent.EventName, queuedEvent.Arg);
            }
        }
        //停止监听
        public static void StopListening(string eventName, Action<object> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= listener;
            }
        }
        //发送事件
        public static void TriggerEvent(string eventName, object arg = null)
        {
            Action<object> thisEvent;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent(arg);
            }
            else
            {
                // 如果没有监听器注册这个事件，将它添加到队列中
                eventQueue.Enqueue(new QueuedEvent { EventName = eventName, Arg = arg });
            }
        }
    }
    public class QueuedEvent
    {
        public string EventName { get; set; }
        public object Arg { get; set; }
    }
}
