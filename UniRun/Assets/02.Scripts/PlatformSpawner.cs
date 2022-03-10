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
public class PlatformSpawner : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
