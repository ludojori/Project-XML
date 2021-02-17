using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QuizGame;

public class UIManager : MonoBehaviour
{
    Canvas questionCanvasPrefab;
    Canvas questionOptionPrefab;
    XMLReader xmlReader;

    List<QuizQuestion> questions;
    List<QuizQuestion> questionsCopy;

    [HideInInspector] public bool isQuestionCanvasActive = false;
    public static QuizQuestion pickedQuestion;

    private void Start()
    {
        questionCanvasPrefab = Resources.Load("QuestionCanvas", typeof(Canvas)) as Canvas;
        questionOptionPrefab = Resources.Load("QuestionOption", typeof(Canvas)) as Canvas;
        xmlReader = GameObject.Find("GlobalGameManager").GetComponent<XMLReader>();
        questions = new List<QuizQuestion>(xmlReader.questions);
        questionsCopy = new List<QuizQuestion>(questions);
    }

    public void GenerateQuestion()
    {
        Destroy(GameObject.Find("QuestionCanvas(Clone)"));
        isQuestionCanvasActive = true;

        // Pick Random Question
        int questionIndex = PickQuestion();
        pickedQuestion = questionsCopy[questionIndex];
        questionsCopy.Remove(pickedQuestion);

        // Instantiate Question Canvas
        Canvas questionCanvas = Instantiate(questionCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        questionCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        Text questionText = questionCanvas.GetComponentInChildren<Text>();
        questionText.text = pickedQuestion.questionText;

        // Instantiate Options Canvases With Parent Question Canvas
        float optionSpacing = 500;
        foreach (QuizOption option in pickedQuestion.options)
        {
            Canvas questionOption = Instantiate(questionOptionPrefab, questionCanvas.transform, false);

            Vector3 optionPosition = questionOption.transform.position;
            optionPosition.y = optionSpacing;

            questionOption.transform.SetPositionAndRotation(optionPosition, Quaternion.identity);
            questionOption.GetComponentInChildren<Text>().text = option.text;

            optionSpacing -= 50;
        }

        GameObject[] options = GameObject.FindGameObjectsWithTag("OptionButton");
        int index = 0;
        foreach(GameObject option in options)
        {
            option.name = index.ToString();
            index++;
        }

        GameObject.Find("CustomFPSController").GetComponent<CustomFirstPersonController>().enabled = false;
    }

    private int PickQuestion()
    {
        if (questionsCopy.Count == 0)
        {
            questionsCopy = new List<QuizQuestion>(questions);
        }
        int index = Random.Range(0, questionsCopy.Count);

        return index;
    }

    public void RemoveCurrentQuestionCanvas()
    {
        GameObject.Find("UIManager").GetComponent<UIManager>().isQuestionCanvasActive = false;
        Destroy(GameObject.Find("QuestionCanvas(Clone)"));
        GameObject.Find("CustomFPSController").GetComponent<CustomFirstPersonController>().enabled = true;
    }

    public void OnOptionClicked()
    {
        int optionIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        
        if (pickedQuestion != null)
        {
            int questionPoints = pickedQuestion.points;
            
            if (pickedQuestion.options[optionIndex].isTrue)
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
                Text pointsText = GameObject.Find("QuizGamePointsCanvas(Clone)/PointsDisplay/PointsCounter").GetComponent<Text>();
                int currentPoints = int.Parse(pointsText.text);
                currentPoints += questionPoints;
                pointsText.text = currentPoints.ToString();
            }
            else
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.red;
            }

            GameObject[] options = GameObject.FindGameObjectsWithTag("OptionButton");
            foreach (GameObject option in options)
            {
                option.GetComponent<Button>().interactable = false;
            }
        }
        
    }

}
