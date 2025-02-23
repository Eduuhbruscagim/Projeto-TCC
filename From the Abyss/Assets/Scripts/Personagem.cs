using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    private Rigidbody2D rbPersonagem;
    public float Speed = 5f;
    private SpriteRenderer spriteRenderer;
    public float JumpForce;
    public bool NoChao = true;

    void Start()
    {
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
            spriteRenderer.flipX = false;
        }
        else if (MovimentoHorizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Jump()
    {

        if (Input.GetButtonDown("Jump") && NoChao)
        {
            rbPersonagem.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            NoChao = false;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            NoChao = true;
        }
    }

}
