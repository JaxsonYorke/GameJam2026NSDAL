using Unity.VisualScripting;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzlePiece[] puzzlePieces;
    private bool puzzleCompleted = false;
    [SerializeField]float ListLength;
    void Start()
    {
        float i = 0.5f;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            PolygonCollider2D poly = piece.GetComponent<PolygonCollider2D>();
            Vector2 center = poly.bounds.center;
            Debug.Log(center);
            piece.transform.position += (Vector3)(new Vector2(ListLength/-2,-3.8f) - center);
            float length = puzzlePieces.Length;
            piece.transform.position += new Vector3(ListLength*i/length,0f,0f);
            i++;
        }
    }
    void Update()
    {
        if (puzzleCompleted) return;

        // Check if all pieces are locked
        bool allLocked = true;
        foreach (PuzzlePiece piece in puzzlePieces)
        {
            if (!piece.IsLocked())
            {
                allLocked = false;
                break;
            }
        }

        if (allLocked)
        {
            puzzleCompleted = true;
            OnPuzzleComplete();
        }
    }

    void OnPuzzleComplete()
    {
        Debug.Log("Puzzle finished");
        
    }
}
