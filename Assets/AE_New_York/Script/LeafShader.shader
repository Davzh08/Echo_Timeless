Shader "URP/LeafShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1) // ��Ҷ�Ļ�����ɫ
        _MainTex ("Texture", 2D) = "white" {} // ������ͼ
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5 // Alpha�ü�ֵ
        _WindStrength ("Wind Strength", Range(0, 1)) = 0.1 // ���ǿ��
        _WindSpeed ("Wind Speed", Range(0, 10)) = 2.0 // ����ٶ�
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off // ˫����Ⱦ
            ZWrite On
            AlphaToMask On // ����Alpha����

            HLSLINCLUDE
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityInput.hlsl"
            #include "Lighting.hlsl"
            #include "UnityPasses.hlsl"
            #include "ShaderLibrary/Common.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDirWS : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                sampler2D _MainTex;
                float _Cutoff;
                float _WindStrength;
                float _WindSpeed;
            CBUFFER_END

            Varyings Vertex(Attributes IN)
            {
                Varyings OUT;
                
                // ��Ч��
                float windOffset = sin(_WindSpeed * _Time.y + IN.positionOS.x * 0.1);
                float3 newPosition = IN.positionOS.xyz + float3(0, windOffset * _WindStrength, 0);
                
                OUT.positionHCS = UnityObjectToClipPos(float4(newPosition, 1.0));
                OUT.uv = IN.uv;
                OUT.worldPos = TransformObjectToWorld(newPosition);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.viewDirWS = normalize(UnityWorldSpaceViewDir(OUT.worldPos));
                
                return OUT;
            }

            half4 Fragment(Varyings IN) : SV_Target
            {
                // �������ɫ����
                half4 albedo = tex2D(_MainTex, IN.uv) * _BaseColor;

                // Alpha�ü�
                clip(albedo.a - _Cutoff);
                
                // �򵥹���
                half3 lighting = LightingLambert(albedo.rgb, IN.normalWS, _WorldSpaceLightPos0.xyz);
                half3 color = lighting * _BaseColor.rgb;

                return half4(color, albedo.a);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}