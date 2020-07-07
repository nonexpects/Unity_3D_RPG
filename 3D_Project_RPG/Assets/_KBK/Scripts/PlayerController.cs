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

    //레이캐스트
    RaycastHit hit;

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

    GameObject damageText;
    GameObject canvas;

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
    public float PlayerMaxHp { get { return maxHp; } }

    //EXP
    float currExp { get; set; }
    public float PlayerExp
    {
        get { return currExp; }
        set
        {
            currExp += value;
            if (currExp > maxExp)
            {
                currExp -= maxExp;
                currLv++;
            }
        }
    }
    float maxExp = 30;
    public float PlayerMaxExp { get { return maxExp; } }

    //GOLD
    int gold { get; set; }
    public int PlayerGold
    {
        get { return gold; }
        set
        {
            gold += value;
        }
    }

    // 레벨
    int currLv;
    public int PlayerLv { get { return currLv; } }

    // 공격
    Text attText;
    public int attStack;
    public bool isAttacking;
    float currAttTime;
    float maxAttTime = 1.5f;


    void Start()
    {
        maxHp = 50;
        maxExp = 30;
        currLv = 1;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        damageText = Resources.Load<GameObject>("DamageText");
        canvas = GameObject.Find("DamageTextPos");

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

        //CheckRay();

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
        GameObject damage = Instantiate(damageText, canvas.transform);
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(Random.insideUnitCircle.x * 20, Random.insideUnitCircle.y * 20, 0);
        damage.transform.position = pos;
        damage.GetComponent<Text>().color = Color.red;
        damage.GetComponent<Text>().text = "- " + value;
        Destroy(damage, 0.5f);

        currHp -= value;
        if (currHp <= 0 && cc.enabled)
        {
            OnPlayerDead();

            GameManager.instance.playerDead = true;
        }
    }

    public void GetExp(int value)
    {
        PlayerExp = value;
    }

    public void GetHp(int value)
    {
        PlayerHp = value;
    }

    public void GetGold(int value)
    {
        PlayerGold = value;
    }

    private void Die()
    {
        cc.enabled = false;
        anim.SetTrigger("Die");
    }
    

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
                attStack = 4;
            }
        }
        
    }

    IEnumerator Attacking()
    {
        while (isAttacking)
        {
            currAttTime += Time.deltaTime;

            if (currAttTime > maxAttTime)
            {
                isAttacking = false;
                attStack = 0;
            }
            yield return null;
        }
    }
}
