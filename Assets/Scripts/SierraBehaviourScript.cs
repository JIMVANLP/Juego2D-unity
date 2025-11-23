using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierraBehaviourScript : MonoBehaviour

{
    public float speed = 3f;   // Velocidad de la sierra
    private Vector2 direction; // Dirección de movimiento

    void Start()
    {
        // La sierra comienza moviéndose hacia la derecha
        direction = Vector2.right;
    }

    void Update()
    {
        // Mueve la sierra en la dirección actual
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cambia la dirección al colisionar con cualquier objeto
        direction = -direction;
    }
}



