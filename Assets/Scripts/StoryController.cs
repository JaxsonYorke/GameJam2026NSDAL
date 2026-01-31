using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomDebug;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]

// This class will be a parent class for other cutscenes
public class StoryController : MonoBehaviour
{
    public enum CutSceneType
    {
        playerWalkThrough,
        autoWalkThrough
    }
    public CutSceneType cutsceneType;

    [SerializeField] public int currentSprite;
    [SerializeField] public List<SpriteRecord> SpriteList;
    private SpriteRenderer sr;

    public bool timeout;




    void Awake()
    {
        // DontDestroyOnLoad(this);

    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        currentSprite = -1;
       
    }

    void startCutscene()
    {
        currentSprite++;
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
        currentSprite++;
        SpriteRecord currRecord = SpriteList[currentSprite];
        sr.sprite = currRecord.sprite;


        timeout = true;
        StartCoroutine(UnTimeout(currRecord.viewtime));
    }

    IEnumerator PlayCutscene()
    {
        while (currentSprite < SpriteList.Count)
        {
            SpriteRecord record = SpriteList[currentSprite];

            sr.sprite = record.sprite;

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
