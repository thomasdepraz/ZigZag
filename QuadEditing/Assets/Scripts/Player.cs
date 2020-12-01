using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public float speed;
    private Vector3 rotation;
    private float distance = 0;
    public float minDistance;
    private int playerscore;
    public GameObject raycastingObject;

    public QuadEditing terrain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Input.GetMouseButtonDown(0))
        {
            Rotate();
        }
        if(distance >= 1)
        {
            distance = 0;
            terrain.ClearLastQuad();
            terrain.StartCoroutine(terrain.CreateQuads());
        }

        DetectPickup();
        DetectTerrain();

        
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
                playerscore++;
                Debug.Log(playerscore);
                if (playerscore % 3 == 0)
                {
                    speed += 0.5f;
                }
            }
        }
    }

    void DetectTerrain()
    {
        Debug.DrawRay(raycastingObject.transform.position, raycastingObject.transform.forward);
        Physics.Raycast(raycastingObject.transform.position, raycastingObject.transform.forward, out RaycastHit hit);
        if(hit.collider == null)
        {
            Time.timeScale = 0;
        }
    }
}
