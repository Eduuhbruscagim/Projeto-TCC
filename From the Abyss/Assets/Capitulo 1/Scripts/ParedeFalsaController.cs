using UnityEngine;

public class ParedeFalsaController : MonoBehaviour
{
    // A tag que seu jogador usa.
    public string playerTag = "Player";

    // A Unity vai ler a propriedade 'isFakeWall' que você criou no Tiled
    // e associar a essa variável no script.
    public bool isFakeWall = false;

    // A Unity chama essa função quando o player entra na área do collider.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que entrou é o player e se esta parede é a falsa.
        if (other.CompareTag(playerTag) && isFakeWall)
        {
            // Pega o componente que desenha a parede na tela.
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            // E o desativa, fazendo a parede sumir.
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}