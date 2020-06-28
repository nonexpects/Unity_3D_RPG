using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnRadius = 5;
    public GameObject archer;
    public GameObject warrior;
    public GameObject mage;

    BoxCollider bc;

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
                //GameObject enemy = Instantiate(warrior, spawnPoint[i + 1].position, Quaternion.identity, GameManager.instance.enemyList.transform);
                //enemy.SetActive(false);
                //enemyList.Add(enemy);
            }

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
}
