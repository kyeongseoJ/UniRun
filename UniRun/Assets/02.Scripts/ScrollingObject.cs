using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // 게임 오브젝트를 지속적으로 왼쪽으로 움직이는 스크립트
public class ScrollingObject : MonoBehaviour
{
    // 이동 속도
    public float speed = 10f;
    // 스타트는 필요 없음

    void Update()
    {
        if (!GameManager.instance.isGameOver)  // isGameOver가 false 라면.. == 살아있다면 움직여라
        {
        // 초당 speed의 속도로 왼쪽으로 평행이동 구현
        // 위치값에 따른 움직임 처리
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        // transform.Translate(new Vector2(10,0)* speed); => transform.Translate(0, 0, 0);
        }
    }

}

