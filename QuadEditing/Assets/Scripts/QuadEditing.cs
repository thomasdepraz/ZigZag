using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;

public class QuadEditing : MonoBehaviour
{
    //Mesh
    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private MeshCollider meshCollider;
    public int startQuadsCount;

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();

    public GameObject meshFadeObj;
    private Mesh meshFade;
    private Animator fadeAnim;

    //Marqueurs
    private int bottomPoint = 0;
    private int rightPoint = 1;
    private int leftPoint = 2;
    private int topPoint = 3;

    private int lastBottomPoint;
    private int lastRightPoint;
    private int lastLeftPoint;
    private int lastTopPoint;

    private int mark = 0;

    //Vectors
    Vector3 leftVector;
    Vector3 rightVector;

    //Pickups
    public List<GameObject> pickups = new List<GameObject>();

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 130), new GUIContent("QuadEditing"));
        if(GUI.Button(new Rect(15, 30, 90, 20), new GUIContent("Left")))
        {
            CreateQuadLeft();
        }
        if(GUI.Button(new Rect(15, 55, 90, 20), new GUIContent("Right")))
        {
            CreateQuadRight();
        }
        if (GUI.Button(new Rect(15, 80, 90, 20), new GUIContent("Clear")))
        {
            ClearLastQuad();
        }
        if (GUI.Button(new Rect(15, 105, 90, 20), new GUIContent("Next")))
        {
            CreateNext();
        }
    }

    void Start()
    {
        //Get original mesh
        var filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;

        var filterFade = meshFadeObj.GetComponent<MeshFilter>();
        meshFade = filterFade.mesh;
        fadeAnim = meshFadeObj.GetComponent<Animator>();

        meshCollider = GetComponent<MeshCollider>();

        //Get original mesh vertices and set triangles
        mesh.GetVertices(vertices);
        if(vertices.Count >= 4)
        {
            CreateTriangles();
        }

        // set left and right vectors
        leftVector = vertices[leftPoint] - vertices[bottomPoint];
        rightVector = vertices[rightPoint] - vertices[bottomPoint];

        //Set new vertice and triangles to mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();


        //initialize first quads
        CreateStartQuad();

        //StartCoroutine(CreateQuads());
    }

    private void CreateQuadLeft()
    {
        lastBottomPoint = bottomPoint;
        lastTopPoint = topPoint;
        lastLeftPoint = leftPoint;
        lastRightPoint = rightPoint;

        Vector3 newVerticeA = vertices[leftPoint] + leftVector;
        vertices.Add(newVerticeA);
        bottomPoint = leftPoint;
        leftPoint = vertices.Count - 1;

        Vector3 newVerticeB = vertices[topPoint] + leftVector;
        vertices.Add(newVerticeB);
        rightPoint = topPoint;
        topPoint = vertices.Count - 1;

        CreateTriangles();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();   
    }

    private void CreateQuadRight()
    {
        lastBottomPoint = bottomPoint;
        lastTopPoint = topPoint;
        lastLeftPoint = leftPoint;
        lastRightPoint = rightPoint;

        Vector3 newVerticeA = vertices[topPoint] + rightVector;
        vertices.Add(newVerticeA);
        leftPoint = topPoint;
        topPoint = vertices.Count - 1;

        Vector3 newVerticeB = vertices[rightPoint] + rightVector;
        vertices.Add(newVerticeB);
        bottomPoint = rightPoint;
        rightPoint = vertices.Count - 1;

        CreateTriangles();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    private void CreateTriangles()
    {
        //First triangle
        triangles.Add(bottomPoint);
        triangles.Add(rightPoint);
        triangles.Add(leftPoint);

        //Second triangle
        triangles.Add(rightPoint);
        triangles.Add(topPoint);
        triangles.Add(leftPoint);
    }

    public void ClearLastQuad()
    {
        newTriangles.Clear();

        for(int i = 0; i < 6; i++)
        {
            newTriangles.Add(triangles[i]); 
        }

        triangles.RemoveRange(0, 6);

        meshFade.triangles = newTriangles.ToArray();
        meshFade.vertices = vertices.ToArray();

        fadeAnim.SetBool("fade", true);

        mesh.triangles = triangles.ToArray();
        mesh.vertices = vertices.ToArray();
    }

    private void CreateStartQuad()
    {
        CreateQuadLeft();
        for (int i = 0; i < startQuadsCount -1; i++ )
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                CreateQuadLeft();
            }
            else
            {
                CreateQuadRight();
            }
        }
        // collider calculation
        Mesh newMesh = new Mesh();
        newMesh.vertices = mesh.vertices;
        newMesh.triangles = mesh.triangles;
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = newMesh;
    }

    public void CreateNext()
    {
        if(mark > 2)
        {
            mark--;
            CreateQuadRight();
        }
        else if(mark < -2)
        {
            mark++;
            CreateQuadLeft();      
        }
        else
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                mark++;
                CreateQuadLeft();
            }
            else
            {
                mark--;
                CreateQuadRight();
            }
        }

        // collider calculation
         Mesh newMesh = new Mesh();
         newMesh.vertices = mesh.vertices;
         newMesh.triangles = mesh.triangles;
         mesh.RecalculateBounds();
         meshCollider.sharedMesh = newMesh;
    }

    public IEnumerator CreateQuads()
    {   
        CreateNext();
        float x = Random.Range(0f, 1f);
        if(x < 0.2)
        {
            CreatePickup(vertices[topPoint], vertices[bottomPoint]);
        }
        yield return new WaitForSeconds(0.5f);

        fadeAnim.SetBool("fade", false);
    }

    private void CreatePickup(Vector3 pointA, Vector3 pointB)
    {
        Vector3 origin = (pointA + pointB) / 2;
        origin.z = 0;

        for(int i = 0; i < pickups.Count; i ++ )
        {
            if(pickups[i].activeSelf == false)
            {
                pickups[i].transform.position = origin;
                //reset anim state

                pickups[i].SetActive(true);

                break;
            }
        }
    }
}
