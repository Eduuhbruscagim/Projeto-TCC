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
    public int Moedas;

    void Start()
    {
        PersonagemAnim = GetComponent<Animator>();
        rbPersonagem = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        MoverPersonagem();
    }

    void Update()
    {
        Jump();
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
                NoChao = false;
                PuloDuplo = false;
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            Moedas++;
        }
    }
}
