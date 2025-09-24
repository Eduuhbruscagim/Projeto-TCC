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
        // Garantir que só exista uma instância
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicaSplashMenu;
        audioSource.Play();

        // Detecta mudanças de cena
        SceneManager.sceneLoaded += OnCenaCarregada;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnCenaCarregada;
    }

    private void OnCenaCarregada(Scene cena, LoadSceneMode modo)
    {
        // Troca música automaticamente dependendo da cena
        if (cena.name == "Capitulo 1")
        {
            if (audioSource.clip != musicaCapitulo1)
            {
                audioSource.Stop();
                audioSource.clip = musicaCapitulo1;
                audioSource.Play();
            }
        }
        else if (cena.name == "Splash" || cena.name == "Menu")
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
