using UnityEngine;
using TMPro;

public class OverCounter : MonoBehaviour
{
    public TextMeshPro overText;

    private int over = 0;
    private int ball = 0;

    void Start()
    {
        UpdateText();
    }

    public void AddBall()
    {
        ball++;

        // ✅ FIX: check == 6 instead of > 6
        if (ball == 6)
        {
            over++;
            ball = 0; // reset to 0 (new over starts)
        }

        UpdateText();
    }

    void UpdateText()
    {
        if (overText != null)
        {
            overText.text = over + "." + ball;
        }
    }
}