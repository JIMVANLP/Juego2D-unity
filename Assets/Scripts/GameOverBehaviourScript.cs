using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverBehaviourScript : MonoBehaviour
{
    int Vidas;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("vidas", 5);
        Vidas = PlayerPrefs.GetInt("vidas");      
       
        // Elimina el checkpoint si pierde todas las vidas
        PlayerPrefs.SetInt("CheckpointActive", 0);     
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueGameOver()
    {
        SceneManager.LoadScene(1);
        
    }
}
