using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuizGame;

public class DoorController : MonoBehaviour
{
    private Animator doorAnim;
    private AudioSource audioSource;
    [HideInInspector] public bool doorOpen = false;

    private void Start()
    {
        doorAnim = gameObject.GetComponent<Animator>();
        if(doorAnim == null)
        {
            doorAnim = gameObject.AddComponent<Animator>();
            // Debug.Log("Animator for object " + gameObject.name + " missing. Creating one now.");
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
            // Debug.Log("Audio source for object " + gameObject.name + " missing. Creating one now.");
        }
        if (!(audioSource.clip = Resources.Load("Old_Door_Creaking-Sound", typeof(AudioClip)) as AudioClip))
        {
            Debug.Log("Failed to load audio clip for " + name + ".");
        }
        gameObject.isStatic = false;
        gameObject.tag = "DungeonDoor";
        gameObject.layer = 8; // Interact Layer
    }

    public void PlayAnimation()
    {
        string doorName = name;
        doorAnim.runtimeAnimatorController = Resources.Load("Door_Wooden_Round_Right", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        if (!doorOpen)
        {
            switch (doorName)
            {
                case "Room1 Entrance":
                    if(audioSource)audioSource.Play();
                    doorAnim.Play("Room1EntranceDoorOpen", 0, 0.0f);
                    doorOpen = true;
                    break;
                case "Room3_Door_Wooden_Extra":
                    if (audioSource) audioSource.Play();
                    doorAnim.Play("Room3EntranceDoorOpen", 0, 0.0f);
                    doorOpen = true;
                    break;
                case "Room3_Basement_Door_Extra":
                    if (audioSource) audioSource.Play();
                    doorAnim.Play("Room1EntranceDoorOpen", 0, 0.0f);
                    doorOpen = true;
                    break;
                case "Room3_DoorGate_Wooden_Extra_Right":
                    if (audioSource) audioSource.Play();
                    doorAnim.Play("Room3EntranceDoorOpen", 0, 0.0f);
                    doorOpen = true;
                    break;
                case "Room3_DoorGate_Wooden_Extra_Left":
                    if (audioSource) audioSource.Play();
                    doorAnim.Play("TowerDoorOpen", 0, 0.0f);
                    doorOpen = true;
                    break;
                case "Room3_Door_Wooden_Round_Extra_2":
                    if (audioSource) audioSource.Play();
                    doorAnim.Play("Room1ExitDoorOpen", 0, 0.0f);
                    doorOpen = true;
                    break;
                default: Debug.Log("Cannot open door."); doorOpen = true; break;
            }
        }

    }
}
