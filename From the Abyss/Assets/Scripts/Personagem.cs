using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    private Animator PersonagemAnim;
    private Rigidbody2D rbPersonagem;
    public float Speed = 5f;
    private SpriteRenderer spriteRenderer;
    public float JumpForce;
    public bool NoChao = true;
    public bool PuloDuplo;

    private ControladorJogo ControladorPersonagem;


    void Start()
    {
        ControladorPersonagem = ControladorJogo.Controlador;
        ControladorPersonagem.Moedas = 0;
        PersonagemAnim = GetComponent<Animator>();
        rbPersonagem = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Atualiza o movimento do personagem a cada frame (para uma movimentação mais suave)
    private void FixedUpdate()
    {
        MoverPersonagem(); // Chama a função de movimentação
    }

    // Atualiza o pulo do personagem a cada frame
    void Update()
    {
        Jump(); // Chama a função de pulo
    }

    // Controla a movimentação horizontal do personagem
    void MoverPersonagem()
    {
        // Pega a entrada de movimento horizontal (A/D ou setas do teclado)
        float MovimentoHorizontal = Input.GetAxisRaw("Horizontal");

        // Aplica a velocidade ao movimento horizontal mantendo a velocidade no eixo Y constante
        rbPersonagem.velocity = new Vector2(MovimentoHorizontal * Speed, rbPersonagem.velocity.y);

        // Controla a animação e a direção do personagem
        if (MovimentoHorizontal > 0)
        {
            PersonagemAnim.SetBool("Andar", true); // Ativa a animação de andar
            spriteRenderer.flipX = false; // Vira o sprite para a direita
        }
        else if (MovimentoHorizontal < 0)
        {
            PersonagemAnim.SetBool("Andar", true); // Ativa a animação de andar
            spriteRenderer.flipX = true; // Vira o sprite para a esquerda
        }
        else
        {
            PersonagemAnim.SetBool("Andar", false); // Desativa a animação de andar quando não há movimento
        }
    }

    // Controla o pulo do personagem
    void Jump()
    {
        // Verifica se o botão de pulo (normalmente espaço) foi pressionado
        if (Input.GetButtonDown("Jump"))
        {
            // Se o personagem está no chão, permite o primeiro pulo
            if (NoChao)
            {
                rbPersonagem.velocity = Vector2.zero; // Zera a velocidade para garantir que o pulo seja controlado
                PersonagemAnim.SetBool("Jump", true); // Ativa a animação de pulo
                rbPersonagem.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Aplica a força do pulo
                NoChao = false; // Marca que o personagem não está mais no chão
                PuloDuplo = true; // Permite o pulo duplo
            }
            // Se o personagem não está no chão, e ainda pode dar o pulo duplo
            else if (!NoChao && PuloDuplo)
            {
                rbPersonagem.velocity = Vector2.zero; // Zera a velocidade novamente
                PersonagemAnim.SetBool("Jump", true); // Ativa a animação de pulo
                rbPersonagem.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse); // Aplica a força do pulo duplo
                NoChao = false; // Marca que o personagem não está no chão
                PuloDuplo = false; // Desabilita o pulo duplo
            }
        }
    }

    // Verifica as colisões com o chão para permitir pular novamente
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se o personagem colidir com o objeto "Ground"
        if (collision.gameObject.name == "Ground")
        {
            PersonagemAnim.SetBool("Jump", false); // Desativa a animação de pulo
            NoChao = true; // Marca que o personagem está no chão
            PuloDuplo = false; // Desabilita o pulo duplo, pois o personagem tocou o chão
        }
    }

    // Verifica as colisões com objetos marcados com a tag "Moedas"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o objeto que o personagem colidiu for uma moeda
        if (collision.gameObject.tag == "Moedas")
        {
            Destroy(collision.gameObject); // Destrói o objeto da moeda
            ControladorPersonagem.Moedas++; // Adiciona o contador de moedas
            ControladorPersonagem.TextoMoeda.text = ControladorPersonagem.Moedas.ToString();
        }
    }
}
