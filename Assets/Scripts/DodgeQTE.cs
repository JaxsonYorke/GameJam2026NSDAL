using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DodgeQTE : MonoBehaviour
{
    [SerializeField] private char input;
    [SerializeField] private Slider slider;
    [SerializeField] public float countdownDuration = 5f;
    private bool isDestroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CountDown());
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (input)
        {
            case 'A':
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Click();
                }
            break;
            case 'S':
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Click();
                }
            break;
            case 'D':
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Click();
                }
            break;

            
            case 'l':
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Click();
                }
            break;
            case 'd':
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Click();
                }
            break;
            case 'r':
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Click();
                }
            break;
        }
    }

    private void Click()
    {
        Debug.Log("Dodged");
        isDestroyed = true;
        FightController.Instance.Dodge();
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
