using UnityEngine;

public class ParallaxMenu : MonoBehaviour
{
    [Header("Configurações do Parallax")]
    public float intensidade = 30f; // Quanto maior, mais a imagem se mexe
    public float suavizacao = 5f;   // Velocidade da suavização

    [Header("Configurações do Zoom")]
    public float intensidadeZoom = 0.05f; // Ex: 0.05 = 5%
    public float velocidadeZoom = 1f;     // Velocidade da respiração

    private RectTransform rect;
    private Vector3 posicaoInicial;
    private Vector3 escalaInicial;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        posicaoInicial = rect.anchoredPosition;
        escalaInicial = rect.localScale;
    }

    void Update()
    {
        // ===== Parallax =====
        Vector2 mouseNormalizado = new Vector2(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height
        );

        mouseNormalizado -= new Vector2(0.5f, 0.5f);

        Vector3 deslocamento = new Vector3(
            mouseNormalizado.x * intensidade,
            mouseNormalizado.y * intensidade,
            0f
        );

        rect.anchoredPosition = Vector3.Lerp(
            rect.anchoredPosition,
            posicaoInicial + deslocamento,
            Time.deltaTime * suavizacao
        );

        // ===== Zoom Animado =====
        float fator = 1f + Mathf.Sin(Time.time * velocidadeZoom) * intensidadeZoom * 0.5f;
        rect.localScale = escalaInicial * fator;
    }
}
