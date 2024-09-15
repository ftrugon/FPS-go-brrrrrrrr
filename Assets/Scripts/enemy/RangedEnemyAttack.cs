using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAttack : MonoBehaviour
{
    public Transform attackPoint;
    public bool needToRotate;
    public float attackDistance;
    public GameObject attackPrefab;
    public bool needToRotateGun;
    public Transform gun;
    public float timeBetweenAtacks;
    public float attackSpeed;

    private Transform playerTransform;
    private bool canAtack = true;
    

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        
    }

    private void Update()
    {
        if (needToRotateGun)
        {
            rotateGunVertically();
        }

        needAutoRotation();

        if (GetComponent<EnemyAwareness>().isAgroed && Vector3.Distance(transform.position,playerTransform.position) < attackDistance)
        {
            
            if (needToRotate)
            {
                rotate();
                if (canAtack)
                {
                    enemyAttack();
                }
                
            }
            else
            {
                if (canAtack)
                {
                    enemyAttack();
                }
            }
            
        }
    }

    private void rotateGunVertically()
    {
        Vector3 direction = playerTransform.position - gun.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, targetRotation, Time.deltaTime * 5);
    }

    private void needAutoRotation()
    {
        if (GetComponent<EnemyAwareness>().isAgroed && Vector3.Distance(transform.position, playerTransform.position) < attackDistance)
        {
            GetComponent<NavMeshAgent>().updateRotation = false;
        }
        else
        {
            GetComponent<NavMeshAgent>().updateRotation = true;
        }
    }

    private void rotate()
    {
        Vector3 direction = playerTransform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }


    public void enemyAttack()
    {
        GameObject bullet = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);

        float angleY = UnityEngine.Random.Range(-5 / 2, 5 / 2);
        float angleX = UnityEngine.Random.Range(-5 / 2, 5 / 2);
        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0);

        bullet.GetComponent<Rigidbody>().velocity = rotation * attackPoint.forward * attackSpeed;

        canAtack = false;

        Invoke(nameof(resetCanAttack), timeBetweenAtacks);
    }


    private void resetCanAttack()
    {
        canAtack = true;
    }

}
