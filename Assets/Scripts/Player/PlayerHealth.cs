using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;


    public int maxArmor;
    public int armor;

    private int numberOfDeaths;

    void Start()
    {
        health = maxHealth;
    }

    public void damagePlayer(int damage)
    {

        if (armor > 0)
        {

            if (armor >= damage)
            {
                armor -= damage;
            }
            else if (armor < damage)
            {
                int remainingDamage = damage - armor;

                armor = 0;

                health -= remainingDamage;
            }

        }
        else
        {
            health -= damage;
        }

        if (health <= 0)
        {
            Debug.Log("Player has died");

            numberOfDeaths ++;
            Scene currentSecene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentSecene.buildIndex);

        }
    }


    public void giveHealth(int amount, GameObject pickup)
    {

        if (health < maxHealth)
        {
            health += amount;
            Destroy(pickup);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }


    public void giveArmor(int amount, GameObject pickup)
    {
        if (armor < maxArmor)
        {
            armor += amount;
            Destroy(pickup);
        }

        if (armor > maxArmor)
        {
            health = maxArmor;
        }

    }
}
