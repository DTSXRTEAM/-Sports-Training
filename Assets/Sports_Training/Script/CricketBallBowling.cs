using UnityEngine;
using System.Collections;

public class CricketBallBowling : MonoBehaviour
{
    [Header("Points")]
    public Transform bowlingPoint;
    public Transform[] pitchPoints;
    public Transform[] batPoints;

    [Header("Bowling Type")]
    public bool enableSpin = false;

    [Header("Speed")]
    public float minSpeed = 12f;
    public float maxSpeed = 18f;

    [Header("Swing")]
    public float swingAmount = 1.5f;

    [Header("Bounce")]
    public float minBounce = 0.5f;
    public float maxBounce = 1.5f;

    [Header("Spin")]
    public float spinForce = 8f;
    public float spinSideForce = 1.2f;

    private Rigidbody rb;
    private float speed;
    private Vector3 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(BowlRoutine());
    }

    IEnumerator BowlRoutine()
    {
        // RESET BALL
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        transform.position = bowlingPoint.position;

        // ❌ REMOVED WAIT HERE (NO DELAY)

        // RANDOM TARGETS
        Transform pitch = pitchPoints[Random.Range(0, pitchPoints.Length)];
        Transform bat = batPoints[Random.Range(0, batPoints.Length)];

        float bounceHeight;
        float swingDir = Random.value > 0.5f ? 1f : -1f;

        if (enableSpin)
        {
            speed = Random.Range(8f, 12f);
            bounceHeight = Random.Range(1.2f, 2f);
        }
        else
        {
            speed = Random.Range(minSpeed, maxSpeed);
            bounceHeight = Random.Range(minBounce, maxBounce);
        }

        yield return MoveWithSwing(pitch.position, swingDir);

        yield return new WaitForSeconds(0.05f);

        yield return MoveAfterBounce(pitch.position, bat.position, bounceHeight, swingDir);
    }

    IEnumerator MoveWithSwing(Vector3 target, float swingDir)
    {
        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, target);
        float duration = distance / speed;

        float time = 0;

        while (time < duration)
        {
            float t = time / duration;

            Vector3 pos = Vector3.Lerp(start, target, t);

            float swing = Mathf.Sin(t * Mathf.PI) * swingAmount * swingDir;
            pos += transform.right * swing;

            pos.y += Mathf.Sin(t * Mathf.PI) * 1f;

            lastDirection = (pos - transform.position).normalized;

            transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }

    IEnumerator MoveAfterBounce(Vector3 pitch, Vector3 target, float bounceHeight, float dir)
    {
        float distance = Vector3.Distance(pitch, target);
        float duration = distance / speed;

        float time = 0;

        while (time < duration)
        {
            float t = time / duration;

            Vector3 pos = Vector3.Lerp(pitch, target, t);

            pos.y += Mathf.Sin(t * Mathf.PI) * bounceHeight;

            if (enableSpin)
            {
                float turn = Mathf.Sin(t * Mathf.PI) * spinSideForce * dir;
                pos += transform.right * turn;
            }

            lastDirection = (pos - transform.position).normalized;

            transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = target;

        // SWITCH TO PHYSICS
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.linearVelocity = lastDirection * speed;
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);

        if (enableSpin)
        {
            rb.AddTorque(Random.onUnitSphere * spinForce, ForceMode.Impulse);
        }
    }

    public void BowlAgain()
    {
        StopAllCoroutines();
        StartCoroutine(BowlRoutine());
    }

    public void ToggleSpin(bool value)
    {
        enableSpin = value;
    }
}