﻿using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour
{
    private static T m_instance;
  
    public static T instance
    {
        get
        {
            if (m_instance == null)
            {
                var obj = new GameObject();
                obj.AddComponent(typeof(T));               
                m_instance = obj.GetComponent<T>();
                obj.name = m_instance.GetType().Name;
                DontDestroyOnLoad(obj);
                Debug.Log("Auto creating singleton " + m_instance + ". Should you be doing this?!", m_instance as GameObject);
            }

            return m_instance;
        }
    }
    
	void Awake()
    {
        if (FindObjectsOfType(typeof(T)).Length > 1)
        {
            Debug.LogError("Instance of " + m_instance + " already exists. Destroying.", m_instance as GameObject);
            Destroy(gameObject);
        }
        else
        {
            m_instance = GetComponent<T>();
            Debug.Log("Creating singleton of type " + m_instance, gameObject);
            DontDestroyOnLoad(gameObject);
        }
	}

    public static bool Exists { get { return m_instance != null; } }
}