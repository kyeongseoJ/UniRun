using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 높이가 순차적으로 생성되는 코인만드려면...?
// 범위내에서 랜덤 생성하고 정렬해줘야한다. 가능하다면 점프 시 그리는 포물선 대로 배치하고 싶음
// 추가적으로 x값도 범위로 생성될 위치를 지정해줘야한다.
// 만약 인공지능으로 플레이어의 최적 경로를 예측하고 그 위치대로 코인을 생성한다면..?
// 하트 아이템을 만들어서 체력을 회복 시키고 싶다면 어떻게 구현해야할까..? 
// hpupdate에 충돌로 아이템 습득 시  += 1 로 만들어보기
public class CoinSpawner : MonoBehaviour
{
    // 생성할 코인의 원본 프리펩
    private GameObject coinPrefab;
    // 생성할 코인 수
    public int count = 5;

    // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMin = 0.2f;
    // 다음 배치까지의 시간 간격 최댓값
    public float timeBetSpawnMax = 0.5f;
    // 다음 배치까지의 시간 간격
    public float timeBetSpawn;


    // 플랫폼 기준으로 설정하기
    // 배치할 위치의 최소 y값
    float coinyMin = PlatformSpawner.yMin + 2.0f;
    // 배치할 위치의 최대 y값
    float coinyMax = PlatformSpawner.yMax + 4.0f;
    // 배치할 위치의 x값
    float coinxPos = PlatformSpawner.xPos - 8.0f;

    // 미리 생성한 코인들
    private GameObject[] coins;
    // 사용할 현재 순번의 코인
    private int currentIndex = 0;

    // 초반에 생성한 코인을 화면 밖에 숨겨둘 위치
    private Vector2 poolPosition = new Vector2(0, -20);
    // 마지막 배치 시점
    private float lastSpawnTime;

    // 변수를 초기화하고 사용할 코인을 미리 생성
    void Start()
    {
        // count 만큼 공간을 가지는 새로운 코인 배열 생성
        coins = new GameObject[count];

        // count 루프하면서 코인 생성
        for (int i = 0; i < count; i++)
        {
            // poolPosition 위치에 복제 생성
            coins[i] = Instantiate(coinPrefab, poolPosition, Quaternion.identity);
        }
        // 마지막 배치 시점 초기화
        lastSpawnTime = 0f;
        // 다음번 배치까지의 시간 간격 초기화
        timeBetSpawn = 0f;
    }

    // 순서를 돌아가며 주기적으로 코인 설치
    // 높이가 순차적으로 나오도록 알고리즘 생각해볼것
    void Update()
    {
        // 게임오버 상태에서 동작하지 않음
        if (GameManager.instance.isGameOver) return;

        // 마지막 배치 시점에서 timeBetSpawn이상 시간이 흘렀다면
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 기록된 마지막 배치 시점을 현재 시점으로 갱신
            lastSpawnTime = Time.time;
            // 다음 배치까지의 시간 간격을 timeBetSpawnMin, timeBetSpawnMax 사이에서 랜덤 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // 배치할 위치의 높이를 yMin과  yMax 사이에서 랜덤 설정
            float yPos = Random.Range(yMin, yMax);


            // 사용할 현재 순번의 코인, 게임의 오브젝트를 비활성화하고 즉시 다시 활성화
            // 이때 코인의 Coin 컴포넌트와 OnEnable 메서드가 실행됨
            coins[currentIndex].SetActive(false);
            coins[currentIndex].SetActive(true);

            // 현재 순반의 코인을 화면 오른쪽에 배치
            coins[currentIndex].transform.position = new Vector2(xPos, yPos);
            // 순번 넘기기
            currentIndex++;

            // 마지막 순번에 도달했다면 순번을 리셋
            if (currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}
