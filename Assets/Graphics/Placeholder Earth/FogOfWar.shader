Shader "Unlit/FogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha  
            ZWrite Off  
            Cull Off   

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Assets/Scripts/Shader Common/min_lib.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                // float2 uv : TEXCOORD0;
                // float3 normal : NORMAL;
            };

            struct v2f
            {
                // float2 uv : TEXCOORD0;
                // UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 objPos : TEXCOORD0;
                // float3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            // float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.worldNormal = UnityObjectToWorldNormal(v.normal);

                // o.objPos = o.vertex;
                o.objPos = v.vertex;

                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 spherePos = normalize(i.objPos);
                
                // float2 texCoord = pointToUV(spherePos);
                float2 longLat = pointToLongitudeLatitude(spherePos);
                float2 texCoord = longitudeLatitudeToUV(longLat);
                
                // FOW
                float4 col = tex2D(_MainTex, texCoord);

                fixed c = max(col.r, max(col.g, col.b));
                col = fixed4(c, c, c, 1 - c);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
