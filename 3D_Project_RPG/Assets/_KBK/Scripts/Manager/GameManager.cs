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
    public Dictionary<int, int> killCounter = new Dictionary<int, int>();

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

        killCounter.Add(0, 0);
        killCounter.Add(1, 0);
        killCounter.Add(2, 0);
        killCounter.Add(3, 0);
        killCounter.Add(4, 0);
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
