using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 게임오버 상태를 표현하고, 게임점수와 UI를 관리하는 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글턴을 할당할 전역 변수

    public bool isGameOver = false; // 게임오버 상태를 표현하는 변수 false : Gameover   true : Not Gameover 
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임오버시 활성화할 UI 오브젝트

    private int score = 0; // 게임 점수

    public int hp = 2; // 체력 변수 설정
    public Text hpText; // 체력표시할 UI 텍스트

    public GameObject menuPanel; // 메뉴 클릭시 패널 활성화를 위한 UI 오브젝트


    public bool isPause = false;// 일시정지 상태

    // 최고기록을 표시할 텍스트 컴포넌트
    public Text recordText;
    

    // 게임 시작과 동시에 싱글턴을 구성
    private void Awake()
    {

        // 싱글턴 변수 instance 가 비어 있나요?
        if (instance == null)
        {
            // instance가 비어있다면 그곳에 내 자신을 할당
            instance = this; // 내 자신을 담을 것이다 => this 사용
        }
        else
        {
            // instance에 이미 다른 게임 오브젝트가 할당되어 있다면...?
            // 하나의 씬에 두개이상의 게임매니저 오브젝트가 존재하는 경우를 의미
            // 싱글턴 오브젝트는 하나만 존재해야 함으로 자신의 게임 오브젝트를 
            // 파괴시킨다.
            Debug.Log("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    // 게임 오버 상태에서 게임을 재시작 할 수 있게 하는 처리 구현
    void Update()
    {
        // 게임 오버 상태에서 사용자가 마우스 왼쪽 버튼을 클릭한다면..? (2가지 조건이 전부 참일 경우 = &&)
        if(isGameOver && Input.GetMouseButtonDown(0))
        {
            // SceneManager.LoadScene("Main");
            // SceneManager.LoadScene(0);
            // 현재 활성화 되있는 씬의 이름을 가져와 (=> 소프트코딩)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 점수를 증가시키는 메서드 따로 생성 ==> (나중에 과제에서 필요한 부분)
    // 외부에서 실행 시 매개변수인 정수 뉴스코어가 따로 필요하다
    public void AddScore(int newScore)
    {
        // 게임오버가 아니라면
        if (!isGameOver)
        {
            // 점수를 증가
            score += newScore;
            // 스코어를 UI로 표시
            scoreText.text = "Score : " + score;
        }
        //if (isGameOver) return;

        //score += newScore;
        //scoreText.text = "Score : " + score;
    }

    // 매개변수 없이 구현
    // 플레이어가 사망 시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        // 현재 상태를 게임오버 상태로 변경
        isGameOver = true;
        // 게임오버 텍스트 게임 오브젝트를 활성화
        gameoverUI.SetActive(true);

        // 저장된 값이 없다면 0 할당된다.
        int bestscore = PlayerPrefs.GetInt("BestScore");

        if (score > bestscore)
        {
            bestscore = score;
            Debug.Log(bestscore);
            PlayerPrefs.SetInt("BestScore", bestscore);
            recordText.text = "Best Score" + bestscore;
        }
    }

    // 체력 감소 표시
    public void HpUpdate()
    {
        // 체력 감소된 값을 텍스트 안에 작성
        hpText.text =  "HP " + hp;
        
    }

    // 점프인식되는 문제 해결안됨
    // 메뉴 클릭시 화면 일시정지 및 패널 활성화
    public void OnMenu()
    {
        if (!isPause)
        {   // 일시정지 중이 아니면 일시정지
            Time.timeScale = 0; // 시간정지
            // 패널활성화
            menuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f; // 시간흐름 비율 1
            menuPanel.SetActive(false);
        }

        isPause = !isPause; // 메뉴 누를 때마다 상태가 반대로 바뀜
    }

    // 재시작 기능 구현
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
    

    // 게임 종료 구현 
    public void ExitGame()
    {
        Application.Quit();
    }

}
