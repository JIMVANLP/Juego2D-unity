using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviourScript : MonoBehaviour

{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guarda la posición del checkpoint en PlayerPrefs
            PlayerPrefs.SetFloat("CheckpointX", transform.position.x);
            PlayerPrefs.SetFloat("CheckpointY", transform.position.y);
            PlayerPrefs.SetInt("CheckpointActive", 1);
            Debug.Log("Checkpoint activado");
        }
    }

}
