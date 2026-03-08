using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuHandler : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadSceneAsync("Scene");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
