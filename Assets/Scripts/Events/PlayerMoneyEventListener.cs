using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMoneyEventListener : MonoBehaviour
{
    public PlayerMoneyEventSO _event;
    public UnityEvent<float> response;

    private void OnEnable()
    {
        _event.AddListener(this);
    }

    private void OnDisable()
    {
        _event.RemoveListener(this);
    }
    public void OnEventRaised(float balance)
    {
        response.Invoke(balance);  
    }
}
