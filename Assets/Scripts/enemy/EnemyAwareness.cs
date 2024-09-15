using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 10f;
    public bool isAgroed;
    private Transform playerTransform;


    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        var dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist < awarenessRadius)
        {
            isAgroed = true;
        }


        if (dist > awarenessRadius)
        {
            isAgroed = false;
        }
    }
}
