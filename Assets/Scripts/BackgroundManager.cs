using System.Collections.Generic;
using UnityEngine;

/**
 * Singleton manager of parallax cloud pool.
 */

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    [SerializeField] private BackgroundCloud _cloudPrefab;
    [SerializeField, Min(0)] private float _maxCount;
    [SerializeField, Min(0)] private float _minWaitSeconds;
    [SerializeField, Min(0)] private float _maxWaitSeconds;

    private HashSet<BackgroundCloud> activeInstances;
    private Queue<BackgroundCloud> inactiveInstances;

    StopWatch spawnWatch;
    float currentWait;

    public static void InitializeBackground() => instance.InitializeBackgroundInstance();
    public static void ReturnToInactivePool(BackgroundCloud cloud) => instance.ReturnToInactivePoolInstance(cloud);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        spawnWatch = new StopWatch();
        activeInstances = new HashSet<BackgroundCloud>();
        inactiveInstances = new Queue<BackgroundCloud>();

        for (int i = 0; i < _maxCount; i++)
        {
            BackgroundCloud instance = Instantiate(_cloudPrefab);
            inactiveInstances.Enqueue(instance);
            instance.transform.parent = transform;
            instance.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        InitializeBackgroundInstance();
    }

    private void OnValidate()
    {
        _maxWaitSeconds = Mathf.Max(_minWaitSeconds, _maxWaitSeconds);
    }

    private void Update()
    {
        if (spawnWatch > currentWait)
        {
            ResetSpawnTimer();

            if (activeInstances.Count < _maxCount)
            {
                SpawnCloud();
            }
        }
    }

    /**
     * Scene change; deactivate all active clouds, then spawn all with random lifetime lerps.
     */
    [ContextMenu("Initialize Background")]
    private void InitializeBackgroundInstance()
    {
        foreach (BackgroundCloud cloud in activeInstances)
        {
            cloud.gameObject.SetActive(false); // kills running coroutine
            inactiveInstances.Enqueue(cloud);
        }
        activeInstances.Clear();

        int spawnCount = Mathf.FloorToInt(Random.Range(0f, _maxCount));
        Debug.Assert(inactiveInstances.Count >= spawnCount);
        for (int i = 0; i <= spawnCount; i++)
        {
            BackgroundCloud cloud = inactiveInstances.Dequeue();
            cloud.Spawn(Random.Range(0f, 1f), Random.Range(0f, 1f));
            activeInstances.Add(cloud);

        }
    }

    /**
     * Returnal of cloud after coroutine.
     */
    private void ReturnToInactivePoolInstance(BackgroundCloud cloud)
    {
        activeInstances.Remove(cloud);
        inactiveInstances.Enqueue(cloud);
    }

    private void SpawnCloud()
    {
        if (inactiveInstances.Count == 0)
        {
            Debug.LogWarning(name + ": No inactive instances");
            return;
        }

        BackgroundCloud instance = inactiveInstances.Dequeue();
        instance.Spawn(Random.Range(0f, 1f));
        inactiveInstances.Enqueue(instance);

    }

    private void ResetSpawnTimer()
    {
        currentWait = Random.Range(_minWaitSeconds, _maxWaitSeconds);
        spawnWatch.Start();
    }
}
