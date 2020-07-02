using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int enemyId = 3;

    float currHp;
    float maxHp = 100;

    bool isAppear;
    
    void Start()
    {
        currHp = maxHp;
    }
    
    void Update()
    {
        if(isAppear)
        {

        }
    }

    void Phase1()
    {

    }

    void Phase2()
    {

    }

    void Phase3()
    {

    }

    public void GetDamage(int value)
    {
        currHp -= value;
        if(currHp <= 0)
        {
            currHp = 0;
            Die();
        }
    }

    void Die()
    {
        GameManager.instance.killCounter[enemyId]++;
    }
}
