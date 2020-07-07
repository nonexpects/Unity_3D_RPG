using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjector : MonoBehaviour
{
    float currTime;
    float maxTime = 0.5f;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            currTime += Time.deltaTime;
            if (currTime > maxTime)
            {
                other.gameObject.GetComponent<PlayerController>().Damaged(2);
                currTime = 0f;
            }
        }

    }
}
