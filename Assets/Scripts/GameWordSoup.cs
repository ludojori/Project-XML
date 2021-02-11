using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class GameWordSoup : MonoBehaviour
{
    //TODO: Need to make a Stack with the last clicked tiles and be able to unclick the last pressed
    public static string[] dictionaryWordSoup = null;
    public static int points = 0; 
    private static GameObject globalGameManager;
    private static bool gameStarted = false;
    public const int HEIGHT = 5;
    public const int WIDTH = 8;
    private const int INIT = 3, FOUND_WORD = 2, END_GAME = 4;


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
        // .......
        //at finding a word, increase points by calling:
        //GlobalGameManager.increasePointsOnly(points);

        //Debug.Log(res);

    }
}
    