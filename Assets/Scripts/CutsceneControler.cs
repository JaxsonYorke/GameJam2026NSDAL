using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomDebug;
using System.Collections;
using UnityEngine.UI;


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

    public bool timeout;


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
       
    }

    void Update()
    {
        if(cutsceneType == CutSceneType.playerWalkThrough && !timeout && !CutsceneMouseIcon.activeSelf)
        {
            // enable the mouse Icon
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
        Debug.Log(currentSprite);
        SpriteRecord currRecord = SpriteList[currentSprite];
        image.sprite = currRecord.sprite;

        currentSprite++;

        timeout = true;
        CutsceneMouseIcon.SetActive(false);
        StartCoroutine(UnTimeout(currRecord.viewtime));
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

}




[System.Serializable]
public class SpriteRecord
{
    public Sprite sprite;
    public int viewtime;
    
}
