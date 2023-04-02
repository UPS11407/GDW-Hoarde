Shader "Alexander/VertexExtrusion"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Amount ("Extrusion Amount", Range(0,1)) = .001
    }
    SubShader
    {
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
       // #pragma surface surf Lambert
        #pragma surface surf Lambert vertex:vert

        

        struct Input
        {
            float2 uv_MainTex;
        };

        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord: TEXCOORD0;
        };
        sampler2D _MainTex;
        half _Amount;

        void vert(inout appdata v) {
            v.vertex.xyz += v.normal * _Amount;
        }
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
