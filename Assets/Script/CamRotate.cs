using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속도 변수 
    public float rotSpeed = 200f;


    float mx = 0;
    float my = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 게임 상태가 Run일 때만 조작할 수 있게 함
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
                
        // 마우스 입력을 받음
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //회전 값 변수에 마우스 입력 값을 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        // 마우스 상하 이동 회전 변수 값을 제한
        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);


    /*
        // 회전 방향 결정
        Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);

        // 물체를 회전하는 코드
        transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

        // y축의 값을 제한
        Vector3 rot = transform.eulerAngles;
        rot.x = Mathf.Clamp(rot.x, -90f, 90f);
        transform.eulerAngles = rot;
    */
    }
} 
