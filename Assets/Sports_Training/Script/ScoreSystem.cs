using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshPro scoreText;

    public GameObject targetObject;
    public float activeTime = 2f;

    [Header("Initial Score")]
    public int totalScore = 0;   // 👈 SET 256 IN INSPECTOR

    private bool hasScored = false;

    private void Start()
    {
        UpdateText();

        if (targetObject != null)
            targetObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasScored) return;

        ScoreCollider sc = other.GetComponent<ScoreCollider>();

        if (sc != null && other.CompareTag("ScoreZone"))
        {
            totalScore += sc.scoreValue; // ✅ adds to existing score
            UpdateText();
            hasScored = true;

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