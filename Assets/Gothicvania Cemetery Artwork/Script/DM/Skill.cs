using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private static Skill _instance;

    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static Skill Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Skill>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Skill>();
                    singletonObject.name = typeof(Skill).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

    // �� Ŭ������ �ٸ� �ν��Ͻ��� ������ �ʵ��� ����
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as Skill;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameObject[] skills;
    private int skills_index = 0;

    public GameObject[] monsters;
    public bool[] authority;
    public float resize_size = 0.3f;
    public float decrease_interval = 0.01f;
    public GameObject player;
    public float moveSpeed = 1.0f;

    public GameObject[] skills_image; // playpanel�� ��ų�̹��� ��ü
    public Sprite[] skill_sprite;
    public float increase_size = 1.3f;
    public float decrease_interval_skill = 0.001f;
    public float increase_interval_skill = 0.001f;

    bool isQCool = false;
    bool isWCool = false;
    bool isECool = false;
    float qCool = 10;
    float wCool = 10;
    float eCool = 10;
    float qCoolNow = 0;
    float wCoolNow = 0;
    float eCoolNow = 0;

    private void Start()
    {
        skills = new GameObject[3];
        for (int i = 0; i < authority.Length; i++)
        {
            authority[i] = false;
        }
    }

    private void Update()
    {
        if (isQCool)
        {
            qCoolNow += Time.deltaTime;
            if(qCool <= qCoolNow)
            {
                qCoolNow = 0;
                isQCool = false;
            }
        }
    }

    public void Imbibition(GameObject self, int index)
    {
        if (monsters.Length <= index)
        {
            Debug.Log("Out of Monsters Index.");
            return;
        }

        StartCoroutine(ImbibitionCoroutine(self, index));
    }

    private IEnumerator ImbibitionCoroutine(GameObject self, int index)
    {
        self.GetComponent<SpriteRenderer>().color = Color.gray;

        Vector3 originalScale = self.transform.localScale;
        Vector3 targetScale = originalScale * resize_size;
        Vector3 targetPosition = player.transform.position;

        while (Vector3.Distance(self.transform.position, targetPosition) > 0.1f || self.transform.localScale.magnitude > targetScale.magnitude)
        {
            // �������� ���������� ����
            if (self.transform.localScale.magnitude > targetScale.magnitude)
            {
                Vector3 newScale = self.transform.localScale - new Vector3(decrease_interval, decrease_interval, decrease_interval);
                // �� ���� ���� ��ǥ �����Ϻ��� �۾����� �ʵ��� ����
                newScale.x = Mathf.Max(newScale.x, targetScale.x);
                newScale.y = Mathf.Max(newScale.y, targetScale.y);
                newScale.z = Mathf.Max(newScale.z, targetScale.z);

                self.transform.localScale = newScale;
            }

            // A ��ü�� B ��ü�� �ε巴�� �̵�
            if (Vector3.Distance(self.transform.position, targetPosition) > 0.1f)
            {
                self.transform.position = Vector3.Lerp(self.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ���� �������� ��Ȯ�� ����
        self.transform.localScale = targetScale;

        // �� ��ü�� �������� A ��ü �ı�
        if (Vector3.Distance(self.transform.position, targetPosition) <= 0.1f)
        {
            Destroy(self);
        }
        StartCoroutine(PostProcessingCtrl.Instance.ChangeGamma(5, PostProcessingCtrl.Instance.levelUpColor));

        authority[index] = true;

        yield return StartCoroutine(SetSkillCoroutine(index, self));
    }


    private IEnumerator SetSkillCoroutine(int index, GameObject self)
    {
        skills_image[index].GetComponent<Image>().sprite = skill_sprite[index];

        RectTransform rectTransform = skills_image[index].GetComponent<RectTransform>();
        Vector2 originalSize = rectTransform.localScale;
        float targetSize = originalSize.magnitude*increase_size; // ��ų �̹����� ��ǥ ũ��

        Debug.Log(rectTransform.localScale.magnitude);
        Debug.Log(targetSize);
        // ũ�� ����
        while (rectTransform.localScale.magnitude < targetSize)
        {
            rectTransform.localScale += new Vector3(increase_interval_skill, increase_interval_skill);
            // ���� �����ӱ��� ���
            yield return null;
        }
        // ���� ũ�⸦ ��Ȯ�� ����
        rectTransform.localScale = originalSize * increase_size;

        targetSize = originalSize.magnitude;

        // ũ�� ����
        while (rectTransform.localScale.magnitude > targetSize)
        {
            rectTransform.localScale -= new Vector3(increase_interval_skill, increase_interval_skill);
            // ���� �����ӱ��� ���
            yield return null;
        }

        // ���� ũ�⸦ ��Ȯ�� ����
        rectTransform.localScale = originalSize;

        skills[skills_index++] = monsters[index];

        Destroy(self);
    }

    // Q:0, W:1, E:2
    public IEnumerator Attack(int index)
    {
        if(index >= skills_index || index >= 3)
        {
            Debug.Log("No Skill.");
            yield break;
        }
        

        switch (index)
        {
            case 0:
                if (isQCool)
                {
                    Debug.Log("Q is cool now.");
                    yield break;
                }
                break;
            case 1:
                if (isWCool)
                {
                    Debug.Log("W is cool now.");
                    yield break;
                }
                break;
            case 2:
                if (isECool)
                {
                    Debug.Log("E is cool now.");
                    yield break;
                }
                break;
        }

        switch (index)
        {
            case 0:
                isQCool = true; break;
            case 1:
                isWCool = true; break;
            case 2:
                isECool = true; break;
        }
        // �ð��Ǹ� ��Ÿ�� ǥ�õ�
        if (skills[index] == null)
        {
            Debug.Log("NULL Monster.");
            yield break;    
        }
        GameObject shadow = Instantiate(skills[index], player.transform.position, player.transform.rotation); 
        shadow.GetComponent<Controller2D>().isEnable = false;
        shadow.GetComponent<Controller2D>().groundLayer = LayerMask.GetMask("Ground");
        shadow.GetComponent<CallSkill>().Call();
        // ��ų �پ��� ��ü�ı�
        Destroy(shadow);
        Debug.Log("SKILL");

        yield return null;
    }
}
