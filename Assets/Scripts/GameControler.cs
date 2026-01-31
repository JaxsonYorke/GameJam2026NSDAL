using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.CustomDebug;


// This class will be a parent class for other cutscenes
public class GameController : MonoBehaviour
{
    [SerializeField]
    private EventHandler eventHandler;
    void Awake()
    {
        DontDestroyOnLoad(this);
        DebugStatsDisplay.Instance.RegisterDebugStatsRequest(
            new DebugStatsRequest("MouseButtonPressed", () => {return Input.GetMouseButton(0);}));
    }
    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    private void getInput()
    {
        if (Input.GetMouseButtonUp(1))
        {
            eventHandler.TriggerMonsterDeath();
        }
    }

    

}