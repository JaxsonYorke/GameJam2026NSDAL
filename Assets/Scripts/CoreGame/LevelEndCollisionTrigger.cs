using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch(GameController.Instance.CurrentState)
            {
                case(GameController.GameState.inMaze):
                    GameController.Instance.SetState(GameController.GameState.inBattle);
                    break;
            }
        }
    }
}
