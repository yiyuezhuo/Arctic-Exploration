Shader "Unlit/CircleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float center = float2(0.5, 0.5);
                float2 uvOffset = i.uv - center;
                float distanceFromCenter = length(uvOffset);

                if(distanceFromCenter < 0.01)
                {
                    return fixed4(0, 1, 0, 1);
                }

                float2 uvOffset1 = float2(0.01, 0.0);
                float2 uvOffset2 = float2(-0.01, 0.0);
                float2 uvOffset3 = float2(0.0, 0.01);
                float2 uvOffset4 = float2(0.0, -0.01);

                fixed4 color1 = tex2D(_MainTex, i.uv + uvOffset1);
                fixed4 color2 = tex2D(_MainTex, i.uv + uvOffset2);
                fixed4 color3 = tex2D(_MainTex, i.uv + uvOffset3);
                fixed4 color4 = tex2D(_MainTex, i.uv + uvOffset4);
                
                fixed4 maxColor = max(max(color1, color2), max(color3, color4));

                return maxColor;
            }
            ENDCG
        }
    }
}
