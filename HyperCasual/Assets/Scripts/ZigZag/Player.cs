using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

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
    public GameObject startText;
    public GameObject restartText;

    [HideInInspector] public bool startMoving = false;
    [HideInInspector] public bool canMove = true;

    public QuadEditing terrain;
    public GameObject pickupParticle;
 


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
                distance--;
                //distance = 0;
                terrain.ClearLastQuad(speed);
                terrain.StartCoroutine(terrain.CreateQuads(speed));
            }

            DetectPickup();
            DetectTerrain();
        }

        if(Input.GetMouseButtonDown(0) && !startMoving)
        {
            startMoving = true;
            startText.SetActive(false);
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
                Instantiate(pickupParticle, hitInfo.transform.position, Quaternion.identity);
                hitInfo.transform.gameObject.SetActive(false);
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
        gameObject.GetComponent<Animator>().SetBool("isDead", true);
        yield return new WaitForSeconds(0.5f);
        restartText.SetActive(true);
        gameObject.SetActive(false);
    }
}
