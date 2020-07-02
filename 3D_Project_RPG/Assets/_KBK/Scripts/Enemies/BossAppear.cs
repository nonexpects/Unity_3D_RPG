using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppear : MonoBehaviour
{
    bool isAllEnemyDead;
    int count;
    public int enemyId = 3;

    EnemySpawner[] checker;

    // Start is called before the first frame update
    void Start()
    {
        checker = GetComponentsInChildren<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < checker.Length; i++)
        {
            if(checker[i].gameObject.activeSelf)
            {
                count++;
            }
        }

        if(count >= 4)
        {
            isAllEnemyDead = true;
        }

        if(isAllEnemyDead)
        {
            Debug.Log("보스 출현");
        }
    }
}
