using UnityEngine;
using System.Collections;

public class CameraPostShader : MonoBehaviour
{
	public Material post_material;
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		post_material.SetTexture("_Velocity", source);
		post_material.SetTexture("_Source", source);
		//mat is the material containing your shader
		Graphics.Blit (null, destination, post_material);
	}

}
