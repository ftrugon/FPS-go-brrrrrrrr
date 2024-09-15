using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{

    public GameObject thingToRotate;
    public Transform bulletSpawn;
    public GameObject enemyBulletPrefab;
    public ParticleSystem onHitParticles;

    private Transform playerTransform;

    private EnemyAwareness awareness;

    public float bulletSpeed = 100; 
    private float rotationSpeed = 5;
    private bool canShoot = true;
    private float timeBetweenShoots = 0.20f;
    private float pelletsPerShoot = 3;
    private float hp = 100;


    void Start()
    {
        awareness = GetComponent<EnemyAwareness>();
        playerTransform = FindObjectOfType<PlayerController>().transform;

    }

    void Update()
    {
        if (awareness.isAgroed && thingToRotate != null)
        {
            Vector3 direction = playerTransform.position - thingToRotate.transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);


            RaycastHit hit;

            if (Physics.Raycast(bulletSpawn.position,direction,out hit))
            {
                if(hit.transform == playerTransform)
                {
                    if (canShoot)
                    {
                        Shoot();
                    }
                }
            }
        }
    }


    private void Shoot()
    {
        for (int i = 0; i < pelletsPerShoot; i++)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            float angleY = UnityEngine.Random.Range(-5 / 2, 5 / 2);
            float angleX = UnityEngine.Random.Range(-5 / 2, 5 / 2);
            Quaternion rotation = Quaternion.Euler(angleX, angleY, 0);

            bullet.GetComponent<Rigidbody>().velocity = rotation * bulletSpawn.forward * bulletSpeed;

            canShoot = false;

            Invoke(nameof(resetCanShoot), timeBetweenShoots);
        }
        
    }


    private void resetCanShoot()
    {
        canShoot = true;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7 && thingToRotate != null)
        {
            onHitParticles.Play();
            takeDamage(collision.gameObject.GetComponent<bullet>().finalDamage);
            Destroy(collision.gameObject);
        }
    }


    private void takeDamage(float amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            canShoot = false ;
            Destroy(thingToRotate);
        }
    }

}
