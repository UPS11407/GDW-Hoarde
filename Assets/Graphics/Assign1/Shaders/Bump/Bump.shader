Shader "Custom/Bump"
{
    Properties{
       _texture("Texture", 2D) = "white" {}
       _myBump("Bump Texture", 2D) = "bump" {}
       _mySlider("Bump Amount", Range(0,10)) = 1
       _Smooth("Smooth Amount", Range(0,1)) = 0.5
       _SpecColor("Specular", Color) = (1,1,1,1)
    }

    SubShader{
        CGPROGRAM
        #pragma surface surf StandardSpecular
        sampler2D _myDiffuse;
        sampler2D _myBump;
        sampler2D _texture;
        half _mySlider;
        float _Smooth;
        float4 _Color;
        struct Input {
            float2 uv_myDiffuse;
            float2 uv_myBump;
            float2 uv_texture;
        };
            void surf(Input IN, inout SurfaceOutputStandardSpecular o) {
            o.Albedo = tex2D(_texture, IN.uv_texture).rgb;
            o.Specular = _SpecColor.rgb;
            o.Smoothness = _Smooth;
            o.Normal = UnpackNormal(tex2D(_myBump, IN.uv_myBump));
            o.Normal *= float3(_mySlider,_mySlider,1);
        }
            ENDCG 
    }    
    Fallback "Diffuse"
}