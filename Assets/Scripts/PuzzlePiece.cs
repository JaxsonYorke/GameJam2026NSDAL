using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PuzzlePiece : MonoBehaviour
{
    public static float snapDistance = 0.4f;
    public Camera puzzleCamera;
    private Vector3 snapPosition;
    private Vector3 dragOffset;
    private bool dragging = false;
    private bool locked = false;

    void Awake()
    {
        if (puzzleCamera == null)
            puzzleCamera = Camera.main;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        
    }

    void Start()
    {
        snapPosition = new Vector3(0f,1.33f,0f);
    }

    void Update()
    {
        if (locked || puzzleCamera == null) return;

        // Get current mouse/touch position
        Vector2 pointerPos = Mouse.current.position.ReadValue(); // New Input System
        Vector3 worldPos = puzzleCamera.ScreenToWorldPoint(pointerPos);
        worldPos.z = 0;

        // Pointer down
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = puzzleCamera.ScreenPointToRay(pointerPos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                dragOffset = transform.position - worldPos;
                dragging = true;
                transform.SetAsLastSibling();
            }
        }

        // Dragging
        if (dragging)
            transform.position = worldPos + dragOffset;

        // Pointer up
        if (Mouse.current.leftButton.wasReleasedThisFrame && dragging)
        {
            dragging = false;
            TrySnap();
        }
    }

    void TrySnap()
    {
        if (Vector2.Distance(transform.position, snapPosition) <= snapDistance)
        {
            transform.position = snapPosition;
            locked = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
    public bool IsLocked()
    {
        return locked;
    }
}
