Shader "Alexander/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline Width", Range(.002,0.1)) = .005
    }

    SubShader 
    {
        //Tags { "RenderType"="Opaque" }
        ZWrite off

        CGPROGRAM //FIRST PASS
        #pragma surface surf Lambert vertex:vert //accessing vertex shader
        struct Input
        {
            float2 uv_MainTex;
        };
        float _Outline;
        float4 _OutlineColor;

        void vert(inout appdata_full v) {
            v.vertex.xyz += v.normal * _Outline; //vertex extrusion method
        }
        sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Emission = _OutlineColor.rgb; //recieving colour of outline
        }
        ENDCG

            ZWrite on //turning on zbuffer before model is drawn, so its texture on top of outline

        CGPROGRAM //SECOND PASS
            #pragma surface surf Lambert //creating basic lambert shader
            struct Input
            {
            float2 uv_MainTex;
            };
        sampler2D _MainTex;
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
