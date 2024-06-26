using System.Collections;
using UnityEngine;


public class HeroCtrl : MonoBehaviour
{
    private Animator animator;
    public bool isGrounded;
    public bool isRun;
    private GameObject attackArea;

    Controller2D controller;

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 5f;

    float gravity;
    float jumpVelocity;
    public Vector3 velocity;
    float velocityXSmoothing;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity; " + gravity + "    Jump Velocity: " + jumpVelocity);

        animator = GetComponent<Animator>();
        attackArea = transform.Find("AttackArea").gameObject;
        attackArea.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("!!!");
            StartCoroutine(Skill.Instance.Attack(0));
        }
            
        else if(Input.GetKeyDown(KeyCode.W))
            StartCoroutine(Skill.Instance.Attack(1));
        else if(Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Skill.Instance.Attack(2));
        if (controller.collisions.below || controller.collisions.above)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        isGrounded = controller.collisions.below || controller.collisions.above;
        isRun = controller.collisions.left || controller.collisions.right;
        HandleAnimation();
    }

    void HandleAnimation()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRun", isRun);

        if (Input.GetKey(KeyCode.DownArrow) && isGrounded)
        {
            animator.SetBool("isCrouch", true);
            //StartCoroutine(DisableCollision());
        }
        else
        {
            animator.SetBool("isCrouch", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
            StartCoroutine(ActivateAttackArea());
        }

        bool curDir = Mathf.Sign(transform.localScale.x) == 1; // left : false, right ; ture
        bool nextDir = controller.collisions.right; // left : false, right : true
        if (isRun && curDir != nextDir) {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    /*
    private IEnumerator DisableCollision()
    {
        float distance = 30f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, distance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.blue, distance);

        if (hits.Length >= 2)
        {
            Bounds bounds = playerCollider.bounds;
            Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y - groundCheckHeight / 2);
            Vector2 boxSize = new Vector2(bounds.size.x, groundCheckHeight);
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, groundLayer);

            foreach (var hit in hitColliders)
            {
                if (hit != playerCollider)
                {
                    Physics2D.IgnoreCollision(playerCollider, hit, true);
                    float groundMaxY = hit.bounds.max.y;
                    StartCoroutine(ReenableCollision(hit));
                }
            }

            yield return new WaitForSeconds(0.2f);
            
        }
    }

    private IEnumerator ReenableCollision(Collider2D groundCollider)
    {
        float reenableBottomThreshold = groundCollider.bounds.max.y - 1f; // 콜라이더를 다시 활성화할 y좌표 기준
        float reenableTopThreshold = groundCollider.bounds.max.y + 1f;

        while (transform.position.y < reenableBottomThreshold || transform.position.y > reenableTopThreshold)
        {
            Debug.Log("다시");
            yield return null;
        }
        Debug.Log("벗어남");
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreCollision(playerCollider, groundCollider, false);
    }
    */

    private IEnumerator ActivateAttackArea()
    {
        attackArea.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackArea.SetActive(false);
    }
}
