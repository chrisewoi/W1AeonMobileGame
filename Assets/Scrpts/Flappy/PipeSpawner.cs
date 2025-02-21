using UnityEngine;

public class PipeSpawner : MonoBehaviour, IStop, IRestart
{
    [SerializeField] private GameObject pipePrefab;

    [Tooltip("How far up or down the pipes can spawn from centre")]
    [SerializeField] private float pipeSpawnRange;

    [SerializeField] private float pipeSpawnDelay;

    private float pipeTimeLastSpawned;

    private bool isActive = true;


    void Update()
    {
        if (!isActive) return;
        
        // If the current time is greater than the time we last spawned plus our delay,
        // our delay is over. We should spawn
        if (Time.time > pipeTimeLastSpawned + pipeSpawnDelay)
        {
            SpawnPipe();
        }
    }

    private void SpawnPipe()
    {
        pipeTimeLastSpawned = Time.time;

        float yOffset = Random.Range(-pipeSpawnRange, pipeSpawnRange);

        GameObject pipes = Instantiate(pipePrefab);
        // If yOffset is -ve, this will spawn the pipes lower,

        pipes.transform.position = transform.position + Vector3.up * yOffset;
    }

    public void Stop()
    {
        isActive = false;
    }

    public void Restart()
    {
        isActive = true;
        // resync our spawner with current time
        pipeTimeLastSpawned = Time.time;
    }
}
