using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GameController))]
public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;

    private GameController gc;
    const int INSPECTORSPACING = 8;
    [Header("Game Wide")][Space(INSPECTORSPACING)]
    [SerializeField] private UnityEvent _CutsceneAdvance = new();

    [SerializeField] private UnityEvent _MB1clicked = new();
    [SerializeField] private UnityEvent _MB2clicked = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        gc = GetComponent<GameController>();
    }

    void Update()
    {
        switch (GameController.Instance.CurrentState)
        {
            case GameController.GameState.MainMenu:

            break;
            case GameController.GameState.FirstCutscene:
            case GameController.GameState.inChapel:
            case GameController.GameState.finalScene:
            case GameController.GameState.give:
            case GameController.GameState.keep:
                if (Input.GetMouseButtonDown(0))
                {
                    _MB1clicked.Invoke();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    _MB2clicked.Invoke();
                }
            break;
        }
    }


    public void triggerCutsceneAdvance() {_CutsceneAdvance.Invoke();}

// Make the public getters

    //Game Wide
    public UnityEvent OnCutsceneAdvance => _CutsceneAdvance;
    public UnityEvent MB1clicked => _MB1clicked;
}

