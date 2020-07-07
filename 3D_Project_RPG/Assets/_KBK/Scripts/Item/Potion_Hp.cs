using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Hp : MonoBehaviour
{
    GameObject earnFx;
    GameObject earnFx2;

    void Start()
    {
        earnFx = Resources.Load("Fx/HpPotion") as GameObject;
        earnFx2 = Resources.Load("Fx/PowerupRed") as GameObject;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            other.gameObject.GetComponent<PlayerController>().GetHp(5);

            GameObject fx = Instantiate(earnFx);
            GameObject fx2 = Instantiate(earnFx2);
            fx.transform.position = other.transform.position;
            fx2.transform.position = transform.position;

            Destroy(fx, 1.5f);
            Destroy(fx2, 1f);

            Destroy(this.gameObject);
        }
    }
}
