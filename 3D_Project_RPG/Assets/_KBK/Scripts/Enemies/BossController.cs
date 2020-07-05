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
    public BossState state;

    GameObject player;
    public GameObject quadPrefab;
    public GameObject hpBarPrefab;
    Projector pj;

    Animator anim;
    Animation appearAnim;
    
    public GameObject fromPortal;
    public GameObject toPortal;
    public int enemyId = 3;
    int p2MaxBall = 5;

    public GameObject ballPrefab;
    List<GameObject> balls = new List<GameObject>();

    WaitForSeconds p1 = new WaitForSeconds(0.2f);
    WaitForSeconds p2 = new WaitForSeconds(2f);
    WaitForSeconds p3 = new WaitForSeconds(7f);
    WaitForSeconds p4 = new WaitForSeconds(11f);

    public float currHp;
    public float maxHp = 80;
    
    bool isAppear;
    
    void Start()
    {
        //fromPortal.SetActive(false);
        currHp = maxHp;
        anim = GetComponentInChildren<Animator>();
        appearAnim = GetComponentInChildren<Animation>();
        pj = GetComponent<Projector>();
        pj.enabled = false;

        //오브젝트 풀링
        for (int i = 0; i < 30; i++)
        {
            GameObject ball = Instantiate(ballPrefab, transform);
            ball.SetActive(false);
            balls.Add(ball);
        }

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
    }

    IEnumerator Attack01 ()
    {
        while(currHp > 0)
        {
            int a = Random.Range(0, 2);

            for (int i = 0; i < p2MaxBall; i++)
            {
                GameObject b = PickUpBullet();

                float angle = 360f - (50f / p2MaxBall);//360f - (50f / p2MaxBall);

                b.transform.position = transform.position + transform.up + transform.forward;
                //랜덤으로 방향 바꾸기
                if (a == 0)
                    b.transform.localEulerAngles = new Vector3(0, 360f - (i * angle - 110f), 0); //360f - (i * angle - 110f)
                else
                    b.transform.localEulerAngles = new Vector3(0, i * angle - 25f, 0); //i * angle - 25f

                b.SetActive(true);

                yield return p1;
            }

            yield return p2;
        }

    }

    //장판
    IEnumerator Attack02()
    {
        while(currHp > 0)
        {
            Vector3[] range = new Vector3[3];

            for (int i = 0; i < range.Length; i++)
            {
                range[i] = transform.position + new Vector3(6f + Random.insideUnitCircle.x * 5f, 0, Random.insideUnitCircle.y * 5f);
                range[i].y = 0.015f;
                GameObject q = Instantiate(quadPrefab);
                q.transform.position = range[i];
                Destroy(q, 4f);
            }

            pj.enabled = true;
            pj.transform.rotation = transform.rotation;

            yield return p3;
        }
        
    }

    IEnumerator Attack03()
    {
        yield return p4;
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

    GameObject PickUpBullet()
    {
        foreach (GameObject item in balls)
        {
            if (!item.activeSelf)
                return item;
        }

        return null;
    }

}
