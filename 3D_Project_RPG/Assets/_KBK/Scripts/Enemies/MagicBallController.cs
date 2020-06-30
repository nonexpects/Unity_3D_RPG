using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallController : MonoBehaviour
{
    float speed = 8f;
    int att = 3;

    Vector3 height = new Vector3(0, 0.5f, 0);

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mov = Vector3.Lerp(transform.position, player.transform.position + height, speed * Time.deltaTime);
        transform.position = mov;

        //transform.Translate(transform.forward );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<PlayerController>().Damaged(att);
        }
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        //rg.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
