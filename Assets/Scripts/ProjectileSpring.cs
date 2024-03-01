using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileSpring : MonoBehaviour
{
    public Rigidbody2D rb;  //  this is current projectile's rigid body
    public Rigidbody2D hook;    //  this is the rigid body attached to the hook game object

    public float releaseTime = 0.15f;   //  the delay when releasing the hold on the mouse
    public float maxDragDistance = 2f;  //  max drag distance of the projectile from the hook

    public GameObject nextProjectile;   //  references a new active projectile

    public bool isPressed = false;  //  checks if projectile is pressed or being interacted
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //  checks the position of the mouse cursor on the screen

            // sets the max drag distance of the projectile from the hook
            if (Vector3.Distance(mousePos, hook.position) > maxDragDistance)
            {
                rb.position = hook.position + (mousePos - hook.position).normalized * maxDragDistance;
            }
            else
            {
                rb.position = mousePos;
            }
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
        rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        rb.isKinematic = false;

        StartCoroutine(Release());
    }

    //  this simulates the slingshot feature
    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);

        GetComponent<SpringJoint2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(2f);
        //  checks if there are projectiles available
        if (nextProjectile != null) //  if a projectile is referenced on the inspector
        {
            nextProjectile.SetActive(true);
        }
        else
        {
            Enemy.enemiesAlive = 0;
            yield return new WaitForSeconds(2f);
            Debug.Log("Retrying");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
