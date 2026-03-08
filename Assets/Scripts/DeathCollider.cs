using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathCollider : MonoBehaviour
{

    Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GlobalManager.ReloadScene();
        }
    }
}
