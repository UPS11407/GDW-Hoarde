Shader "Unlit/ScanLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanLineTex("ScanLine Texture", 2D) = "white" {}
        _ScanLineZoom("ScanLine Zoom", float) = 1
        _ScanLineStr("ScanLine Strength", float) = 1
        _NVColor("Night Vision Color", Color) = (1,1,1,1)
        _NVStrength("Night Vision Strength", float) = 1
        _LuminaStrength("Luminosity Strength", float) = 1
        _Distortion("Distortion", Range(-1, 1)) = 0.1
        _Scale("Scale", Range(0, 10)) = 1
        _Center("Center", Vector) = (0.5, 0.5, 0, 0)
        _OffsetR("OffsetR", Range(-0.01, 0.01)) = 0.001
        _OffsetG("OffsetG", Range(-0.01, 0.01)) = 0.001
        _OffsetB("OffsetB", Range(-0.01, 0.01)) = -0.001
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        

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
                float4 vertex : SV_POSITION;
                float4 diff : COLOR0;
            };
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _ScanLineTex;
            float _ScanLineStr;
            float _ScanLineZoom;
            float4 _NVColor;
            float _NVStrength;
            float _LuminaStrength;

            float _Distortion;
            float _Scale;
            float4 _Center;
            float _OffsetR;
            float _OffsetG;
            float _OffsetB;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;
                float2 p = i.uv - center;
                float r2 = dot(p, p);
                float2 distortion = p * (_Distortion * r2 + 1);
                float2 uv = center + distortion * _Scale;
                // Sample the texture three times with different offsets
                float2 offsetR = float2(_OffsetR, 0);
                float2 offsetG = float2(_OffsetG, 0);
                float2 offsetB = float2(_OffsetB, 0);
                float4 texR = tex2D(_MainTex, uv + offsetR);
                float4 texG = tex2D(_MainTex, uv + offsetG);
                float4 texB = tex2D(_MainTex, uv + offsetB);

                // sample the texture
                fixed4 col = float4(texR.r, texG.g, texB.b, 1);
                float4 scanLine = tex2D(_ScanLineTex, i.uv * _ScreenParams.y / _ScanLineZoom);
                float luminosity = saturate(lerp(0.0125,Luminance(col.rgb), _LuminaStrength));
                //scanLine += i.diff * (1-_ScanLineStr)
                col.rgb = _NVColor.rgb * luminosity * _NVStrength;

                luminosity;
                return lerp(col, col * scanLine, _ScanLineStr);
            }
            ENDCG
        }
    }
}
