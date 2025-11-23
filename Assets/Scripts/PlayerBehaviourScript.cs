using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerBehaviourScript : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    private int jumpCount = 0;
    Animator animator;
    public GameObject rino;
    public GameObject bat;
    public GameObject block;
    public static int numeroMaznanas;
    int Vidas , hits;

    //corazones
    public GameObject corazon1, corazon2, corazon3;
   
    // Audio
    
    public AudioSource musicAudioSource;  // AudioSource para la música
    public AudioSource sfxAudioSource;    // AudioSource para efectos de sonido
    public AudioClip jumpSound;
    public AudioClip doubleJumpSound;
    public AudioClip hitByFireSound;
    public AudioClip enemyKillSound;
    public AudioClip deathSound;
    public AudioClip apearing;
    public AudioClip FinNivel;    
    public AudioClip CorazonSound;
    public AudioClip RocaSound;
    private bool hasPlayedRockSound = false;
    public AudioClip vientoSound;
    private bool hasPlayedvientoSound;
    public AudioClip CheckSound;
    private bool hasPlayedCheckPointSound;
    public AudioClip nadarSound;
    public AudioClip TrampolinSound;
    public AudioClip Plataforma;
    private bool hasPlayedPlataformaSound;

    //timmer
    public TextMeshProUGUI textTimmer, textVidas;
    float time;
    int levelTime;

    public LayerMask layerMask;
    public LayerMask layerMaskEnemy;
    private float normalSpeed = 4.5f;  // Velocidad normal de caminar
    private float runSpeed = 8f;   // Velocidad al correr

    bool controller, hit, inWater;
    bool wasGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        musicAudioSource = audioSources[0];  // Asumimos que el primer AudioSource es para la música
        sfxAudioSource = audioSources[1];    // El segundo AudioSource es para efectos de sonido
        //controller = true;        
        hits = 3;
        inWater = false;
        numeroMaznanas = 0;
        Vidas = PlayerPrefs.GetInt("vidas");
        textVidas.SetText(""+ Vidas);
        if (Vidas <= 0) 
        {
            SceneManager.LoadScene(6);
        }
        //Timer
        time = Time.time;
        levelTime = 250;
        textTimmer.SetText("" + levelTime);
        isAppear();


        if (PlayerPrefs.GetInt("CheckpointActive") == 1)
        {
            // Reaparece en el checkpoint
            float checkpointX = PlayerPrefs.GetFloat("CheckpointX");
            float checkpointY = PlayerPrefs.GetFloat("CheckpointY");
            transform.position = new Vector2(checkpointX, checkpointY);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > time)
        {
            levelTime--;
            textTimmer.SetText("" + levelTime);
            time++;

            if (levelTime == 0) 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // Detecta si se presiona la tecla Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Carga la escena 0 (menú principal)
            SceneManager.LoadScene(0);
        }

        // CONTROLESSSS

        if (controller) 
        {
            float currentSpeed = normalSpeed;  // Inicia con velocidad normal

            bool grounded = isGrounded();
            

            if (grounded != wasGrounded)  // Solo imprime cuando cambie el estado
            {
                Debug.Log("¿En el suelo? " + grounded);
                
                wasGrounded = grounded;  // Actualiza el estado previo
            }

            // Detecta si está corriendo (tecla D)
            if (Input.GetKey(KeyCode.D))
            {
                currentSpeed = runSpeed;  // Aumenta la velocidad si se presiona D
            }

            // Movimiento a la derecha
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rigidbody2D.velocity = new Vector2(currentSpeed, rigidbody2D.velocity.y);
                spriteRenderer.flipX = false;
            }

            // Movimiento a la izquierda
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rigidbody2D.velocity = new Vector2(-currentSpeed, rigidbody2D.velocity.y);
                spriteRenderer.flipX = true;
            }


            // SALTO

            if(Input.GetKeyDown(KeyCode.S) && inWater)
            {
                sfxAudioSource.PlayOneShot(nadarSound);
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 4);
                animator.SetInteger("estado", 3);                
            }

            if (Input.GetKeyDown(KeyCode.S) && (jumpCount < 1) && !inWater)
            {              
                
                    // Primer salto: solo si está en el suelo
                if (grounded)
                {
                    PlayJumpSound();
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 10);  // Salto normal
                    jumpCount = 1;  // Primer salto realizado

                    Debug.Log("Primer salto. Contador de saltos: " + jumpCount);
                }
                // Doble salto: solo si ya ha hecho un salto y no está en el suelo
                else
                {
                    PlayDoubleJumpSound();
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 12);  // Segundo salto
                    animator.SetInteger("estado", 5);  // Activa la animación del doble salto
                    jumpCount++;  // Segundo salto realizado
                    Debug.Log("Doble salto. Contador de saltos: " + jumpCount);
                    
                }              
                
            }
        }

        // Restablecer el contador de saltos cuando toca el suelo
        if (isGrounded())
        {
            //controller = true;
            jumpCount = 0;  // Restablece el contador de saltos cuando toca el suelo
        }

        // Control de animaciones
        if (isGrounded() && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("estado", 0);  // Quieto
        }
        else if (isGrounded() && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            animator.SetInteger("estado", 1);  // Caminando

            if (Input.GetKey(KeyCode.D))
            {
                animator.SetInteger("estado", 2);  // Corriendo
            }
        }
        else if (rigidbody2D.velocity.y > 0 && jumpCount == 0)
        {
            animator.SetInteger("estado", 3);
        }
        else if (rigidbody2D.velocity.y < 0)
        {
            animator.SetInteger("estado", 4);  // Cayendo
        }

    }

    bool isGrounded()
    {
        
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.2f, layerMask);
        
    }

    bool isEnemy()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.2f, layerMaskEnemy);
    }

    void isDestroy() 
    {
        Vidas = PlayerPrefs.GetInt("vidas");
        Vidas--;
        PlayerPrefs.SetInt("vidas", Vidas);    

    }

    void isAppear()
    {
        PlayApearing();
        animator.SetInteger("estado",0);
        
    }

    void controlOn()
    {
        controller = true;
        hit = false;
    }


    // Colisiones
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "picos" || collision.gameObject.tag == "block" || collision.gameObject.tag == "muerte" || collision.gameObject.tag == "tortuga")
        {
            StartCoroutine(DeathSequence());
        }



        if (collision.gameObject.tag == "rino") 
        {
            if (!isGrounded())
            {
                PlayEnemyKillSound();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 15);
                collision.gameObject.GetComponent<Animator>().SetTrigger("Destroy");
                //collision.gameObject.GetComponent<RinoBehaviourScript>().RinoDestroy();
            }
            else 
            {
                StartCoroutine(DeathSequence());
            }                
              
        }

        if (collision.gameObject.tag == "Bat")
        {
            // Verifica si el jugador está cayendo y su posición en Y es mayor que la del murciélago
            if (rigidbody2D.velocity.y < 0 && transform.position.y > collision.gameObject.transform.position.y)
            {
                PlayEnemyKillSound();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 15); // Rebota tras pisar al murciélago
                collision.gameObject.GetComponent<Animator>().SetTrigger("Destroybat");  // Ejecuta la animación de destrucción
            }
            else
            {
                StartCoroutine(DeathSequence());
            }
        }

        // Objetos que quitan corazones///////////


        if (collision.gameObject.tag == "fire" && hit == false || collision.gameObject.tag == "sierra" && hit == false)
        {
            PlayHitByFireSound();
            // Inicia la corutina de invulnerabilidad
            StartCoroutine(InvulnerabilityCoroutine());
            controller = false;
            hit = true;
            hits--;

            if (inWater)
            {
                if (spriteRenderer.flipX)
                {
                    rigidbody2D.velocity = new Vector2(5, 2);
                }
                else
                {
                    rigidbody2D.velocity = new Vector2(-5, 2);
                }
            }
            else 
            {
                if (spriteRenderer.flipX)
                {
                    rigidbody2D.velocity = new Vector2(10, 5);
                }
                else
                {
                    rigidbody2D.velocity = new Vector2(-10, 5);
                }
            }

            if (hits == 2)
            {
                corazon1.SetActive(false);
                animator.SetTrigger("golpe");

            }
            if (hits == 1)
            {
                corazon2.SetActive(false);
                animator.SetTrigger("golpe");

            }
            if (hits <= 0)
            {
                corazon3.SetActive(false);
                StartCoroutine(DeathSequence());
            }




        }
        // Corutina para manejar el tiempo de invulnerabilidad
        IEnumerator InvulnerabilityCoroutine()
        {
            yield return new WaitForSeconds(2f);  // Espera 1 segundo
            hit = false;  // Permite al jugador recibir daño nuevamente
            controller = true;  // Restablece el control del jugador
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "fall")
        {
            // Solo afecta al objeto que está colisionando con el jugador no por tag
            Rigidbody2D fallRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (!hasPlayedPlataformaSound)
            {
                sfxAudioSource.PlayOneShot(Plataforma);
                hasPlayedPlataformaSound = true;  // Marca el sonido como reproducido
            }            
            fallRigidbody.bodyType = RigidbodyType2D.Dynamic;
            fallRigidbody.freezeRotation = true;
            fallRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            fallRigidbody.gravityScale = 3;
        }


        if (collision.gameObject.tag == "elevadorLat") 
        {
            transform.SetParent(collision.gameObject.transform);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "elevadorLat")
        {
            transform.SetParent(null);
        }

        if (collision.gameObject.tag == "fall") 
        {
            hasPlayedPlataformaSound = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Check")
        {
            if (!hasPlayedCheckPointSound)
            {
                sfxAudioSource.PlayOneShot(CheckSound);
                hasPlayedCheckPointSound = true;  // Marca el sonido como reproducido
            }
        }

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fan") 
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20);

            if (!hasPlayedvientoSound)
            {
                sfxAudioSource.PlayOneShot(vientoSound);
                hasPlayedvientoSound = true;  // Marca el sonido como reproducido
            }
        }

        if (collision.gameObject.tag == "fanInv")
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,-10);

            if (!hasPlayedvientoSound)
            {
                sfxAudioSource.PlayOneShot(vientoSound);
                hasPlayedvientoSound = true;  // Marca el sonido como reproducido
            }
        }

        if (collision.gameObject.tag == "endLevel")
        {
            controller = false;
            PlayFinSound();
            collision.gameObject.GetComponent<Animator>().SetTrigger("endlevel");
            
        }

        if (collision.gameObject.tag == "block")
        {
            if (inWater)
            {
                Rigidbody2D blockRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                // Aplica las modificaciones solo al bloque que ha colisionado
                blockRigidbody.bodyType = RigidbodyType2D.Dynamic;
                blockRigidbody.freezeRotation = true;
                blockRigidbody.gravityScale = 0.5f;
            }
            else
            {
                // Obtén el Rigidbody2D del bloque que colisiona, no de uno global
                Rigidbody2D blockRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                // Aplica las modificaciones solo al bloque que ha colisionado
                blockRigidbody.bodyType = RigidbodyType2D.Dynamic;
                blockRigidbody.freezeRotation = true;
                blockRigidbody.gravityScale = 2;
            }

            if (!hasPlayedRockSound) 
            {
                sfxAudioSource.PlayOneShot(RocaSound);
                hasPlayedRockSound = true;  // Marca el sonido como reproducido
            }
        }


        if (collision.gameObject.tag == "Tramp")
        {
            sfxAudioSource.PlayOneShot(TrampolinSound);
            GameObject.FindGameObjectWithTag("Tramp").GetComponent<Animator>().SetBool("EstadoTramp", true);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20);
        }

        if (collision.gameObject.tag == "agua")
        {
            rigidbody2D.gravityScale = 0.5f;
            inWater = true;
        }

        if (collision.gameObject.tag == "zanahoria")
        {
            // Si el jugador no tiene el máximo de corazones
            sfxAudioSource.PlayOneShot(CorazonSound);
            if (hits < 3)
            {
                hits++;  // Incrementa la cantidad de corazones
                

                // Activa los corazones según la cantidad de hits
                if (hits == 2)
                {
                    corazon2.SetActive(true);
                }
                else if (hits == 3)
                {
                    corazon1.SetActive(true);
                }
            }

            // Destruye la zanahoria después de recolectarla
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tramp")
        {
            GameObject.FindGameObjectWithTag("Tramp").GetComponent<Animator>().SetBool("EstadoTramp", false);
        }

        if (collision.gameObject.tag == "agua")
        {
            rigidbody2D.gravityScale = 3f;
            inWater = false;
        }

        if (collision.gameObject.tag == "block")
        {
            hasPlayedRockSound = false;  // Permite que el sonido se reproduzca de nuevo la próxima vez que colisione
        }

        if (collision.gameObject.tag == "Fan")
        {
            hasPlayedvientoSound = false;  // Permite que el sonido se reproduzca de nuevo la próxima vez que colisione
        }

        if (collision.gameObject.tag == "fanInv")
        {
            hasPlayedvientoSound = false;  // Permite que el sonido se reproduzca de nuevo la próxima vez que colisione
        }
    }

    private void PerderVida() 
    {
        Vidas = PlayerPrefs.GetInt("vidas");
        Vidas--;
        PlayerPrefs.SetInt("vidas", Vidas);
   
    }


    void PlayJumpSound()
    {
        if (jumpSound != null)
            sfxAudioSource.PlayOneShot(jumpSound);
    }

    void PlayDoubleJumpSound()
    {
        if (doubleJumpSound != null)
            sfxAudioSource.PlayOneShot(doubleJumpSound);
    }

    void PlayHitByFireSound()
    {
        if (hitByFireSound != null)
            sfxAudioSource.PlayOneShot(hitByFireSound);
    }

    void PlayEnemyKillSound()
    {
        if (enemyKillSound != null)
            sfxAudioSource.PlayOneShot(enemyKillSound);
    }

    IEnumerator DeathSequence()
    {
        // Reproduce el sonido de muerte
        PlayDeathSound();
        rigidbody2D.velocity = Vector2.zero;

        // Desactiva el control del jugador
        controller = false;

        // Desactiva el Rigidbody2D para evitar más colisiones y movimiento
        rigidbody2D.isKinematic = true;


        // Desactiva el Collider2D para evitar más colisiones
        boxCollider.enabled = false;

        // Activa la animación de destrucción
        animator.SetTrigger("destroy");


        // Espera a que termine la animación (1 segundo, o ajusta según la duración de tu animación)
        yield return new WaitForSeconds(1f);

         //Reinicia la escena actual después de la animación
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

   
 
    }


    void PlayDeathSound()
    {
        if (deathSound != null)
            sfxAudioSource.PlayOneShot(deathSound);
    }

    void PlayFinSound() 
    {
        if (FinNivel !=null)
            sfxAudioSource.PlayOneShot(FinNivel);
    }

    void PlayApearing() 
    {
        if(apearing != null) 
        {
            sfxAudioSource.PlayOneShot(apearing);
        }
    }

}
