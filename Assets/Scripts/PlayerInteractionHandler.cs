using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionHandler : MonoBehaviour {
    private IInteractable _currentInteractable;

    public void SetInteractable(IInteractable newInteractable) {
        this._currentInteractable = newInteractable;
    }

    // The thought here is that the interactable will call this when you leave its trigger radius, and if it is the current interactable
    // it will clear it, otherwise it will be ignored. I think this will be barebones handling of overlaps, though I think we should 
    // probably not need it.
    public void ClearInteractable(IInteractable requestingInteractable) {
        Debug.Log("ClearInteractable called by: " + requestingInteractable);
        if (this._currentInteractable == requestingInteractable) {
            this._currentInteractable = null;
            Debug.Log("Interactable removed: " + requestingInteractable);
        }
    }

    public void Interact(InputAction.CallbackContext context) {
        if (!context.performed) {
            return;
        }
        Debug.Log("Interacting with: " + this._currentInteractable);
        this._currentInteractable?.Interact();
    }
}
