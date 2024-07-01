Shader "Custom/PixelatedShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", Float) = 0.01
        _BlurAmount("Blur Amount", Range(0, 1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _PixelSize;
                float _BlurAmount;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 p = i.uv / _PixelSize;
                    p = floor(p) * _PixelSize;

                    fixed4 texColor = tex2D(_MainTex, p);

                    // Apply blur effect
                    fixed4 blurredColor = texColor;
                    float blurRadius = _PixelSize * _BlurAmount;

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            blurredColor += tex2D(_MainTex, p + float2(x, y) * blurRadius);
                        }
                    }

                    blurredColor /= 9.0;

                    return lerp(texColor, blurredColor, _BlurAmount);
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
