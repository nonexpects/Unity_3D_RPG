using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    float currentTime;
    float maxTime = 1.5f;

    Text attText;

    Animator anim;

    int attStack;
    bool isAttacking;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        attText = GameObject.Find("AttStack").GetComponent<Text>();        
    }

    private void Update()
    {
        if (isAttacking)
        {
            attText.color = Color.red;
        }
        else
        {
            attText.color = Color.green;
        }
        attText.text = "Att : " + attStack.ToString();
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            currentTime = 0f;
            attStack = 0;
            isAttacking = true;
            anim.SetTrigger("Attack");
            anim.SetInteger("AttackCombo", attStack);
            StartCoroutine(Attacking());
        }
        else
        {
            attStack++;
            anim.SetInteger("AttackCombo", attStack);

            if (attStack > 4)
            {
                isAttacking = false;
            }
        }
        
        Debug.Log("Attack Stack : " + attStack);
    }

    IEnumerator Attacking()
    {
        while (isAttacking)
        {
            currentTime += Time.deltaTime;
            
            if (currentTime > maxTime)
            {
                isAttacking = false; 
            }
            yield return null;
        }
        


    }
}
