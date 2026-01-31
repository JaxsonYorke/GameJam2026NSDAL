using System.Collections.Generic;
using UnityEngine;

// This class will be a parent class for other cutscenes
public class StoryController : MonoBehaviour
{
    [SerializeField]
    public List<SpriteRecord> SpriteList = new();
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
    public int index;
}
