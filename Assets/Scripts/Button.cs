using UnityEngine;

public class Button : MonoBehaviour, IInteractable {
    [SerializeField]
    private Gate door;

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
<<<<<<< HEAD
        _renderer.sprite = sprite[1];
        _stopWatch.Start();
=======
        AudioManager.PlayGateButtonPress();
>>>>>>> 9e84c7a1284eaf9dc36cc1d3bca1b10a1b4f1e05
    }
}
