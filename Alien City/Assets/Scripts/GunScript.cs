using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Animator anim;                               // Referência para o Animator do objeto Tiro
    public Rigidbody2D rigidBody;                       // Referência para o Rigidbody2D do objeto Tiro
    public float fire1Speed;                            // A velocidade que o projétil se move
    public float timeToDestroy = 5f;                    // O tempo que leva para o projétil se destruido, limitando seu alcance
    public bool facingRight;                            // Para qual lado o projétil será disparado



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        fire1Speed = 20f;
        timeToDestroy = 5f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Define quanto o projétil se moverá na tela

        // Move o projétil no eixo horizontal
        if (facingRight)
        {
//            rigidBody.velocity = new Vector2 (fire1Speed, rigidBody.velocity.y);
            rigidBody.AddForce(transform.right * fire1Speed);
        }
        else
        {
//            rigidBody.velocity = new Vector2(-fire1Speed, rigidBody.velocity.y);
            rigidBody.AddForce(transform.right * -fire1Speed);
        }

        // Destói o projétil quando alcançar o limite de tempo
        Destroy(this.gameObject, timeToDestroy);
    }




    // Recebe mensagem do PlayerScript indicando para qual lado o projétil está sendo disparado
    void FacingR()
    {
        facingRight = true;
    }

    void FacingL()
    {
        facingRight = false;
        Flip();
    }

    // Multiplica o componente x do localScale por -1, fazendo o sprite do projétil virar para o outro lado
    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Detecta colisão do tiro
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string otherTag = collision.gameObject.tag;

        // Compara a Tag do objeto envolvido na colisão e destrói o projétil
        if (otherTag == "Untagged")
        {
            anim.SetTrigger("Hit");           // carrega animação Hit do tiro (tiro_arma1_hit)
            rigidBody.velocity = new Vector2 (-10, 0);
            Destroy(this.gameObject, 0.06f);
        }
    }

}
