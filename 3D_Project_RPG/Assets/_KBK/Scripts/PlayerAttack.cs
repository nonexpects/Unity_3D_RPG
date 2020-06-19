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

        if(Input.GetKeyDown(KeyCode.V))
        {
            attStack++;
        }
    }

    public void Attack()
    {
        if(!isAttacking)
        {
            StartCoroutine(Attacking());
            
        }
    }

    IEnumerator Attacking()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return null;
    }
}
