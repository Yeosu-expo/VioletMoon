using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallSkill : MonoBehaviour
{
    public UnityEvent Skill;

    void Start()
    {
    }

    public void Call()
    {

        Debug.Log(gameObject.name);
        if (gameObject.name == "Knight(Clone)")
        {
            Skill.AddListener(GetComponent<KnightCtrl>().Skill);
        }
        if (gameObject.name == "Wizard3(Clone)")
        {
            Skill.AddListener(GetComponent<Wizard3Ctrl>().Skill);
        }
        Skill?.Invoke();
    }
}
