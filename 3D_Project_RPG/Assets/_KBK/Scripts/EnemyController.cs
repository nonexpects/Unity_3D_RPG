using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 몬스터 상태 enum문
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState state; //몬스터 상태변수

    /// <summary>
    /// 유용한기능 (/// 치면 나옴)
    /// </summary>



    #region "Idle 상태에 필요한 변수들"
    #endregion
    #region "Move 상태에 필요한 변수들"
    #endregion
    #region "Attack 상태에 필요한 변수들"
    #endregion
    #region "Return 상태에 필요한 변수들"
    #endregion
    #region "Damaged 상태에 필요한 변수들"
    #endregion
    #region "Die 상태에 필요한 변수들"
    #endregion

    /// 필요한 변수들
    /// 
    public float findRange = 10f;  //플레이어 찾는 범위
    public float moveRange = 25f;   //시작 지접에서 최대 이용가능한 범위
    public float attackRange = 2f; //공격 가능 범위
    Vector3 spawnPos;  //시작지점
    Transform player;           //플레이어 찾기 위해서
    //GameObject player;
    CharacterController cc;     //캐릭터 이동 위해 캐릭터 컨트롤러 필요
    Animator anim;
    float moveSpeed = 5f;

    /// 몬스터 일반 변수
    float currHp;
    float maxHp = 10f;
    float att = 5f;      //공격력
    float speed = 5f;   //스피드

    //공격 딜레이
    float attTime = 2f; //2초에 한 번 공격
    float timer = 0;    //타이머

    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        spawnPos = transform.position;
        //플레이어 트랜스폼
        player = GameObject.Find("Player").GetComponent<Transform>();

        cc = GetComponent<CharacterController>();

        currHp = maxHp;
    }

    void Update()
    {
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }//end of void Update()
    }

    private void Idle()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < findRange)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change State Idle to Move State");
        }
    }

    private void Move()
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
            
            cc.SimpleMove(dir * moveSpeed);
        }
        else //공격범위 안에 들어옴
        {
            // - 상태 변경
            state = EnemyState.Attack;
            // - 상태 전환 출력
            print("Change State Move to Attack State");
        }
    }

    private void Attack()
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
                print("공격");
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

    //복귀 상태
    private void Return()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위 벗어나면 다시 돌아옴
            Vector3 dir = (spawnPos - transform.position).normalized;
            cc.SimpleMove(dir * moveSpeed);
        }
        else
        {
            //위치값을 초기값으로
            transform.position = spawnPos;
            // - 상태 변경
            state = EnemyState.Idle;
            // - 상태 전환 출력
            print("Change State Return to Idle State");
        }

    }

    //플레이어쪽에서 충돌감지를 할 수 있으니 이 함수는 퍼블릭으로 만들자
    public void hitDamage(int value)
    {
        //예외처리
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //체력깍기
        currHp -= value;

        //몬스터의 체력이 1이상이면 피격상태
        if (currHp > 0)
        {
            state = EnemyState.Damaged;
            print("상태 : EnemyState -> Damaged");
            print("HP : " + currHp);

            Damaged();
        }
        else
        {
            state = EnemyState.Die;
            print("상태 : EnemyState -> Die");

            Die();
        }
    }

    //피격상태 (Any State)
    private void Damaged()
    {
        StartCoroutine(DamageProc());
    }

    private void Die()
    {
        //혹시 진행중인 모든 코루틴을 정지한다
        StopAllCoroutines();

        StartCoroutine(DieProc());

    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1f);
        //상태 전환하기
        state = EnemyState.Move;
        print("상태 전환 : Damaged -> Move");
    }

    IEnumerator DieProc()
    {
        //굳이 안해줘도 되지만 오브젝트 날리기 전에 비활성화 해주는게 좋다.
        cc.enabled = false;

        //2초 후에 자기자신 제거
        yield return new WaitForSeconds(2f);
        print("죽음");
        Destroy(gameObject);
    }

    //시각적으로 범위 표시
    private void OnDrawGizmos()
    {
        //공격 가능 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //플레이어 탐지 범위 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);

        //이동 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPos, moveRange);
    }
}
