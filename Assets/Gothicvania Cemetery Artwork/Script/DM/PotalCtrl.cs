using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalCtrl : MonoBehaviour
{
    [SerializeField]private bool isIn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isIn)
        {
            if (GMScript.Instance.isClear)
            {
                GMScript.Instance.isPotalIn = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("!");
        if (collision.tag.Equals("Player"))
            isIn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("!!");
        if (collision.tag.Equals("Player"))
            isIn = false;
    }
}
