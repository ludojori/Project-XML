using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace QuizGame
{
    public class XMLReader : MonoBehaviour
    {
        XmlDocument quizGameXml;

        // Start is called before the first frame update
        void Awake()
        {
            quizGameXml = new XmlDocument();
            try
            {
                quizGameXml.Load("Assets/Resources/QuizGame.xml");
            }
            catch (System.Exception e)
            {
                Debug.LogException(e, this);
                Debug.Log("Failed to load QuizGame.xml");
            }
            InstantiateAllRooms();
        }

        public List<QuizQuestion> LoadAllQuestions()
        {
            XmlNode root = quizGameXml.DocumentElement;
            XmlNodeList quizquestions = root.SelectNodes("descendant::quizquestions/quizquestion");
            List<QuizQuestion> questions = new List<QuizQuestion>(quizquestions.Count);

            foreach (XmlNode quizquestion in quizquestions)
            {
                QuizQuestion question = new QuizQuestion(quizquestion);
                questions.Add(question);
            }
            return questions;
        }

        private void InstantiateAllRooms()
        {
            XmlNode root = quizGameXml.DocumentElement;
            XmlNodeList xmlRooms = root.SelectNodes("descendant::rooms/room");

            foreach(XmlNode xmlRoom in xmlRooms)
            {
                string prefabName = xmlRoom.Attributes["prefab_name"].Value;
                GameObject roomPrefab = Resources.Load(prefabName, typeof(GameObject)) as GameObject;
                if (roomPrefab != null)
                {
                    XmlNode xmlRoomTransform = xmlRoom["transform"];
                    XmlNode xmlRoomPosition = xmlRoomTransform["position"];
                    XmlNode xmlRoomRotation = xmlRoomTransform["rotation"];
                    XmlNode xmlRoomScale = xmlRoomTransform["scale"];

                    float posX = float.Parse(xmlRoomPosition.Attributes["x"].Value);
                    float posY = float.Parse(xmlRoomPosition.Attributes["y"].Value);
                    float posZ = float.Parse(xmlRoomPosition.Attributes["z"].Value);
                    Vector3 position = new Vector3(posX, posY, posZ);

                    float rotX = float.Parse(xmlRoomRotation.Attributes["x"].Value);
                    float rotY = float.Parse(xmlRoomRotation.Attributes["y"].Value);
                    float rotZ = float.Parse(xmlRoomRotation.Attributes["z"].Value);
                    Vector3 rotation = new Vector3(rotX, rotY, rotZ);
                    Quaternion quaternion = new Quaternion();
                    quaternion.eulerAngles = rotation;

                    float scaleX = float.Parse(xmlRoomScale.Attributes["x"].Value);
                    float scaleY = float.Parse(xmlRoomScale.Attributes["y"].Value);
                    float scaleZ = float.Parse(xmlRoomScale.Attributes["z"].Value);
                    Vector3 scale = new Vector3(scaleX, scaleY, scaleZ);

                    GameObject instance = Instantiate<GameObject>(roomPrefab, position, quaternion) as GameObject;
                    instance.transform.localScale = scale;
                }
                else
                {
                    Debug.Log("Prefab is null.");
                }
            }
        }

        public List<Door> LoadAllDoors()
        {
            XmlNode root = quizGameXml.DocumentElement;
            XmlNodeList xmlDoors = root.SelectNodes("descendant::doors/door");
            List<Door> doors = new List<Door>(xmlDoors.Count);

            foreach(XmlNode xmlDoor in xmlDoors)
            {
                Door door = new Door(xmlDoor);
                doors.Add(door);
            }

            return doors;
        }
    }

    public class QuizOption
    {
        public string text { get; private set; }
        public bool isTrue { get; private set; }

        public QuizOption(XmlNode optionNode)
        {
            text = optionNode.InnerText;
            isTrue = bool.Parse(optionNode.Attributes["isTrue"].Value);
        }
    }

    public class QuizQuestion
    {
        public int points { get; private set; }
        public string questionText { get; private set; }
        public List<QuizOption> options { get; private set; }

        public QuizQuestion(XmlNode quizQuestionNode)
        {
            questionText = quizQuestionNode["question"].InnerText;
            points = int.Parse(quizQuestionNode.Attributes["points"].Value);

            XmlNodeList nodeOptions = quizQuestionNode.SelectNodes("descendant::option");
            options = new List<QuizOption>(nodeOptions.Count);

            foreach (XmlNode nodeOption in nodeOptions)
            {
                QuizOption option = new QuizOption(nodeOption);
                options.Add(option);
            }
        }
    }

    public class Door
    {
        public string doorName { get; private set; }
        public float minPoints { get; private set; }

        public Door(XmlNode xmlDoor)
        {
            doorName = xmlDoor.Attributes["prefab_name"].Value;
            minPoints = float.Parse(xmlDoor.Attributes["limit_points"].Value);
        }
    }
}
