using UnityEngine;

public class Freezeable : MonoBehaviour {
    protected bool IsFrozen { get; private set; } = false;
    private float _thawTime = 0f;
    protected BoxCollider2D _collider;

    public void Awake() {
        _collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() {
        if (IsFrozen) {
            if (Time.time > _thawTime) {
                Unfreeze();
            }
        }
    }

    // Does this need to be an event, or will the freeze be able to call the Freeze method on all local objects?
    public virtual void Freeze(float freezeTime) {
        _thawTime = Time.time + freezeTime; // Current time + time frozen
        IsFrozen = true;
    }
    public virtual void Unfreeze() {
        if (IsFrozen) {
            IsFrozen = false;
            _thawTime = 0f; // Probably not necessary, but cleans up
        }
    }
}
