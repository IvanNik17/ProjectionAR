Shader "UI/ProjectionWarp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _TopLeft ("Top Left", Vector) = (0,0,0,0)
        _TopRight ("Top Right", Vector) = (1,0,0,0)
        _BottomLeft ("Bottom Left", Vector) = (0,1,0,0)
        _BottomRight ("Bottom Right", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _TopLeft;
            float4 _TopRight;
            float4 _BottomLeft;
            float4 _BottomRight;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS =
                    TransformObjectToHClip(IN.positionOS.xyz);

                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                float2 top = lerp(
                    _TopLeft.xy,
                    _TopRight.xy,
                    uv.x
                );

                float2 bottom = lerp(
                    _BottomLeft.xy,
                    _BottomRight.xy,
                    uv.x
                );

                float2 warpedUV = lerp(
                    top,
                    bottom,
                    uv.y
                );

                return SAMPLE_TEXTURE2D(
                    _MainTex,
                    sampler_MainTex,
                    warpedUV
                );
            }

            ENDHLSL
        }
    }
}