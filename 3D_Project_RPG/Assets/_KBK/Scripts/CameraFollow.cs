using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;

    public float distance = 5f;
    public float height = 5f;

    float camSpeed = 10f;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }
    private void LateUpdate()
    {

        
        float currHeight = transform.position.y;
        float targetHeight = target.position.y + height;

        float currAngle = transform.eulerAngles.y;
        float targetAngle = target.eulerAngles.y;

        float wantedHeight = Mathf.Lerp(currHeight, targetHeight, camSpeed * Time.deltaTime);
        float wantedAngle = Mathf.LerpAngle(currAngle, targetAngle, camSpeed * Time.deltaTime);

        Quaternion rotateAngle = Quaternion.Euler(0, wantedAngle, 0);

        transform.position = target.position;
        transform.position -= rotateAngle * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, wantedHeight, transform.position.z);

        transform.LookAt(target);
    }
}
