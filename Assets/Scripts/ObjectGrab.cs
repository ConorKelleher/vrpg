using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectGrab : MonoBehaviour
{

    public Transform palm;
    private List<GameObject> CollidingObjects;
    private GameObject objectInHand;
    private InputDevice device;
    private SpawnDice m_SpawnDice;
    private bool wasGripping = false;

    void Start()
    {
        CollidingObjects = new List<GameObject>();
        m_SpawnDice = gameObject.GetComponent<SpawnDice>();
    }

    public void SetGripping(bool gripping)
    {
        bool startedGripping = gripping && !wasGripping;

        if (startedGripping && !objectInHand && CollidingObjects.Count > 0)
        {
            GrabObject();
        }

        if (!gripping && objectInHand)
        {
            ReleaseObject();
        }

        wasGripping = gripping;
    }

    GameObject GetNearestCollidingObject()
    {
        GameObject nearest = CollidingObjects[0];
        float nearestDistance = Mathf.Infinity;
        for (int i = CollidingObjects.Count - 1; i > -1; i--)
        {
            // If colliding object was destroyed externally, we may still be tracking it. Filter the list now
            if (CollidingObjects[i] == null)
            {
                CollidingObjects.RemoveAt(i);
            }
        }
        foreach (GameObject collidingObject in CollidingObjects)
        {
            float distance = Vector3.Distance(collidingObject.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = collidingObject;
            }
        }
        return nearest;
    }

    public void GrabObject()
    {
        objectInHand = GetNearestCollidingObject();

        // Let SpawnDice know we've grabbed an object so it can check if it needs removal from selection wheel
        // Do this first so we can persist original pos/rot
        m_SpawnDice.ObjectGrabbed(objectInHand);

        objectInHand.transform.SetParent(palm);
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Collider col in objectInHand.GetComponents<Collider>())
        {
            col.enabled = false;
        }
        objectInHand.transform.localPosition = Vector3.zero;
    }

    private void ReleaseObject()
    {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        foreach (Collider col in objectInHand.GetComponents<Collider>())
        {
            col.enabled = true;
        }
        objectInHand.transform.SetParent(null);
        Interactible interactibleScript = objectInHand.GetComponent<Interactible>();
        if (interactibleScript.Throwable)
        {
            Vector3 controllerVelocity;
            Vector3 controllerAngularVelocity;
            device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out controllerVelocity);
            device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceAngularVelocity, out controllerAngularVelocity);

            objectInHand.GetComponent<Rigidbody>().velocity = controllerVelocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerAngularVelocity;
        }
        objectInHand = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!CollidingObjects.Contains(other.gameObject) && other.gameObject.GetComponent<Interactible>())
        {
            CollidingObjects.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (CollidingObjects.Contains(other.gameObject))
        {
            CollidingObjects.Remove(other.gameObject);
        }
    }
}
