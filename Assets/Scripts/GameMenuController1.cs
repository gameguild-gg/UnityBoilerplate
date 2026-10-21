using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

   
}
