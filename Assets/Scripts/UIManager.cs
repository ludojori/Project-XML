using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

struct QuizOption
{
    string text { set; get; }
    bool isTrue { set; get; }

    public QuizOption(string text, bool isTrue)
    {
        this.text = text;
        this.isTrue = isTrue;
    }
}

struct QuizQuestion
{
    string question { set; get; }
    List<QuizOption> options { set; get; }

    public QuizQuestion(string question, List<QuizOption> options)
    {
        this.question = question;
        this.options = options;
    }
}

public class UIManager : MonoBehaviour
{
    
    /*
    private void Awake()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("Resources/QuizGame");
        XDocument doc = XDocument.Parse(txtXmlAsset.text);
        var allQuestions = doc.Element("quizquestions").Elements("quizquestion");

        // To be continued...
    }
    */
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
