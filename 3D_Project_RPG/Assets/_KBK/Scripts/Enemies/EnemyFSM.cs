using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // 몬스터 상태 enum문
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,  
        Damaged,
        Return,
        Die
    }

    public EnemyState state; //몬스터 상태변수

    protected Vector3 spawnPos;
    public float findRange = 15;
    public float moveRange = 8;
    public float attackRange = 4;
    
    protected Transform player;
    protected CharacterController cc;
    protected Animator anim;
    protected float moveSpeed;
    protected float returnSpeed;

                    
    /// 몬스터 일반 변수
    protected float currHp;
    protected float maxHp;
    protected int att;      //공격력
    //float speed = 5f;   //스피드

    //공격 딜레이
    protected float attTime; //2초에 한 번 공격
    protected float timer;    //타이머

    protected Quaternion rot = Quaternion.Euler(new Vector3(0, 1, 0));

    protected GameObject deathFx;

    protected virtual void Start()
    { 
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        spawnPos = transform.position;
        //플레이어 트랜스폼
        player = GameObject.Find("Player").GetComponent<Transform>();

        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();

        deathFx = Resources.Load("Fx/EnemyDeath") as GameObject;

        currHp = maxHp;
    }

    void Update()
    {
        if (GameManager.instance.playerDead)
        {
            state = EnemyState.Idle;
            anim.SetTrigger("Idle");
        }

        if (gameObject.activeSelf)
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
            }//end of void Update()
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < findRange)
        {
            // - 상태 변경
            state = EnemyState.Move;
            // - 상태 전환 출력
            print("Change State Idle to Move State");

            anim.SetTrigger("Move");
        }
    }

    protected virtual void Move()
    {
        // - 공격 범위 2미터
        if (Vector3.Distance(spawnPos, transform.position) > moveRange || GameManager.instance.playerDead)
        {
            // - 상태 변경
            state = EnemyState.Return;
            anim.SetTrigger("Move");
            // - 상태 전환 출력
            print("Change State Move to Return State");
            
        }
        //moveRange를 벗어나지 않고 공격범위에 있지도 않음
        else if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            //플레이어 추격
            //이동방법 (벡터의 뺄셈)
            Vector3 dir = (player.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir) * rot, 10 * Time.deltaTime);
            
            cc.SimpleMove(dir * moveSpeed);
            
        }
        else //공격범위 안에 들어옴
        {
            // - 상태 변경
            state = EnemyState.Attack;
            anim.SetTrigger("Idle");
            // - 상태 전환 출력
            print("Change State Move to Attack State");
        }
    }

    protected virtual void Attack() {}

    //복귀 상태
    protected virtual void Return()
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
            anim.SetTrigger("Damaged");

            Damaged();
        }
        else
        {
            state = EnemyState.Die;
            print("상태 : EnemyState -> Die");
            anim.SetTrigger("Die");
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
        player.GetComponent<PlayerController>().getExp(10);
        StartCoroutine(DieProc());

    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1f);
        //상태 전환하기
        state = EnemyState.Move;
        anim.SetTrigger("Move");
        print("상태 전환 : Damaged -> Move");
    }

    IEnumerator DieProc()
    {
        //굳이 안해줘도 되지만 오브젝트 날리기 전에 비활성화 해주는게 좋다.
        cc.enabled = false;
        
        //2초 후에 자기자신 제거
        yield return new WaitForSeconds(2f);
        GameObject fx = Instantiate(deathFx);
        fx.transform.position = transform.position;
        Destroy(fx, 1f);
        Destroy(this.gameObject);
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
