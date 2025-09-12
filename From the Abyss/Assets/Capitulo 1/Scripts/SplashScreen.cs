using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public Image titleImage;
    public float fadeInDuration = 1.0f;
    public float displayDuration = 1.3f;
    public float fadeOutDuration = 1.1f;
    public float fadeInScale = 1.08f;
    public float fadeOutOvershoot = 1.12f;
    public Color startColor = new Color(1f, 0.9f, 0.7f, 0f);
    public Color endColor = new Color(1f, 1f, 1f, 1f);

    void Start()
    {
        titleImage.color = startColor;
        titleImage.transform.localScale = Vector3.one * 0.92f;
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade-in
        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration, Vector3.one * 0.92f, Vector3.one * fadeInScale, true));

        // Mantém visível
        yield return new WaitForSeconds(displayDuration);

        // Fade-out
        yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration, Vector3.one * fadeInScale, Vector3.one * 1.1f, false));

        SceneManager.LoadScene("Menu");
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration, Vector3 startScale, Vector3 endScale, bool fadeIn)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Curvas: ease-in para fade-in, ease-out para fade-out
            t = fadeIn ? t * t : 1f - Mathf.Pow(1f - t, 3f);

            // Alpha e cor
            Color c = Color.Lerp(startColor, endColor, t);
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            titleImage.color = c;

            // Escala com overshoot no fade-out
            Vector3 scaleTarget = fadeIn ? endScale : Vector3.Lerp(endScale, Vector3.one * fadeOutOvershoot, Mathf.Sin(t * Mathf.PI * 0.5f));
            titleImage.transform.localScale = Vector3.Lerp(startScale, scaleTarget, t);

            yield return null;
        }

        Color finalColor = Color.Lerp(startColor, endColor, 1f);
        finalColor.a = endAlpha;
        titleImage.color = finalColor;
        titleImage.transform.localScale = fadeIn ? endScale : Vector3.one * 1.1f;
    }
}
