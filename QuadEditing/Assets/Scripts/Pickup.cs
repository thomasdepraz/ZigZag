using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Animator anim;
    public float value;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //appear anim
        anim.SetBool("appear", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPickup()
    {
        //ajouter le score
        anim.SetBool("disappear", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPickup();
        }
    }


    public void GetAnimationEvent(string eventName)
    {
        if(eventName == "endAppear")
        {
            anim.SetBool("appear", false);
        }
        if(eventName == "endDisappear")
        {
            anim.SetBool("disappear", false);
            gameObject.SetActive(false);
        }
    }
}
