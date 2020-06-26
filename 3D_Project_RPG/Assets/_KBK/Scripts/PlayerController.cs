using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{

    public delegate void PlayerDeadEvent();

    public static event PlayerDeadEvent OnPlayerDead;

    //캐릭터 컨트롤러 
    CharacterController cc;

    /// 이동 및 점프 관련 변수
    public float speed = 5f;
    float gravity = -10f;
    float velocityY;

    float jumpPower = 3f;
    int jumpCount;
    /// 이동관련 변수
    
    //조이스틱
    public VariableJoystick joystick;

    //애니메이터 
    Animator anim;

    //HP
    float currHp;
    public float PlayerHp
    {
        get { return currHp; }
        set
        {
            currHp += value;
            if (currHp > maxHp)
                currHp = maxHp;
        }
    }
    float maxHp { get; set; }
    public float PlayerMaxHp { get; }

    //EXP
    float currExp { get; set; }
    public float PlayerExp
    {
        get { return currExp; }
        set
        {
            currExp += value;
            if (currExp > maxExp)
                currExp = maxExp;
        }
    }
    float maxExp = 30;
    public float PlayerMaxExp { get; }
    
    // 레벨
    int currLv;
    public int PlayerLv { get { return currLv; } }

    // 공격
    Text attText;
    int attStack;
    public bool isAttacking;
    float currAttTime;
    float maxAttTime = 1.5f;


    void Start()
    {
        maxHp = 20;
        maxExp = 30;
        currLv = 1;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        currHp = maxHp;

        //어택스택 확인용  Text UI
        attText = GameObject.Find("AttStack").GetComponent<Text>();

        OnPlayerDead += Die;
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

        CheckAttStack();
    }

    private void CheckAttStack()
    {
        if (isAttacking)
        {
            attText.color = Color.red;
        }
        else
        {
            attText.color = Color.green;
        }
        attText.text = "Att : " + attStack.ToString();
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
        if (currHp <= 0 && cc.enabled)
        {
            OnPlayerDead();

            GameManager.instance.playerDead = true;
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
        anim.SetTrigger("Die");
        //StartCoroutine(DieProc());
        
        Debug.Log("주거따");
    }

    //IEnumerator DieProc()
    //{
    //
    //    yield return null;
    //}

    public void Attack()
    {
        if (!isAttacking)
        {
            currAttTime = 0f;
            attStack = 0;
            isAttacking = true;
            anim.SetTrigger("Attack");
            anim.SetInteger("AttackCombo", attStack);
            StartCoroutine(Attacking());
        }
        else
        {
            attStack++;
            anim.SetInteger("AttackCombo", attStack);

            if (attStack > 4)
            {
                isAttacking = false;
            }
        }

        Debug.Log("Attack Stack : " + attStack);
    }

    IEnumerator Attacking()
    {
        while (isAttacking)
        {
            currAttTime += Time.deltaTime;

            if (currAttTime > maxAttTime)
            {
                isAttacking = false;
            }
            yield return null;
        }
    }
}
