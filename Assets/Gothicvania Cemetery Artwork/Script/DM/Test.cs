using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool isDo = false;
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDo && GMScript.Instance.isStart)
        {
            Skill.Instance.Imbibition(this.gameObject, 0);
            isDo = true;
        }
    }
}
