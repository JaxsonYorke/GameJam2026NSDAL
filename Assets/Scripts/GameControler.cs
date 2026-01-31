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
        OnRoad
    }

    [Header("State")]
    public GameState CurrentState { get; private set; }

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

        if(scene.name == "GameIntro")
        {
            CurrentState = GameState.FirstCutscene;
        }
    }

    /* ============================================================
     * PUBLIC ENTRY POINTS
     * ============================================================ */

    public void AdvanceFromMainMenu()
    {
        SetState(GameState.FirstCutscene);
    }

    /* ============================================================
     * STATE TRANSITIONS
     * ============================================================ */

    void SetState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.FirstCutscene:
                StartCoroutine(LoadSceneRoutine("GameIntro"));

            break;

        }
    }

    /* ============================================================
     * INPUT HANDLING (EVENT-DRIVEN)
     * ============================================================ */

    void OnPrimaryClick()
    {
        Debug.Log("mb1 pressed");
        switch(CurrentState)
        {
            case GameState.FirstCutscene when !storyController.timeout:
                storyController.AdvanceCutscene();
            break;
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
