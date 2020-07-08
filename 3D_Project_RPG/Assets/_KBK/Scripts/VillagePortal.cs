using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagePortal : MonoBehaviour
{
    public Transform portalPoint;
    public GameObject portalFx;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            Debug.Log("포탈이동");
            other.transform.position = portalPoint.position + new Vector3(0, 1, 0);
            other.transform.rotation = portalPoint.rotation;
            GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>().enabled = false;
            
            Invoke("PortalMove", 0.1f);
        }
    }

    void PortalMove()
    {
        GameObject fx = Instantiate(portalFx);
        fx.transform.position = portalPoint.position;
        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>().enabled = true;
    }

}
