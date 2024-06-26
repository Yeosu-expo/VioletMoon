using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Wizard3Ctrl : MonoBehaviour
{
    public Animator animator;
    public bool isRun;
    public GameObject fireBall;
    public Transform firePos;
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
    }

    private void Update()
    {
        if (controller.collisions.below || controller.collisions.above)
        {
            velocity.y = 0;
        }

        if (isMove)
        {
            animator.SetBool("isRun", true);
            UpdateTimer();
            velocity.x = moveSpeed * (movingRight ? 1 : -1);
            velocity.y += gravity * Time.deltaTime;

            // �̵� ���⿡ Ground�� ���ٸ� �̵� ����.
            if (controller.isProceedable(velocity * Time.deltaTime))
            {
                controller.Move(velocity * Time.deltaTime);
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
            Flip();
            SetRandomSwitchTime();
        }
    }
    void SetRandomSwitchTime()
    {
        switchTime += Random.Range(minSwitchTime, maxSwitchTime);
        attackTime = Random.Range(minSwitchTime, switchTime);
        timer = 0;
        attacked = false;
    }

    public void Flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("P_Attack"))
        {
            Debug.Log("����");
        }

    }

    private IEnumerator Attack()
    {
        isMove = false;
        animator.SetTrigger("attack");
        Debug.Log("ani_attack");

        float attackAnimationLength = GetAnimationClipLength(animator, "Attack");
        yield return new WaitForSeconds(attackAnimationLength);

        // ��� �� ������ �簳
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

    public void Fire()
    {
        Quaternion rot = firePos.rotation;
        if (!movingRight)
        {
            rot.z = 180;
        }
        GameObject fireB = Instantiate(fireBall, firePos.position, rot);
        /*
        if (isSkill)
        {
            fireB.GetComponent<fireBall>().SetTargetTag("Monster");
        }
        */
        isMove = true;
    }

    public void Skill()
    {
        //isSkill = true;
        gameObject.tag = "Untagged";
        fireBall.GetComponent<fireBall>().SetTargetTag("Monster");
        fireBall.GetComponent<fireBall>().damage = 50f;
        Coroutine runningCoroutine = StartCoroutine(Attack());
        StartCoroutine(WaitForCoroutineAndDestroy(runningCoroutine));
    }
    private IEnumerator WaitForCoroutineAndDestroy(Coroutine coroutine)
    {
        // �ڷ�ƾ�� ����� ������ ��ٸ�
        yield return coroutine;

        // �ڷ�ƾ�� �Ϸ�Ǹ� ������Ʈ�� �ı�
        Destroy(gameObject);
    }
}
