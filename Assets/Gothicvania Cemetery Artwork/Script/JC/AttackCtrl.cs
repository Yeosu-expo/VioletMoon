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
            hitEnemies.Clear(); // ���� ���� �� �ǰݵ� �� ��� �ʱ�ȭ
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
                collision.GetComponent<UnitHealth>().TakeDamage(10); // ���Ϳ��� 10 ������
                hitEnemies.Add(collision.gameObject); // �ǰݵ� �� ��Ͽ� �߰�
            }
        }
    }

    public void SetTargetTag(string newTargetTag)
    {
        Debug.Log("Call SetTargetTag");
        targetTag = newTargetTag;
    }
}
