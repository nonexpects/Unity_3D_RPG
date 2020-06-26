using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public GameObject enemyList;
    
    public bool playerDead;

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

    
}
