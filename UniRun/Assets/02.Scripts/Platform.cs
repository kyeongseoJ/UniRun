using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour
{
    // 장애물 오브젝트들을 담는 배열
    public GameObject[] obstacles;

    // 코인을 담는 배열
    public GameObject[] coins;

    // 하트를 담을 배열
    public GameObject[] hearts;

    // 플레이어 캐릭터가 밟았는지  (발판 밟을 때 처음 1회만 점수 올리기)
    private bool stepped = false;

    // 새로운 유니티 이벤트 메서드를 확인
    private void OnEnable()
    {
        // Awake()나 Start()같은 유니티 이벤트 메서드입니다.
        // Start()메서드 처럼 컴포넌트가 활성화 될 때 자동으로 한번 실행되는 메서드 입니다.
        // 그런데, 처음 한번만 실행되는 Start()메서드와 달리 OnEnable()메서드는 컴포넌트가 활성화 될 때 마다
        // 매번 다시 실행되는 메서드라서, 컴포넌트를 끄고 다시 켜는 방식으로 재실행될 수 있는 메서드 입니다.

        // 발판을 리셋하는 처리

        // 밝힘 상태를 리셋
        stepped = false;
        // 나중에 장애물을 추가하거나 제거할 때 접근을 위해 소프트 코딩
        // 장애물의 수만큼 루프
        // for문으로 배열의 공간을 확인 후 접근하면 된다.
        for (int i = 0; i < obstacles.Length; i++)
        {
            // 현재 순번의 장애물을 1/3의 확률로 활성화
            if(Random.Range(0,3) == 0)
            {
                // 0, 1, 2 중 0이 나온다면... = 1/3 확률
                obstacles[i].SetActive(true); // 활성화
            }
            else
            {
                obstacles[i].SetActive(false); // 비활성화
            }

            // 조건연산자로 한번에 처리
            // obstacles[i].SetActive(Random.Range(0, 3) == 0 ? true : false);
        }
        // 코인 1/5 확률로 생성
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].SetActive(Random.Range(0, 5) == 0 ? true : false);
        }

        // 하트 1/20 확률로 생성
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(Random.Range(0, 50) == 0 ? true : false);
        }

    }

    // 플레이어 캐릭터가 자신을 밟았을 때 점수를 추가하는 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 상대방의 태그가 Player이고(1차 확인), 이전에 플레이어 캐릭터가 밟지 않았다면(2차확인)...
        if (collision.collider.tag == "Player" && !stepped)
        {
            // 점수를 추가하고 밟힌 상태를 참으로 변경
            stepped = true;
            // 점수 계산해주는 기능 (증가시킬 점수 정수로 입력)
            GameManager.instance.AddScore(1);
        }
    }

}
