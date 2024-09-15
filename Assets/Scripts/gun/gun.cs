using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gun : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerRb;
    public ParticleSystem muzzleFlash;

    //bullet spawning
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    public int pelletsPerShot = 1;
    
    //some properties
    private float bulletSpeed = 100 ;
    public float knockbackForce = 25;
    private float spreadAngle = 15f;

    public float timeBetweenShot = 1f;
    private bool readyToShoot = true;

    private Animator animator;


    public float gunShotRadius = 3000;

    public LayerMask enemyLayerMask;


    private void Start()
    {
        animator = GetComponent<Animator>();

        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        myInput();
    }

    private void myInput()
    {
        if (InputManager.hasShooted() && readyToShoot)
        {
            shoot();
            applyKnockback();
        }
    }


    private void shoot()
    {


        Collider[] enemyColliders;
        enemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);


        foreach (var enemyCollider in enemyColliders)
        {
            EnemyAwareness enemyAwareness = enemyCollider.GetComponent<EnemyAwareness>();

            if (enemyAwareness != null)
            {
                enemyAwareness.isAgroed = true;
            }
        }



        animator.SetTrigger("Shoot");   

        readyToShoot = false;
        muzzleFlash.Play();

        for (int i = 0; i < pelletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            
            float angleY = UnityEngine.Random.Range(-spreadAngle/2 , spreadAngle / 2);
            float angleX = UnityEngine.Random.Range(-spreadAngle /2,spreadAngle /2);
            Quaternion rotation = Quaternion.Euler(angleX, angleY, 0);


            bullet.GetComponent<Rigidbody>().velocity = rotation * bulletSpawn.forward * bulletSpeed;

        }
        Invoke(nameof(setReadyToTrue), timeBetweenShot);

    }


    private void setReadyToTrue()
    {
        readyToShoot = true;
    }

    private void applyKnockback()
    {
        playerRb.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
    }
}
