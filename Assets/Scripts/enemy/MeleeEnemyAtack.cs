using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAtack : MonoBehaviour
{
    public Animator animator;
    public int meleeDamage; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.gameObject.GetComponent<PlayerHealth>() != null)
            {
                other.gameObject.GetComponent<PlayerHealth>().damagePlayer(meleeDamage);
            }


            animator.SetTrigger("attack");

            other.attachedRigidbody.AddForce(transform.forward * 20, ForceMode.Impulse);
        }
    }
}
