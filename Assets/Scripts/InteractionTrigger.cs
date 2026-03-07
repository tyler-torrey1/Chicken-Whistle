using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    PlayerInteractionHandler playerInteractionHandler;
    public void OnTriggerEnter2D(Collider2D other)
    {
        playerInteractionHandler = other.GetComponent<PlayerInteractionHandler>();
        playerInteractionHandler?.SetInteractable(GetComponentInParent<IInteractable>());
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        playerInteractionHandler = other.GetComponent<PlayerInteractionHandler>();
        playerInteractionHandler?.ClearInteractable(GetComponentInParent<IInteractable>());
    }
}
