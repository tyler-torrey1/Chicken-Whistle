using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        GlobalManager.NextScene();
    }
}
