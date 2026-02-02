using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour
{

    public static FightController Instance;

    [SerializeField] private GameObject Minotaur;
    [SerializeField] private GameObject Player;

    // TODO: put the sprites here for hurt, block, puch hit etc

    private int minotaurMaxHealth = 20;

    [SerializeField] private int MinotaurHealth = 20;
    [SerializeField] private int MinotaurDamage = 2;
    [SerializeField] private int PlayerHealth = 20;
    [SerializeField] private int PlayerDamage = 2;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject character;

    [SerializeField] private GameObject hitQTE;
    [SerializeField] private GameObject blockQTE;
    [SerializeField] private GameObject dodgeQTEa;
    [SerializeField] private GameObject dodgeQTEs;
    [SerializeField] private GameObject dodgeQTEd;
    [SerializeField] private GameObject dodgeQTELeftArrow;
    [SerializeField] private GameObject dodgeQTEDownArrow;
    [SerializeField] private GameObject dodgeQTERightArrow;

    [SerializeField] private bool currentResolved = true;

    [SerializeField] public bool IsInFight = false;

    [SerializeField] private float easySpeedMultiplier = 1f;
    [SerializeField] private float hardSpeedMultiplier = 0.5f;

    [SerializeField] private Sprite MinotaurAttack;
    [SerializeField] private Sprite MinotaurHurtAttack;
    [SerializeField] private Sprite MinotaurIdle;
    [SerializeField] private Sprite MinotaurHurtIdle;

    [SerializeField] private Sprite KidAttack;
    [SerializeField] private Sprite KidIdle;
    [SerializeField] private Sprite KidBlock;




    private enum Difficulty
    {
        Easy,
        Hard
    }


    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas.SetActive(false);
        if (currentResolved)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(MinotaurHealth <= 0)
        {
            PlayerWin();
        }else if(PlayerHealth <= 0)
        {
            MinotaurWin();
        }
    }

    public void startFight()
    {
        Canvas.SetActive(true);
        IsInFight = true;
        //? Maybe minataur apears, slides from top?
        character.SetActive(false);
        StartCoroutine(manageWaves());

    }

    IEnumerator manageWaves()
{
    yield return null; // allow canvas/UI to initialize

    while (IsInFight)
    {
        Difficulty difficulty =
            MinotaurHealth > minotaurMaxHealth / 2
                ? Difficulty.Easy
                : Difficulty.Hard;


        string nextQTE = GetRandomQTE(difficulty);

        yield return StartCoroutine(spawnQTE(nextQTE));

        // Wait until player resolves the QTE
        yield return new WaitUntil(() => currentResolved);

        // Small pacing delay
        yield return new WaitForSeconds(0.25f * GetCurrentSpeedMultiplier());
    }
}

string GetRandomQTE(Difficulty difficulty)
{
    if (difficulty == Difficulty.Easy)
    {
        string[] easyQTEs =
        {
            "hitQTE",
            "dodgeQTEa",
            "dodgeQTERightArrow",
            "blockQTE"
        };

        return easyQTEs[UnityEngine.Random.Range(0, easyQTEs.Length)];
    }
    else // Hard
    {
        string[] hardQTEs =
        {
            "hitQTE",
            "blockQTE",
            "dodgeQTEa",
            "dodgeQTEs",
            "dodgeQTEd",
            "dodgeQTELeftArrow",
            "dodgeQTEDownArrow",
            "dodgeQTERightArrow"
        };

        return hardQTEs[UnityEngine.Random.Range(0, hardQTEs.Length)];
    }
}



    public void MinotaurWin()
    {
        IsInFight = false;
        GameController.Instance.SetState(GameController.GameState.inChapel);
    }

    public void PlayerWin()
    {
        IsInFight = false;
        Canvas.SetActive(false);
        character.SetActive(true);
    }


