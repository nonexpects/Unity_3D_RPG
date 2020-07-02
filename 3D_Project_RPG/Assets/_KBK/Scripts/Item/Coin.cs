using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    GameObject earnFx;

    private void Start()
    {
        earnFx = Resources.Load("Fx/Sparkle") as GameObject;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            other.gameObject.GetComponent<PlayerController>().GetGold(10);
            GameObject fx = Instantiate(earnFx);
            fx.transform.position = transform.position;

            Destroy(fx, .5f);
            Destroy(this.gameObject);
        }
    }
}
