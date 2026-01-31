using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
class ClickIconColorAlternate : MonoBehaviour
{
    private float time;
    private int direction = 1;

    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 1f;
    [SerializeField] private float period = 1f;

    private Image icon;

    void Start()
    {
        icon = GetComponent<Image>();
        StartCoroutine(Alternate());
    }

    void OnEnable()
    {
        icon = GetComponent<Image>();
        StartCoroutine(Alternate());
    }


    IEnumerator Alternate()
    {
        while (true)
        {
            time = 0f;
            
            while (time < period)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / period);
                
                if (direction == 1)
                {
                    // Fading in from min to max
                    icon.color = new Color(1f, 1f, 1f, Mathf.Lerp(min, max, t));
                }
                else
                {
                    // Fading out from max to min
                    icon.color = new Color(1f, 1f, 1f, Mathf.Lerp(max, min, t));
                }
                
                yield return null;
            }
            
            // Toggle direction for next cycle
            direction *= -1;
        }
    }
}