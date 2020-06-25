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
    [HideInInspector]
    public GameObject enemyList;

    private void Awake() // => instance = this;
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        enemyList = new GameObject("EnemyList");
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    //public void AddListner(EventType eventType, PlayerEventHandler delegateFunc)
    //{
    //    if(_delegateDic.ContainsKey(eventType) == false)
    //    {
    //        _delegateDic.Add(eventType, delegateFunc);
    //    }
    //
    //    _delegateDic[eventType] += delegateFunc;
    //}
}
