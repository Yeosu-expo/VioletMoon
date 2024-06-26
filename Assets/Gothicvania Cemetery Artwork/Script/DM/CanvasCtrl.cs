using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasCtrl : MonoBehaviour
{
    public GameObject canvas;
    public GameObject startPanel;
    public GameObject escPanel;
    public GameObject playPanel;
    public GameObject endPanel;
    public GameObject clearTime;
    public GameObject playTime;
    public GameObject DiePanel;
    public GameObject DiePlayTime;

    private static CanvasCtrl _instance;
    public RectTransform imageRectTransform;
    public RectTransform buttonRectTransform;
    public float imageSpeed = 1000f;
    public float buttonSpeed = 2000f;

    public static CanvasCtrl Instance
    {
        get
        {
            // ?????????? ?????? ?????? ??????????
            if (_instance == null)
            {
                _instance = FindObjectOfType<CanvasCtrl>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(CanvasCtrl).ToString());
                    _instance = singletonObject.AddComponent<CanvasCtrl>();
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

    public void StartMoveAndHide()
    {
        StartCoroutine(MoveImageAndButton());
    }

    public IEnumerator MoveImageAndButton()
    {
        Vector3 imageStartPos = imageRectTransform.anchoredPosition;
        Vector3 buttonStartPos = buttonRectTransform.anchoredPosition;
        Vector3 imageEndPos = imageStartPos + new Vector3(Screen.width, 0, 0);
        Vector3 buttonEndPos = buttonStartPos + new Vector3(Screen.width * -1, 0, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (imageSpeed / 1000f));
            imageRectTransform.anchoredPosition = Vector3.Lerp(imageStartPos, imageEndPos, t);

            t = Mathf.Clamp01(elapsedTime / (buttonSpeed / 1000f));
            buttonRectTransform.anchoredPosition = Vector3.Lerp(buttonStartPos, buttonEndPos, t);

            yield return null; // ?????????? ????????
        }

        // ???????????? ???? ?? ?????? ???? ?????? ????
        imageRectTransform.anchoredPosition = imageEndPos;
        buttonRectTransform.anchoredPosition = buttonEndPos;
        SetStartPannelActive(false);
    }

    public void SetCanvasActive(bool sign)
    {
        canvas.SetActive(sign);
    }

    public void SetStartPannelActive(bool sign)
    {
        startPanel.SetActive(sign);
    }

    public void SetEscPannelActive(float time)
    {
        if (escPanel.activeSelf) 
        {
            escPanel.SetActive(false);
            playPanel.SetActive(true);
        } 
        else escPanel.SetActive(true);

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formmattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        playTime.GetComponent<TextMeshProUGUI>().text = "Play Time: " + formmattedTime;
    }

    public void SetPlayPanelActive(bool sign)
    {
        playPanel.SetActive(sign);
    }

    public void SetEndPanelActive(bool sign, float time)
    {
        endPanel.SetActive(sign);

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formmattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        clearTime.GetComponent<TextMeshProUGUI>().text = "Clear Time: " + formmattedTime;
    }

    public void DieEndPanelActive(bool sign, float time)
    {
        DiePanel.SetActive(sign);

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formmattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        DiePlayTime.GetComponent<TextMeshProUGUI>().text = "Play Time: " + formmattedTime;
    }

    public void SetPanel()
    {
        startPanel.SetActive(true);
        escPanel.SetActive(false);
        playPanel.SetActive(false);
        endPanel.SetActive(false);
        DiePanel.SetActive(false);
    }
}
