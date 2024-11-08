Shader "Unlit/ghost_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FresnelColor ("Fresnel Color", Color) = (0.5, 0.5, 1.0, 1.0)
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 2.0
        _Intensity ("Intensity", Range(0, 2)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FresnelColor;
            float _FresnelPower;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = mul((float3x3)unity_WorldToObject, v.vertex.xyz);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 worldViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float fresnelEffect = pow(1.0 - dot(i.worldNormal, worldViewDir), _FresnelPower);

                fixed4 col = tex2D(_MainTex, i.uv) * _Intensity;
                col.rgb += _FresnelColor.rgb * fresnelEffect;
                col.a *= fresnelEffect;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}