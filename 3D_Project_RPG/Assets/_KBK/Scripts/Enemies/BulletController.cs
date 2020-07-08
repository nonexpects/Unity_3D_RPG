using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    GameObject hitFx;

    float speed = 10f;
    int att = 2;

    float currTime;
    float maxTime = 4f;

    Rigidbody rg;

    private void Start()
    {
        rg = GetComponent<Rigidbody>();
        hitFx = Resources.Load<GameObject>("Fx/Boss/LightningMuzzlePink 1");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);
        //rg.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
        
        currTime += Time.deltaTime;
        if(currTime > maxTime)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            currTime = 0f;
            //rg.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Damaged(att);
            GameObject fx = Instantiate(hitFx, other.transform.position + other.transform.up, Quaternion.identity);
            Destroy(fx, 1f);

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            //rg.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
