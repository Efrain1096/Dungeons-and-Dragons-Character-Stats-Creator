using UnityEngine;
using UnityEngine.SceneManagement;



public class ChangeScene : MonoBehaviour
{

    public void characterScene()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void menuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void quitGame()
    {
        //Only works when built and run as an application
        Application.Quit();
    }
}