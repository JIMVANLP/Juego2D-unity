using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el objeto colisionado NO es el jugador
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject, 0.5f);
        }
    }
}
