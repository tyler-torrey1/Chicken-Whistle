using UnityEngine;
using UnityEngine.InputSystem;
public class Whistle : MonoBehaviour {
    [SerializeField]
    private float whistleRadius = 2.5f;

    [SerializeField]
    private float freezeTime = 5f;

    [SerializeField]
    private Rigidbody2D _rb;
    void Awake() {
        this._rb = this.GetComponent<Rigidbody2D>();
    }

    public void BlowWhistle(InputAction.CallbackContext context) {
        if (!context.performed) {
            return;
        }

        Debug.Log("Blowing the whistle!");
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(this._rb.worldCenterOfMass, this.whistleRadius);
        foreach (Collider2D collider in nearbyColliders) {
            Freezeable freezeable = collider.GetComponent<Freezeable>();
            freezeable?.Freeze(this.freezeTime);
        }
    }
    public void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this._rb.worldCenterOfMass, this.whistleRadius);
    }
}
