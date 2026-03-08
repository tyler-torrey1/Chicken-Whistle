using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour {

    PlayerInteractionHandler playerInteractionHandler;

    public void OnTriggerEnter2D(Collider2D other) {
        this.playerInteractionHandler = other.GetComponent<PlayerInteractionHandler>();
        this.playerInteractionHandler?.SetInteractable(this.GetComponentInParent<IInteractable>());
    }
    public void OnTriggerExit2D(Collider2D other) {
        this.playerInteractionHandler = other.GetComponent<PlayerInteractionHandler>();
        this.playerInteractionHandler?.ClearInteractable(this.GetComponentInParent<IInteractable>());
    }
}
