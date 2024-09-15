using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private float timeBeforeDissapear = 2f;
    private Vector3 startPosition;
    public float distance;



    private float baseDamage = 5f;
    private float maxMultiplier = 2f;
    private float minMultiplier = 0.5f;
    private float maxRange = 50f;

    public float finalDamage;

    private void Awake()
    {
        Destroy(gameObject,timeBeforeDissapear);
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        distance = Vector3.Distance(startPosition,transform.position);

    }


    private void OnCollisionEnter(Collision collision)
    { 
        finalDamage = baseDamage * calculateMultiplier();
    }

    float calculateMultiplier()
    {

        if (distance >= maxRange)
        {
            return minMultiplier;
        }

        float multiplier = maxMultiplier - ((maxMultiplier - minMultiplier) * (distance / maxRange));

        return Mathf.Clamp(multiplier, minMultiplier, maxMultiplier);
    }

}
