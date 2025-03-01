using System.Threading.Tasks;
using UnityEngine;

public class TestCircleShader : MonoBehaviour
{
    public RenderTexture renderTextureA;
    public RenderTexture renderTextureB;

    protected Material material;

    public float deltaAcc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderTextureA = new RenderTexture(512, 512, 24);
        renderTextureB = new RenderTexture(512, 512, 24);

        var meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material = meshRenderer.material;
    }

    public void Spread()
    {
        Graphics.Blit(renderTextureA, renderTextureB, material);
        // material.SetTexture("_MainTex", renderTextureB);
        material.mainTexture = renderTextureB;

        var temp = renderTextureA;
        renderTextureA = renderTextureB;
        renderTextureB = temp;
    }

    // Update is called once per frame
    void Update()
    {
        deltaAcc += Time.deltaTime;
        while(deltaAcc > 0.2)
        {
            deltaAcc -= 0.2f;
            Spread();
        }
    }
}
