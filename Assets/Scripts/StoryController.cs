using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.CustomDebug;

// This class will be a parent class for other cutscenes
public class StoryController : MonoBehaviour
{
    [SerializeField]
    public List<SpriteRecord> SpriteList = new();



    public enum CutSceneType
    {
        playerWalkThrough,
        autoWalkThrough
    }
    public CutSceneType cutsceneType;



    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class SpriteRecord
{
    public Sprite sprite;
}
