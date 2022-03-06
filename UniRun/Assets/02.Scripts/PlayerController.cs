using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어함
public class PlayerController : MonoBehaviour
{
    //  플에이어가 사망시 재생할 오디오 클립
    public AudioClip deathClip;
    // 점프 힘
    public float jumpForce = 700f;

    // 누적 점프 횟수 카운트
    private int jumpCount = 0;
    // 플레이어가 바닥에 닿았는지 확인(시작 시 공중에 떠있기 때문에 false )
    private bool isGrounded = false;
    // 플레이어의 사망 상태 : 죽었는지 살았는지
    private bool isDead = false;
    // 이동에 사용할 리지드바디 컴포넌트
    private Rigidbody2D playerRigidbody;
    // 사용할 오디오 소스 컴포넌트
    private AudioSource playerAudio;
    // 사용할 애니메이터 컴포넌트
    private Animator animator;

    void Start()
    {
        // 전역변수의 초기화 진행
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // 
    void Update()
    {
        // 사용자의 입력을 감지하고 점프하는 처리
        // 1. 현재 상황에 알맞은 애니를 재생
        // 2. 마우스 좌클릭 감지하고 점프 구현
        // 3. 마우스 좌클릭 길게 누를 때 높이 점프하게 처리
        // 4. 최대점프횟수에 도달하면 점프를 못하게 막기

        // 사망 시 더이상 처리를 진행하지 않고 종료하기
        if (isDead) return;

        // 마우스 좌클릭 시 & 최대점프 횟수 2회 도달하지 않았다면
        // 점프 한번 한 경우
        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            // 점프 횟수 증가
            jumpCount++;
            // 점프 직전에 속도를 순간적으로 제로(0,0)으로 변경
            // => 점프 직전까지의 힘 또는 속도가 상쇄되거나 합쳐져서 점프 높이가 비일관적으로 되는 현상을 막기 위해서 작성
            playerRigidbody.velocity = Vector2.zero; // new Vector2(0,0) = Vector2.zero

            // 리지드바디에 위쪽으로 힘주기
            playerRigidbody.AddForce(new Vector2(0, jumpForce));

            // 오디오 소스 재생
            playerAudio.Play();
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody.velocity.y > 0)
        {
            // 마우스 좌클릭에서 손을 떼는 순간과 y값이 양수라면(=위로 상승 중) 현재속도를 절반으로 변경 (y값이 음수면 아래)
            playerRigidbody.velocity = playerRigidbody.velocity * 0.5f;
           
        }

        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신 => OnCollisionExit2D 여기서 사용하기 위해 계속 업데이트 해줘야함
        // 현재 상태값을 변경하는 것 = Set
        animator.SetBool("Grounded", isGrounded);

        // 현재 상태 값을 읽어오는 것 = Get
        // animator.GetBool("Grounded");

        // 점프 시 속도 변화 보기
        // Debug.Log(playerRigidbody.velocity.y);
    }

    void Die()
    {
        // 사망 처리
        // 만약 플레이어가 죽었다면 애니메이터의 Die 트리거 파라미터를 Set
        animator.SetTrigger("Die");

        // 오디오 소스에 할당된 점프 사운드 오디오 클립을 deathClip으로 변경
        playerAudio.clip = deathClip;
        // 파일 교체 후 사망 효과음 재생
        playerAudio.Play();

        // 사망시 속도를 제로로 변경
        playerRigidbody.velocity = Vector2.zero;
        // 나 사망했어...사망상태 true로 변경해서 알림
        isDead = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿자마자 감지하는 처리 : isGrounded 사용 해서 true 인지 false인지 확인
        // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있는지 확인
        if(collision.contacts[0].normal.y > 0.7f)
        {
            //contacts :  충돌 지점의 정보를 담는 ContactPoint2D 타입의 데이터를 contacts라는 배열 변수로 제공받는다.
            // normal : 충돌지점에서 충돌 표면의 방향(노말벡터)를 알려주는 변수입니다.
            // isGrounded를 true로 변경하고 누적 점프횟수를 0으로 리셋
            isGrounded = true;
            jumpCount = 0;


        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 바닥에 벗어나자 마자 처리
        // 어떤 콜라이더에서 떼어진 경우 isGrounded를 false로 변경
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거 콜라이더를 가진 장애물과의 충돌 감지
        // 장애물에서 컴포넌트 is Trigger 체크 할 것
        // 충돌이 감지된 오브젝트가 장애물이나 데드존인지 인지 할것 => 태그사용
        // 충돌한 상대방의 태그가 Dead 이면서 아직 사망하지 않았다면...!
        if (collision.tag == "Dead" && !isDead)
        {
            Die();
        }
    }
}
