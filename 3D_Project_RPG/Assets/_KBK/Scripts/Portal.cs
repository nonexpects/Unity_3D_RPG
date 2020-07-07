using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject openFx;
    GameObject decal;

    void Start()
    {
        decal = Resources.Load<GameObject>("Fx/ExplosionDecalBlue");
        StartCoroutine(OpenPortal());
    }
    
    IEnumerator OpenPortal()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject fx = Instantiate(openFx);
        GameObject decalfx = Instantiate(decal);
        fx.transform.position = transform.position;
        decalfx.transform.position = transform.position;
        Destroy(fx, 2f);
        Destroy(decalfx, 5f);

        Destroy(this.gameObject);
    }
    
}
