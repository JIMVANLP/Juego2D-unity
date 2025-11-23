using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppleBehaviourScript : MonoBehaviour
{
    public static int numeroManzanas, vidas;
    public TextMeshProUGUI text, textVidas;
    public AudioClip VidaSound;
    public AudioClip itemsound;

    private AudioSource playerAudioSource;  // Variable para el AudioSource del jugador

    void Start()
    {
        vidas = PlayerPrefs.GetInt("vidas");
        text.SetText("" + numeroManzanas);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si la colisión es con el jugador
        if (collision.CompareTag("Player"))
        {
            // Obtén el AudioSource del jugador
            playerAudioSource = collision.GetComponent<AudioSource>();
                    
           
            // Destruye la manzana y actualiza el contador de manzanas
            Destroy(gameObject);
            numeroManzanas++;
            text.SetText("" + numeroManzanas);
            
            // Si el jugador recolectó 5 manzanas, incrementa las vidas
            if (numeroManzanas == 5)
            {
                numeroManzanas = 0;
                text.SetText("" + numeroManzanas);
                vidas++;
                if (playerAudioSource != null)
                {
                    playerAudioSource.PlayOneShot(VidaSound);  // Reproduce el sonido de vida
                }
                PlayerPrefs.SetInt("vidas", vidas);
                textVidas.SetText("" + vidas);
            }
            else 
            {
                if (playerAudioSource != null)
                {
                    playerAudioSource.PlayOneShot(itemsound);
                }

            }


        }
    }
}

