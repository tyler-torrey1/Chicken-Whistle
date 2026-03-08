using UnityEngine;

public class Gate : Freezeable {
    private bool _isOpen;
    private float _closeTime = 0f;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite _openUnfrozen;

    [SerializeField]
    private Sprite _openFrozen;

    [SerializeField]
    private Sprite _closed;
    public void Start() {
        gameObject.layer = LayerMask.NameToLayer("Collider");
        _renderer = gameObject.GetComponent<SpriteRenderer>();
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
            _renderer.sprite = _openUnfrozen;
            _closeTime = Time.time + openSeconds;
        }
        AudioManager.PlayGateOpen();
    }
    private void Close() {
        if (_isOpen) {
            gameObject.layer = LayerMask.NameToLayer("Collider");
            _isOpen = false;
            _renderer.sprite = _closed;
            _closeTime = 0f; // Probably not necessary, but cleans up
        }
    }
    public override void Freeze(float freezeTime) {
        if (!_isOpen) {
            return; // Only open doors can be frozen
        }
        base.Freeze(freezeTime);
        _renderer.sprite = _openFrozen;
    }
    public override void Unfreeze() {
        base.Unfreeze();
        _renderer.sprite = _openUnfrozen;
    }
}
