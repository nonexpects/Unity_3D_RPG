using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float currentTime;
    float maxTime;

    Animator anim;

    int attStack;

    bool isAttacking;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public IEnumerator timer()
    {
        currentTime = 0f;
        maxTime = 10f;
        while (!(Input.GetKeyDown(KeyCode.V) || currentTime > maxTime))
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        //if(Input.GetKeyDown(KeyCode.V))
        //{
        //    attStack++;
        //    if (attStack > 4) attStack = 0;
        //}
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(Attacking());
        }
        else
        {
            attStack++;
            if (attStack > 4) attStack = 0;
        }
    }

    IEnumerator Attacking()
    {
        isAttacking = true;
        
        anim.SetInteger("AttackCombo", attStack);
        yield return null;
    }
}
