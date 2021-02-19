using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QuizGame;


[DisallowMultipleComponent]
public class UIManager : MonoBehaviour
{
    Canvas questionCanvasPrefab;
    Canvas questionOptionPrefab;
    XMLReader xmlReader;

    //public static List<int> arrayList;
    static Dictionary<string, int> diamonds;

    List<QuizQuestion> questions;
    List<QuizQuestion> questionsCopy;

    [HideInInspector] public bool isQuestionCanvasActive = false;
    public static QuizQuestion pickedQuestion;
    public static int pointsCount;
    public static string nameOfDiamond;
    public static bool isDoorOpen = false;

    private void Start()
    {
        questionCanvasPrefab = Resources.Load("QuestionCanvas", typeof(Canvas)) as Canvas;
        questionOptionPrefab = Resources.Load("QuestionOption", typeof(Canvas)) as Canvas;
        Canvas quizGamePointsCanvasPrefab = Resources.Load("QuizGamePointsCanvas", typeof(Canvas)) as Canvas;
        Instantiate(quizGamePointsCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        quizGamePointsCanvasPrefab.name = "QuizGamePointsCanvas";
        quizGamePointsCanvasPrefab.renderMode = RenderMode.ScreenSpaceOverlay;


        xmlReader = GameObject.Find("GlobalGameManager").GetComponent<XMLReader>();
        questions = new List<QuizQuestion>(xmlReader.questions);
        questionsCopy = new List<QuizQuestion>(questions);
        
        //arrayList = new List<int>();

        diamonds = xmlReader.LoadAllDiamonds();
       /* foreach (int value in diamonds.Values)
        {
            if (!arrayList.Contains(value)) 
            {
                arrayList.Add(value);
            }
            
        }*/
        
    }

    public void GenerateQuestion()
    {
        //RaycastHit hit;
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
        if (pointsCount >= 300)
        {
            GameObject endGameCanvasPrefab = Resources.Load("EndGameCanvas") as GameObject;
            GameObject endGameCanvasInstance = Instantiate(endGameCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            endGameCanvasPrefab.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            Destroy(endGameCanvasInstance, 3.0f);
        }
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
                pointsCount += questionPoints;
                pointsText.text = pointsCount.ToString();

                nameOfDiamond = SelectionManager.diamondName;
                switch (nameOfDiamond)
                {
                    case "QuizDiamond1":
                        if (pointsCount >= diamonds[nameOfDiamond]) 
                        { 
                            GameObject.Find("RoomsAll(Clone)/Room 1/"+ nameOfDiamond).GetComponent<MeshCollider>().enabled = false;

                            GameObject.Find("RoomsAll(Clone)/Room 1/Walls/Room1 Exit").GetComponent<Animator>().Play("Room1ExitDoorOpen", 0, 0.0f);
                            GameObject.Find("RoomsAll(Clone)/Room 1/Walls/Room1 Exit").GetComponent<DoorController>().doorOpen = true;

                            if (GameObject.Find("RoomsAll(Clone)/Room 1/Walls/Room1 Exit").GetComponent<AudioSource>())
                            {
                                GameObject.Find("RoomsAll(Clone)/Room 1/Walls/Room1 Exit").GetComponent<AudioSource>().Play();
                            }
                        } 
                        break;
                    case "QuizDiamond2":
                        if (pointsCount >= diamonds[nameOfDiamond])
                        {
                            GameObject.Find("RoomsAll(Clone)/Room 2 (Scale X 2)/QuizDiamonds/" + nameOfDiamond).GetComponent<MeshCollider>().enabled = false;

                            GameObject.Find("RoomsAll(Clone)/Room 2 (Scale X 2)/Walls/Room_2_Intermediary_Gate_1").GetComponent<Animator>().Play("Room2GateOpen", 0, 0.0f);
                            GameObject.Find("RoomsAll(Clone)/Room 2 (Scale X 2)/Walls/Room_2_Intermediary_Gate_1").GetComponent<DoorController>().doorOpen = true;

                            if (GameObject.Find("RoomsAll(Clone)/Room 2 (Scale X 2)/Walls/Room_2_Intermediary_Gate_1").GetComponent<AudioSource>())
                            {
                                GameObject.Find("RoomsAll(Clone)/Room 2 (Scale X 2)/Walls/Room_2_Intermediary_Gate_1").GetComponent<AudioSource>().Play();
                            }
                        }
                        break;
                    case "QuizDiamond3":
                        if (pointsCount >= diamonds[nameOfDiamond])
                        {
                            GameObject.Find("RoomsAll(Clone)/Room 2 (Scale X 2)/QuizDiamonds/" + nameOfDiamond).GetComponent<MeshCollider>().enabled = false;

                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Entrance").GetComponent<Animator>().Play("Room3EntranceDoorOpen", 0, 0.0f);
                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Entrance").GetComponent<DoorController>().doorOpen = true;

                            if (GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Entrance").GetComponent<AudioSource>())
                            {
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Entrance").GetComponent<AudioSource>().Play();
                            }
                        }
                        break;
                    case "QuizDiamond4":
                        if (pointsCount >= diamonds[nameOfDiamond])
                        {
                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/QuizDiamonds/" + nameOfDiamond).GetComponent<MeshCollider>().enabled = false;

                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Iron").GetComponent<Animator>().Play("TowerDoorOpen", 0, 0.0f);
                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Iron").GetComponent<DoorController>().doorOpen = true;

                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Wooden_Round").GetComponent<Animator>().Play("Room1EntranceDoorOpen", 0, 0.0f);
                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Wooden_Round").GetComponent<DoorController>().doorOpen = true;

                            if (GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Iron").GetComponent<AudioSource>())
                            {
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Iron").GetComponent<AudioSource>().Play();
                                
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Room3_Door_Wooden_Round").GetComponent<AudioSource>().Play();
                            }
                        }
                        break;
                    case "QuizDiamond5":
                        if (pointsCount >= diamonds[nameOfDiamond])
                        {
                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/QuizDiamonds/QuizDiamond5").GetComponent<MeshCollider>().enabled = false;

                            if (!isDoorOpen)
                            {
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Tower_Door").GetComponent<Animator>().Play("TowerDoorOpen", 0, 0.0f);
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Tower_Door").GetComponent<DoorController>().doorOpen = true;
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Barrel_Secret_Entrance").GetComponent<Animator>().Play("BarrelOpen", 0, 0.0f);
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Barrel_Secret_Entrance").GetComponent<DoorController>().doorOpen = true;

                                if (GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Tower_Door").GetComponent<AudioSource>())
                                {
                                    GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Tower_Door").GetComponent<AudioSource>().Play();
                                }
                                isDoorOpen = true;
                            }
                            diamonds["QuizDiamond6"] += 50;
                        }
                        break;
                    case "QuizDiamond6":
                        if (pointsCount >= diamonds[nameOfDiamond])
                        {
                            GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/QuizDiamonds/QuizDiamond6").GetComponent<MeshCollider>().enabled = false;

                            if (!isDoorOpen)
                            {
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Barrel_Secret_Entrance").GetComponent<Animator>().Play("BarrelOpen", 0, 0.0f);
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Barrel_Secret_Entrance").GetComponent<DoorController>().doorOpen = true;
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Tower_Door").GetComponent<Animator>().Play("TowerDoorOpen", 0, 0.0f);
                                GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Tower_Door").GetComponent<DoorController>().doorOpen = true;

                                if (GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Barrel_Secret_Entrance").GetComponent<AudioSource>())
                                {
                                    GameObject.Find("RoomsAll(Clone)/Room 3 (Scale X 2)/Miscellaneous/Barrel_Secret_Entrance").GetComponent<AudioSource>().Play();
                                }
                                isDoorOpen = true;
                            }
                            diamonds["QuizDiamond5"] += 50;
                        }
                        break;
                    default: break;
                }
                
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
