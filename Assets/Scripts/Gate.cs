using UnityEngine;

public class Gate : Freezeable {
    [SerializeField]
    private bool _isOpen;
    private float _closeTime = 0f;

    public void Start() {
        gameObject.layer = LayerMask.NameToLayer("Collider");
    }

    protected override void Update() {
        base.Update();

        if (_closeTime > 0 && Time.time > _closeTime && _isOpen && !IsFrozen) {
            Debug.Log("Closing");
            Close();
        }
    }
    public void Open(float openSeconds) {
        _isOpen = true;
        gameObject.layer = LayerMask.NameToLayer("Non-Collider");
        if (openSeconds < 0) {
            _closeTime = -1; // Flag for 'permanent' open. Presumably, levers will pass a -1.
        } else {
            Debug.Log("Setting CloseTime to: " + _closeTime);
            _closeTime = Time.time + openSeconds;
        }
        AudioManager.PlayGateOpen();
    }
    private void Close() {
        if (_isOpen) {
            gameObject.layer = LayerMask.NameToLayer("Collider");
            _isOpen = false;
            _closeTime = 0f; // Probably not necessary, but cleans up
        }
    }
}
