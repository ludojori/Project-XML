using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5.0f;
    [SerializeField] private bool XAxis = true;
    [SerializeField] private bool YAxis = true;
    [SerializeField] private bool ZAxis = true;

    void Update()
    {
        if (XAxis)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed, Space.Self);
        }
        if (YAxis)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.Self);
        }
        if (ZAxis)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed, Space.Self);
        }
    }
}
