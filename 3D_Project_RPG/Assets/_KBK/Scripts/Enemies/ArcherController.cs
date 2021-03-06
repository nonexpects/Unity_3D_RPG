﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyFSM
{
    //public GameObject quiver;
    public GameObject arrowPrefab;
    int maxArrow = 5;
    List<GameObject> arrows;

    protected override void Start()
    {
        enemyId = 1;
        maxHp = 10f;
        att = 2;
        attTime = 1.5f;

        attackRange = 8f;

        arrows = new List<GameObject>();

        base.Start();
        
        for (int i = 0; i < maxArrow; i++)
        {
            GameObject ar = Instantiate(arrowPrefab,transform);
            arrows.Add(ar);
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
                anim.SetTrigger("Attack");
                Invoke("ShootArrow", 0.3f);

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

    void ShootArrow()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            if (!arrows[i].activeSelf)
            {
                arrows[i].transform.position = transform.position + (transform.forward + transform.up).normalized;
                arrows[i].transform.right = transform.forward;
                arrows[i].GetComponent<Rigidbody>().AddForce((transform.up + transform.right).normalized * 1000f);
                arrows[i].SetActive(true);
                break;
            }
        }
    }
}


