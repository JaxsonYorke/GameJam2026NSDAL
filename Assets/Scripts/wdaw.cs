using UnityEngine;

public class wdaw : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.Instance.SetState(GameController.GameState.inBattle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
