using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Personagem : MonoBehaviour
{
    #region Variáveis e Componentes

    // --- Referências de Componentes ---
    private Animator personagemAnim;
    private Rigidbody2D rbPersonagem;
    private SpriteRenderer spriteRenderer;
    private ControladorJogo controladorPersonagem;

    // --- Variáveis Públicas Configuráveis ---

    [Header("Movimento")]
    [Tooltip("A velocidade de movimento horizontal do personagem.")]
    public float Speed = 2f;
    [Tooltip("A força aplicada no pulo inicial.")]
    public float JumpForce = 6;

    [Header("Dash")]
    [Tooltip("A força do impulso do dash.")]
    public float DashForce = 10f;
    [Tooltip("A duração do dash em segundos.")]
    public float DashTime = 0.1f;
    [Tooltip("O tempo de espera (cooldown) entre um dash e outro.")]
    public float DashCooldown = 1f;

    // --- Variáveis de Estado Privadas ---
    private bool noChao = true;
    public bool EstaNoChao { get { return noChao; } }
    private bool puloDuplo;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime;
    private Vector2 dashDirection;
    private float moveInput;

    #endregion

    #region Métodos do Ciclo de Vida da Unity

    private void Start()
    {
        // Inicializa as referências dos componentes
        personagemAnim = GetComponent<Animator>();
        rbPersonagem = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Encontra o controlador do jogo e zera as moedas
        controladorPersonagem = ControladorJogo.Controlador;
        controladorPersonagem.Moedas = 0;
    }

    private void Update()
    {
        // Lê os inputs do jogador a cada frame
        moveInput = Input.GetAxisRaw("Horizontal");

        // Chama as funções que dependem de inputs de frame a frame
        Jump();
        HandleDashInput();
    }

    private void FixedUpdate()
    {
        // Funções de física são chamadas no FixedUpdate
        if (!isDashing)
        {
            MoverPersonagem();
        }
        else
        {
            Dash();
        }

        // Aplica a lógica de pulo com gravidade variável
        HandleBetterJump();
    }

    #endregion

    #region Movimentação e Ações

    
    // Aplica o movimento horizontal ao personagem com base no input.
    
    private void MoverPersonagem()
    {
        rbPersonagem.velocity = new Vector2(moveInput * Speed, rbPersonagem.velocity.y);

        // Controla a animação de andar e a direção do sprite
        if (moveInput != 0)
        {
            personagemAnim.SetBool("Andar", true);
            spriteRenderer.flipX = moveInput < 0;
        }
        else
        {
            personagemAnim.SetBool("Andar", false);
        }
    }

    
    // Gerencia a lógica do pulo e do pulo duplo.
    
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (noChao)
            {
                rbPersonagem.velocity = new Vector2(rbPersonagem.velocity.x, 0);
                personagemAnim.SetBool("Jump", true);
                rbPersonagem.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

                noChao = false;
                puloDuplo = true;
            }
            else if (puloDuplo)
            {
                rbPersonagem.velocity = new Vector2(rbPersonagem.velocity.x, 0);
                personagemAnim.SetBool("Jump", true);
                rbPersonagem.AddForce(Vector2.up * (JumpForce * 0.8f), ForceMode2D.Impulse);

                puloDuplo = false;
            }
        }
    }

    
    // Ouve o input do dash e inicia a ação se as condições forem atendidas.
    
    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + DashCooldown)
        {
            float direcaoX = Input.GetAxisRaw("Horizontal");
            float direcaoY = Input.GetAxisRaw("Vertical");

            // Bloqueia o dash para cima
            if (direcaoY > 0)
            {
                direcaoY = 0;
            }

            dashDirection = new Vector2(direcaoX, direcaoY).normalized;

            // Cláusula de guarda: se não houver input de direção, não faz nada.
            if (dashDirection == Vector2.zero)
            {
                return;
            }

            isDashing = true;
            dashTimeLeft = DashTime;
            lastDashTime = Time.time;
            rbPersonagem.gravityScale = 0;
        }
    }

    
    // Aplica a força do dash enquanto ele estiver ativo.
    
    private void Dash()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, DashForce * Time.fixedDeltaTime, LayerMask.GetMask("Ground"));

        if (dashTimeLeft > 0 && hit.collider == null)
        {
            rbPersonagem.velocity = dashDirection * DashForce;
            dashTimeLeft -= Time.fixedDeltaTime;
        }
        else
        {
            isDashing = false;
        }
    }

    
    // Melhora a sensação do pulo ajustando a gravidade dinamicamente.
    
    private void HandleBetterJump()
    {
        if (isDashing) return; // Não aplica gravidade extra durante o dash

        if (rbPersonagem.velocity.y < 0)
        {
            rbPersonagem.gravityScale = 2f; // Descendo mais rápido
        }
        else if (rbPersonagem.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rbPersonagem.gravityScale = 4f; // Subindo, mas o jogador soltou o botão
        }
        else
        {
            rbPersonagem.gravityScale = 1.8f; // Gravidade padrão
        }
    }

    #endregion

    #region Colisões e Gatilhos

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica colisões com o chão para resetar o pulo
        if (collision.gameObject.CompareTag("Ground"))
        {
            personagemAnim.SetBool("Jump", false);
            noChao = true;
            puloDuplo = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica o toque em moedas
        if (collision.gameObject.CompareTag("Moedas"))
        {
            collision.gameObject.SetActive(false);
            controladorPersonagem.Moedas++;
            controladorPersonagem.TextoMoeda.text = controladorPersonagem.Moedas.ToString();
        }
    }

    #endregion
}