using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnRadius = 5;
    public GameObject archer;
    public GameObject warrior;

    Vector3[] spawnPoint = new Vector3[5];

    BoxCollider bc;

    //List<Transform> enemySpawnPoint;

    void Start()
    {
        bc = GetComponent<BoxCollider>();

        for (int i = 0; i < 5; i++)
        {
            spawnPoint[i] = (Random.insideUnitSphere * spawnRadius) + transform.position;
            spawnPoint[i].y = 0;
        }
        //spawnPosition =  spawnPoint;
        //spawnPosition.y = 1;
        
        //GameObject.Find("EnemySpawn").GetComponentsInChildren<Transform>(enemySpawnPoint);
        
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(spawnPoint, spawnRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            for (int i = 0; i < 2; i++)
            {
                Instantiate(archer, spawnPoint[i], Quaternion.identity);
            }
            for (int i = 0; i < 3; i++)
            {
                Instantiate(warrior, spawnPoint[i], Quaternion.identity);
            }

            bc.enabled = false;
        }
        
    }
}
