using UnityEngine;

public class Door : Freezeable
{
    [SerializeField]
    private bool _isOpen;
    private float _closeTime = 0f;

    private Collider2D _collider;
    private SpriteRenderer _renderer;
    public void Start()
    {
        this._collider = GetComponent<Collider2D>();
        this._renderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (this._closeTime > 0 && Time.time > this._closeTime && this._isOpen && !this.IsFrozen)
        {
            Debug.Log("Closing");
            this.Close();
        }
    }
    public void Open(float openSeconds)
    {
        this._renderer.color = Color.white;
        this._isOpen = true;
        this._collider.enabled = false;
        if (openSeconds < 0)
        {
            this._closeTime = -1; // Flag for 'permanent' open. Presumably, levers will pass a -1.
        } else
        {
            Debug.Log("Setting CloseTime to: " + _closeTime);
            this._closeTime = Time.time + openSeconds;
        }
    }
    private void Close()
    {
        if (this._isOpen) {
            this._collider.enabled = true;
            this._isOpen = false;
            this._closeTime = 0f; // Probably not necessary, but cleans up
        }
    }
}
