using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playercontroller : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator myAnimator;
    private Vector2 moveInput; 
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // 바닥 체크 
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // 캐릭터 좌우 반전 
        FlipSprite();

        // 애니메이션 
        if (myAnimator != null)
        {
            myAnimator.SetBool("move", moveInput.magnitude > 0);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FlipSprite()
    {
        if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 리스폰 및 사망 처리
        if (collision.CompareTag("Respawn") || collision.CompareTag("enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // 다음 레벨 이동
        if (collision.CompareTag("Finish"))
        {
            var levelObj = collision.GetComponent<LevelObject>();
            if (levelObj != null) levelObj.MoveToNextLevel();
        }
    }
}