using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public static class EventManager 
{
    private static Dictionary<Type, Delegate> m_events = new Dictionary<Type, Delegate>();

    public static void Add<T>(EventHandler<T> callback) where T : EventArgs
    {
        if (m_events.ContainsKey(typeof(T)))
        {
            var evs = m_events[typeof(T)].GetInvocationList().Concat(callback.GetInvocationList()).ToArray();
            m_events[typeof(T)] = ((EventHandler<T>)m_events[typeof(T)]) + callback;
        }
        else
            m_events.Add(typeof(T), callback);
    }

    public static void Remove<T>(EventHandler<T> callback) where T : EventArgs
    {
        if (m_events.ContainsKey(typeof(T)))
        {               
            m_events[typeof(T)] = ((EventHandler<T>)m_events[typeof(T)]) - callback;
            if (m_events[typeof(T)] == null)
                m_events.Remove(typeof(T));
        }       
        else
            Debug.Log("Trying to remove an event that was never added.");  
    }

    public static void Send<T>(object sender, T eventInfo) where T : EventArgs
    {
        if (m_events.ContainsKey(typeof(T)))
            m_events[typeof(T)].DynamicInvoke(sender, eventInfo);
        else
            Debug.Log("No listeners for sent event: " + eventInfo);
    }
}
