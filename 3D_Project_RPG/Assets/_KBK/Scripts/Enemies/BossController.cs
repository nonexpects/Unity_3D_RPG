using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    public static event BossAppearEvent BossCamEvent;
    public static event BossAppearEvent BossAppearance;
    public static event BossAppearEvent BossDead;

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
    WaitForSeconds wfsLaserDelay = new WaitForSeconds(0.6f);

    //레이저관련
    public Transform beamStartPos;
    bool laserOn;

    public float currHp;
    public float maxHp = 80;

    bool isAppear;

    public BossBulletPool bullets;

    MaterialPropertyBlock mpb;
    public SkinnedMeshRenderer smr;
    GameObject appearCam;
    ChromaticAberration ca;

    GameObject dieFx;
    GameObject laserhitFx;
    public GameObject laserStartFx;

    public GameObject bossChest;

    void Start()
    {
        fromPortal.SetActive(false);
        currHp = maxHp;
        anim = GetComponentInChildren<Animator>();
        appearAnim = GetComponentInChildren<Animation>();
        appearCam = transform.GetChild(1).gameObject;
        ca = appearCam.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>();
        ca.intensity.value = 0f;
        appearCam.SetActive(false);

        lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        player = GameObject.FindGameObjectWithTag("Player");
        dieFx = Resources.Load<GameObject>("Fx/Boss/BossDead");
        laserhitFx = Resources.Load<GameObject>("Fx/Boss/LightningExplosionHit");
        laserStartFx.SetActive(false);
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
        BossCamEvent();
        Invoke("ChromaticAberrationOn", 1.5f);
        appearAnim.Play();
        appearCam.SetActive(true);
        
        yield return new WaitForSeconds(4f);
        state = BossState.Phase1;

        appearCam.SetActive(false);
        //이걸로 수정확인
        Debug.Log("Phase 1");
        StartCoroutine(Attack01());
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

            StartCoroutine(Teleport());
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
        while (currHp> 0)
        {
            anim.SetTrigger("Attack1");

            laserStartFx.SetActive(true);

            Vector3 dist = (transform.position - player.transform.position).normalized;

            yield return wfsLaserDelay;

            lr.enabled = true;

            lr.SetPosition(0, laserStartFx.transform.position);

            lr.SetPosition(1, player.transform.position + transform.up);

            player.GetComponent<PlayerController>().Damaged(5);

            GameObject hitFx = Instantiate(laserhitFx, player.transform.position + transform.up, Quaternion.identity);
            
            yield return wfsLaserDelay;
            
            laserStartFx.SetActive(false);
            
            lr.enabled = false;

            Destroy(hitFx);

            yield return wfsAtt3;
            
        }
    }

    public void GetDamage(int value)
    {
        if (state == BossState.Die) return;

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
        anim.SetTrigger("Die");
        StopCoroutine(Teleport());
        Invoke("BossDeadFx", 2f);
        GameManager.instance.killCounter[enemyId]++;
        fromPortal.SetActive(true);

        Instantiate(bossChest, movePosition[5].position, Quaternion.identity);

        BossDead();
    }


    IEnumerator SetDamagedMaterial()
    {
        smr.SetPropertyBlock(GameManager.instance.mpb);
        yield return wfsDelay;
        smr.SetPropertyBlock(null);
    }

    void ChromaticAberrationOn()
    {
        while (ca.intensity.value < 1)
        {
            ca.intensity.value += Mathf.Lerp(0f, 1f, 0.5f);
        }
    }

    IEnumerator Teleport()
    { 
        int p = Random.Range(0, 5);
        transform.position = movePosition[p].position;

        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;

        yield return null;
    }

    void BossDeadFx()
    {
        GameObject fx = Instantiate(dieFx, transform.position + transform.up, Quaternion.identity);
        Destroy(fx, 2f);

        Destroy(gameObject);
    }
}