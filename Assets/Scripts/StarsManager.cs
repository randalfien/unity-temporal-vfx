using System.Collections;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    public Camera RenderCamera;
    public GameObject Quad;

    private RenderTexture _renderTexture;
    private RenderTexture _renderTextureTemp;

    public Material _dimStarsMat;

    void Start()
    {
        if (RenderCamera.targetTexture != null)
        {
            RenderCamera.targetTexture.Release();
        }
        
        // Create Render Textures
        _renderTexture = new RenderTexture( Screen.width, Screen.height, 24);
        _renderTextureTemp = new RenderTexture( Screen.width, Screen.height, 24);
        RenderCamera.targetTexture = _renderTexture;
        
        // Make quad full screen
        var sizeH = RenderCamera.orthographicSize * 2f;
        var sizeW = sizeH * Screen.width / Screen.height;
        Quad.transform.localScale = new Vector3(sizeW,sizeH,1);
        
        var meshRenderer = Quad.GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_MainTex", _renderTexture);
        
        StartCoroutine(RotateStars());
    }
    
    private IEnumerator RotateStars()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 360; i++)
        {
            transform.rotation = Quaternion.Euler(0,0,20f*EaseOutCubic(i/360f)-10);
            yield return null;
        }
    }

    private float EaseOutCubic(float x) //Easing function from https://easings.net/
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }

    public void OnPreRender()
    {
        Graphics.Blit(_renderTexture, _renderTextureTemp, _dimStarsMat);

        // Blitting a texture into itself is not defined so we swap these two textures back and forth every frame
        var temp = _renderTexture;
        _renderTexture = _renderTextureTemp;
        _renderTextureTemp = temp;
    }
}
