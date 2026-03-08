using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WinCollider : MonoBehaviour
{

    Collider2D _collider;

    PlayerController _player;
    private Animator animator;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator WinProcess()
    {
        _player.LockMovement();
        animator.SetTrigger("jump");

        yield return null;

        // Play 'remove bandage' animation
        // particles?

        _player.SetMoveInput(Vector2.left);
        _player.StartFlightCoroutine();


        yield return new WaitForSeconds(5f);

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
            animator = _player.GetComponent<Animator>();  
            _spriteRenderer.enabled = false; 
            StartCoroutine(WinProcess());
        }
    }
}
