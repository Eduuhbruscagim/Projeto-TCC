using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    private Animator PersonagemAnim;

    private Rigidbody2D rbPersonagem;

    private SpriteRenderer spriteRenderer;

    [Header("Movimento")]
    public float Speed = 2f;

    public float JumpForce = 6;

    public bool NoChao = true;

    public bool PuloDuplo;

    [Header("Dash")]
    public float DashForce = 10f; // Velocidade do dash

    public float DashTime = 0.1f; // Duração do dash

    public float DashCooldown = 1f; // Tempo de cooldown entre dashes

    private bool isDashing = false;

    private float dashTimeLeft;

    private float lastDashTime;

    private Vector2 dashDirection;

    private ControladorJogo ControladorPersonagem;

    void Start()
    {
        ControladorPersonagem = ControladorJogo.Controlador;

        ControladorPersonagem.Moedas = 0;

        PersonagemAnim = GetComponent<Animator>();

        rbPersonagem = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Jump(); // Chama a função de pulo

        HandleDashInput(); // Captura input de dash
    }

    private void FixedUpdate()
    {
        if (!isDashing) // Enquanto não estiver dashing, aplica movimentação normal
        {
            MoverPersonagem();
        }
        else // Se estiver dashando
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

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (NoChao)
            {
                rbPersonagem.velocity = Vector2.zero;

                PersonagemAnim.SetBool("Jump", true);

                rbPersonagem.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);

                NoChao = false;

                PuloDuplo = true;
            }
            else if (!NoChao && PuloDuplo)
            {
                rbPersonagem.velocity = Vector2.zero;

                PersonagemAnim.SetBool("Jump", true);

                rbPersonagem.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);

                PuloDuplo = false;
            }
        }
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + DashCooldown)
        {
            dashDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;

            if (dashDirection == Vector2.zero) // Se não apertar direção, dash para frente
                dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            isDashing = true;

            dashTimeLeft = DashTime;

            lastDashTime = Time.time;

            rbPersonagem.gravityScale = 0; // Desativa gravidade durante dash

            rbPersonagem.velocity = dashDirection * DashForce;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Teto")
            return;

        if (collision.gameObject.name == "Ground")
        {
            PersonagemAnim.SetBool("Jump", false);

            NoChao = true;

            PuloDuplo = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Moedas")
        {
            Destroy(collision.gameObject);

            ControladorPersonagem.Moedas++;

            ControladorPersonagem.TextoMoeda.text = ControladorPersonagem.Moedas.ToString();
        }
    }
}
