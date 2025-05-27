using UnityEngine;
using System.Collections.Generic;

public class CarTransparencyController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float opacity = 0.5f; // Default to half opacity
    
    private List<Material> originalMaterials = new List<Material>();
    private List<Material> transparentMaterials = new List<Material>();
    
    void Start()
    {
        // Find all mesh renderers that have mesh filters
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);
        
        foreach (MeshFilter meshFilter in meshFilters)
        {
            MeshRenderer renderer = meshFilter.GetComponent<MeshRenderer>();
            
            if (renderer != null)
            {
                // Store original materials and create transparent versions
                foreach (Material originalMat in renderer.materials)
                {
                    originalMaterials.Add(originalMat);
                    
                    // Create a new material instance to avoid modifying shared materials
                    Material transparentMat = new Material(originalMat);
                    SetupTransparentMaterial(transparentMat);
                    transparentMaterials.Add(transparentMat);
                }
                
                // Replace the materials with our new transparent versions
                renderer.materials = GetTransparentMaterialsFor(renderer);
            }
        }
        
        // Apply the initial opacity
        SetOpacity(opacity);
    }
    
    private Material[] GetTransparentMaterialsFor(Renderer renderer)
    {
        Material[] originals = renderer.sharedMaterials;
        Material[] transparent = new Material[originals.Length];
        
        for (int i = 0; i < originals.Length; i++)
        {
            int index = originalMaterials.FindIndex(m => m == originals[i]);
            if (index >= 0)
            {
                transparent[i] = transparentMaterials[index];
            }
            else
            {
                // Create a new transparent material if not already created
                Material newTransparentMat = new Material(originals[i]);
                SetupTransparentMaterial(newTransparentMat);
                
                originalMaterials.Add(originals[i]);
                transparentMaterials.Add(newTransparentMat);
                
                transparent[i] = newTransparentMat;
            }
        }
        
        return transparent;
    }
    
    private void SetupTransparentMaterial(Material material)
    {
        // Universal approach for standard shader
        if (material.HasProperty("_Mode"))
        {
            // Standard shader setup
            material.SetFloat("_Mode", 3); // Transparent mode
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
        
        // For both standard and URP/HDRP materials
        if (material.HasProperty("_Surface"))
        {
            // URP/HDRP setup
            material.SetFloat("_Surface", 1); // 0 = opaque, 1 = transparent
        }
        
        // Enable these properties if they exist (works for most shader types)
        if (material.HasProperty("_ZWrite"))
        {
            material.SetInt("_ZWrite", 0);
        }
        
        // Set render queue for transparency
        material.renderQueue = 3000;
    }
    
    public void SetOpacity(float value)
    {
        // Clamp value between 0 and 1
        opacity = Mathf.Clamp01(value);
        
        for (int i = 0; i < transparentMaterials.Count; i++)
        {
            Material material = transparentMaterials[i];
            
            // Get the current color
            Color color = material.color;
            
            // Set the alpha value while keeping the RGB the same
            color.a = opacity;
            material.color = color;
            
            // For some shaders, we need to set additional properties
            if (material.HasProperty("_BaseColor"))
            {
                // URP/HDRP main color property
                Color baseColor = material.GetColor("_BaseColor");
                baseColor.a = opacity;
                material.SetColor("_BaseColor", baseColor);
            }
        }
    }
    
    void OnDestroy()
    {
        // Clean up created materials to prevent memory leaks
        foreach (Material mat in transparentMaterials)
        {
            if (Application.isPlaying)
                Destroy(mat);
            else
                DestroyImmediate(mat);
        }
    }
}