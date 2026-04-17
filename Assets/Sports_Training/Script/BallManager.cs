using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class BallManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefab;
    public Transform spawnPoint;

    [Header("Over Counter")]
    public OverCounter overCounter;

    [Header("Score System")]
    //public ScoreSystem scoreSystem; // 👈 ADD THIS

    [Header("Destroy Time")]
    public float destroyAfterSeconds = 5f;

    [Header("XR Input")]
    public XRNode controllerNode = XRNode.RightHand;
    public float triggerThreshold = 0.8f;

    private InputDevice device;
    private bool isPressed = false;
    private bool isActive = false;

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

        // 🔥 SPAWN BALL
        GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // 🔥 RESET SCORE FOR NEW BALL
        //if (scoreSystem != null)
        //{
        //    //scoreSystem.ResetBallScore(); // 👈 allow next ball to score
        //}

        // 🔥 OVER COUNT
        if (overCounter != null)
        {
            overCounter.AddBall();
        }

        StartCoroutine(DestroyAfterTime(obj));
    }

    IEnumerator DestroyAfterTime(GameObject obj)
    {
        yield return new WaitForSeconds(destroyAfterSeconds);

        if (obj != null)
            Destroy(obj);

        isActive = false;
    }
}