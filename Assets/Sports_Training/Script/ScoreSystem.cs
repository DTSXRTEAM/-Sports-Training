using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public TextMeshPro scoreText;

    private int totalScore = 0;   // ✅ NEVER reset this
    private bool hasScored = false;

    private void Start()
    {
        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasScored) return;

        ScoreCollider sc = other.GetComponent<ScoreCollider>();

        if (sc != null && other.CompareTag("ScoreZone"))
        {
            totalScore += sc.scoreValue; // ✅ ADD ONLY
            UpdateText();
            hasScored = true;
        }
    }

    // ✅ ONLY unlock scoring for next ball
    public void ResetBallScore()
    {
        hasScored = false; // ❌ DO NOT touch totalScore
    }

    void UpdateText()
    {
        scoreText.text = "Score: " + totalScore;
    }
}