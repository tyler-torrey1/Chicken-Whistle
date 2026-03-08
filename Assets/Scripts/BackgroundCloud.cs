using System.Collections;
using UnityEngine;

public class BackgroundCloud : MonoBehaviour
{
    [SerializeField, Min(0)] private float _minScale;
    [SerializeField, Min(0)] private float _maxScale;

    [SerializeField, Min(0)] private float _minLifetime; // seconds
    [SerializeField, Min(0)] private float _maxLifetime; // seconds
    [SerializeField, Range(0f, 1f)] private float _lifetimeRandomPercent;

    float _lifetimeTotal; // seconds
    float _depthLerp;
    float _heightLerp;
    float _lifetimeLerp = 0f;

    public void Spawn(float depthLerp, float completionLerp = 0f)
    {
        _depthLerp = Mathf.Clamp01(depthLerp);
        _lifetimeLerp = completionLerp;

        transform.localScale = Vector3.one * Mathf.Lerp(_maxScale, _minScale, _depthLerp); // the further, the smaller
        _lifetimeTotal = Random.Range(1f - _lifetimeRandomPercent, 1f + _lifetimeRandomPercent) * Mathf.Lerp(_minLifetime, _maxLifetime, Mathf.Clamp01(_depthLerp));
        _heightLerp = Random.Range(0f, 1f);

        gameObject.SetActive(true);

        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        Vector2 mapBottomLeft = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 mapTopRight = Camera.main.ViewportToWorldPoint(Vector2.one);

        Debug.Log("Bottom Left: " + mapBottomLeft);
        Debug.Log("Top Right: " + mapTopRight);

        float minHeight = mapBottomLeft.y - 1;
        float maxHeight = mapTopRight.y + 1;
        float minSide = mapBottomLeft.x - 5;
        float maxSide = mapTopRight.x + 5;

        while (_lifetimeLerp < 1f)
        {
            transform.position = new Vector3(
                Mathf.Lerp(minSide, maxSide, _lifetimeLerp), // path
                Mathf.Lerp(minHeight, maxHeight, _heightLerp), // height
                Mathf.Lerp(10f, 100f, _depthLerp)); // depth

            _lifetimeLerp += Time.deltaTime / _lifetimeTotal;

            yield return null;
        }

        BackgroundManager.ReturnToInactivePool(this);
        gameObject.SetActive(false);
    }
}
