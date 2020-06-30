using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    float speed = 15f;
    int att = 1;

    Rigidbody rg;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        transform.right += rg.velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<PlayerController>().Damaged(att);
        }
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rg.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
