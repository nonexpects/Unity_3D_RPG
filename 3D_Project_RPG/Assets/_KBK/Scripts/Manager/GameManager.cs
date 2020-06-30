using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public GameObject enemyList;
    GameObject boss;

    public bool playerDead;
    public int appearCheck;

    GameObject[] bossChecker;

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
        bossChecker = GameObject.FindGameObjectsWithTag("Respawn");
        boss = GameObject.FindGameObjectWithTag("Boss");
        boss.SetActive(false);
    }
    
    void Update()
    {
        if(appearCheck == bossChecker.Length)
        {
            Debug.Log("Boss Appear!!");
            appearCheck = 0;
            boss.SetActive(true);
        }
    }

    
}
