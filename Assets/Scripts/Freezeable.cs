using UnityEngine;

public class Freezeable : MonoBehaviour {
    private bool _isFrozen = false;
    private float _thawTime = 0f;
    public virtual void Update() {
        if (this._isFrozen) {
            if (Time.time > this._thawTime) {
                this.Unfreeze();
            }
        }
    }

    // Does this need to be an event, or will the freeze be able to call the Freeze method on all local objects?
    public virtual void Freeze(float freezeTime) {
        this._thawTime = Time.time + freezeTime; // Current time + time frozen
        this._isFrozen = true;
    }
    public virtual void Unfreeze() {
        if (this._isFrozen) {
            this._isFrozen = false;
            this._thawTime = 0f; // Probably not necessary, but cleans up
        }
    }
}
