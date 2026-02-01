using UnityEngine;

public class TriggerMover : MonoBehaviour
{
    public Transform targetBox;  // The solid BoxCollider2D object to move
    public Vector3 targetPosition;  // World coords to animate to (e.g., new Vector3(5f, 2f, 0f))
    
    private bool isAnimating = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isAnimating)  // Assumes player has "Player" tag
        {
            StartCoroutine(AnimateMove());
        }
    }
    
    private System.Collections.IEnumerator AnimateMove()
    {
        isAnimating = true;
        Vector3 startPos = targetBox.position;
        float duration = 1f;  // Animation time in seconds
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            targetBox.position = Vector3.Lerp(startPos, targetPosition, t);  // Smooth interpolation
            yield return null;
        }
        
        targetBox.position = targetPosition;  // Snap to exact end position
        isAnimating = false;
    }
}
