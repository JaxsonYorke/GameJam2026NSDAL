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
    }

    void OnDisable()
    {
        if (eventHandler != null)
            eventHandler.MB1clicked.RemoveListener(OnPrimaryClick);
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

    IEnumerator TransitionToFirstCutscene()
    {// Load scene completely
        yield return LoadSceneRoutine("GameIntro");

        // Scene is now active — safe to find objects
        storyController = GameObject
            .FindWithTag("StoryController")
            .GetComponent<CutsceneController>();

        storyController.startCutscene();
    }

    void SetState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                StartCoroutine(TransitionToFirstCutscene());
            break;

        }
    }

    /* ============================================================
     * INPUT HANDLING (EVENT-DRIVEN)
     * ============================================================ */

    void OnPrimaryClick()
    {
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
            yield return FadeIn();
    }

    IEnumerator FadeOut()
    {
        yield return FadeController.Instance.FadeOut();
    }

    IEnumerator FadeIn()
    {
        yield return FadeController.Instance.FadeIn();
    }
}
