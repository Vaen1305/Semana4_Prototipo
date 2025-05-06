using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameEvents;
using UnityEngine;

[CreateAssetMenu(fileName = "Int Event", menuName = "Game Events/Int Event")]
public class GameIntEvent : ScriptableObject
{
    private List<GameIntEventListener> listeners;

    private void OnEnable()
    {
        listeners = new List<GameIntEventListener>();
    }

    private void OnDisable()
    {
        listeners.Clear();
    }

    public void SetUp()
    {
        listeners = new List<GameIntEventListener>();
    }

    public void Raise(int value)
    {
        foreach (GameIntEventListener sub in listeners)
        {
            sub.OnEventRaised(value);
        }
    }

    public void Register(GameIntEventListener newListener)
    {
        if (listeners.Contains(newListener)) return;

        listeners.Add(newListener);
    }

    public void Unregister(GameIntEventListener newListener)
    {
        if (!listeners.Contains(newListener)) return;

        listeners.Remove(newListener);
    }
}