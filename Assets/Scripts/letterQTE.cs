using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class letterQTE : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string possibleLetters = "`1234567890-=qwertyuiop[]asdfghjkl;'zxcvbnm,./";
    [SerializeField] private TextMeshProUGUI test;
    private char chosenletter;

    [SerializeField] private Slider slider;
    [SerializeField] public float countdownDuration = 5f;

    private bool isDestroyed = false;

    void Start()
    {
        test = GetComponentInChildren<TextMeshProUGUI>();
        DecideLetter();
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        // Listen for the key corresponding to the currently chosen letter
        switch (chosenletter)
        {
            case '`':
                if (Input.GetKeyDown(KeyCode.BackQuote)) Click();
                break;
            case '1':
                if (Input.GetKeyDown(KeyCode.Alpha1)) Click();
                break;
            case '2':
                if (Input.GetKeyDown(KeyCode.Alpha2)) Click();
                break;
            case '3':
                if (Input.GetKeyDown(KeyCode.Alpha3)) Click();
                break;
            case '4':
                if (Input.GetKeyDown(KeyCode.Alpha4)) Click();
                break;
            case '5':
                if (Input.GetKeyDown(KeyCode.Alpha5)) Click();
                break;
            case '6':
                if (Input.GetKeyDown(KeyCode.Alpha6)) Click();
                break;
            case '7':
                if (Input.GetKeyDown(KeyCode.Alpha7)) Click();
                break;
            case '8':
                if (Input.GetKeyDown(KeyCode.Alpha8)) Click();
                break;
            case '9':
                if (Input.GetKeyDown(KeyCode.Alpha9)) Click();
                break;
            case '0':
                if (Input.GetKeyDown(KeyCode.Alpha0)) Click();
                break;
            case '-':
                if (Input.GetKeyDown(KeyCode.Minus)) Click();
                break;
            case '=':
                if (Input.GetKeyDown(KeyCode.Equals)) Click();
                break;
            case 'q':
                if (Input.GetKeyDown(KeyCode.Q)) Click();
                break;
            case 'w':
                if (Input.GetKeyDown(KeyCode.W)) Click();
                break;
            case 'e':
                if (Input.GetKeyDown(KeyCode.E)) Click();
                break;
            case 'r':
                if (Input.GetKeyDown(KeyCode.R)) Click();
                break;
            case 't':
                if (Input.GetKeyDown(KeyCode.T)) Click();
                break;
            case 'y':
                if (Input.GetKeyDown(KeyCode.Y)) Click();
                break;
            case 'u':
                if (Input.GetKeyDown(KeyCode.U)) Click();
                break;
            case 'i':
                if (Input.GetKeyDown(KeyCode.I)) Click();
                break;
            case 'o':
                if (Input.GetKeyDown(KeyCode.O)) Click();
                break;
            case 'p':
                if (Input.GetKeyDown(KeyCode.P)) Click();
                break;
            case '[':
                if (Input.GetKeyDown(KeyCode.LeftBracket)) Click();
                break;
            case ']':
                if (Input.GetKeyDown(KeyCode.RightBracket)) Click();
                break;
            case 'a':
                if (Input.GetKeyDown(KeyCode.A)) Click();
                break;
            case 's':
                if (Input.GetKeyDown(KeyCode.S)) Click();
                break;
            case 'd':
                if (Input.GetKeyDown(KeyCode.D)) Click();
                break;
            case 'f':
                if (Input.GetKeyDown(KeyCode.F)) Click();
                break;
            case 'g':
                if (Input.GetKeyDown(KeyCode.G)) Click();
                break;
            case 'h':
                if (Input.GetKeyDown(KeyCode.H)) Click();
                break;
            case 'j':
                if (Input.GetKeyDown(KeyCode.J)) Click();
                break;
            case 'k':
                if (Input.GetKeyDown(KeyCode.K)) Click();
                break;
            case 'l':
                if (Input.GetKeyDown(KeyCode.L)) Click();
                break;
            case ';':
                if (Input.GetKeyDown(KeyCode.Semicolon)) Click();
                break;
            case '\'':
                if (Input.GetKeyDown(KeyCode.Quote)) Click();
                break;
            case 'z':
                if (Input.GetKeyDown(KeyCode.Z)) Click();
                break;
            case 'x':
                if (Input.GetKeyDown(KeyCode.X)) Click();
                break;
            case 'c':
                if (Input.GetKeyDown(KeyCode.C)) Click();
                break;
            case 'v':
                if (Input.GetKeyDown(KeyCode.V)) Click();
                break;
            case 'b':
                if (Input.GetKeyDown(KeyCode.B)) Click();
                break;
            case 'n':
                if (Input.GetKeyDown(KeyCode.N)) Click();
                break;
            case 'm':
                if (Input.GetKeyDown(KeyCode.M)) Click();
                break;
            case ',':
                if (Input.GetKeyDown(KeyCode.Comma)) Click();
                break;
            case '.':
                if (Input.GetKeyDown(KeyCode.Period)) Click();
                break;
            case '/':
                if (Input.GetKeyDown(KeyCode.Slash)) Click();
                break;
            default:
                break;
        }
    }

    

    

    private void DecideLetter()
    {
        int character = UnityEngine.Random.Range(0, possibleLetters.Length);
        chosenletter = possibleLetters[character];
        test.text = "" + possibleLetters[character]; // why tf does this work
        // test.text = possibleLetters[character].ToString();
    }

    private void Click()
    {
        Debug.Log("Clicked: " + chosenletter);
        isDestroyed = true;
        FightController.Instance.Hit();
        Destroy(gameObject);
    }

    

    IEnumerator CountDown()
    {
        float timeRemaining = countdownDuration;
        
        while (timeRemaining > 0 && !isDestroyed)
        {
            timeRemaining -= Time.deltaTime;
            slider.value = timeRemaining/countdownDuration;
            yield return null;
        }
        
        // Time ran out, take a hit and destroy
        if(!isDestroyed){
            FightController.Instance.TakeHit();
            Destroy(gameObject);

        }
    }

}
