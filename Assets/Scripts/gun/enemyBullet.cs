using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{

    private float timeBeforeDissapear = 2;

    private void Awake()
    {
        Destroy(gameObject, timeBeforeDissapear);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "EnemyBullet")
        {
            Destroy(gameObject);
        }

        
    }
}
