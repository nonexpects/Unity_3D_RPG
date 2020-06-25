using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Animator anim;

    float currHp;
    float maxHp = 20;
    float currExp;
    float maxExp = 30;

    int currLv = 1;

    public Image hpBar;
    public Image expBar;
    public Text lvText;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        currHp = maxHp;
    }

    void Update()
    {
        hpBar.fillAmount = currHp / maxHp;
        expBar.fillAmount = currExp / maxExp;
        lvText.text = "LV " + currLv;

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
        if(cc.enabled) cc.Move(transform.TransformDirection(dir) * speed * Time.deltaTime);

        //애니메이터
        anim.SetFloat("Run", Mathf.Abs(v));

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            velocityY = 0f;
            anim.ResetTrigger("Jump");
            anim.SetTrigger("JumpEnd");
            jumpCount = 0;
        }
        else
        {
            if(velocityY < 0)
            {
                anim.SetTrigger("JumpAir");
            }
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        if (jumpCount > 1) return;
        
        anim.SetTrigger("Jump");
        velocityY = jumpPower;
        jumpCount++;
    }

    public void Damaged(int value)
    {
        currHp -= value;
        if (currHp <= 0)
        {
            Die();
        }
    }

    public void getExp(int value)
    {
        currExp += value;
        if (currExp >= maxExp)
        {
            currExp = 0;
            currLv++;
        }
    }

    private void Die()
    {
        cc.enabled = false;
        StartCoroutine(DieProc());
        
        Debug.Log("주거따");
    }

    IEnumerator DieProc()
    {
        yield return null;
    }
}
