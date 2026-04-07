using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class XROpacityController : MonoBehaviour
{
    public Renderer[] renderers;
    public XRSlider xrSlider;

    void Update()
    {
        if (xrSlider == null) return;

        float value = xrSlider.value;
        ChangeOpacity(value);
    }

    void ChangeOpacity(float value)
    {
        foreach (Renderer rend in renderers)
        {
            if (rend == null) continue;

            foreach (Material mat in rend.materials)
            {
                if (mat == null) continue;

                if (mat.HasProperty("_Opacity"))
                    mat.SetFloat("_Opacity", value);

                if (mat.HasProperty("_BaseColor"))
                {
                    Color col = mat.GetColor("_BaseColor");
                    col.a = value;
                    mat.SetColor("_BaseColor", col);
                }
            }
        }
    }
}