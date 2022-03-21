using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // 아이템 오브젝트를 담는 배열
    public GameObject[] items;

    private void OnEnable()
    {
        for (int i = 0; i < items.Length; i++)
        {
            // 조건연산자로 한번에 처리
            items[i].SetActive(Random.Range(0, 3) == 0 ? true : false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 코인 충돌 시 점수 증가
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.AddScore(100); // 코인당 점수 증가
            collision.gameObject.SetActive(false); // 코인 획득시 코인 삭제
        }
    }
}
