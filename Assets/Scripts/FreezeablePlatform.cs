using UnityEngine;
public class FreezeablePlatform : Freezeable {

    private SpriteRenderer _renderer;
    private
    void Start() {
        this._renderer = this.GetComponent<SpriteRenderer>();
        this.gameObject.layer = LayerMask.NameToLayer(layerName: "Non-Collider");
    }

    // Update is called once per frame
    public override void Freeze(float freezeTime) {
        // Set Collider to Colliding Layer
        this.gameObject.layer = LayerMask.NameToLayer("Collider");
        base.Freeze(freezeTime);
    }
    public override void Unfreeze() {
        // Set Collider to Uncolliding Layer
        this.gameObject.layer = LayerMask.NameToLayer(layerName: "Non-Collider");
        base.Unfreeze();
    }
}
