using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GMScript : MonoBehaviour
{
    private static GMScript _instance;
    public GameObject hero;
    public bool isStart = false;
    bool isChecking = false;
    float startTime = 0;
    float clearTime = 0;
    bool isEndTimeChecked = false;
    public bool isClear = false;
    public bool isPotalIn = false;

    public bool isDie = false;
    public bool isDieCheck = false;
    

    public static GMScript Instance
    {
        get
        {
            // ?????????? ?????? ?????? ??????????
            if (_instance == null)
            {
                _instance = FindObjectOfType<GMScript>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PostProcessingCtrl).ToString());
                    _instance = singletonObject.AddComponent<GMScript>();
                }
            }
            return _instance;
        }
    }

    // ?? ???????? ?????????? ???? ?? ???????? ?????? ??????
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        hero.SetActive(false);
        CanvasCtrl.Instance.SetPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart && !isChecking)
        {
            startTime = Time.time;
            isChecking = true;
        }
        if(Input.GetKeyUp(KeyCode.Escape)) { InEsc(); }
        if(isClear && isPotalIn)
        {
            if(!isEndTimeChecked)
            {
                isEndTimeChecked = true;
                clearTime = Time.time - startTime;
            }
            
            CanvasCtrl.Instance.SetEndPanelActive(true, clearTime);
        }
        if (isDie && !isDieCheck)
        {
            isDieCheck = true;
            float playTime = Time.time - startTime;
            CanvasCtrl.Instance.DieEndPanelActive(true, playTime);
        }
    }

    public void OnClickStartBtn()
    {
        CanvasCtrl.Instance.StartMoveAndHide(); 
        hero.SetActive(true);
        isStart = true;
        CanvasCtrl.Instance.SetPlayPanelActive(true);
    }

    public void InEsc()
    {
        CanvasCtrl.Instance.SetPlayPanelActive(false);
        if (hero.activeSelf) hero.SetActive(false);
        else hero.SetActive(true);

        float playTime = Time.time - startTime;
        CanvasCtrl.Instance.SetEscPannelActive(playTime);
    }

    public void OnClickCancelOnEsc()
    {
        if (!CanvasCtrl.Instance.playPanel.activeSelf)
            CanvasCtrl.Instance.SetPlayPanelActive(true);
        hero.SetActive(true);
        float playTime = Time.time - startTime;
        CanvasCtrl.Instance.SetEscPannelActive(playTime);
    }

    public void OnClickRestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
