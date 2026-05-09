using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoBehaviourScript : MonoBehaviour
{

    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public GameObject rino;
    float time_mov;
    bool movingRight = false;  // Variable para controlar la dirección


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        time_mov = Time.time;  // Inicia el tiempo de movimiento
    }

    // Update is called once per frame
    void Update()
    {
        // Cambiar dirección cada 2 segundos
        if (Time.time - time_mov > 2)
        {
            // Cambiar la dirección del movimiento
            movingRight = !movingRight;

            // Invertir el sprite solo cuando cambie de dirección
            spriteRenderer.flipX = movingRight;

            time_mov = Time.time;  // Reinicia el temporizador
        }

        // Movimiento en la dirección actual
        if (movingRight)
        {
            rigidbody2D.linearVelocity = new Vector2(2, 0);  // Mover hacia la derecha
        }
        else
        {
            rigidbody2D.linearVelocity = new Vector2(-2, 0);  // Mover hacia la izquierda
        }
    }



    public void RinoDestroy() 
    {
      Destroy(gameObject);
    }
}
