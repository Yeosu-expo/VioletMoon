using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    public float damage = 10f;
    public string targetTag = "Monster";

    private HashSet<GameObject> hitEnemies;
    private Collider2D attackAreaCollider;

    void Awake()
    {
        hitEnemies = new HashSet<GameObject>();
        attackAreaCollider = GetComponent<Collider2D>();
    }
    void OnEnable()
    {
        attackAreaCollider.enabled = true;
        if (hitEnemies.Count > 0) {
            hitEnemies.Clear(); // 공격 시작 시 피격된 적 목록 초기화
        }
    }

    private void OnDisable()
    {
        attackAreaCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if(collision.gameObject.name == "Hero")
            {
                    PostProcessingCtrl.Instance.FadeOutAndIn(0.7f, 0.7f);
            }
            if (!hitEnemies.Contains(collision.gameObject))
            {
                collision.GetComponent<UnitHealth>().TakeDamage(10); // 몬스터에게 10 데미지
                hitEnemies.Add(collision.gameObject); // 피격된 적 목록에 추가
            }
        }
    }

    public void SetTargetTag(string newTargetTag)
    {
        Debug.Log("Call SetTargetTag");
        targetTag = newTargetTag;
    }
}
