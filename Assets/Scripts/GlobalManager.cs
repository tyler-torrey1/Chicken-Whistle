using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Scene and game state control via singleton.
 * We proceed linearly through stages only.
 */

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance = null;

    [SerializeField] private Image blackscreen;
    [SerializeField, Min(0)] private float fadeTime;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning(name + ": Singleton betrayal!");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;
    }

    public void PlayGame()
    {
        StartCoroutine(DelayedSceneChange(1));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public static void NextScene() => instance.NextSceneInstance();
    public static void ReloadScene() => instance.ReloadSceneInstance();


    [ContextMenu("Next Scene")]
    private void NextSceneInstance()
    {
        Scene current = SceneManager.GetActiveScene();

        if (current.buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            // Finished game
        }
        else
        {
            StartCoroutine(DelayedSceneChange(current.buildIndex + 1));
        }
    }

    [ContextMenu("Reload Scene")]
    private void ReloadSceneInstance()
    {
        Scene current = SceneManager.GetActiveScene();
        StartCoroutine(DelayedSceneChange(current.buildIndex));
    }

    private IEnumerator DelayedSceneChange(int targetSceneIndex)
    {
        yield return BlackScreenFade(true);
        SceneManager.LoadScene(targetSceneIndex);
        yield return BlackScreenFade(false);
    }

    private IEnumerator BlackScreenFade(bool toBlack)
    {
        float targetLerp = toBlack ? 1f : 0f;
        float startLerp = 1f - targetLerp;

        StopWatch watch = new StopWatch();
        Color color = Color.black;
        do
        {
            float resolvedAlpha = Mathf.Lerp(startLerp, targetLerp, watch / fadeTime);
            color.a = resolvedAlpha;
            blackscreen.color = color;

            yield return null;
        } while (watch < fadeTime);

        color.a = targetLerp;
        blackscreen.color = color;
    }
}
