using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshPro scoreText;

    public GameObject targetObject;   // 👈 Assign in Inspector
    public float activeTime = 2f;     // 👈 Seconds before disable

    private int totalScore = 0;
    private bool hasScored = false;

    private void Start()
    {
        UpdateText();

        if (targetObject != null)
            targetObject.SetActive(false); // start disabled
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasScored) return;

        ScoreCollider sc = other.GetComponent<ScoreCollider>();

        if (sc != null && other.CompareTag("ScoreZone"))
        {
            totalScore += sc.scoreValue;
            UpdateText();
            hasScored = true;

            // ✅ Enable + auto disable
            if (targetObject != null)
                StartCoroutine(EnableTemporarily());
        }
    }

    IEnumerator EnableTemporarily()
    {
        targetObject.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        targetObject.SetActive(false);
    }

    public void ResetBallScore()
    {
        hasScored = false;
    }

    void UpdateText()
    {
        scoreText.text = "Score: " + totalScore;
    }
}