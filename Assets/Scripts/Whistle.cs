using UnityEngine;
using UnityEngine.InputSystem;
public class Whistle : MonoBehaviour {
    [SerializeField]
    private float _whistleRadius = 2.5f;

    [SerializeField]
    private float _freezeTime = 5f;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private float _whistleCooldown = 5.5f;
    private float _nextWhistleTime = 0f;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void BlowWhistle(InputAction.CallbackContext context) {
        if (!context.performed || _nextWhistleTime > Time.time) {
            return;
        }

        Debug.Log("Blowing the whistle!");
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(_rb.worldCenterOfMass, _whistleRadius);
        foreach (Collider2D collider in nearbyColliders) {
            Freezeable freezeable = collider.GetComponent<Freezeable>();
            freezeable?.Freeze(_freezeTime);
        }
        _nextWhistleTime = Time.time + _whistleCooldown;
    }
    public void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_rb.worldCenterOfMass, _whistleRadius);
    }
}
