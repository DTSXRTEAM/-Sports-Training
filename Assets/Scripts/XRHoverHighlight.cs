using UnityEngine;

public class XRHoverHighlight : MonoBehaviour
{
    public MeshRenderer[] renderers;   // Assign BOTH parts here
    public Material highlightMaterial;

    private Material[] originalMaterials;

    void Awake()
    {
        // Store original materials of all parts
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    public void OnHoverEnter()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = highlightMaterial;
        }
    }

    public void OnHoverExit()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}