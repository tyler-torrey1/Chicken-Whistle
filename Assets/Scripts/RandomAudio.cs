using UnityEngine;

public class RandomAudio : MonoBehaviour {
    [SerializeField] private AudioClip[] _clips;

    [SerializeField, Min(0)] private float _minWait;
    [SerializeField, Min(0)] private float _maxWait;
    [SerializeField, Range(0f, 1f)] float volume;

    StopWatch _waitWatch;
    float _currWaitAmount;

    private void Start() {
        _waitWatch = new StopWatch();
        ResetWait();
    }

    private void Update() {
        if (_waitWatch > _currWaitAmount) {
            PlayAudio();
        }
    }

    private void OnValidate() {
        _maxWait = Mathf.Max(_maxWait, _minWait);
    }

    public void PlayAudio() {
        int randomIndex;
        int sanity = 0;
        do {
            randomIndex = Mathf.FloorToInt(Random.Range(0, _clips.Length));
            sanity++;
        } while (sanity < _clips.Length + 1 && _clips[randomIndex] == null);

        AudioSource.PlayClipAtPoint(_clips[randomIndex], transform.position, volume);

        ResetWait(_clips[randomIndex].length);
    }

    private void ResetWait(float extraWait = 0f) {
        _currWaitAmount = extraWait + Random.Range(_minWait, _maxWait);

        _waitWatch.Start();
    }
}
