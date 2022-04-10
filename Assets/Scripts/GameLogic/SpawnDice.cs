using System.Collections.Generic;
using UnityEngine;

public class SpawnDice : MonoBehaviour
{
    public GameObject[] dicePrefabs;
    public Transform spawnOrigin;
    private List<GameObject> SpawnedObjects = new List<GameObject>();
    public Vector3[] dicePositionOffsets;
    private const float RADIUS = 15f;
    private const float ARC = 35f;

    private void Start()
    {
        dicePositionOffsets = new Vector3[dicePrefabs.Length];
        Vector3 radialOffset = Vector3.up * RADIUS;
        float midIndex = (dicePositionOffsets.Length / 2f) - 0.5f;
        for (int i = 0; i < dicePositionOffsets.Length; i++)
        {
            float arcAngle = ARC * (i - midIndex);
            dicePositionOffsets[i] = Quaternion.Euler(0, 180, arcAngle) * radialOffset;
        }
        Debug.Log(dicePositionOffsets.Length);
    }

    public void SetPressing(bool pressing)
    {
        if (pressing && SpawnedObjects.Count == 0)
        {
            Spawn();
        }

        if (!pressing && SpawnedObjects.Count > 0)
        {
            Despawn();
        }
    }

    void PrepareSpawnedDie(GameObject spawnedDie)
    {
        spawnedDie.GetComponent<Rigidbody>().isKinematic = true;
        spawnedDie.GetComponent<ParticleSystem>().Play();

        foreach (Collider col in spawnedDie.GetComponents<Collider>())
        {
            if (!col.isTrigger)
            {
                col.enabled = false;
            }
        }
    }

    public void ObjectGrabbed(GameObject die)
    {
        if (SpawnedObjects.Contains(die))
        {
            int dieIndex = SpawnedObjects.IndexOf(die);
            ParticleSystem.EmissionModule emission = SpawnedObjects[dieIndex].GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
            SpawnedObjects[dieIndex] = GameObject.Instantiate(dicePrefabs[dieIndex], die.transform.position, die.transform.rotation);
            PrepareSpawnedDie(SpawnedObjects[dieIndex]);
        }
    }

    public void Spawn()
    {
        Debug.Log("Spawning");
        int prefabIndex = 0;
        Vector3 originalEulers = spawnOrigin.eulerAngles;
        spawnOrigin.LookAt(Camera.main.transform);
        spawnOrigin.forward = -spawnOrigin.forward;
        Debug.Log($"DicePrefab count: {dicePrefabs.Length}");
        foreach (GameObject diePrefab in dicePrefabs)
        {
            GameObject spawnedDie = Instantiate(diePrefab, spawnOrigin);
            spawnedDie.transform.localPosition = dicePositionOffsets[prefabIndex];
            spawnedDie.transform.localEulerAngles = Vector3.zero;
            spawnedDie.transform.parent = null;
            spawnedDie.transform.localScale = diePrefab.transform.localScale;
            PrepareSpawnedDie(spawnedDie);
            SpawnedObjects.Add(spawnedDie);
            prefabIndex++;
        }

        spawnOrigin.eulerAngles = originalEulers;
    }

    void Despawn()
    {
        Debug.Log("Despawning");
        foreach (GameObject spawnedObject in SpawnedObjects)
        {
            Destroy(spawnedObject);
        }
        SpawnedObjects = new List<GameObject>();
    }
}
