using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using QuizGame;

public class UIManager : MonoBehaviour
{
    Canvas questionCanvas;
    XMLReader xmlReader;
    List<QuizQuestion> questions;

    private void Start()
    {
        xmlReader = new XMLReader();
        questions = xmlReader.LoadAllQuestions();
    }

    public static void GenerateQuestion()
    {
        // Display Question Canvas
        Debug.Log("GenerateQuestion() called.");
    }

    private void PickQuestion()
    {
        // Pick Random Question
        
    }
}
