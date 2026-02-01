using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] [OptionalField] private GameObject MazeDoor;
    [SerializeField] [OptionalField] private GameObject MazeDoorEnd;
    [SerializeField] [OptionalField] private GameObject MazeBlockEnd;

    [SerializeField] private List<MaskedPlayerSprites> maskedPlayerSprites;

    private SpriteRenderer sr;

    public int CurrMask = 1;



    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;



    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Mask3" && FightController.Instance.IsInFight)
        {
            horizontal = 0;
            vertical = 0;
            return;
        }
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down


        // if(horizontal == 1)
        // {
        //     sr.sprite = maskedPlayerSprites[CurrMask].right;
        // } else if( horizontal == -1)
        // {
        //     sr.sprite = maskedPlayerSprites[CurrMask].left;
        // } else if (vertical == 1)
        // {
        //     sr.sprite = maskedPlayerSprites[CurrMask].up;
        // } else if(vertical == -1)
        // {
        //     sr.sprite = maskedPlayerSprites[CurrMask].down;
        // }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        } 

        body.linearVelocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(SceneManager.GetActiveScene().name == "Mask3")
        {
            if(collision.name == "FightTrigger")
            {
                collision.gameObject.SetActive(false);
                FightController.Instance.startFight();
            }
            if(collision.name == "Mask")
            {
                GameController.Instance.SetState(GameController.GameState.finalDecision);
            }
        } else if (SceneManager.GetActiveScene().name == "Mask2")
        {
            if (collision.name == "CloseTrigger")
            {
                collision.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(SlideDoorUp());
            } else if(collision.name == "SecondMask")
            {
                // TODO: change the sprites
                collision.gameObject.SetActive(false);
                StartCoroutine(SlideDoorDown());
                MazeBlockEnd.SetActive(false);

            } else if(collision.name == "LevelEndTrigger")
            {
                GameController.Instance.SetState(GameController.GameState.inBattle);
            }
        }
    }


    IEnumerator SlideDoorUp()
    {
        Vector3 startPos = MazeDoor.transform.position;
        Vector3 endPos = startPos + Vector3.up;
        float duration = 1f; // seconds
        float elapsed = 0f;

        while (elapsed < duration)
        {
            MazeDoor.transform.position =
                Vector3.Lerp(startPos, endPos, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        MazeDoor.transform.position = endPos;
    }

    IEnumerator SlideDoorDown()
    {
        Vector3 startPos = MazeDoorEnd.transform.position;
        Vector3 endPos = startPos + Vector3.down; // move 1 unit down
        float duration = 1f; // move over 1 second
        float elapsed = 0f;

        while (elapsed < duration)
        {
            MazeDoorEnd.transform.position =
                Vector3.Lerp(startPos, endPos, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        MazeDoorEnd.transform.position = endPos; // ensure exact final position
        MazeDoorEnd.SetActive(false);
    }

}
[System.Serializable]
public class MaskedPlayerSprites
{
    public int maskNumber;
    public Sprite up;
    public Sprite right;
    public Sprite down;
    public Sprite left;
}