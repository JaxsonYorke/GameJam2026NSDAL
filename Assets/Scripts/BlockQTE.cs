using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockQTE : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] public float countdownDuration = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isDestroyed = false;


    void Start()
    {
        StartCoroutine(CountDown());
        slider = GetComponentInChildren<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Click();
        }
    }

    private void Click()
    {
        Debug.Log("Blocked");
        isDestroyed = true;
        FightController.Instance.Block();
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
            isDestroyed = true;
            Destroy(gameObject);

        }
    }
}
