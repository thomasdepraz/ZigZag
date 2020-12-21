using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("isDead", true);
        Debug.Log("heho");
    }

    public void GetAnimationEvent(string eventName)
    {

        if(eventName == "endDeath")
        {
            gameObject.SetActive(false);
        }
    }
}
