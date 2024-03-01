using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;

    public float health = 5; // enemy's base health
    public static int enemiesAlive = 0; //  variable that stores and counts how many enemies are present in the scene
    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive++; //  counts how many enemy game object are present in the scene upon starting
    }

    private void OnCollisionEnter2D(Collision2D actor)
    {
        //  the enemy will die if the enemy is hit with a velocity.magnitude greater than 5, this is the player's projectile velocity
        if (actor.relativeVelocity.magnitude > health)
        {
            Die();
        }
    }

    void Die()
    {
        enemiesAlive--; //  subtracts the defeated enemy from the total count

        //  this condition will check if there are any enemies left
        if (enemiesAlive <= 0)
        {
            SceneManager.LoadScene("Lvl" + (gameManager.levelAt + 1));
        }
        Destroy(gameObject);    //  destroys the game object where the enemy script is attached
    }
}
