using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public Material enemyAgroedMat;
    public float hp = 150;
    public ParticleSystem hitParticle;


    private void Update()
    {

        if (GetComponent<EnemyAwareness>().isAgroed)
        {
            GetComponentInChildren<Renderer>().material = enemyAgroedMat;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            takeDamage(collision.gameObject.GetComponent<bullet>().finalDamage);
            Destroy(collision.gameObject);
        }
    }

    private void takeDamage(float amount)
    {
        hitParticle.Play();

        GetComponent<EnemyAwareness>().isAgroed = true;
        hp -= amount;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
