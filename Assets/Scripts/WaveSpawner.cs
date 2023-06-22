using System.Collections;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [SerializeField] GameObject zombie1;
    [SerializeField] GameObject zombie2;
    [SerializeField] Transform[] spawnZombiesPos;

    public int maxZombies = 10;
    public int minZombies = 1;
    public float initialSpawnDelay = 60f;
    public float spawnDelayDecrease = 5f;
    public float minSpawnDelay = 10f;

    private float currentSpawnDelay;

    private void Start()
    {
        currentSpawnDelay = initialSpawnDelay;
        StartCoroutine(SpawnHorde());
    }

    private IEnumerator SpawnHorde()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnDelay); // Tiempo de espera entre weaves
            Debug.Log("Spawneando weave");
            SpawnZombies();

            if (currentSpawnDelay > minSpawnDelay)
            {
                currentSpawnDelay -= spawnDelayDecrease; // Ir decrementando el tiempo de weaves
            }
        }
    }

    private void SpawnZombies()
    {
        int numZombies = Random.Range(minZombies, maxZombies + 1); //Para seleccionar numero aleatorio por weave

        int numZombies1 = Random.Range(0, numZombies + 1); // Numero aleatorio de zombies
        int numZombies2 = numZombies - numZombies1;

        for (int i = 0; i < numZombies1; i++)
        {
            SpawnZombie(zombie1);
        }

        for (int i = 0; i < numZombies2; i++)
        {
            SpawnZombie(zombie2);
        }
    }

    private void SpawnZombie(GameObject prefab)
    {
        // L�gica para instanciar un zombie en una posici�n aleatoria
        int r = Random.Range(0, spawnZombiesPos.Length);
        Instantiate(prefab, spawnZombiesPos[r].position, Quaternion.identity);
    }
}
