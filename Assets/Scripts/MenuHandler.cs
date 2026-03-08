using UnityEngine;

public class MenuHandler : MonoBehaviour {
    [SerializeField]
    private GameObject Title;
    [SerializeField]
    private GameObject Credits;

    [ContextMenu("Toggle active window")]
    public void toggleActiveScreen() {
        if (Title.activeSelf) {
            Title.SetActive(false);
            Credits.SetActive(true);
        } else if (Credits.activeSelf) {
            Title.SetActive(true);
            Credits.SetActive(false);
        }
    }

    public void Quit() {
        Application.Quit();
    }
}