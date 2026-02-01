using System;
using UnityEngine;

public class FightController : MonoBehaviour
{

    public static FightController Instance;

    [SerializeField] private GameObject Minotaur;
    [SerializeField] private GameObject Player;

    // TODO: put the sprites here for hurt, block, puch hit etc

    [SerializeField] private int MinotaurHealth = 20;
    [SerializeField] private int MinotaurDamage = 2;
    [SerializeField] private int PlayerHealth = 20;
    [SerializeField] private int PlayerDamage = 2;



    
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        //TODO: Change each sprite to have been hit and hit
        MinotaurHealth -= PlayerDamage;

    }

    public void TakeHit()
    {
        //TODO: change sprites ofc
        PlayerHealth -= MinotaurDamage;
    }

    public void Dodge()
    {
        throw new NotImplementedException();
    }
}
