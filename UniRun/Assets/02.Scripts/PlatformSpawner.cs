using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이전 총알 생성기에서 사용했던 방식인 생성 방식이 아닌 오브젝트 풀링 방식을 사용해 구현할 거다.
// 게임 초기에 필요한 만큼 오브젝트를 미리 만들어 Pool : 웅덩이(=
// 일정한 공간)에 쌓아두고 필요할 때마다 가져와서 사용하는 방식이다.
// Instantiate() 메서드처럼 오브젝트를 실시간으로 생성하거나, Destroy() 메서드 처럼
// 오브젝트를 실시간으로 생성하거나 파괴하는 처리는 성능을 많이 요구 합니다.
// 또한 메모리를 정리하는 GC(가비지 컬렉션)유발하기 쉽다..
// 게임 도중에 오브젝트를 너무 자주 생성하거나 파괴하면 게임 끊김(프리즈, 버벅거림) 현상이 발생


// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour
{
    // 생성할 발판의 원본 프리팹
    public GameObject platformPrefab; 
    // 생성할 발 판 수
    public int count = 3;

    // 다음 배치까지의 시간 간격 최솟값
    public float timeBetSpawnMin = 1.25f;
    // 다음 배치까지의 시간 간격 최댓값
    public float timeBetSpawnMax = 2.25f;
    // 다음 배치까지의 시간 간격
    public float timeBetSpawn;

    // 배치할 위치의 최소 y값
    public float yMin = -3.5f;
    // 배치할 위치의 최대 y값
    public float yMax = 1.5f;
    // 배치할 위치의 x값
    private float xPos = 20f;

    // 미리 생성한 발판들
    private GameObject[] platforms;
    // 사용할 현재 순번의 발판
    private int currenIndex = 0;

    // 초반에 생성한 발판을 화면 밖에 숨겨둘 위치
    private Vector2 poolPosition = new Vector2(0, -25);
    // 마지막 배치 시점
    private float lastSpawnTime;


   // 변수를 초기화하고 사용할 발판을 미리 생성
    void Start()
    {
        // count 만큼의 공간을 가지는 새로운 발판 배열 생성
        platforms = new GameObject[count];
        // count 루프하면서 발판 생성
        for( int i = 0; i < count; i++)
        {
            // platformPrefab을 원본으로 새 발판을 poolPosition 위치에 복제 생성
            // 생성된 발판을 platforms 배열에 할당
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
            // Quaternion.identity = Quaternion.Euler(new Vector3(0, 0, 0)); 축약법으로 작성
        }
        // 마지막 배치 시점 초기화
        lastSpawnTime = 0f;
        // 다음번 배치까지의 시간 간격을 초기화
        timeBetSpawn = 0f;
    }

    
   // 순서를 돌아가며 주기적으로 발판 설치
    void Update()
    {
        // 게임오버 상태에서는 동작하지 않음
        if(GameManager.instance.isGameOver)
        {
            return;
        }
        // 마지막 배치 시점에서 timeBetSpawn이상 시간이 흘렀다면
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // 기록된 마지막 배치 시점을 현재 시점으로 갱신
            lastSpawnTime = Time.time;
            // 다음 배치까지의 시간 간격을 timeBetSpawnMin, timeBetSpawnMax 사이에서 랜덤 설정
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // 배치할 위치의 높이를 yMin과  yMax 사이에서 랜덤 설정
            float yPos = Random.Range(yMin, yMax);


            // 사용할 현재 순번의 발판, 게임의 오브젝트를 비활성화하고 즉시 다시 활성화
            // 이때 발판의 Platform 컴포넌트와 OnEnable 메서드가 실행됨
            platforms[currenIndex].SetActive(false);
            platforms[currenIndex].SetActive(true);

            // 현재 순반의 발판을 화면 오른쪽에 배치
            platforms[currenIndex].transform.position = new Vector2(xPos, yPos);
            // 순번 넘기기
            currenIndex++;

            // 마지막 순번에 도달했다면 순번을 리셋
            if(currenIndex >= count)
            {
                currenIndex = 0;
            }
        }
    }

}
