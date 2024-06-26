using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBall : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 7.0f;
    public float limitDistance = 15f;
    private float moveDistance = 0f;

    public string targetTag = "Player";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDistance += speed * Time.deltaTime;
        if(moveDistance > limitDistance)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void SetTargetTag(string newTarget)
    {
        targetTag = newTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Coroutine runningCoroutine;
        Debug.Log(collision.gameObject.name + " Attack Collision");
        Debug.Log(collision.gameObject.tag);
        Debug.Log(targetTag);
        Debug.Log(collision.gameObject.tag == targetTag);
        if (collision.gameObject.tag == targetTag)
        {
            collision.GetComponent<UnitHealth>().TakeDamage(damage);
            if (collision.gameObject.name == "Hero")
            {
                runningCoroutine = StartCoroutine(PostProcessingCtrl.Instance.ChangeChromTimes(4, 2, 2, 1, 0.2f)); // ȥ��
                Debug.Log("ȥ��");
                StartCoroutine(WaitForCoroutineAndDestroy(runningCoroutine));
            }
        }
    }


    private IEnumerator WaitForCoroutineAndDestroy(Coroutine coroutine)
    {
        // �ڷ�ƾ�� ����� ������ ��ٸ�
        yield return coroutine;

        // �ڷ�ƾ�� �Ϸ�Ǹ� ������Ʈ�� �ı�
        Destroy(gameObject);
    }


}
