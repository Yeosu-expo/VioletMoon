using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSC : MonoBehaviour
{
    public float spawn_interval = 5.5f;
    public GameObject hero;
    float hero_x;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("Hero");
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hero_x = hero.transform.position.x;
        if (transform.position.x - hero_x <= spawn_interval)
        {
            animator.SetBool("spawn", true);
        }
    }
}
