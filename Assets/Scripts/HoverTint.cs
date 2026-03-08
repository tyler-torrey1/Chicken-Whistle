using UnityEngine;
using UnityEngine.UI;

public class HoverTint : MonoBehaviour {
    [SerializeField]
    Image image;

    void Start() {
        Image image = GetComponent<Image>();
    }
    public void OnHover() {
        image.color = new Color(0.8f, 0.8f, 0.8f, .3f);
    }

    public void OnExit() {
        image.color = new Color(0.8f, 0.8f, 0.8f, 0f);
    }
}
