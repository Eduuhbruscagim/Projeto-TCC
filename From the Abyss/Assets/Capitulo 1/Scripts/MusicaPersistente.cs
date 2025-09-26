using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicaManager : MonoBehaviour
{
    public AudioClip musicaSplashMenu;
    public AudioClip musicaCapitulo1;

    private AudioSource audioSource;
    private static MusicaManager instancia;

    void Awake()
    {
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // Já configura a música certa com base na cena atual
        DefinirMusica(SceneManager.GetActiveScene().name);

        // Continua reagindo a mudanças de cena
        SceneManager.sceneLoaded += OnCenaCarregada;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnCenaCarregada;
    }

    private void OnCenaCarregada(Scene cena, LoadSceneMode modo)
    {
        DefinirMusica(cena.name);
    }

    private void DefinirMusica(string nomeCena)
    {
        if (nomeCena == "Capitulo 1")
        {
            if (audioSource.clip != musicaCapitulo1)
            {
                audioSource.Stop();
                audioSource.clip = musicaCapitulo1;
                audioSource.Play();
            }
        }
        else if (nomeCena == "Splash" || nomeCena == "Menu")
        {
            if (audioSource.clip != musicaSplashMenu)
            {
                audioSource.Stop();
                audioSource.clip = musicaSplashMenu;
                audioSource.Play();
            }
        }
    }
}
