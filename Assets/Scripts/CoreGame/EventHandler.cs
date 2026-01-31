using UnityEngine;
using UnityEngine.Events;


public class EventHandler : MonoBehaviour
{
    const int INSPECTORSPACING = 8;
    [Header("Game Wide")][Space(INSPECTORSPACING)]
    [SerializeField] private UnityEvent _CutsceneAdvance = new();

    [SerializeField] private UnityEvent _MB1clicked = new();


    void Start()
    {

    }


    public void triggerCutsceneAdvance() {_CutsceneAdvance.Invoke();}

// Make the public getters

    //Game Wide
    public UnityEvent OnCutsceneAdvance => _CutsceneAdvance;
    public UnityEvent MB1clicked => _MB1clicked;
}

