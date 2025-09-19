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

    // Input horizontal armazenado
    private float moveInput;

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
        moveInput = Input.GetAxisRaw("Horizontal");

        Jump(); 
        HandleDashInput(); 
    }

    private void FixedUpdate()
    {
        if (!isDashing)
            MoverPersonagem();
        else
        {
            // Raycast para frente para detectar colisão durante o dash
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, DashForce * Time.fixedDeltaTime, LayerMask.GetMask("Ground"));
            
            if (dashTimeLeft > 0 && hit.collider == null)
            {
                rbPersonagem.velocity = dashDirection * DashForce;
                dashTimeLeft -= Time.fixedDeltaTime;
            }
            else
            {
                isDashing = false;
                rbPersonagem.gravityScale = 3; // volta gravidade
            }
        }

        // Better Jump - gravidade variável
        if (rbPersonagem.velocity.y < 0)
            rbPersonagem.gravityScale = 2f; // descendo rápido
        else if (rbPersonagem.velocity.y > 0 && !Input.GetButton("Jump"))
            rbPersonagem.gravityScale = 4f; // subindo mas soltou botão
        else
            rbPersonagem.gravityScale = 3f; // gravidade padrão
    }

    void MoverPersonagem()
    {
        rbPersonagem.velocity = new Vector2(moveInput * Speed, rbPersonagem.velocity.y);

        if (moveInput > 0)
        {
            PersonagemAnim.SetBool("Andar", true);
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            PersonagemAnim.SetBool("Andar", true);
            spriteRenderer.flipX = true;
        }
        else
            PersonagemAnim.SetBool("Andar", false);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (NoChao)
            {
                // Primeiro pulo
                float jumpForceActual = JumpForce;
                rbPersonagem.velocity = new Vector2(rbPersonagem.velocity.x, 0);
                PersonagemAnim.SetBool("Jump", true);
                rbPersonagem.AddForce(Vector2.up * jumpForceActual, ForceMode2D.Impulse);

                NoChao = false;
                PuloDuplo = true;
            }
            else if (!NoChao && PuloDuplo)
            {
                // Segundo pulo mais suave
                float jumpForceActual = JumpForce * 0.8f;
                rbPersonagem.velocity = new Vector2(rbPersonagem.velocity.x, 0);
                PersonagemAnim.SetBool("Jump", true);
                rbPersonagem.AddForce(Vector2.up * jumpForceActual, ForceMode2D.Impulse);

                PuloDuplo = false;
            }
        }

        // Variable height - segura o botão para subir mais
        if (Input.GetButton("Jump") && rbPersonagem.velocity.y > 0)
        {
            rbPersonagem.velocity += Vector2.up * 0.08f; // pequeno impulso extra enquanto segura
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

            if (dashDirection == Vector2.zero) 
                dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

            isDashing = true;
            dashTimeLeft = DashTime;
            lastDashTime = Time.time;
            rbPersonagem.gravityScale = 0; 
            rbPersonagem.velocity = dashDirection * DashForce;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ceiling"))
        {
            if (!NoChao)
                PersonagemAnim.SetBool("Jump", false);

            if (collision.gameObject.CompareTag("Ground"))
            {
                NoChao = true;
                PuloDuplo = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moedas"))
        {
            collision.gameObject.SetActive(false);
            ControladorPersonagem.Moedas++;
            ControladorPersonagem.TextoMoeda.text = ControladorPersonagem.Moedas.ToString();
        }
    }
}
