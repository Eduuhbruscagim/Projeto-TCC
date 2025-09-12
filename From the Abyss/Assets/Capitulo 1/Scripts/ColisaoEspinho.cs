using UnityEngine;

public class ColisaoEspinho : MonoBehaviour
{
    public Transform SpawnPoint; // Ponto de reinício, onde o personagem será teleportado depois de colidir com o espinho.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto que colidiu com o espinho tem o nome "Personagem"
        if (collision.gameObject.name == "Personagem")
        {
            // Reposiciona o personagem no ponto de reinício
            collision.gameObject.transform.position = SpawnPoint.position;

            // Garante que o personagem esteja sempre olhando para a direita
            SpriteRenderer spriteRendererPersonagem = collision.gameObject.GetComponent<SpriteRenderer>();
            spriteRendererPersonagem.flipX = false;
        }
    }
}
