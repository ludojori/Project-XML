using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnim;
    private AudioSource audioSource;
    private bool doorOpen = false;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAnimation()
    {
        if(audioSource)
        {
            audioSource.Play();
        }
        if (!doorOpen)
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("DoorClose", 0, 0.0f);
            doorOpen = false;
        }
    }
}
