using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava : MonoBehaviour
{

    public int lavaDamage = 30 ;
    private bool canHit = true;


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null && canHit)
        {
            other.GetComponent<PlayerHealth>().damagePlayer(lavaDamage);
            canHit = false;
            Invoke(nameof(canHitTrue), 0.5f);
        }
    }
    private void canHitTrue()
    {
        canHit = true;
    }
}
