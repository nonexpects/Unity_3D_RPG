using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : EnemyFSM
{
    protected override void Start()
    {
        maxHp = 10f;
        att = 5;
        attTime = 2f;

        base.Start();
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
                //player.GetComponent<PlayerController>().Damaged(att);
                Debug.Log("밀리어택!");
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
