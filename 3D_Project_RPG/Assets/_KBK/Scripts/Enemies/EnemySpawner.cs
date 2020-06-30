using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnRadius = 5;
    public GameObject archer;
    public GameObject warrior;
    public GameObject mage;

    GameObject boxPrefab;

    BoxCollider bc;

    bool isEnemyAlive = true;

    //List<Transform> enemySpawnPoint;

    Transform[] spawnPoint;
    List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        bc = GetComponent<BoxCollider>();
        spawnPoint = GetComponentsInChildren<Transform>();

        for (int i = 0; i < 5; i++)
        {
            if(i == 0)
            {
                GameObject enemy = Instantiate(mage, spawnPoint[i + 1].position, Quaternion.identity, GameManager.instance.enemyList.transform);
                enemy.SetActive(false);
                enemyList.Add(enemy);
            }
            else if(i > 0 && i < 3)
            {
                GameObject enemy = Instantiate(archer, spawnPoint[i + 1].position, Quaternion.identity, GameManager.instance.enemyList.transform);
                enemy.SetActive(false);
                enemyList.Add(enemy);
            }
            else
            {
                GameObject enemy = Instantiate(warrior, spawnPoint[i + 1].position, Quaternion.identity, GameManager.instance.enemyList.transform);
                enemy.SetActive(false);
                enemyList.Add(enemy);
            }

        }

        boxPrefab = Resources.Load("Model/Chest") as GameObject;
    }

    private void Update()
    {
        isEnemyAlive = EnemyCheck();

        if(!isEnemyAlive)
        {
            Debug.Log("다사라졌다");
            Instantiate(boxPrefab, transform.position, Quaternion.identity);

            GameManager.instance.appearCheck++;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].SetActive(true);
            }
            bc.enabled = false;
        }
        
    }

    public bool EnemyCheck()
    {
        int c = 0;

        for (int i = 0; i < enemyList.Count; i++)
        {
            c += (enemyList[i] == null) ? 0 : 1;
        }

        return c > 0 ? true : false;
    }
    
}
