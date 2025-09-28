using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextoFinal : MonoBehaviour
{
    [Header("Referências")]
    public TextMeshProUGUI caixaDeTexto; 
    public Image imagemNormal; // imagem limpa
    public Image imagemBlur;   // imagem desfocada
    public TextMeshProUGUI avisoEnter; // mensagem piscando com fade
    public Image fadePreto; // imagem preta full screen para fade de cena

    [Header("Configurações")]
    [TextArea(3, 10)]
    public string[] frases;
    public float velocidade = 0.05f;
    public float pausaEntreFrases = 2f;
    public float tempoImagem = 3f; // tempo que a imagem limpa fica antes do fade
    public float tempoTransicao = 2f; // duração do fade entre normal e blur
    public float duracaoFadeAviso = 1f; // tempo de fade do aviso
    public float duracaoFadeCena = 2f; // tempo da transição de cena

    [Header("Após o fim do texto")]
    public string nomeCenaMenu = "Menu";

    private bool textoTerminado = false;

    private void Start()
    {
        // Inicializa o aviso transparente
        if (avisoEnter != null)
        {
            Color c = avisoEnter.color;
            c.a = 0f;
            avisoEnter.color = c;
        }

        // Inicializa a imagem de fade preta transparente
        if (fadePreto != null)
        {
            Color c = fadePreto.color;
            c.a = 0f;
            fadePreto.color = c;
        }

        StartCoroutine(SequenciaFinal());
    }

    void Update()
    {
        if (textoTerminado && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TransicaoCena());
        }
    }

    IEnumerator SequenciaFinal()
    {
        // estado inicial
        caixaDeTexto.text = "";
        SetAlpha(imagemNormal, 1f);
        SetAlpha(imagemBlur, 0f);

        // espera com a imagem limpa
        yield return new WaitForSeconds(tempoImagem);

        // fade para blur
        float t = 0;
        while (t < tempoTransicao)
        {
            t += Time.deltaTime;
            float alpha = t / tempoTransicao;
            SetAlpha(imagemNormal, 1f - alpha);
            SetAlpha(imagemBlur, alpha);
            yield return null;
        }

        // texto letra por letra
        foreach (string frase in frases)
        {
            caixaDeTexto.text = "";
            foreach (char letra in frase)
            {
                caixaDeTexto.text += letra;
                yield return new WaitForSeconds(velocidade);
            }
            yield return new WaitForSeconds(pausaEntreFrases);
        }

        // termina texto e inicia fade do aviso
        textoTerminado = true;
        if (avisoEnter != null)
            StartCoroutine(FadeAvisoLoop());
    }

    IEnumerator FadeAvisoLoop()
    {
        while (true)
        {
            // fade in
            yield return StartCoroutine(FadeAviso(0f, 1f));
            // fade out
            yield return StartCoroutine(FadeAviso(1f, 0f));
        }
    }

    IEnumerator FadeAviso(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c = avisoEnter.color;

        while (t < duracaoFadeAviso)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / duracaoFadeAviso);
            c.a = alpha;
            avisoEnter.color = c;
            yield return null;
        }

        c.a = endAlpha;
        avisoEnter.color = c;
    }

    IEnumerator TransicaoCena()
    {
        // bloqueia múltiplos triggers
        textoTerminado = false;

        float t = 0f;
        Color c = fadePreto.color;

        while (t < duracaoFadeCena)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duracaoFadeCena);
            fadePreto.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePreto.color = c;

        SceneManager.LoadScene(nomeCenaMenu);
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
