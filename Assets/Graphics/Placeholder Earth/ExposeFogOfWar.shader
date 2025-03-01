Shader "Unlit/ExposeFogOfWar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CenterLat ("Center Latitude", Float) = 0.0
        _CenterLon ("Center Longitude", Float) = 0.0
        _Radius ("Radius (in nautical miles)", Float) = 20.0
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
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Assets/Scripts/Shader Common/min_lib.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _CenterLat;
            float _CenterLon;
            float _Radius;

            // Calculate the distance between two points on the Earth's surface in nautical miles
            float Haversine(float lat1, float lon1, float lat2, float lon2)
            {
                float dLat = radians(lat2 - lat1);
                float dLon = radians(lon2 - lon1);
                float a = sin(dLat / 2.0) * sin(dLat / 2.0) +
                          cos(radians(lat1)) * cos(radians(lat2)) *
                          sin(dLon / 2.0) * sin(dLon / 2.0);
                float c = 2.0 * atan2(sqrt(a), sqrt(1.0 - a));
                float distance = 3440.065 * c; // Earth's radius in nautical miles
                return distance;
            }

            v2f vert (appdata v)
            {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert UV to latitude and longitude
                // float lat = 90.0 - i.uv.y * 180.0;
                float lat = -90.0 + i.uv.y * 180.0;
                float lon = i.uv.x * 360.0 - 180.0;

                // Calculate the distance from the center point
                float distance = Haversine(_CenterLat, _CenterLon, lat, lon);

                // If the distance is within the radius, color it white
                if (distance <= _Radius)
                {
                    return fixed4(1.0, 1.0, 1.0, 1.0);
                    // return fixed4(1.0, 0.0, 0.0, 1.0);
                }

                // Otherwise, use the original texture color
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
