Shader "Custom/Specular"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MetallicTex("Albedo (R)", 2D) = "white" {}
        _SpecColor("Specular", Color) = (1,1,1,1)
        _mySlider("Bump Amount", Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags { "Queue" = "Geometry" }


            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf StandardSpecular

            // Use shader model 3.0 target, to get nicer looking lighting

            sampler2D _MetallicTex;
            fixed4 _Color;
            half _mySlider;
            struct Input
            {
                float2 uv_MetallicTex;
            };



            void surf(Input IN, inout SurfaceOutputStandardSpecular o)
            {
                // Albedo comes from a texture tinted by color
                o.Albedo = _Color.rgb;
                // Metallic and smoothness come from slider variables
                o.Specular = _SpecColor.rgb * _mySlider;
                o.Smoothness = tex2D(_MetallicTex, IN.uv_MetallicTex).rgb;
            }
            ENDCG
        }
            FallBack "Diffuse"
}