using UnityEngine;
public class FreezeablePlatform : Freezeable {
    private BoxCollider2D _collider;
    private Sprite _sprite;
    private SpriteRenderer _renderer;
    private
    void Start() {
        this._sprite = this.GetComponent<Sprite>();
        this._collider = this.GetComponent<BoxCollider2D>();
        this._renderer = this.GetComponent<SpriteRenderer>();
    }
    [SerializeField]
    private float tempFreezeTime = 3f; // 3 seconds for default freeze for temp testing

    [SerializeField]
    private bool freeze;

    void Update() {
        if (this.freeze) {
            this.Freeze(this.tempFreezeTime);
            this.freeze = false;
        }
        base.Update();
    }

    // Update is called once per frame
    public override void Freeze(float freezeTime) {
        this._renderer.color = Color.blue;
        this._collider.enabled = true;
        base.Freeze(this.tempFreezeTime);
    }
    public override void Unfreeze() {
        this._renderer.color = Color.white;
        this._collider.enabled = false;
        base.Unfreeze();
    }
}
