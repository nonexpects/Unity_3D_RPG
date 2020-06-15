using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    Vector3 dir;
    void Start()
    {
        
    }
    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        dir.Set(h, 0, v);
        dir.Normalize();
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
