using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 10;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = "Interact";

    private DoorController raycastedObj;

    [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;
    [SerializeField] private string interactibleTag = "DungeonDoor";

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(cursorRay, out hit, rayLength, mask))
        {
            if (hit.collider.CompareTag(interactibleTag))
            {
                raycastedObj = hit.collider.gameObject.GetComponent<DoorController>();
            }

            if (Input.GetKeyDown(openDoorKey))
            {
                raycastedObj.PlayAnimation();
            }
        }
    }
}
