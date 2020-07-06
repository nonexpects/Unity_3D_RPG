using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Hide,
    Appear,
    Phase1,
    Phase2,
    Phase3,
    Die
}

public class BossController : MonoBehaviour
{
    public delegate void BossAppearEvent();

    public static event BossAppearEvent BossAppearance;

    public BossState state;

    GameObject player;
    public GameObject quadPrefab;
    Projector pj;

    public Transform[] movePosition;

    Animator anim;
    Animation appearAnim;
    LineRenderer lr;

    public GameObject fromPortal;
    public GameObject toPortal;
    public int enemyId = 3;
    int p2MaxBall = 5;

    WaitForSeconds wfsAttDelay = new WaitForSeconds(0.2f);
    WaitForSeconds wfsAtt1 = new WaitForSeconds(2f);
    WaitForSeconds wfsAtt2 = new WaitForSeconds(9f);
    WaitForSeconds wfsAtt3 = new WaitForSeconds(5f);

    public Transform beamStartPos;

    public float currHp;
    public float maxHp = 80;
    
    bool isAppear;

    public BossBulletPool bullets;
    
    void Start()
    {
        //fromPortal.SetActive(false);
        currHp = maxHp;
        anim = GetComponentInChildren<Animator>();
        appearAnim = GetComponentInChildren<Animation>();

        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.4f;
        lr.enabled = false;
        lr.SetColors(Color.red, Color.yellow);

        pj = GetComponent<Projector>();
        pj.enabled = false;
        
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    
    void Update()
    {
        switch (state)
        {
            case BossState.Appear:
                Appear();
                break;
            case BossState.Phase1:
                if(currHp < 40f)
                {
                    Debug.Log("Phase 2");
                    state = BossState.Phase2;
                    StartCoroutine(Attack02());
                }
                break;
            case BossState.Phase2:
                if (currHp < 20f)
                {
                    Debug.Log("Phase 3");
                    state = BossState.Phase3;
                    StartCoroutine(Attack03());
                }
                break;
        }
    }

    public void Appear()
    {
        if (isAppear) return;

        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;

        isAppear = true;
        StartCoroutine(AppearProc());
    }

    IEnumerator AppearProc()
    {
        appearAnim.Play();
        
        yield return new WaitForSeconds(1f);
        state = BossState.Phase1;
        //이걸로 수정확인
        Debug.Log("Phase 1");
        StartCoroutine(Attack02());
        BossAppearance();
    }

    IEnumerator Attack01 ()
    {
        while(currHp > 0)
        {
            int a = Random.Range(0, 2);
            anim.SetTrigger("Attack2");
            float angle = (50f / p2MaxBall);//360f - (50f / p2MaxBall);
            for (int i = 0; i < p2MaxBall; i++)
            {

                //Vector3 dr = (transform.forward - transform.right).normalized;

                //if(transform.rotation.y > 0f || transform.rotation.y < 200f)
                //{
                //    Debug.Log("y가 0이상");
                //    b.transform.eulerAngles = new Vector3(0, 360f - (i * angle), 0); //360f - (i * angle - 110f)
                //}
                //else
                //{
                //    //랜덤으로 방향 바꾸기
                //    if (a == 0)
                //    {
                //        Debug.Log("a=0");
                //        b.transform.localEulerAngles = new Vector3(0, 360f - (i * angle), 0); //360f - (i * angle - 110f)
                //    }
                //    else
                //    {
                //        Debug.Log("a=1");
                //        b.transform.eulerAngles = new Vector3(0, i * angle, 0); //i * angle - 25f
                //    }
                //}

                //b.transform.eulerAngles = transform.eulerAngles + new Vector3(0, (i * angle), 0); //360f - (i * angle - 110f)

                //if (transform.rotation.y > 0f && transform.rotation.y < 0.5f)
                //{
                //    Debug.Log("a > 0");
                //    b.transform.localRotation = Quaternion.Euler(0, transform.rotation.y - (i * angle) , 0) * Quaternion.Euler(transform.TransformDirection((transform.forward - transform.right).normalized));
                //    //transform.eulerAngles = transform.forward - (i * angle);
                //    //b.transform.localRotation = Quaternion.Euler(0, transform.+ ( i * angle) - 20f, 0);
                //}
                //else
                //{
                //    Debug.Log("a < 0");
                //    Vector3 bDir = transform.eulerAngles + new Vector3(0, (i * angle) + 20f, 0);
                //    b.transform.localRotation = Quaternion.Euler(0, bDir.y, 0) * Quaternion.Euler(transform.TransformDirection((transform.forward - transform.right).normalized));
                //    //b.transform.localRotation = Quaternion.Euler(0, transform.rotation.y - (i * angle), 0) * Quaternion.Euler(transform.forward);
                //    //Debug.Log("a < 0");
                //    //b.transform.localRotation = Quaternion.Euler(0, (1 - transform.rotation.y) + (i * angle), 0);
                //}
                //b.transform.eulerAngles = Quaternion.Euler(0, (i * angle), 0 ) * transform.TransformDirection((transform.forward - transform.right).normalized); //
                //b.transform.eulerAngles = transform.eulerAngles + new Vector3(0, i * angle + 10f, 0); //i * angle - 25f
                
                GameObject bullet = bullets.PickUpBullet();
                bullet.transform.position = transform.position + transform.up + transform.forward;
                //bullet.transform.forward = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (i * angle), transform.eulerAngles.z);
                bullet.transform.eulerAngles = transform.eulerAngles;
                bullet.transform.Rotate(0f, angle * i - 20f, 0f);
                //if (a == 0)
                //{
                //    bullet.transform.localEulerAngles = new Vector3(0, 360f - (i * angle), 0); //360f - (i * angle - 110f)
                //}
                //else
                //{
                //    bullet.transform.eulerAngles = new Vector3(0, i * angle, 0); //i * angle - 25f
                //}

                bullet.SetActive(true);

                yield return wfsAttDelay;
            }
            
            yield return wfsAtt1;
            
            int p = Random.Range(0, 5);
            transform.position = movePosition[p].position;

            Vector3 dir = (player.transform.position - transform.position).normalized;
            dir.y = 0;
            transform.forward = dir;
        }

    }

    //장판
    IEnumerator Attack02()
    {
        while(currHp > 0)
        {
            Vector3[] range = new Vector3[3];

            anim.SetTrigger("Attack3");
            for (int i = 0; i < range.Length; i++)
            {
                range[i] = transform.position + new Vector3(Random.insideUnitCircle.x * 8f, 0, Random.insideUnitCircle.y * 8f);
                range[i].y = 0.015f;
                GameObject q = Instantiate(quadPrefab);
                q.transform.position = range[i];
                Destroy(q, 6f);
            }

            pj.enabled = true;
            pj.transform.rotation = transform.rotation;

            yield return wfsAtt2;
        }
        
    }

    IEnumerator Attack03()
    {
        while(currHp > 0)
        {
            anim.SetTrigger("Attack1");

            lr.enabled = true;

            yield return new WaitForSeconds(0.7f);

            lr.SetPosition(0, beamStartPos.position); //시작점Idx : 0
            lr.SetPosition(1, player.transform.position); //시작점Idx : 0

            yield return wfsAtt3;
        }
    }

    public void GetDamage(int value)
    {
        currHp -= value;
        if(currHp <= 0)
        {
            currHp = 0;
            state = BossState.Die;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Dead");
        GameManager.instance.killCounter[enemyId]++;
        fromPortal.SetActive(true);
    }

    
}
