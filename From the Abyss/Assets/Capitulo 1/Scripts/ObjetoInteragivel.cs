using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetoInteragivel : MonoBehaviour
{
    [Header("Elemento Visual da UI")]
    public GameObject promptVisual;

    [Header("Configuração da Cena")]
    public string nomeDaCenaParaCarregar;

    private bool jogadorEstaNaArea = false;

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
        if (jogadorEstaNaArea && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(nomeDaCenaParaCarregar);
        }
    }
}