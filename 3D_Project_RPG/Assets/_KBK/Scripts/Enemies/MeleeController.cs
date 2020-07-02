using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : EnemyFSM
{
    protected override void Start()
    {
        enemyId = 0;
        maxHp = 10f;
        att = 2;
        attTime = 1f;
        moveSpeed = 3f;
        returnSpeed = 2f;

        attackRange = 1f;

        base.Start();
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
                player.GetComponent<PlayerController>().Damaged(att);
                anim.SetTrigger("Attack");
                //타이머 초기화
                timer = 0f;
            }
        }
        else //현재 상태를 Move로 변경(재추격)
        {
            // - 상태 변경
            state = EnemyState.Move;
            anim.SetTrigger("Move");
            // - 상태 전환 출력
            timer = 0f;
        }
    }

    //복귀 상태
    protected override void Return()
    {
        float dist = Vector3.Distance(spawnPos, transform.position);
        if (dist > 0.3f)
        {
            //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위 벗어나면 다시 돌아옴
            Vector3 dir = (spawnPos - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir) * rot, 10 * Time.deltaTime);
            cc.SimpleMove(dir * returnSpeed);
        }
        else
        {
            //위치값을 초기값으로
            transform.position = spawnPos;
            transform.rotation = Quaternion.identity;
            // - 상태 변경
            state = EnemyState.Idle;
            anim.SetTrigger("Idle");
        }
    }
}
