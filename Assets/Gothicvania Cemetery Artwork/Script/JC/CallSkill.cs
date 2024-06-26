using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallSkill : MonoBehaviour
{
    public UnityEvent Skill;

    void Start()
    {
        if (gameObject.name == "Knight")
            Skill.AddListener(GetComponent<KnightCtrl>().Skill);
        if (gameObject.name == "Wizard3")
            Skill.AddListener(GetComponent<Wizard3Ctrl>().Skill);
    }

    public void Call()
    {
        Skill?.Invoke();
    }
}
