using UnityEngine;
using System.Collections;

public class CricketBallBowling : MonoBehaviour
{
    [Header("Points")]
    public Transform bowlingPoint;
    public Transform[] pitchPoints;
    public Transform[] batPoints;

    [Header("Speed (per ball full constant)")]
    public float minSpeed = 20f;
    public float maxSpeed = 40f;

    [Header("Swing (after pitch only)")]
    public float swingAmount = 0.5f;

    [Header("Bounce Control")]
    public float fastBounceMin = 0.3f;
    public float fastBounceMax = 0.8f;

    public float spinBounceMin = 0.6f;
    public float spinBounceMax = 1.2f;

    [Header("Spin")]
    public bool enableSpin = false;
    public float spinSideForce = 0.5f;
    public float spinTorque = 6f;

    private Rigidbody rb;
    private float speed;
    private float bounceHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(BowlRoutine());
    }

    IEnumerator BowlRoutine()
    {
        // RESET
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        transform.position = bowlingPoint.position;

        Transform pitch = pitchPoints[Random.Range(0, pitchPoints.Length)];
        Transform bat = batPoints[Random.Range(0, batPoints.Length)];

        float swingDir = Random.value > 0.5f ? 1f : -1f;

        // ✅ SAME SPEED FOR FULL BALL
        speed = Random.Range(minSpeed, maxSpeed);

        // ✅ SEPARATE BOUNCE TYPES
        if (enableSpin)
            bounceHeight = Random.Range(spinBounceMin, spinBounceMax);
        else
            bounceHeight = Random.Range(fastBounceMin, fastBounceMax);

        yield return MoveFull(pitch.position, bat.position, swingDir);
    }

    IEnumerator MoveFull(Vector3 pitch, Vector3 target, float swingDir)
    {
        bool reachedPitch = false;

        float bounceTimer = 0f;
        float bounceDuration = 0.35f;

        while (true)
        {
            Vector3 currentTarget = reachedPitch ? target : pitch;

            // ✅ CONSTANT SPEED
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget,
                speed * Time.deltaTime
            );

            // 👉 BEFORE PITCH (NO SWING)
            if (!reachedPitch)
            {
                if (Vector3.Distance(transform.position, pitch) < 0.05f)
                {
                    reachedPitch = true;
                }
            }
            // 👉 AFTER PITCH (SWING + BOUNCE + SPIN)
            else
            {
                bounceTimer += Time.deltaTime;

                float t = bounceTimer / bounceDuration;
                float height = Mathf.Sin(t * Mathf.PI) * bounceHeight;

                // BOUNCE
                transform.position += Vector3.up * height * Time.deltaTime;

                // SWING (only after pitch)
                transform.position += transform.right * swingAmount * swingDir * Time.deltaTime;

                // SPIN SIDE EFFECT
                if (enableSpin)
                {
                    transform.position += transform.right * spinSideForce * swingDir * Time.deltaTime;
                }

                if (Vector3.Distance(transform.position, target) < 0.1f)
                    break;
            }

            yield return null;
        }

        transform.position = target;

        // PHYSICS CONTINUE SAME SPEED
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 finalDir = (target - pitch).normalized;
        rb.linearVelocity = finalDir * speed;

        if (enableSpin)
        {
            rb.AddTorque(Random.onUnitSphere * spinTorque, ForceMode.Impulse);
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