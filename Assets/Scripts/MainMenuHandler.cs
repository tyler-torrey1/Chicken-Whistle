using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuHandler : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
