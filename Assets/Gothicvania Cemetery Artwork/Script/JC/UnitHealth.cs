using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    private Animator animator;

    public Sprite deadSprite;
    public MonoBehaviour[] movementScripts;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " Attacked");
        animator.SetTrigger("hurt");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {

        float waitTime = 3f;
        float runTime = GetAnimationClipLength(animator, "Death");
        foreach (var script in movementScripts)
        {
            script.enabled = false;
        }

        yield return new WaitForSeconds(0.1f);
        Debug.Log(gameObject.name + " Died");
        Debug.Log("runTime : " + runTime);
        if (runTime != -1)
        {
            animator.SetTrigger("death");

            yield return new WaitForSeconds(runTime);
            Debug.Log("finished wait");
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
        animator.enabled = false;
        
        //yield return new WaitForSeconds(waitTime);
        int index = -1;

        switch (gameObject.name)
        {
            case "Wizard3":
                index = 1; break;
            case "Knight":
                index = 0; break;
        }

        if (index != -1)
        {
            Debug.Log("흡수!");
            Skill.Instance.Imbibition(gameObject, index);
            yield break;
        }
        Debug.Log("흡수실패");

        Destroy(gameObject);
    }


    float GetAnimationClipLength(Animator animator, string clipName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return -1f; // 애니메이션 클립을 찾지 못한 경우
    }
}
