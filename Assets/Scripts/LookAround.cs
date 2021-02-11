using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;

    [Tooltip("The player this camera is attached to.")]
    public Transform player;

    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    private float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = mouseSensitivity * Input.GetAxis("Mouse X") * Time.deltaTime;
        mouseY = mouseSensitivity * Input.GetAxis("Mouse Y") * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        player.Rotate(Vector3.up * mouseX);
    }
}
