using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WinCollider : MonoBehaviour
{

    Collider2D _collider;

    PlayerController _player;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private IEnumerator WinProcess()
    {
        _player.LockMovement();

        yield return null;

        // Play 'remove bandage' animation
        // particles?

        _player.SetMoveInput(Vector2.right);
        _player.StartFlightCoroutine();


        yield return new WaitForSeconds(1f);

        // fade in text

        _player.PrepareDeletion();
        GlobalManager.PrepareDeletion();

        GlobalManager.DoBlackScreenFade(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Main Menu");
        GlobalManager.DoBlackScreenFade(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            _player = PlayerController.instance;
            StartCoroutine(WinProcess());
        }
    }
}
