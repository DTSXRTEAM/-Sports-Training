using UnityEngine;
using TMPro;

public class OverCounter : MonoBehaviour
{
    [Header("3D Text")]
    public TextMeshPro overText;

    private int over = 0;
    private int ball = 0;

    void Start()
    {
        UpdateText();
    }

    // 🔥 CALL THIS FROM BALL MANAGER
    public void AddBall()
    {
        ball++;

        if (ball > 6)
        {
            over++;
            ball = 1;
        }

        UpdateText();
    }

    void UpdateText()
    {
        if (overText != null)
        {
            overText.text = over.ToString() + "." + ball.ToString();
        }
    }
}