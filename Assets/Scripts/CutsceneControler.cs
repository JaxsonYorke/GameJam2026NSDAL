using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomDebug;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Profiling;


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
        DebugStatsDisplay.Instance.RegisterDebugStatsRequest(new DebugStatsRequest("First Time", () => {return firstTime;}));
        DebugStatsDisplay.Instance.RegisterDebugStatsRequest(new DebugStatsRequest("timeout", () => {return timeout;}));
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



    private IEnumerator UnTimeout(float time)
    {
        yield return new WaitForSeconds(time);
        timeout = false;
    }

    public void AdvanceCutscene()
    {
        // If dialogue is playing or we’re past the last sprite, go to next scene
        if (currentSprite >= SpriteList.Count)
        {
            if(GameController.Instance.CurrentState == GameController.GameState.FirstCutscene)
            {
                GameController.Instance.AdvanceFromGameIntro();
            } else if (GameController.Instance.CurrentState == GameController.GameState.inChapel)
            {
                GameController.Instance.AdvanceFromInChapel();
            }
            return;
        }
        if (isPlayingDialogue)
        {
            return;
        }

        SpriteRecord currRecord = SpriteList[currentSprite];

        // Show main slide immediately
        image.sprite = currRecord.sprite;

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
    }
    public Sprite sprite;
    public int viewtime;
    public bool hasDialogue;
    public List<Dialogue> dialogues;
}
