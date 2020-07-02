using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : EnemyFSM
{
    //public GameObject quiver;
    public GameObject magicBallPrefab;
    int maxArrow = 8;
    List<GameObject> magicBall;

    GameObject castFx1, castFx2;
    
    protected override void Start()
    {
        enemyId = 2;

        maxHp = 10f;
         
        attTime = 2.5f;

        attackRange = 8f;

        magicBall = new List<GameObject>();
        castFx1 = Resources.Load("Fx/MageCast") as GameObject;
        castFx2 = Resources.Load("Fx/MageCastSphere") as GameObject;

        base.Start();

        for (int i = 0; i < maxArrow; i++)
        {
            GameObject ar = Instantiate(magicBallPrefab, transform);
            ar.SetActive(false);
            magicBall.Add(ar);
        }
    }

    protected override void Move()
    {
        // - 공격 범위 2미터
        if (Vector3.Distance(spawnPos, transform.position) > moveRange)
        {
            // - 상태 변경
            state = EnemyState.Return;
        }
        //moveRange를 벗어나지 않고 공격범위에 있지도 않음
        else if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            //플레이어 추격
            //이동방법 (벡터의 뺄셈)
            Vector3 dir = (player.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            //cc.SimpleMove(dir * moveSpeed);
        }
        else //공격범위 안에 들어옴
        {
            // - 상태 변경
            state = EnemyState.Attack;
            // - 상태 전환 출력
            //anim.SetTrigger("Attack");
        }
    }

    protected override void Attack()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.forward = dir;
        // - 공격 범위 1미터
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                //차징 FX
                GameObject fx1 = Instantiate(castFx1, transform);
                fx1.transform.position = transform.position;
                GameObject fx2 = Instantiate(castFx2, transform);
                fx2.transform.position = transform.position + new Vector3(0, 1, 0);

                Destroy(fx1, 1f);
                Destroy(fx2, 1f);

                anim.SetTrigger("Attack");

                Invoke("MagicCast", 0.9f);

                //타이머 초기화
                timer = 0f;
            }
        }
        else //현재 상태를 Move로 변경(재추격)
        {
            // - 상태 변경
            state = EnemyState.Move;
            timer = 0f;
        }
    }

    void MagicCast()
    {
        for (int i = 0; i < magicBall.Count; i++)
        {
            if (!magicBall[i].activeSelf)
            {
                magicBall[i].transform.position = transform.position + (transform.forward + transform.up);
                magicBall[i].transform.rotation = transform.rotation;
                magicBall[i].GetComponent<Rigidbody>().AddForce(transform.up * 1000f);
                magicBall[i].SetActive(true);
                break;
            }
        }
    }
    
}
