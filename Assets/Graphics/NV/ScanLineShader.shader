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
            
            v2f vert(appdata v)
            {
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

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
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
