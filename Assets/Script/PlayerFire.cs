using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 무기를 발사할 위치 지정
    public GameObject fireposition;

    // 무기 오브젝트
    public GameObject bombfactory;
    
    // 투척 파워
    public float throwpower = 15f;

    // 총알 이펙트 오브젝트
    public GameObject bulleteffect;

    // 총알 이펙트 파티클 시스템
    ParticleSystem ps;

    // 공격력
    public int weaponPower = 5;

    // Start is called before the first frame update
    void Start()
    {
        ps = bulleteffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        // 게임 상태가 Run일 때만 조작할 수 있게 함
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        // 마우스 오른쪽 버튼을 통해 무기를 발상
        if(Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombfactory);
            bomb.transform.position = fireposition.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            // 카메라의 정면 방향으로 무기에 물리적 힘을 가함
            rb.AddForce(Camera.main.transform.forward * throwpower, ForceMode.Impulse);
        }

        // 마우스 왼쪽 버튼 입력
        if (Input.GetMouseButtonDown(0))
        {
            //레이(Ray)를 생성하고 발사될 위치와 방향을 설정
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //레이가 부딪힌 대상의 정보 저장
            RaycastHit hitinfo = new RaycastHit();
            // 만일 부딪힌 물체가 있으면 피격 이펙트를 표시
            if (Physics.Raycast(ray, out hitinfo))
            {
                // 만일 부딫힌 대상의 레이어가 Enemy 라면 Damage 함수를 실행 
                if(hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitinfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                bulleteffect.transform.position = hitinfo.point;  
                // 피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 수직으로 발생. 충돌 지점의 방향과 일치.
                bulleteffect.transform.forward = hitinfo.normal;
                ps.Play();                    
                }
            }
        }
    }
} 