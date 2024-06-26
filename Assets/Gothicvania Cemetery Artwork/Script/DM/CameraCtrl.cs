using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject hero;
    public GameObject hero_right;
    public GameObject hero_left;
    public GameObject hero_mid;
    public float y_pos;

    private int moveInterval = 1;
    private float CameraSpeed = 1;
    public GameObject boundary;

    public GameObject background;
    public GameObject yard;
    public List<GameObject> yardList;
    public GameObject mountain;
    public List<GameObject> mountainList;

    public float yardDelay = 1;
    public float mountainDelay = 2;
    private GameObject yardTmp;
    private GameObject prevYard;
    private GameObject mountainTmp;
    private GameObject prevMountain;
    // Start is called before the first frame update

    private Vector2 velocity = Vector2.zero;
    public float smoothTime = 0.3f;
    private void Start()
    {
        yardList = new List<GameObject>();
        prevYard = yard;
        yardList.Add(yard);
        mountainList = new List<GameObject>();
        prevMountain = mountain;
        mountainList.Add(mountain);
        SetResolution(); // ?????? ???? ?????? ????

        CameraSpeed = 1;
    }

    private void Update()
    {
        MovingCamera();
        Vector3 pos = transform.position;
        pos.z = background.transform.position.z;
        background.transform.position = pos;
    }

    /* ?????? ???????? ???? */
    public void SetResolution()
    {
        int setWidth = 1920; // ?????? ???? ????
        int setHeight = 1080; // ?????? ???? ????

        int deviceWidth = Screen.width; // ???? ???? ????
        int deviceHeight = Screen.height; // ???? ???? ????

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution ???? ?????? ????????

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ?????? ?????? ???? ?? ?? ????
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ?????? ????
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ?????? Rect ????
        }
        else // ?????? ?????? ???? ?? ?? ????
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ?????? ????
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ?????? Rect ????
        }
    }

    public void MovingCamera()
    {
        GameObject target;
        Vector3 dir = new Vector3();
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) { target = hero_right; dir = Vector3.left; }
        else if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) { target = hero_left; dir = Vector3.right; }
        else target = hero_mid;

        Vector3 targetPos = new Vector3(target.transform.position.x, y_pos, 0);
        Vector2 pos = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        Vector3 pos2 = pos;
        pos2.z = -10;
        transform.position = pos2;

        MovingMap(dir);
        
    }

    public void MovingMap(Vector3 dir)
    {
        MovingYard(dir);
        MovingMountain(dir);
    }

    private void MovingYard(Vector3 dir)
    {
        Vector3 viewport = Camera.main.WorldToViewportPoint(prevYard.transform.position);

        if (viewport.x != 0.5 && yardTmp == null)  // yard?? ?????? ?????? ???? ???? ?????? ????
        {
            yardTmp  = Instantiate(prevYard, prevYard.transform.position + new Vector3(18.0411092f, 0,0), Quaternion.identity);
            yardList.Add(yardTmp);
        }
        
        if (viewport.x <= -0.5)
        {
            prevYard = yardTmp;
            yardTmp = null;
        }

        foreach(var view in yardList)
        {
            view.transform.Translate(dir.normalized*yardDelay*Time.deltaTime);
        }
    }

    private void MovingMountain(Vector3 dir)
    {
        Vector3 viewport = Camera.main.WorldToViewportPoint(prevMountain.transform.position);

        if (viewport.x != 0.5 && mountainTmp == null)  // yard?? ?????? ?????? ???? ???? ?????? ????
        {
            mountainTmp = Instantiate(prevMountain, prevMountain.transform.position + new Vector3(18.039782f, 0, 0), Quaternion.identity);
            mountainList.Add(mountainTmp);
        }

        if (viewport.x <= -0.5)
        {
            prevMountain = mountainTmp;
            mountainTmp = null;
        }

        foreach (var view in mountainList)
        {
            view.transform.Translate(dir.normalized * mountainDelay * Time.deltaTime);
        }
    }
}
