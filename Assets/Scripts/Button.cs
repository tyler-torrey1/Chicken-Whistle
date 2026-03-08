using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Gate door;

    [SerializeField]
    private float openTime = 5f; // default to 5 seconds
    

    public void Interact()
    {
        Debug.Log("Interact in Button.cs");
        this.ButtonPressed();
    }

    private void ButtonPressed()
    {
        Debug.Log("Open door for " + openTime + " seconds.");
        door.Open(openTime);
        AudioManager.PlayGateButtonPress();
    }
}
