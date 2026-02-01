using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private float runSpeed = 3.5f;
    private Rigidbody2D body;
    private Vector2 moveInput;
    private Animator animator;
    [SerializeField] [OptionalField] private GameObject MazeDoorEnd;
    [SerializeField] [OptionalField] private GameObject MazeBlockEnd;

    [SerializeField] private List<MaskedPlayerSprites> maskedPlayerSprites;

    private SpriteRenderer sr;

    public int CurrMask = 1;






    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        body.linearVelocity = moveInput * runSpeed;

        if(SceneManager.GetActiveScene().name == "Mask3" && FightController.Instance.IsInFight)
        {
            body.linearVelocity = moveInput * 0;
            return;
        }
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
                GameController.Instance.SetState(GameController.GameState.finalScene);
            }
        } else if (SceneManager.GetActiveScene().name == "Mask2")
        {
            if(collision.name == "SecondMask")
            {
                // TODO: change the sprites
                collision.gameObject.SetActive(false);
                SlideDoorDown();
                MazeBlockEnd.SetActive(false);

            } else if(collision.name == "LevelEndTrigger")
            {
                GameController.Instance.SetState(GameController.GameState.inBattle);
            }
        }
    }

    void SlideDoorDown()
    {
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