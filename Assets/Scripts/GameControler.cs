using UnityEngine;
using Assets.Scripts.CustomDebug;
using UnityEngine.SceneManagement;


// This class will be a parent class for other cutscenes
public class GameController : MonoBehaviour
{
    public enum GameState {
        firstCutscene,
        onRoad
    }

    [SerializeField] private EventHandler eventHandler;
    [SerializeField] private StoryController storyController;

    public GameState currStoryState;

    [SerializeField] public bool isInCutscene;
    [SerializeField] public bool isInWalkingAround;
    [SerializeField] public bool isInMingame;

    

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        // DebugStatsDisplay.Instance.RegisterDebugStatsRequest(
        //     new DebugStatsRequest("MouseButtonPressed", () => {return Input.GetMouseButton(0);}));
        // if (SceneManager.GetActiveScene().name == "GameIntro")
        // {
        //     isInCutscene = true;
        // }


    }
    // Update is called once per frame
    void Update()
    {
        switch (currStoryState)
        {
            case GameState.firstCutscene:
                if(storyController.cutsceneType == StoryController.CutSceneType.playerWalkThrough){
                    if (Input.GetMouseButtonUp(0) && !storyController.timeout)
                    {
                        storyController.AdvanceCutscene();
                    }
                    
                }
                
                break;
            case GameState.onRoad:
                {
                    
                }
                break;
        }
    }

    public void SetGameState(GameState newState)
    {
        currStoryState = newState;
        switch (newState)
        {
            case GameState.firstCutscene:
            
            break;
        }
    }


}

public class StoryVariables
{
    public bool isStartOfGame;
    public bool firstCutsceneFinshed;

    public StoryVariables()
    {
        isStartOfGame = false;
        firstCutsceneFinshed = false;
    }
}
