using UnityEngine;

public class StuffSpawner : MonoBehaviour
{

    public float velocity;

    public FloatRange timeBetweenSpawns, scale, randomVelocity, angularVelocity;

    float currentSpawnDelay;

    public Stuff[] stuffPrefabs;

    float timeSinceLastSpawn;

    public Material stuffMaterial;
    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= currentSpawnDelay)
        {
            timeSinceLastSpawn -= currentSpawnDelay;
            currentSpawnDelay = timeBetweenSpawns.RandomInRange;
            SpawnStuff();
        }
    }

    void SpawnStuff()
    {
        Stuff prefab = stuffPrefabs[Random.Range(0, stuffPrefabs.Length)];
        Stuff spawn = prefab.GetPooledInstance<Stuff>();

        spawn.transform.localScale = Vector3.one * scale.RandomInRange;
        spawn.transform.localRotation = Random.rotation;
        spawn.transform.localPosition = transform.position;

        spawn.Body.velocity = transform.up * velocity +
            Random.onUnitSphere * randomVelocity.RandomInRange;
        spawn.Body.angularVelocity = Random.onUnitSphere * angularVelocity.RandomInRange;

        spawn.SetMaterial(stuffMaterial);
    }
}