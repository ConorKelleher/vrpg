using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpawnDice : MonoBehaviour
{
    public GameObject[] dicePrefabs;
    public Transform spawnOrigin;
    private InputDevice device;
    private List<GameObject> SpawnedObjects = new List<GameObject>();
    private Vector3[] dicePositionOffsets;
    private const float RADIUS = 0.15f;
    private const float ARC = 35f;

    private void Start()
    {
        dicePositionOffsets = new Vector3[dicePrefabs.Length];
        Vector3 radialOffset = Vector3.up * RADIUS;
        float midIndex = (dicePositionOffsets.Length / 2f) - 0.5f;
        for (int i = 0; i < dicePositionOffsets.Length; i++)
        {
            float arcAngle = ARC * (i - midIndex);
            Debug.Log(arcAngle);
            dicePositionOffsets[i] = Quaternion.Euler(0, 180, arcAngle) * radialOffset;
        }
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
    }

    private InputDevice GetCorrectHand()
    {
        return gameObject.name == "RightController" ? UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand) : UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.LeftHand);
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        InputDevice correctHand = GetCorrectHand();

        if (correctHand == device)
        {
            this.device = device;
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        InputDevice correctHand = GetCorrectHand();

        if (correctHand == device)
        {
            //this.device = null;
        }
    }

    void Update()
    {
        bool pressing;
        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out pressing);

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

    void Spawn()
    {
        int prefabIndex = 0;
        Vector3 originalEulers = spawnOrigin.eulerAngles;
        //spawnOrigin.LookAt(Camera.main.transform);
        spawnOrigin.rotation = Camera.main.transform.rotation;

        foreach (GameObject diePrefab in dicePrefabs)
        {
            GameObject spawnedDie = GameObject.Instantiate(diePrefab, spawnOrigin);
            spawnedDie.transform.localPosition = dicePositionOffsets[prefabIndex];
            spawnedDie.transform.localEulerAngles = Vector3.zero;
            spawnedDie.transform.parent = null;
            PrepareSpawnedDie(spawnedDie);
            SpawnedObjects.Add(spawnedDie);
            prefabIndex++;
        }

        spawnOrigin.eulerAngles = originalEulers;
    }

    void Despawn()
    {
        Debug.Log("Calling despawn");
        foreach (GameObject spawnedObject in SpawnedObjects)
        {
            GameObject.Destroy(spawnedObject);
        }
        SpawnedObjects = new List<GameObject>();
    }
}
