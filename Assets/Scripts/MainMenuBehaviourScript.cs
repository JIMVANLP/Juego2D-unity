using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Método para iniciar el juego
    public void StartGame()
    {
        // Aquí carga la escena del primer nivel (ajusta "GameScene" al nombre de tu escena de juego)
        PlayerPrefs.SetInt("Progress", 0);
        PlayerPrefs.SetInt("vidas", 5);
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(1);
    }



    // Método para salir del juego
    public void QuitGame()
    {
        // Cierra la aplicación en el build final
        Application.Quit();
        // Mensaje de debug para pruebas en el editor
        Debug.Log("El juego se ha cerrado");
    }


}
