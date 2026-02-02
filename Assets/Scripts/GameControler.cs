using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public enum GameState
    {
        MainMenu,
        FirstCutscene,
        inChapel,
        inPuzzle,
        inMaze,
        inBattle,
        finalScene,
        finalDecision,
        give,
        keep
    }

    [Header("State")]
    [SerializeField] public GameState CurrentState;

    [Header("References")]
    [SerializeField] private EventHandler eventHandler;

    [SerializeField] private CutsceneController storyController;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.tag = "GameController";


    }

    void OnEnable()
    {
        // Global input → event
        if (eventHandler != null)
            eventHandler.MB1clicked.AddListener(OnPrimaryClick);


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        if (eventHandler != null)
            eventHandler.MB1clicked.RemoveListener(OnPrimaryClick);

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find it — but don't assume it exists
        GameObject storyObj = GameObject.FindWithTag("StoryController");

        if (storyObj == null)
            return; // Normal scene, nothing to do

        storyController = storyObj.GetComponent<CutsceneController>();
        storyController.StartCutscene();
    }

    /* ============================================================
     * PUBLIC ENTRY POINTS
     * ============================================================ */

    public void AdvanceFromMainMenu()
    {
        SetState(GameState.FirstCutscene);
    }

    public void AdvanceFromGameIntro()
    {
        SetState(GameState.inChapel);
    }

    public void AdvanceFromInChapel()
    {
        SetState(GameState.inPuzzle);
    }

    public void AdvanceFromFinalScene()
    {
        SetState(GameState.finalDecision);
    }

    public void AdvanceToGive()
    {
        SetState(GameState.give);
    }

    public void AdvanceToKeep()
    {
        SetState(GameState.keep);
    }

    /* ============================================================
     * STATE TRANSITIONS
     * ============================================================ */

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {

            case GameState.FirstCutscene:
                StartCoroutine(LoadSceneRoutine("GameIntro"));
            break;
            case GameState.inChapel:
                StartCoroutine(LoadSceneRoutine("InsideChapel"));
            break;
            case GameState.inPuzzle:
                StartCoroutine(LoadSceneRoutine("Mask1"));
            break;
            case GameState.inMaze:
                StartCoroutine(LoadSceneRoutine("Mask2"));
            break;
            case GameState.inBattle:
                StartCoroutine(LoadSceneRoutine("Mask3"));
            break;
            case GameState.finalScene:
                StartCoroutine(LoadSceneRoutine("FinalScene"));
            break;
            case GameState.finalDecision:
                StartCoroutine(LoadSceneRoutine("Decision"));
            break;
            case GameState.give:
                StartCoroutine(LoadSceneRoutine("give"));
            break;
            case GameState.keep:
                StartCoroutine(LoadSceneRoutine("keep"));
            break;

        }
    }

    /* ============================================================
      * ============================================================ */

    void OnPrimaryClick()
    {
        Debug.Log("mb1 pressed");
        try
        {
            switch(CurrentState)
            {
                
                case GameState.FirstCutscene:
                case GameState.inChapel:
                case GameState.finalScene:
                case GameState.give:
                case GameState.keep:
                    if (!storyController.timeout)
                    {
                        storyController.AdvanceCutscene();
                    }
                break;
            } 
        }
        catch (System.Exception err)
        {
            Debug.LogError(err);
        }

    }

    /* ============================================================
     * SCENE LOADING
     * ============================================================ */

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        yield return FadeController.Instance.FadeOut();

        // Wait until scene is loaded (90%)
        while (op.progress < 0.9f)
            yield return null;

        // Activate scene
        op.allowSceneActivation = true;

        // Wait until activation is complete
        while (!op.isDone)
            yield return FadeController.Instance.FadeIn();
    }

}
