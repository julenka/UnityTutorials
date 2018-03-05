using UnityEngine;

[ExecuteInEditMode()]
[RequireComponent(typeof(Camera))]
public class TurnOnDepthBuffer : MonoBehaviour
{
    [Tooltip("Material containing depth buffer shader")]
    public Material depthMaterial;
    // Use this for initialization
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to
        Graphics.Blit(source, destination, depthMaterial);
    }
}
