using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game Events/New Event")]
public class PlayerMoneyEventSO : ScriptableObject
{
    private readonly List<PlayerMoneyEventListener> eventListeners = new List<PlayerMoneyEventListener>();

    public void Raise(float balacne)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(balacne);
        }
    }

    public void AddListener(PlayerMoneyEventListener l)
    {
        if (!eventListeners.Contains(l))
            eventListeners.Add(l);
    }

    public void RemoveListener(PlayerMoneyEventListener l)
    {
        if (eventListeners.Contains(l))
            eventListeners.Remove(l);
    }
}
