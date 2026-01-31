using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject.transform.parent);
        
    }

    void Start()
    {
        // Ensure we start visible or invisible depending on needs
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeOut()
    {
        canvasGroup.blocksRaycasts = true;
        yield return Fade(1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(0f);
        canvasGroup.blocksRaycasts = false;
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
