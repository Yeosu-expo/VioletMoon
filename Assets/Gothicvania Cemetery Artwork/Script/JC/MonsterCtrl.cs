using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    private Animator animator;

    Controller2D controller;

    public float moveSpeed = 1f;
    float gravity = -30;
    public Vector3 velocity;

    public float minX;
    public float maxX;
    public float minSwitchTime = 2f;
    public float maxSwitchTime = 5f;

    float switchTime;
    float timer;
    bool movingRight = false;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();

        animator.SetTrigger("spawn");
    }

    private void Update()
    {
        if (controller.collisions.below || controller.collisions.above)
        {
            velocity.y = 0;
        }

        UpdateTimer();
        velocity.x = moveSpeed * (movingRight ? 1 : -1);
        velocity.y += gravity * Time.deltaTime;

        // 이동 방향에 Ground가 없다면 이동 안함.
        if (controller.isProceedable(velocity * Time.deltaTime))
        {
            controller.Move(velocity * Time.deltaTime);
        }

        bool isRun = controller.collisions.left || controller.collisions.right;
        bool curDir = Mathf.Sign(transform.localScale.x) == -1; // left : false, right ; ture
        bool nextDir = controller.collisions.right; // left : false, right : true
        if (isRun && curDir != nextDir)
        {
            Flip();
        }
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (timer >= switchTime)
        {
            movingRight = !movingRight;
            SetRandomSwitchTime();
        }
    }
    void SetRandomSwitchTime()
    {
        switchTime += Random.Range(minSwitchTime, maxSwitchTime);
        timer = 0;
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("P_Attack"))
        {
            Debug.Log("끄악");
        }
    }
}
