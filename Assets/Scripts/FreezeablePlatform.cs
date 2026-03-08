using UnityEngine;
public class FreezeablePlatform : Freezeable {

    private SpriteRenderer _renderer;
    private
    void Start() {
        this._renderer = this.GetComponent<SpriteRenderer>();
        this.gameObject.layer = LayerMask.NameToLayer(layerName: "Non-Collider");
    }
    [SerializeField]
    private float tempFreezeTime = 3f; // 3 seconds for default freeze for temp testing

    [SerializeField]
    private bool freeze;

    protected override void Update() {
        base.Update();
        if (this.freeze) {
            this.Freeze(this.tempFreezeTime);
            this.freeze = false;
        }
    }

    // Update is called once per frame
    public override void Freeze(float freezeTime) {
        // Set Collider to Colliding Layer
        this.gameObject.layer = LayerMask.NameToLayer("Collider");
        base.Freeze(this.tempFreezeTime);
    }
    public override void Unfreeze() {
        // Set Collider to Uncolliding Layer
        this.gameObject.layer = LayerMask.NameToLayer(layerName: "Non-Collider");
        base.Unfreeze();
    }
}
