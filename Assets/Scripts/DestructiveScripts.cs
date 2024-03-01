using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiveScripts : MonoBehaviour
{
    public float health = 20f;

    public SpriteRenderer cementSprite;
    public Sprite sDamaged;
    // Start is called before the first frame update
    void Start()
    {
        cementSprite = GetComponent<SpriteRenderer>();
    }

    void Die()
    {
        Destroy(gameObject);    //  destroys the brick
    }

    private void OnCollisionEnter2D(Collision2D actor)
    {
        if (actor.gameObject.CompareTag("Projectile"))
        {
            health -= actor.relativeVelocity.magnitude;
            cementSprite.sprite = sDamaged;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
