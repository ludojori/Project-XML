using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class GameWordSoup : MonoBehaviour
{
    //TODO: Need to make a Stack with the last clicked tiles and be able to unclick the last pressed
    public static string resultWord = "";
    public static int wordsFound = 0;
    public static string[] dictionaryWordSoup = null;
    public static int points = 0; 
    public static Stack<GameObject> lastPressed = new Stack<GameObject>();
    public static int direction = 0; //could be either vertical downwards or horizontal to the right; 1 for vertical, 2 for horizontal
    private static GameObject globalGameManager;
    private static bool gameStarted = false;
    public const int HEIGHT = 5;
    public const int WIDTH = 8;
    private const int INIT = 3, FOUND_WORD = 2, END_GAME = 4;

    private bool contains(string[] allWords, string currentWord)
    {
        //.........
        return false;
    }

    private bool isValidTile(string currentName)
    {
        //TODO: Check validity for tiles that are parts of already foud words

        string lastPressedName = lastPressed.Peek().name;
        //pretty hardcoded names for now, but this will be the idea

        //Debug.Log(lastPressedName);
        //Debug.Log(currentName);

        //..
        return false;
    }

    private void playTaDaSound(int whatToPlay)
    {
        AudioSource[] audioSources = globalGameManager.GetComponents<AudioSource>();
        if (audioSources[whatToPlay] != null && !audioSources[whatToPlay].isPlaying && audioSources[whatToPlay].clip != null) {
            audioSources[whatToPlay].Play();
        }
    }

    private void initializeGlobalManager()
    {
        globalGameManager = GameObject.Find("/GlobalGameManager");
    }

    private void loadDictionary(GameObject slide) {
        GameObject dictionary = slide.transform.Find("WordSoupDictionary").gameObject;
        if (dictionary == null)
        {
            Debug.Log("No dictionary found for the Word Soup game at slide " + slide.name);
            return;
        }
        string words = dictionary.transform.Find("Words").GetComponent<TextMeshPro>().text;
        //Debug.Log("Words found: " + words);
        dictionaryWordSoup = words.Split(" "[0]);
    }

    void OnMouseDown()
    {
        GameObject currentObject = GameObject.Find(gameObject.name);
        GameObject slide = currentObject.transform.parent.gameObject;
        if (!gameStarted)
        {
            initializeGlobalManager();
            //parseDictionaryFromXML();
            //initializeLetters();
            //display Chars And Hints
            slide.transform.Find("Text").gameObject.SetActive(false);
            //slide.transform.GetChild(0).GetComponent().enabled = false;
            //slide.transform.Find("Tile00").gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().enabled = true;
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    slide.transform.Find("Tile" + System.Convert.ToChar(i + '0') + System.Convert.ToChar(j + '0')).gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().enabled = true;
                }
            }
            slide.transform.Find("Hints").gameObject.SetActive(true);
            loadDictionary(slide);
            points = Convert.ToInt32(slide.transform.Find("WordSoupDictionary").gameObject.transform.Find("Points").GetComponent<TextMeshPro>().text);
            //Debug.Log("WordSoup Game for slide: " + slide.name + ", points for each found word: " + points.ToString());
            playTaDaSound(INIT); //using the same sound to start the game
            gameStarted = true;
            return;
        }

        string text = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text;

        GameObject current = GameObject.Find(gameObject.name);
        Material tileMaterial = GetComponent<Renderer>().material;

        // means the word was already found. Probably should work with tags instead of colors ¯\_(ツ)_/¯
        //..
        //Debug.Log(res);

    }
}
    