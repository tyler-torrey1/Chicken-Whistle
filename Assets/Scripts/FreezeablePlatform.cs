using UnityEngine;
public class FreezeablePlatform : Freezeable {

    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite _frozenSprite;

    [SerializeField]
    private Sprite _nonFrozenSprite;
    void Start() {
        _renderer = GetComponent<SpriteRenderer>();
        gameObject.layer = LayerMask.NameToLayer(layerName: "Non-Collider");
        _renderer.sprite = _nonFrozenSprite;
    }

    // Update is called once per frame
    public override void Freeze(float freezeTime) {
        // Set Collider to Colliding Layer
        base.Freeze(freezeTime);
        gameObject.layer = LayerMask.NameToLayer("Collider");
        _renderer.sprite = _frozenSprite;
    }
    public override void Unfreeze() {
        // Set Collider to Uncolliding Layer
        base.Unfreeze();
        gameObject.layer = LayerMask.NameToLayer(layerName: "Non-Collider");
        _renderer.sprite = _nonFrozenSprite;
    }
}
