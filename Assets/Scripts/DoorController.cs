using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnim;
    private AudioSource audioSource;
    private bool doorOpen = false;

    private void Start()
    {
        doorAnim = gameObject.GetComponent<Animator>();
        if(doorAnim == null)
        {
            doorAnim = gameObject.AddComponent<Animator>();
            Debug.Log("Animator for object " + gameObject.name + " missing. Creating one now.");
        }
        
        if (!gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }
        
        audioSource = gameObject.GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("Audio source for object " + gameObject.name + " missing. Creating one now.");
        }
        if (!(audioSource.clip = Resources.Load("Old_Door_Creaking-Sound", typeof(AudioClip)) as AudioClip))
        {
            Debug.Log("Failed to load audio clip.");
        }
        gameObject.isStatic = false;
        gameObject.tag = "DungeonDoor";
        gameObject.layer = 8; // Interact Layer
    }

    public void PlayAnimation()
    {
        string doorName = name;
        doorAnim.runtimeAnimatorController = Resources.Load("Door_Wooden_Round_Right", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        if (audioSource)
        {
            audioSource.Play();
        }
        if (!doorOpen)
        {
            switch (doorName)
            {
                case "Room1 Entrance":             doorAnim.Play("Room1EntranceDoorOpen", 0, 0.0f); break;
                case "Room1 Exit":                 doorAnim.Play("Room1ExitDoorOpen", 0, 0.0f); break;
                case "Room_2_Intermediary_Gate_1": doorAnim.Play("Room2GateOpen", 0, 0.0f); break;
                case "Room3_Entrance":             doorAnim.Play("Room3EntranceDoorOpen", 0, 0.0f); break;
                case "Tower_Door":                 doorAnim.Play("TowerDoorOpen", 0, 0.0f); break;
                case "Barrel_Secret_Entrance":     doorAnim.Play("BarrelOpen", 0, 0.0f); break;
                default: Debug.Log("Unidentified door, cannot open."); break;
            }
            doorOpen = true;
        }

    }
}
