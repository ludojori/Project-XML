using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomRoomCollider : MonoBehaviour
{
    private static int index = 0;
    private void Start()
    {
        //GameObject.Find("CustomFPSController").GetComponent<CapsuleCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("CustomFPSController"))
        {
            
            //Destroy(GameObject.Find("QuizGamePointsCanvas(Clone)"));

            // Instantiate Canvas for Showing Points
            Canvas canvas = Resources.Load("QuizGamePointsCanvas", typeof(Canvas)) as Canvas;
            Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);
            canvas.name = "QuizGamePointsCanvas" + index;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            

            Debug.Log("CustomRoomCollider.cs attached to and called at: " + gameObject.name);
            string points = UIManager.pointsCount.ToString();
            Text instanceText = GameObject.Find("QuizGamePointsCanvas(Clone)"+ index + "/PointsDisplay/PointsCounter").GetComponent<Text>();
            if (instanceText != null)
            {
                instanceText.text = UIManager.pointsCount.ToString();
            }
            index++;
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
