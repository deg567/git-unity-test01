using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public float currentTime = 0f;

    private float time_start;
    private float time_current;


    // 충돌체 처리 함수 구현
    private void OnCollisionEnter(Collision collision)
    {   
//        if(currentTime>=3f)
//        {
//        // 이펙트 프리팹 생성
//       GameObject eff = Instantiate(bombEffect);

        // 이펙트 프리팹 위치 설정 
//        eff.transform.position = transform.position;

        //자기 오브젝트를 제거 
//        Destroy(gameObject);
//        }
    }


    // Start is called before the first frame update
    void Start()
    {
        time_start = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;
        time_current = time_start + 3;        
    }

    // Update is called once per frame
    void Update()
    {
//        currentTime += Time.deltaTime;
        if (time_current == (float)System.DateTime.Now.TimeOfDay.TotalSeconds)
        {
            // 이펙트 프리팹 생성
            GameObject eff = Instantiate(bombEffect);

            // 이펙트 프리팹 위치 설정 
            eff.transform.position = transform.position;

            //자기 오브젝트를 제거 
            Destroy(gameObject);
        }

    }


}
