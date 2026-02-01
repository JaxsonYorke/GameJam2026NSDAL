using UnityEngine;

public class DecisionHandller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void give()
    {
        GameController.Instance.AdvanceToGive();
    }

    public void keep()
    {
        GameController.Instance.AdvanceToKeep();
    }
}
