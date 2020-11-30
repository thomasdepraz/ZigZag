using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Vector3 rotation;
    private float distance = 0;

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
}
