Shader "Custom/Rim"
{
    Properties{
      _MainTex("Texture", 2D) = "green" {}

      _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.5)
      _RimPower("Rim Power", Range(0.5,8.0)) = 3.0
      _Transparency("Transparency", Range(0,1)) = 1
    }
        SubShader{
          Tags { "Queue" = "Transparent" }
          Pass {
                ZWrite On

                ColorMask 0
        }
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade
        struct Input {
            float2 uv_MainTex;
            float3 viewDir;
        };
        sampler2D _MainTex;

        float4 _RimColor;
        float _RimPower;
        float _Transparency;
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow(rim, _RimPower);
            o.Alpha = pow(rim, _RimPower) * _Transparency;
        }
        ENDCG
      }
          Fallback "Diffuse"
}