using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;

    public float distance = 10f;
    public float height = 8f;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }
    private void LateUpdate()
    {
        float currHeight;
        float wantedHeight;
    }
}
