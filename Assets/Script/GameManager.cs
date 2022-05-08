using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 싱글톤 변수 : 한 개의 클래스 인스턴스를 갖도록 보장하고, 이에 대한 전역적인 접근점 제공
    public static GameManager gm;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;
    
    // UI 텍스트 컴포넌트 변수
    Text gameText;

    PlayerMove player;

    // Player Move 
    
    private void  Awake() 
    {
        if(gm == null)
        {
            gm = this;
        }
    }

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver,
    }

    // 현재 게임 상태 변수 
    public GameState gState;

    // Start is called before the first frame update
    void Start()
    {
        // 초기 게임 상탤르 준비 상태로 설정
        gState = GameState.Ready;
        
        // UI 오브젝트에서 텍스트 컴포넌트 할당
        gameText = gameLabel.GetComponent<Text>();

        //텍스트의 내용을 준비로 기입
        gameText.text = "Ready";

        //텍스트의 색상을 주황색으로 변경
        gameText.color = new Color32(255, 181, 0, 255);

        // 게임 준비에서 Run 상태로 변경
        StartCoroutine(ReadyToStart());

        // 플레이어 오브젝트를 찾은 후 플레이어의 PlayerMove 컴포넌트 받아옴
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 상태가 Run일 때만 조작할 수 있게 함
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 플레어의 hp가 0 이하면 게임 오버 상태로 전환
        if(player.hp <= 0)
        {
            // 텍스트 활성화
           gameLabel.SetActive(true);
           gameText.text = "Game Over";      
           gameText.color = new Color32(255, 0, 0, 255);   
           gState = GameState.GameOver;   
        }
    }

    IEnumerator ReadyToStart()
    {
        // 2초간 대기 
        yield return new WaitForSeconds(2f);

        // 텍스트의 내용을 Go로 변경
        gameText.text = "Go";

    
        // 2초간 대기 
        yield return new WaitForSeconds(1f);

        gameLabel.SetActive(false);
        
        // 상태를 Run으로 변경
        gState = GameState.Run; 
    }
}
