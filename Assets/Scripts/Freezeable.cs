using UnityEngine;

public class Freezeable : MonoBehaviour {
    protected bool IsFrozen { get; private set; } = false;
    private float _thawTime = 0f;

    protected virtual void Update() {
        if (this.IsFrozen) {
            if (Time.time > this._thawTime) {
                this.Unfreeze();
            }
        }
    }

    // Does this need to be an event, or will the freeze be able to call the Freeze method on all local objects?
    public virtual void Freeze(float freezeTime) {
        this._thawTime = Time.time + freezeTime; // Current time + time frozen
        this.IsFrozen = true;
    }
    public virtual void Unfreeze() {
        if (this.IsFrozen) {
            this.IsFrozen = false;
            this._thawTime = 0f; // Probably not necessary, but cleans up
        }
    }
}
