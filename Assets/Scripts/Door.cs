using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public Animator doorAnimator;

    public bool requiresKey;
    public bool reqRed, reqGreen, reqBlue;

    public GameObject areaToSpawn;
    private bool hasOpened = false;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if (requiresKey)
            {

                if (reqRed && other.GetComponent<Inventory>().hasRed)
                {
                    openDoor();
                }

                if (reqBlue && other.GetComponent<Inventory>().hasBlue)
                {
                    openDoor();
                }

                if (reqGreen && other.GetComponent<Inventory>().hasGreen)
                {
                    openDoor();
                }

            }
            else
            {
                openDoor();
            }

        }
    }

    void openDoor()
    {
        if (!hasOpened) {
            hasOpened = true;

            doorAnimator.SetTrigger("DoorOpen");
            
            if (areaToSpawn != null)
            {
                areaToSpawn.SetActive(true);
            }
        }
    }
}
