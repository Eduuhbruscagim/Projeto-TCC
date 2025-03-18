using UnityEngine;

public class ColisaoEspinho : MonoBehaviour
{
    public Transform SpawnPoint; // Ponto de reinício, onde o personagem será teleportado depois de colidir com o espinho.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto que colidiu com o espinho tem o nome "Personagem"
        if (collision.gameObject.name == "Personagem")
        {
            // Quando o personagem colide com o espinho, reposiciona ele para o ponto de reinício (SpawnPoint)
            collision.gameObject.transform.position = SpawnPoint.position;
        }
    }
}
