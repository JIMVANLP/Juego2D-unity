using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapBehaviourScript : MonoBehaviour
{
    public GameObject flag1, flag2, flag3, flag4;
    public TextMeshProUGUI text;
    public AudioClip moveSound, selectSound;
    public Animator animator;
    private AudioSource audioSource;
    int location, user_progress;

    void Start()
    {
        user_progress = PlayerPrefs.GetInt("Progress");
        audioSource = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("CheckpointActive", 0);

        if (animator == null)
            Debug.LogError("Animator no asignado.");

        if (audioSource == null)
            Debug.LogError("AudioSource no encontrado.");

        if (selectSound == null)
            Debug.LogError("selectSound no asignado.");

        // Configuración inicial basada en el progreso del jugador
        switch (user_progress)
        {
            case 0:
                flag1.SetActive(false);
                flag2.SetActive(false);
                flag3.SetActive(false);
                flag4.SetActive(false);
                location = 1;
                transform.position = new Vector2(-4.5f, -0.4f);
                text.SetText("Level 1");
                break;
            case 1:
                flag1.SetActive(true);
                flag2.SetActive(false);
                flag3.SetActive(false);
                flag4.SetActive(false);
                location = 2;
                transform.position = new Vector2(-1.5f, -0.4f);
                text.SetText("Level 2");
                break;
            case 2:
                flag1.SetActive(true);
                flag2.SetActive(true);
                flag3.SetActive(false);
                flag4.SetActive(false);
                location = 3;
                transform.position = new Vector2(1.5f, -0.4f);
                text.SetText("Level 3");
                break;
            case 3:
                flag1.SetActive(true);
                flag2.SetActive(true);
                flag3.SetActive(true);
                flag4.SetActive(false);
                location = 4;
                transform.position = new Vector2(4.5f, -0.4f);
                text.SetText("Level 4");
                break;
            case 4:
                flag1.SetActive(true);
                flag2.SetActive(true);
                flag3.SetActive(true);
                flag4.SetActive(true);
                location = 4;
                transform.position = new Vector2(4.5f, -0.4f);
                text.SetText("Finished!!");
                break;
        }
    }

    void Update()
    {

        // Detecta si se presiona la tecla Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Carga la escena 0 (menú principal)
            SceneManager.LoadScene(0);
        }

        if (location == 1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && user_progress > 0)
            {
                PlayMoveSound();
                transform.position = new Vector2(-1.5f, -0.4f);
                text.SetText("Level 2");
                location = 2;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadSceneWithDelay(2));
            }
        }
        else if (location == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && user_progress >= 1)
            {
                PlayMoveSound();
                transform.position = new Vector2(-4.5f, -0.4f);
                text.SetText("Level 1");
                location = 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && user_progress > 1)
            {
                PlayMoveSound();
                transform.position = new Vector2(1.5f, -0.4f);
                text.SetText("Level 3");
                location = 3;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadSceneWithDelay(3));
            }
        }
        else if (location == 3)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && user_progress >= 2)
            {
                PlayMoveSound();
                transform.position = new Vector2(-1.5f, -0.4f);
                text.SetText("Level 2");
                location = 2;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && user_progress > 2)
            {
                PlayMoveSound();
                transform.position = new Vector2(4.5f, -0.4f);
                text.SetText("Level 4");
                location = 4;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadSceneWithDelay(4));
            }
        }
        else if (location == 4)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && user_progress >= 3)
            {
                PlayMoveSound();
                transform.position = new Vector2(1.5f, -0.4f);
                text.SetText("Level 3");
                location = 3;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(LoadSceneWithDelay(5));
            }
        }
    }

    void PlayMoveSound()
    {
        audioSource.PlayOneShot(moveSound);
    }

    void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }

    IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        PlaySelectSound();
        animator.SetTrigger("destroy");
        yield return new WaitForSeconds(1f); // Espera 1 segundo para la animación
        SceneManager.LoadScene(sceneIndex);
    }
}
