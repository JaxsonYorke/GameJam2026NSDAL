using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomDebug;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

// This class will be a parent class for other cutscenes
public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance;

    public enum CutSceneType
    {
        playerWalkThrough,
        autoWalkThrough
    }
    public CutSceneType cutsceneType;

    [SerializeField] public static int currentSprite;
    [SerializeField] public List<SpriteRecord> SpriteList;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject CutsceneMouseIcon;
    [SerializeField] private Image image;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private Image dialoguePortrait;
    [SerializeField] private Image dialoguePortrait2;
    private bool isPlayingDialogue = false;
    public bool timeout;
    private bool firstTime = true;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(this);
        gameObject.tag = "StoryController";

    }

    void Start()
    {
        CutsceneMouseIcon.SetActive(false);
        dialogueImage.gameObject.SetActive(false);
        dialogueImage.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            dialogueImage.sprite = dialoguePortrait2.sprite;
        }
        dialoguePortrait.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //check if we are in the finale; if so, change sprite to the one with mask 3
    }
    void Update()
    {
        if(cutsceneType == CutSceneType.playerWalkThrough && !timeout && !isPlayingDialogue && !CutsceneMouseIcon.activeSelf)
        {
            CutsceneMouseIcon.SetActive(true);
        }

    }

    public void StartCutscene()
    {
        if(cutsceneType == CutSceneType.playerWalkThrough)
        {
            timeout = false;
            AdvanceCutscene();
        } else if(cutsceneType == CutSceneType.autoWalkThrough)
        {
            StartCoroutine(PlayCutscene());
        }
    }



    // private IEnumerator UnTimeout(float time)
    // {
    //     yield return new WaitForSeconds(time);
    //     timeout = false;
    // }

    public void AdvanceCutscene()
    {
        // If dialogue is playing or we’re past the last sprite, do nothing
        if (isPlayingDialogue)
        {
            return;
        }
        if (currentSprite >= SpriteList.Count)
        {
            currentSprite = 0;
            if(SceneManager.GetActiveScene().name == "GameIntro")
            {
                GameController.Instance.AdvanceFromGameIntro();
            } else if(SceneManager.GetActiveScene().name == "insideChapel")
            {
                GameController.Instance.AdvanceFromInChapel();
            } else if(SceneManager.GetActiveScene().name == "FinalScene")
            {
                GameController.Instance.AdvanceFromFinalScene();
            }
            return;
        }

        SpriteRecord currRecord = SpriteList[currentSprite];

        // Show main slide immediately
        image.sprite = currRecord.sprite;

        // AUTO SKIP SLIDES
        if (currRecord.AutoSkip)
        {
            StartCoroutine(AutoAdvance(currRecord.viewtime));
            return;
        }

        // NORMAL FLOW
        if (currRecord.hasDialogue && currRecord.dialogues.Count > 0)
        {
            if (firstTime)
            {
                firstTime = false;
                return;
            }
            StartCoroutine(PlayDialogue(currRecord.dialogues, currRecord.viewtime));
        }
        else
        {
            StartCoroutine(AdvanceScene(currRecord.viewtime));
        }
    }

    private IEnumerator AutoAdvance(int time)
    {
        timeout = true;
        CutsceneMouseIcon.SetActive(false);

        yield return new WaitForSeconds(time);

        timeout = false;
        currentSprite++;

        AdvanceCutscene();
    }


    IEnumerator PlayCutscene()
    {
        while (currentSprite < SpriteList.Count)
        {
            SpriteRecord record = SpriteList[currentSprite];

            image.sprite = record.sprite;

            yield return new WaitForSeconds(record.viewtime);

            currentSprite++;
        }

        // EndCutscene();
    }

    private IEnumerator PlayDialogue(List<SpriteRecord.Dialogue> dialogues, int mainSlideTime)
    {
        bool clicked;
        isPlayingDialogue = true;
        dialogueImage.gameObject.SetActive(true);

        // Wait a frame to avoid leftover clicks skipping the first dialogue
        yield return null;

        foreach (var dialogue in dialogues)
        {
            dialogueImage.sprite = dialogue.dialogueSprite;
            dialogueImage.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            if (dialogue.isKid)
            {
                dialoguePortrait.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            // Step 1: Wait for the dialogue cooldown
            float elapsed = 0f;
            while (elapsed < dialogue.dialogueviewtime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Step 2: Enable mouse icon for click prompt
            CutsceneMouseIcon.SetActive(true);

            // Step 3: Wait for player click to advance
            clicked = false;
            while (!clicked)
            {
                if (Input.GetMouseButtonDown(0))
                    clicked = true;
                yield return null;
            }
            dialoguePortrait.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            // Disable the icon immediately after clicking
            CutsceneMouseIcon.SetActive(false);
        }

        dialogueImage.gameObject.SetActive(false);
        dialogueImage.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        yield return new WaitForSeconds(mainSlideTime);
        clicked = false;
        CutsceneMouseIcon.SetActive(true);
        // while (!clicked)
        // {
        //     if (Input.GetMouseButtonDown(0))
        //         clicked = true;
        //     yield return null;
        // }
        
        timeout = false;
        firstTime = true;

        currentSprite++;
        isPlayingDialogue = false;
    }






private IEnumerator AdvanceScene(int time)
    {
        // Block clicks until timeout ends
        timeout = true;
        CutsceneMouseIcon.SetActive(false);

        yield return new WaitForSeconds(time);

        timeout = false;

        currentSprite++; // move to next slide
    }
}


[System.Serializable]
public class SpriteRecord
{
    [System.Serializable]
    public class Dialogue
    {
        public Sprite dialogueSprite;
        public int dialogueviewtime;
        public bool isKid;
    }
    public Sprite sprite;
    public int viewtime;
    public bool hasDialogue;
    public bool AutoSkip;
    public List<Dialogue> dialogues;
}
