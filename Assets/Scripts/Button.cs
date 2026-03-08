using UnityEngine;

public class Button : MonoBehaviour, IInteractable {
    [SerializeField]
    private Door door;

    [SerializeField]
    private float openTime = 5f; // default to 5 seconds

    [SerializeField]
    private Sprite[] sprite;
    private SpriteRenderer _renderer;
    private StopWatch _stopWatch;

    [SerializeField]
    private float depressTime = .5f;
    void Start() {
        _renderer = GetComponent<SpriteRenderer>();
        _stopWatch = new();
    }

    void Update() {
        if (_renderer != null) {
            if (_stopWatch > depressTime) {
                _renderer.sprite = sprite[0];
            }
        }
    }
    public void Interact() {
        Debug.Log("Interact in Button.cs");
        ButtonPressed();
    }

    private void ButtonPressed() {
        Debug.Log("Open door for " + openTime + " seconds.");
        door.Open(openTime);
        _renderer.sprite = sprite[1];
        _stopWatch.Start();
    }
}
