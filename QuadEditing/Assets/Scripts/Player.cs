using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    private Vector3 rotation;
    private float distance = 0;
    public float minDistance;
    private int playerScore;
    public GameObject raycastingObject;
    public TextMeshProUGUI scoreText;

    private bool startMoving = false;
    private bool canMove = true;

    public QuadEditing terrain;

    // Update is called once per frame
    void Update()
    {
        if(startMoving)
        {
            Move();

            if(Input.GetMouseButtonDown(0) && canMove)
            {
                Rotate();
            }
            if(distance >= 1)
            {
                distance = 0;
                terrain.ClearLastQuad(speed);
                terrain.StartCoroutine(terrain.CreateQuads(speed));
            }

            DetectPickup();
            DetectTerrain();
        }

        if(Input.GetMouseButtonDown(0) && !startMoving)
        {
            startMoving = true;
        }
        
    }

    void Move()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        distance += Time.deltaTime * speed;
    }

    void Rotate()
    {

        if(rotation == new Vector3(0,0,0))
        {
            rotation = new Vector3(0, 0, -90);
            transform.Rotate(rotation);
        }
        else 
        {
            rotation = new Vector3(0, 0, 90);
            transform.Rotate(rotation);
            rotation = new Vector3(0, 0, 0);
        }
       
    }

    void DetectPickup()
    {
        Physics.Raycast(transform.position, transform.up, out RaycastHit hitInfo);
        if(hitInfo.collider != null)
        {
            if((transform.position - hitInfo.transform.position).magnitude <= minDistance)
            {
                hitInfo.collider.gameObject.SetActive(false);
                playerScore++;
                scoreText.text = playerScore.ToString();
                speed += 0.05f;

                if(speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
        }
    }

    void DetectTerrain()
    {
        Physics.Raycast(raycastingObject.transform.position, raycastingObject.transform.forward, out RaycastHit hit);
        if(hit.collider == null)
        {
            canMove = false;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
