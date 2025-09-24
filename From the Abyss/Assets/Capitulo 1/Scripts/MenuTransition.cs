using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuTransition : MonoBehaviour
{
    public CanvasGroup CanvasGroup; 
    public float fadeDuration = 1.2f;

    public void LoadCapitulo1()
    {
        StartCoroutine(FadeOutAndLoad("Capitulo 1")); // Certifique-se do nome exato da cena
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        float elapsed = 0f;
        CanvasGroup.interactable = false; 
        CanvasGroup.blocksRaycasts = false;

        float startAlpha = CanvasGroup.alpha;
        float endAlpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / fadeDuration);
            CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        CanvasGroup.alpha = endAlpha;

        // Agora usa o parâmetro passado
        SceneManager.LoadScene(sceneName);
    }
}
