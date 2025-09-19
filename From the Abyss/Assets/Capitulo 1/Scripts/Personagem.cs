using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    // Componentes do personagem
    private Animator PersonagemAnim;
    private Rigidbody2D rbPersonagem;
    private SpriteRenderer spriteRenderer;

    [Header("Movimento")]
    public float Speed = 2f;
    public float JumpForce = 6;
    public bool NoChao = true;
    public bool PuloDuplo;

    [Header("Dash")]
    public float DashForce = 10f;
    public float DashTime = 0.1f;
    public float DashCooldown = 1f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime;
    private Vector2 dashDirection;

    // Controlador do jogo
    private ControladorJogo ControladorPersonagem;

    // Inicialização
    void Start()
    {
        ControladorPersonagem = ControladorJogo.Controlador;
        ControladorPersonagem.Moedas = 0;

        PersonagemAnim = GetComponent<Animator>();
        rbPersonagem = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Atualização por frame
    void Update()
    {
        Jump();
        HandleDashInput();
    }

    // Atualização de física
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            MoverPersonagem();
        }
        else
        {
            if (dashTimeLeft > 0)
            {
                rbPersonagem.velocity = dashDirection * DashForce;
                dashTimeLeft -= Time.fixedDeltaTime;
            }
            else
            {
                isDashing = false;
                rbPersonagem.gravityScale = 3; // Volta gravidade padrão
            }
        }
    }

    // Movimentação horizontal
    void MoverPersonagem()
    {
        float MovimentoHorizontal = Input.GetAxisRaw("Horizontal");
        rbPersonagem.velocity = new Vector2(MovimentoHorizontal * Speed, rbPersonagem.velocity.y);

        if (MovimentoHorizontal > 0)
        {
            PersonagemAnim.SetBool("Andar", true);
            spriteRenderer.flipX = false;
        }
        else if (MovimentoHorizontal < 0)
        {
            PersonagemAnim.SetBool("Andar", true);
            spriteRenderer.flipX = true;
        }
        else
        {
            PersonagemAnim.SetBool("Andar", false);
        }
    }

    // Pulo simples e duplo
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (NoChao)
            {
                rbPersonagem.velocity = new Vector2(rbPersonagem.velocity.x, 0); // Reseta Y
                PersonagemAnim.SetBool("Jump", true);
                rbPersonagem.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

                NoChao = false;
                PuloDuplo = true;
            }
            else if (!NoChao && PuloDuplo)
            {
                rbPersonagem.velocity = new Vector2(rbPersonagem.velocity.x, 0); // Reseta Y
                PersonagemAnim.SetBool("Jump", true);
                rbPersonagem.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

                PuloDuplo = false;
            }
        }
    }

    // Controle de dash
    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + DashCooldown)
        {
            dashDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

            if (dashDirection == Vector2.zero)
                dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            isDashing = true;
            dashTimeLeft = DashTime;
            lastDashTime = Time.time;
            rbPersonagem.gravityScale = 0;
            rbPersonagem.velocity = dashDirection * DashForce;
        }
    }

    // Detecta colisão com o chão
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            PersonagemAnim.SetBool("Jump", false);
            NoChao = true;
            PuloDuplo = false;
        }
    }

    // Detecta entrada em trigger (ex: moedas)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moedas"))
        {
            Destroy(collision.gameObject);
            ControladorPersonagem.Moedas++;
            ControladorPersonagem.TextoMoeda.text = ControladorPersonagem.Moedas.ToString();
        }
    }
}
