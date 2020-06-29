using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    GameObject portal;
    public GameObject portalFx;
    public GameObject openFx;
    public GameObject portalOffFx;
    public GameObject portalOffFx2;

    float speed = 10f;

    bool portalClosed;
    
    void Start()
    {
        portal = Instantiate(portalFx, transform.position, Quaternion.identity, transform); ;
        portal.transform.localScale = Vector3.zero;


        StartCoroutine(OpenPortal());
        StartCoroutine(ClosePortal());
    }

    private void Update()
    {
        if(portalClosed)
        {
            StartCoroutine(portalCloseFx());
        }
    }

    IEnumerator portalCloseFx()
    {
        yield return null;

        GameObject fx = Instantiate(portalOffFx);
        GameObject fx2 = Instantiate(portalOffFx2);
        fx.transform.position = transform.position;
        fx2.transform.position = transform.position;

        Debug.Log("포탈 닫는 이펙트");
        Destroy(fx, 0.5f);
        Destroy(fx2, 0.5f);

        this.gameObject.SetActive(false);
    }

    IEnumerator OpenPortal()
    {
        Debug.Log("오픈 포탈");
        
        while (portal.transform.localScale.z < 1)
        {
            portal.transform.localScale = Vector3.Lerp(portal.transform.localScale, Vector3.one, Time.deltaTime * speed);

            yield return null;
        }

        GameObject fx = Instantiate(openFx);
        fx.transform.position = transform.position;
        Destroy(fx, 1f);
    }

    IEnumerator ClosePortal()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("클로즈 포탈");

        while (portal.transform.localScale.z >= 0)
        {
            portal.transform.localScale -= Vector3.one * Time.deltaTime * speed;

            yield return null;
        }

        portalClosed = true;
    }
}
