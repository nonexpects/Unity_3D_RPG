using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    public float speed = 5f;
    float gravity = -10f;
    float velocityY;
    float jumpPower = 3f;
    int jumpCount;
    Vector3 dir;

    public VariableJoystick joystick;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0)
        {
            h = joystick.Horizontal;
            v = joystick.Vertical;
        }

        //dir.Set(h, 0, v);
        //dir.Normalize();
        //dir = Camera.main.transform.TransformDirection(dir);
        //transform.Translate(dir * speed * Time.deltaTime);

        Vector3 dir = Vector3.forward * v;
        //중력
        velocityY += gravity * Time.deltaTime;
        dir.y = velocityY;

        transform.Rotate(Vector3.up * h * 90f * Time.deltaTime);
        cc.Move(transform.TransformDirection(dir) * speed * Time.deltaTime);

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0f;
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (jumpCount > 1) return;

        velocityY = jumpPower;
        jumpCount++;
    }
}
