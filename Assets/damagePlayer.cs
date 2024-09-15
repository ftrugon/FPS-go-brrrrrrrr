using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagePlayer : MonoBehaviour
{

    public int bulletDamage;
    public int bigBulletDamage;
    public int minYDistance;
   

    private PlayerHealth health;

    private void Start()
    {
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if(transform.position.y < minYDistance)
        {
            health.damagePlayer(100);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            health.damagePlayer(bulletDamage);
        }
        else if (collision.gameObject.tag == "EnemyBigBullet")
        {
            health.damagePlayer(bigBulletDamage);
        }
    }
}
