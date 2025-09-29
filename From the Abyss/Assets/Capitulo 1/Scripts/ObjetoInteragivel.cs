using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjetoInteragivel : MonoBehaviour
{
    [Header("Elemento Visual da UI")]
    public GameObject promptVisual;

    [Header("Configuração da Cena")]
    public string nomeDaCenaParaCarregar;

    [Header("Fade")]
    public Image fadePreto;        // arraste a Image preta aqui
    public float duracaoFade = 2f; // duração do fade em segundos

    private bool jogadorEstaNaArea = false;
    private bool emTransicao = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorEstaNaArea = true;
            promptVisual.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorEstaNaArea = false;
            promptVisual.SetActive(false);
        }
    }

    private void Update()
    {
        if (!emTransicao && jogadorEstaNaArea && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FazerFadeECarregarCena());
        }
    }

    private IEnumerator FazerFadeECarregarCena()
    {
        emTransicao = true;

        // inicia fade da música se existir MusicaManager
        MusicaManager mm = FindObjectOfType<MusicaManager>();
        if (mm != null)
        {
            mm.FadeOutMusica(duracaoFade);
        }

        float t = 0f;
        Color c = fadePreto.color;

        while (t < duracaoFade)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duracaoFade);
            fadePreto.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePreto.color = c;

        SceneManager.LoadScene(nomeDaCenaParaCarregar);
    }
}
