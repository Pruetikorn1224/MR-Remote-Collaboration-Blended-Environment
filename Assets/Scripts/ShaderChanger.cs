using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChanger : MonoBehaviour
{
    // The shader for the experiment
    public string newShaderName = "Meta/Depth/BiRP/Occlusion Standard";

    private int currentAvatarCount;
    private int previousAvatarCount;

    void Start()
    {
        currentAvatarCount = this.transform.childCount;
        previousAvatarCount = currentAvatarCount;
    }

    void Update()
    {
        // Count number of users
        currentAvatarCount = this.transform.childCount;

        // If the number of users increases
        if (currentAvatarCount != previousAvatarCount)
        {
            // Change the shader to all users
            foreach (Transform child in transform)
            {
                GameObject body = child.transform.Find("Body").transform.Find("Floating_Head").gameObject;
                ChangeShader(body);
            }
        }

        previousAvatarCount = currentAvatarCount;
    }

    // Method to change the shader
    public void ChangeShader(GameObject body)
    {
        // The renderer of the object whose material you want to change
        Renderer targetRenderer = body.GetComponent<Renderer>(); 

        if (targetRenderer != null)
        {
            // Get the material of the target renderer
            Material targetMaterial = targetRenderer.material;

            // Load the new shader
            Shader newShader = Shader.Find(newShaderName);

            if (newShader != null)
            {
                // Assign the new shader to the material
                targetMaterial.shader = newShader;
                Debug.Log("Shader changed to: " + newShaderName);
            }
            else
            {
                Debug.LogError("Shader not found: " + newShaderName);
            }
        }
        else
        {
            Debug.LogError("Target Renderer is not assigned.");
        }
    }
}
