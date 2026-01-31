using UnityEngine;
using Assets.Scripts.CustomDebug;
using UnityEngine.SceneManagement;


// This class will be a parent class for other cutscenes
public class GameController : MonoBehaviour
{
    [SerializeField] private EventHandler eventHandler;
    [SerializeField] private StoryController storyController;


    [SerializeField] public bool isInCutscene;

    void Awake()
    {
        // DontDestroyOnLoad(this);
    }

    void Start()
    {
        // DebugStatsDisplay.Instance.RegisterDebugStatsRequest(
        //     new DebugStatsRequest("MouseButtonPressed", () => {return Input.GetMouseButton(0);}));
        if (SceneManager.GetActiveScene().name == "GameIntro")
        {
            isInCutscene = true;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isInCutscene)
        {
            if(storyController.cutsceneType == StoryController.CutSceneType.playerWalkThrough){
                if (Input.GetMouseButtonUp(0) && !storyController.timeout)
                {
                    storyController.AdvanceCutscene();
                }
            }
        }
    }


}
