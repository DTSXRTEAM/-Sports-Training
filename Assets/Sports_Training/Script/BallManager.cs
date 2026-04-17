using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefab;
public Transform spawnPoint;

[Header("Destroy Time")]
public float destroyAfterSeconds = 3f;

[Header("XR Input")]
public XRNode controllerNode = XRNode.RightHand;
public float triggerThreshold = 0.8f;

private InputDevice device;
private bool isPressed = false;
private bool isActive = false; // BLOCK SPAWN DURING LIFETIME

void Start()
{
    device = InputDevices.GetDeviceAtXRNode(controllerNode);
}

void Update()
{
    if (!device.isValid)
        device = InputDevices.GetDeviceAtXRNode(controllerNode);

    float triggerValue;
    if (device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
    {
        // PRESS
        if (triggerValue > triggerThreshold && !isPressed)
        {
            isPressed = true;

            if (!isActive)
            {
                SpawnObject();
            }
        }

        // RELEASE
        if (triggerValue < 0.2f)
        {
            isPressed = false;
        }
    }
}

void SpawnObject()
{
    isActive = true;

    GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

    StartCoroutine(DestroyAfterTime(obj));
}

System.Collections.IEnumerator DestroyAfterTime(GameObject obj)
{
    yield return new WaitForSeconds(destroyAfterSeconds);

    if (obj != null)
        Destroy(obj);

    isActive = false; // ALLOW NEXT SPAWN
}
}