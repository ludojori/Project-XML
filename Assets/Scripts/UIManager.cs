using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuizGame;

    public class UIManager : MonoBehaviour
    {
        Canvas questionCanvasPrefab;
        Canvas questionOptionPrefab;
        XMLReader xmlReader;
        List<QuizQuestion> questions;
        List<QuizQuestion> questionsCopy;
        private int optionOfChoice;

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

        // Pick Random Question
            int questionIndex = PickQuestion();
            QuizQuestion pickedQuestion = questionsCopy[questionIndex];
            questionsCopy.Remove(pickedQuestion);
 
            // Instantiate Question Canvas
            Canvas questionCanvas = Instantiate(questionCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            questionCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            Text questionText = questionCanvas.GetComponentInChildren<Text>();
            questionText.text = pickedQuestion.questionText;

            // Instantiate Options Canvases
            int index = 0;
            float optionSpacing = 500;
            foreach (QuizOption option in pickedQuestion.options)
            {
                Canvas questionOption = Instantiate(questionOptionPrefab, questionCanvas.transform, false);

                Vector3 optionPosition = questionOption.transform.position;
                optionPosition.y = optionSpacing;

                questionOption.transform.SetPositionAndRotation(optionPosition, Quaternion.identity);
                questionOption.GetComponentInChildren<Text>().text = option.text;
                questionOption.name = index.ToString();
                questionOption.GetComponentInChildren<Button>().onClick.AddListener(questionOption.GetComponentInChildren<ButtonManager>().OptionOnClick);

                optionSpacing -= 50;
                index++;
            }


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

        public void RemoveCurrentCanvas()
        {
            Destroy(GameObject.Find("QuestionCanvas(Clone)"));
        }

    }
