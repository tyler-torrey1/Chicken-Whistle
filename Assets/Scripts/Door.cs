using UnityEngine;

public class Door : Freezeable {
    [SerializeField]
    private bool _isOpen;
    private float _closeTime = 0f;

    private SpriteRenderer _renderer;
    public void Start() {
        this._renderer = this.GetComponent<SpriteRenderer>();
        this.gameObject.layer = LayerMask.NameToLayer("Collider");
    }

    protected override void Update() {
        base.Update();

        if (this._closeTime > 0 && Time.time > this._closeTime && this._isOpen && !this.IsFrozen) {
            Debug.Log("Closing");
            this.Close();
        }
    }
    public void Open(float openSeconds) {
        this._isOpen = true;
        this.gameObject.layer = LayerMask.NameToLayer("Non-Collider");
        if (openSeconds < 0) {
            this._closeTime = -1; // Flag for 'permanent' open. Presumably, levers will pass a -1.
        } else {
            Debug.Log("Setting CloseTime to: " + this._closeTime);
            this._closeTime = Time.time + openSeconds;
        }
        AudioManager.PlayGateOpen();
    }
    private void Close() {
        if (this._isOpen) {
            this.gameObject.layer = LayerMask.NameToLayer("Collider");
            this._isOpen = false;
            this._closeTime = 0f; // Probably not necessary, but cleans up
        }
    }
}
