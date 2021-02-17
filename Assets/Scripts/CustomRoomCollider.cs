using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomCollider : MonoBehaviour
{
    private bool isQuizGamePointsCanvasActive = false;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Canvas");
        if (collision.gameObject == GameObject.Find("CustomFPSController"))
        {
            Debug.Log("Canvas called for gameObject");
            if (!isQuizGamePointsCanvasActive)
            {
                Debug.Log("Canvas called for instantiation.");
                Canvas isQuizGamePointsCanvas = Resources.Load("QuizGamePointsCanvas", typeof(Canvas)) as Canvas;
                Instantiate(isQuizGamePointsCanvas, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Debug.Log("Canvas here");
                isQuizGamePointsCanvasActive = true;
            }
        }
    }
}
