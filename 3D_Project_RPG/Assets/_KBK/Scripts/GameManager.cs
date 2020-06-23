using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum EventType
    {
        Die
    }

    public static GameManager instance;
    public delegate void PlayerEventHandler();
    public static event PlayerEventHandler OnPlayerEvent;

    private void Awake() => instance = this;
    
    public static Dictionary<EventType, PlayerEventHandler> _delegateDic;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void AddListner(EventType eventType, PlayerEventHandler delegateFunc)
    {
        if(_delegateDic.ContainsKey(eventType) == false)
        {
            _delegateDic.Add(eventType, delegateFunc);
        }

        _delegateDic[eventType] += delegateFunc;
    }
}
