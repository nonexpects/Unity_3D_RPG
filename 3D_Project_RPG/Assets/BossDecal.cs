using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDecal : MonoBehaviour
{
    float currTime;
    float maxTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currTime += Time.deltaTime;
            if(currTime > maxTime)
            {
                other.gameObject.GetComponent<PlayerController>().Damaged(1);
                currTime = 0f;
            }
        }
    }
    
}
