using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public bool isGreen, isBlue, isRed;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isRed)
            {
                other.GetComponent<Inventory>().hasRed = true;
            }

            if (isGreen)
            {
                other.GetComponent<Inventory>().hasGreen = true;
            }

            if (isBlue)
            {
                other.GetComponent<Inventory>().hasBlue = true;
            }


            Destroy(gameObject);
        }
    }
}
