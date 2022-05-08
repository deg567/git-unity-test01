using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    CharacterController  cc;
    
    // 중력 변수 
    float gravity = -20f;

    // 수직 속력 변수
    float yVelocity = 0;

    // 점프력 변수 
    public float jumpPower = 10f;
    public int limitJump = 2;
    private int currentJump = 0;

    // 점프 상태 변수
    public bool isJumping = false;

    // 플레이어 체력 변수
    public int hp = 20;

    // 플레이어 최대 체력 변수
    int maxHp = 20;

    // 체력 슬라이더 변수 
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // 게임 상태가 Run일 때만 조작할 수 있게 함
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        float h = Input.GetAxis("Horizontal"); //A~D
        float v = Input.GetAxis("Vertical"); //W~S

        // 이동 방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 메인 카메라를 기준으로 방향 변경
        dir = Camera.main.transform.TransformDirection(dir);
        
        // 점프 중이고, 바닥에 착지했다면
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
            currentJump = 0;
        }

        if (limitJump <= currentJump)
        {
            isJumping = true;
        }

        // 점프 구현
        if (Input.GetButtonDown("Jump") && !isJumping) 
        {
            yVelocity = jumpPower;
            currentJump += 1;
        }

        // 이동 속도 설정
        transform.position += dir * moveSpeed * Time.deltaTime;

        // 캐릭터의 수직 속도에 중력 값 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 이동 함수 
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 현재 플레이어의 체력을 hp 슬라이더 값에 반영 
        hpSlider.value = (float)hp / (float)maxHp;
    }

    // 플레이어의 피격 함수 
    public void DamageAction(int damage)
    {
        // 적의 공격력 만큼 플레이어의 체력을 깎음
        hp -= damage;

        // 체력이 음수가 될 경우에 0으로 초기화 
        if(hp < 0)
        {
            hp = 0;
        }
    }
}