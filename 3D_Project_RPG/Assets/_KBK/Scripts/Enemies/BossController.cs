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
    public GameObject projPrefab;

    public Transform[] movePosition;

    Animator anim;
    Animation appearAnim;
    LineRenderer lr;

    public GameObject fromPortal;
    public GameObject toPortal;
    public int enemyId = 3;
    int p2MaxBall = 5;

    WaitForSeconds wfsDelay = new WaitForSeconds(0.2f);
    WaitForSeconds wfsAtt1 = new WaitForSeconds(2f);
    WaitForSeconds wfsAtt2 = new WaitForSeconds(9f);
    WaitForSeconds wfsAtt3 = new WaitForSeconds(5f);

    public Transform beamStartPos;

    public float currHp;
    public float maxHp = 80;

    bool isAppear;

    public BossBulletPool bullets;

    MaterialPropertyBlock mpb;
    public SkinnedMeshRenderer smr;

    void Start()
    {
        fromPortal.SetActive(false);
        currHp = maxHp;
        anim = GetComponentInChildren<Animator>();
        appearAnim = GetComponentInChildren<Animation>();

        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.4f;
        lr.enabled = false;
        lr.SetColors(Color.red, Color.yellow);

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
                if (currHp < 40f)
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

    IEnumerator Attack01()
    {
        while (currHp > 0)
        {
            int a = Random.Range(0, 2);
            anim.SetTrigger("Attack2");
            float angle = (50f / p2MaxBall);
            for (int i = 0; i < p2MaxBall; i++)
            {
                GameObject bullet = bullets.PickUpBullet();
                bullet.transform.position = transform.position + transform.up + transform.forward;

                bullet.transform.eulerAngles = transform.eulerAngles;
                bullet.transform.Rotate(0f, angle * i - 20f, 0f);

                bullet.SetActive(true);

                yield return wfsDelay;
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
        while (currHp > 0)
        {
            Vector3[] range = new Vector3[3];

            anim.SetTrigger("Attack3");
            for (int i = 0; i < range.Length; i++)
            {
                range[i] = transform.position + new Vector3(5f + Random.insideUnitCircle.x * 8f, 5f, 3f+Random.insideUnitCircle.y * 8f);
                //range[i].y = 0.015f;
                GameObject q = Instantiate(projPrefab);
                q.transform.position = range[i];
                Destroy(q, 6f);
            }

            yield return wfsAtt2;
        }

    }

    IEnumerator Attack03()
    {
        while (currHp > 0)
        {
            anim.SetTrigger("Attack1");

            lr.enabled = true;

            yield return new WaitForSeconds(0.7f);

            lr.SetPosition(0, beamStartPos.position); //시작점Idx : 0
            lr.SetPosition(1, player.transform.position); //시작점Idx : 0

            player.GetComponent<PlayerController>().Damaged(4);

            yield return wfsAtt3;
        }
    }

    public void GetDamage(int value)
    {
        StartCoroutine(SetDamagedMaterial());
        currHp -= value;
        if (currHp <= 0)
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


    IEnumerator SetDamagedMaterial()
    {
        smr.SetPropertyBlock(GameManager.instance.mpb);
        yield return wfsDelay;
        smr.SetPropertyBlock(null);
    }
}