using UnityEngine;

public class FogOfWarSphere : MonoBehaviour
{
    public RenderTexture renderTextureSrc;
    public RenderTexture renderTextureDst;
    public Texture2D initialFogOfWarTexture;
    protected Material material;
    public Material renderMaterial;

    void Awake()
    {
        renderTextureSrc = CreateRenderTexture();
        renderTextureDst = CreateRenderTexture();
        Graphics.Blit(initialFogOfWarTexture, renderTextureSrc);

        var meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material = meshRenderer.material; // copy material and take reference
    }

    public void UpdateFogOfWar(float latitudeDeg, float longitudeDeg, float radiusNm)
    {
        renderMaterial.SetFloat("_CenterLat", latitudeDeg);
        renderMaterial.SetFloat("_CenterLon", longitudeDeg);
        renderMaterial.SetFloat("_Radius", radiusNm);

        Graphics.Blit(renderTextureSrc, renderTextureDst, renderMaterial);

        var temp = renderTextureDst;
        renderTextureDst = renderTextureSrc;
        renderTextureSrc = temp;
        material.mainTexture = renderTextureSrc;
    }

    RenderTexture CreateRenderTexture()
    {
        var width = initialFogOfWarTexture.width;
        var height = initialFogOfWarTexture.height;
        var renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        renderTexture.Create();
        return renderTexture;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static FogOfWarSphere _Instance;
    public static FogOfWarSphere Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindFirstObjectByType<FogOfWarSphere>();
            }
            return _Instance;
        }
    }

    public void OnDestroy()
    {
        if(_Instance == this)
        {
            _Instance = null;
        }
    }

}
