<<<<<<< HEAD
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
=======
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
>>>>>>> 2bd222e336acc0fa7f03a784bc5cceed3f8c8536

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float runSpeed = 3.5f;
    private Rigidbody2D body;
    private Vector2 moveInput;
    private Animator animator;
=======
    Rigidbody2D body;
    [SerializeField] [OptionalField] private GameObject MazeDoor;
    [SerializeField] [OptionalField] private GameObject MazeDoorEnd;
    [SerializeField] [OptionalField] private GameObject MazeBlockEnd;

    [SerializeField] private List<MaskedPlayerSprites> maskedPlayerSprites;

    private SpriteRenderer sr;

    public int CurrMask = 1;


>>>>>>> 2bd222e336acc0fa7f03a784bc5cceed3f8c8536




    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        animator = GetComponent<Animator>();
=======
        sr = GetComponent<SpriteRenderer>();
>>>>>>> 2bd222e336acc0fa7f03a784bc5cceed3f8c8536
    }

    void Update()
    {
<<<<<<< HEAD
        body.linearVelocity = moveInput * runSpeed;
=======
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
>>>>>>> 2bd222e336acc0fa7f03a784bc5cceed3f8c8536
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if(context.canceled)
        {
            animator.SetBool("isWalking", false);
            if(moveInput.x != 0)
            {
                animator.SetFloat("LastInputX", moveInput.x);
            }
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
    }

<<<<<<< HEAD
=======
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

>>>>>>> 2bd222e336acc0fa7f03a784bc5cceed3f8c8536
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