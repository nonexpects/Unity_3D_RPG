using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    float speed = 3f;
    int att = 1;

    Rigidbody rg;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
        transform.right = rg.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Damaged(att);
            Debug.Log("마자따!");
        }
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        this.gameObject.SetActive(false);
        
    }
}
