using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject skeleton;

    public float posX, posY;
    public int test = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (test > 0)
        {
            Spawn(skeleton, posX, posY);
            test--;
        }
    }

    public void Spawn(GameObject monster, float posX, float posY)
    {
        Vector3 pos = new(posX, posY, 0);
        Instantiate(monster, pos, Quaternion.identity);
    }
}