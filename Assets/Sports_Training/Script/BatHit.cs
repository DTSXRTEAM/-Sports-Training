using UnityEngine;

public class BatHit : MonoBehaviour
{
    public float hitMultiplier = 15f;
    private Vector3 lastPos;
    private Vector3 velocity;

    void Update()
    {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                Vector3 hitDir = velocity.normalized;

                ballRb.linearVelocity = Vector3.zero; // reset old motion
                ballRb.AddForce(hitDir * velocity.magnitude * hitMultiplier, ForceMode.Impulse);
            }
        }
    }
}