using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{


    public bool isHealth, isArmor;
   

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();

        if (other.CompareTag("Player"))
        {
            if (isHealth)
            {
                ph.giveHealth((ph.maxHealth - ph.health) / 2, gameObject);
            }

            if (isArmor)
            {
                ph.giveArmor((ph.maxArmor - ph.armor) / 2, gameObject);
            }
        }
    }
}
