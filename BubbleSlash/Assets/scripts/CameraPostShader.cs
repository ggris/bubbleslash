using UnityEngine;
using System.Collections;

public class CameraPostShader : MonoBehaviour
{
	public Material post_material;
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		
		//mat is the material containing your shader
		Graphics.Blit (source, destination, post_material);
	}

}
