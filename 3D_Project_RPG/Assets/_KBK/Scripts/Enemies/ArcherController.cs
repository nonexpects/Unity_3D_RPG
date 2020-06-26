﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy_Status;

public class ArcherController : MonoBehaviour
{
    EnemyState state; //몬스터 상태변수

    public GameObject quiver;
    public GameObject arrowPrefab;
    int maxArrow = 30;
    List<GameObject> arrows;

    protected override void Start()
    {
        maxHp = 10f;
        att = 2;
        attTime = 3f;

        attackRange = 8f;
        
        base.Start();
        
        for (int i = 0; i < maxArrow; i++)
        {
            GameObject ar = Instantiate(arrowPrefab, quiver.transform);
            
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
            // - 상태 전환 출력
            print("Change State Move to Return State");
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
            print("Change State Move to Attack State");
        }
    }

    protected override void Attack()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        transform.forward = dir;
        // - 공격 범위 1미터
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                player.GetComponent<PlayerController>().Damaged(att);
                Debug.Log("화살어택!");
                for (int i = 0; i < arrows.Count; i++)
                {
                    if (!arrows[i].activeSelf)
                    {
                        arrows[i].transform.position = transform.position;
                        arrows[i].transform.rotation = transform.rotation;
                        arrows[i].SetActive(true);
                        break;
                    }
                }

                //타이머 초기화
                timer = 0f;
            }
        }
        else //현재 상태를 Move로 변경(재추격)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change State Attack to Move State");
            timer = 0f;
        }
    }
}
