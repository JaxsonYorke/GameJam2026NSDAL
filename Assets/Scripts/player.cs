using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public int horizontal;
    public Rigidbody2D body;
    public int runSpeed;

    public SpriteRenderer sr;
    public Sprite left;
    public Sprite right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        body.linearVelocity = new Vector2 (Input.GetAxisRaw("Horizontal") * runSpeed, Input.GetAxisRaw("Vertical") * runSpeed);

        if(Input.GetAxisRaw("Horizontal") == -1)
        {
            sr.sprite = left;
        } else
        {
            sr.sprite = right;
        }
        if(SceneManager.GetActiveScene().name == "Mask3" && FightController.Instance.IsInFight)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }
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
        }
    }

}
