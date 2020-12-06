using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    private Vector3 rotation;
    private float distance = 0;
    public float minDistance;
    private int playerScore;
    public GameObject raycastingObject;
    public Text scoreText;

    bool startMoving = false;

    public QuadEditing terrain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startMoving)
        {
            Move();

            if(Input.GetMouseButtonDown(0))
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
                scoreText.text = "Score : " + playerScore;
                if (playerScore % 3 == 0)
                {
                    speed += 0.5f;
                }
            }
        }
    }

    void DetectTerrain()
    {
        Physics.Raycast(raycastingObject.transform.position, raycastingObject.transform.forward, out RaycastHit hit);
        if(hit.collider == null)
        {
            gameObject.SetActive(false);
        }
    }
}
