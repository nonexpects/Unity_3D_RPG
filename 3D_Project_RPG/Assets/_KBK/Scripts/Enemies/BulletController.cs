using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    float speed = 10f;
    int att = 2;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Damaged(att);
        }
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        //rg.velocity = Vector3.zero;
        gameObject.SetActive(false);
        
    }
}
