using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCtrl : MonoBehaviour
{
    private Animator animator;
    public bool isRun;
    public GameObject attackArea;
    private bool isMove = true;
    private bool attacked = false;
    //public bool isSkill = false;

    Controller2D controller;

    public float moveSpeed = 1f;
    float gravity = -30;
    public Vector3 velocity;

    public float minX;
    public float maxX;
    public float minSwitchTime = 2f;
    public float maxSwitchTime = 5f;

    public float attackTime;

    float switchTime;
    float timer;
    bool movingRight = false;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();

        animator.SetTrigger("spawn");

        attackArea = transform.Find("AttackArea").gameObject;
        attackArea.SetActive(false);
    }

    private void Update()
    {
        if (controller.collisions.below || controller.collisions.above)
        {
            velocity.y = 0;
        }

        UpdateTimer();
        if (isMove)
        {
            animator.SetBool("isRun", true);
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
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        if (!attacked && timer >= attackTime)
        {
            StartCoroutine(Attack());
            attacked = true;
        }
        if (timer >= switchTime)
        {
            movingRight = !movingRight;
            SetRandomSwitchTime();
        }
    }
    void SetRandomSwitchTime()
    {
        attacked = false;
        switchTime += Random.Range(minSwitchTime, maxSwitchTime);
        attackTime = Random.Range(minSwitchTime, switchTime);
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


    private IEnumerator Attack()
    {
        Debug.Log("Knight Attack");
        

        isMove = false;
        animator.SetTrigger("attack");
        float attackAnimationLength = GetAnimationClipLength(animator, "Attack");
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator AttackCallByAni()
    {
        attackArea.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackArea.SetActive(false);

        isMove = true;
    }

    private float GetAnimationClipLength(Animator animator, string clipName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.LogError("Animation clip not found: " + clipName);
        return 0;
    }
    

    public void Skill()
    {
        //isSkill = true;
        attackArea.GetComponent<AttackCtrl>().SetTargetTag("Monster");
        StartCoroutine(Attack());
    }
}