IEnumerator spawnQTE(string name)
{
    currentResolved = false;

    GameObject qtePrefab = GetQTEPrefab(name);
    GameObject qteInstance = Instantiate(qtePrefab, Canvas.transform, false);

    float speedMultiplier = GetCurrentSpeedMultiplier();
    if (name.StartsWith("hit"))
    {
        qteInstance.GetComponent<letterQTE>().countdownDuration *= speedMultiplier;
    }
    else if (name.StartsWith("blockQTE")) {
        qteInstance.GetComponent<BlockQTE>().countdownDuration *= speedMultiplier;
    } else if (name.StartsWith("dodge"))
    {
        qteInstance.GetComponent<DodgeQTE>().countdownDuration *= speedMultiplier;
    }



    yield return new WaitUntil(() => currentResolved);
}

GameObject GetQTEPrefab(string name)
{
    switch (name)
    {
        case "hitQTE": return hitQTE;
        case "blockQTE": return blockQTE;
        case "dodgeQTEa": return dodgeQTEa;
        case "dodgeQTEs": return dodgeQTEs;
        case "dodgeQTEd": return dodgeQTEd;
        case "dodgeQTELeftArrow": return dodgeQTELeftArrow;
        case "dodgeQTEDownArrow": return dodgeQTEDownArrow;
        case "dodgeQTERightArrow": return dodgeQTERightArrow;
        default:
            Debug.LogError($"Unknown QTE type: {name}");
            return null;
    }
}


float GetCurrentSpeedMultiplier()
{
    return MinotaurHealth <= minotaurMaxHealth / 2
        ? hardSpeedMultiplier
        : easySpeedMultiplier;
}


    public void Hit()
    {
        //TODO: Change each sprite to have been hit and hit
        Player.GetComponent<Image>().sprite = KidAttack;
        if(MinotaurHealth < minotaurMaxHealth / 2)
        {
            // Minotaur.GetComponent<Image>().sprite = MinotaurHurtAttack;
            
        }
        StartCoroutine(backToIdle(0));
        // StartCoroutine(backToIdle(1));
        MinotaurHealth -= PlayerDamage;
        currentResolved = true;
    }   

    public void TakeHit()
    {
        //TODO: change sprites ofc
        Player.GetComponent<Image>().sprite = KidAttack;
        if(MinotaurHealth < minotaurMaxHealth / 2)
        {
            Minotaur.GetComponent<Image>().sprite = MinotaurHurtAttack;
            
        } else
        {
            Minotaur.GetComponent<Image>().sprite = MinotaurAttack;
        }
        StartCoroutine(backToIdle(0));
        StartCoroutine(backToIdle(1));
        PlayerHealth -= MinotaurDamage;
        currentResolved = true;
    }

    public void Dodge()
    {
        // TODO: Change sprites, no dmg change
        if(MinotaurHealth < minotaurMaxHealth / 2)
        {
            Minotaur.GetComponent<Image>().sprite = MinotaurHurtAttack;
            
        } else
        {
            Minotaur.GetComponent<Image>().sprite = MinotaurAttack;
        }
        StartCoroutine(backToIdle(1));
        currentResolved = true;
    }

    public void Block()
    {
        Player.GetComponent<Image>().sprite = KidBlock;
        if(MinotaurHealth < minotaurMaxHealth / 2)
        {
            Minotaur.GetComponent<Image>().sprite = MinotaurHurtAttack;
            
        } else
        {
            Minotaur.GetComponent<Image>().sprite = MinotaurAttack;
        }
        StartCoroutine(backToIdle(0));
        StartCoroutine(backToIdle(1));
        PlayerHealth -= MinotaurDamage / 2;        
        currentResolved = true;
    }

    internal void Miss()
    {
        currentResolved = true;
    }

    IEnumerator backToIdle(int which)
    {
        yield return new WaitForSeconds(0.3f);
        if(which == 0)
        {
            Player.GetComponent<Image>().sprite = KidIdle;
        } else
        {
            if(MinotaurHealth < minotaurMaxHealth / 2)
                {
                    Minotaur.GetComponent<Image>().sprite = MinotaurHurtIdle;
                    
                } else
                {
                    Minotaur.GetComponent<Image>().sprite = MinotaurIdle;
                }
            
        }
        yield return null;
    }
}

