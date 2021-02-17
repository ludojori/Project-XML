using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRoomCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("CustomFPSController"))
        {
            Destroy(GameObject.Find("QuizGamePointsCanvas(Clone)"));

            // Instantiate Canvas for Showing Points
            Canvas canvas = Resources.Load("QuizGamePointsCanvas", typeof(Canvas)) as Canvas;
            Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            canvas.name = "QuizGamePointsCanvas";
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObject.Find("CustomFPSController"))
        {
            // Destroy Canvas on Exit
            Destroy(GameObject.Find("QuizGamePointsCanvas(Clone)"));
        }
    }

}
