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
    BossController bossCtrl;
    public MaterialPropertyBlock mpb;

    public FadeScript fade;

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

        mpb = new MaterialPropertyBlock();
    }
    
    void Start()
    {
        bossChecker = GameObject.FindGameObjectsWithTag("Respawn");
        boss = GameObject.FindGameObjectWithTag("Boss");
        bossCtrl = boss.GetComponent<BossController>();
        bossCtrl.state = BossState.Hide;
        boss.SetActive(false);
        mpb.SetColor("_Color", Color.red);

        fade.FadeOut(4f);
    }
    
    void Update()
    {
        if(appearCheck == bossChecker.Length)
        {
            Debug.Log("Boss Appear!!");
            appearCheck = 0;

            bossCtrl.state = BossState.Appear;
            //boss.GetComponent<BossController>().Appear();
            boss.SetActive(true);
        }
    }
    
}
