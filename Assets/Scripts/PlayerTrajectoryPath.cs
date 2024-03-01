using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrajectoryPath : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    private Rigidbody2D rigidBody;
    private Vector3 forceAtPlayer;
    public float forceFactor;

    public GameObject trajectoryDot;    //  the material that we are going to use for the dot
    private GameObject[] trajectoryDots;    //  the generated path made out of dots
    public int numberOfDots;    //  number of trajectory dots that will appear

    public GameObject nextProjectile;   //  reference the next or new active projectile game object

    public GameObject hookPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        trajectoryDots = new GameObject[numberOfDots];
        this.gameObject.transform.position = new Vector3(hookPosition.transform.position.x, hookPosition.transform.position.y, hookPosition.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //  left click
        {
            //  click
            startPos = gameObject.transform.position;
            for (int i = 0; i < numberOfDots; i++)
            {
                trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
            }
        }

        if (Input.GetMouseButton(0))
        {
            //  drag
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 5);
            gameObject.transform.position = endPos;
            forceAtPlayer = endPos - startPos;
            for (int i = 0; i < numberOfDots; i++)
            {
                trajectoryDots[i].transform.position = CalculatePosition(i * 0.1f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //  release
            rigidBody.gravityScale = 1;
            rigidBody.velocity = new Vector2(-forceAtPlayer.x * forceFactor, -forceAtPlayer.y * forceFactor);
            for (int i = 0; i < numberOfDots; i++)
            {
                Destroy(trajectoryDots[i]);
            }
            StartCoroutine(Release());
        }
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        return new Vector2(endPos.x, endPos.y) +
            new Vector2(-forceAtPlayer.x * forceFactor, -forceAtPlayer.y * forceFactor) * elapsedTime +
            0.5f * Physics2D.gravity * elapsedTime * elapsedTime;
    }

    IEnumerator Release()
    {
        yield return new WaitForSeconds(3f);
        if (nextProjectile != null) //  checks if there are still projectiles left
        {
            nextProjectile.SetActive(true);
        }
        else
        {
            Enemy.enemiesAlive = 0;
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   //  reloads the current scene
        }

        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }
}
