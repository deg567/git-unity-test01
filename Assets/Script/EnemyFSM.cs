using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
        // 적 상태 상수 
    enum EnemyState
    {
        idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // 적 상태 변수
    EnemyState m_State;

    // 플레이어 발견 범위
    public float findDistance = 8f;
    Transform player;

    // 이동 속도 
    public float moveSpeed = 4f; 

    // 공격 가능 범위 
    public float attackDistance = 2f;

    CharacterController cc;

    // 누적 시간
    float currentTime = 0;

    // 공격 딜레이 시간
    float attackDelay = 2f;

    // 적의 공격력
    public int attackPower = 3;

    // 초기 위치 저장용 변수 
    Vector3 originPos;
    
    // 이동 가능 범위
    public float moveDistance = 20f;

    // 체력 설정 
    public int Hp;
    public int maxHp = 15;

    // 적 hp Slider 변수
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        // 적 상태의 초기 값은 idle로 설정

        m_State = EnemyState.idle;

        // 플레이어의 트랜스폼 컴포넌트를 받아옴 
        player = GameObject.Find("Player").transform;

        // cc 할당
        cc = GetComponent<CharacterController>();

        // 적의 초기 위치 값 저장 
        originPos = transform.position;

        Hp = maxHp;

    }

    // Update is called once per frame
    void Update()
    {
        // 현재 상태를 체크해 상태별로 정해진 기능 수행 
        switch(m_State)
        {
            case EnemyState.idle:
                idle();
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
                // Damaged();
                break;    
            case EnemyState.Die:
                // Die();
                break;                                                                                                                                   
        }
//    Debug.Log(Hp);

    //현재 체ㅕㄱ의 비율을 슬라이더의 값에 반영
    hpSlider.value = (float)Hp / (float) maxHp;
    }

        void idle()
    {
        // 만일 플레이어와의 거리가 fidnDistance 이내라면 Move 상태로 전환 
        if(Vector3.Distance(transform.position, player.position) < findDistance )
        {
            m_State = EnemyState.Move;
            print("상태 전환: idle ->Move");
        }
    }

        void Move()
    {
        // 만약 현재 위치가 초기 위치에서 moveDistance 값을 넘어간다면 복귀 상태로 전환 
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        //만약 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동        
        else if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;

            //플레이어를 향해 이동, 캐릭터 컨트롤러 컴포넌트 사용
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        //현재 상태를 공격 상태로 전환 
        else
        {
            m_State = EnemyState.Attack;         
            currentTime = attackDelay;   
            print("상태 전환: Move ->Attack");            
        }

    }

        void Attack()
    {
        //만일 플레이어가 공격 범위 이내에 있다면 플레이어 공격 
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격 
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                //PlayerMove 스크립트의 DamageAction 함수를 받아옴 
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;
            }    
        }
        //현재 상태를 이동으로 전환
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack ->Move");             
            currentTime = 0;         
        }

    }

        void Return()
    {
        //만일 초기 위치에서의 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동 
        if(Vector3.Distance(transform.position, originPos) > 2f )
        {
            Vector3 dir = (originPos - transform.position).normalized; //원래 위치에서 현재 위치하면 원래 방향을 바라봄
            cc.Move(dir * moveSpeed * Time.deltaTime);    
        }
        // 적의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환 
        else
        {
            transform.position = originPos;
            Hp = maxHp;
            m_State = EnemyState.idle;
            print("상태 전환: Return -> Idle");
        }
    }      

        void Damaged()
    {
        StartCoroutine(DamageProcess());
    }      

    // 데미지 코루틴 함수
    IEnumerator DamageProcess()
    {
        //공격 애니메이션 시간만큼 대기
        yield return new WaitForSeconds(0.5f);

        // 현재 상태를 이동으로 전환
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    // 데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        // 만일 데미지 상태, 사망 상태, 복귀 상태일 경우 아무런 처리 하지 않고 함수 종료
        // 플레이어의 공격력 만큼 적의 체력이 감소
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        Hp -= hitPower;

        // 적의 체력이 0보다 크면 Damage 상태로 전환 
        if(Hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");
            Damaged();
        }
        // 적의 체력이 0보다 작다면 Die 상태로 전환 
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any State -> Damaged");            
            Die();
        }


    }

        void Die()
    {
        // 진행중인 코루틴을 중지
        StopAllCoroutines();

        StartCoroutine(DieProcess());        
    }  

    //사망 상태를 처리하기 위한 코루틴 
    IEnumerator DieProcess()
    {
        // 캐릭터 컨트롤러 컴포넌트 비활성화 
        cc.enabled = false;

        // 일정 시간 대기 후 자기 자신을 제거 
        yield return new WaitForSeconds(2f);
        print("소멸");
        Destroy(gameObject);
    }
}
