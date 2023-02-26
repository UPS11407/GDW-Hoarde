Shader "Custom/Enemy"
{
    Properties
    {
        
        _MainTex ("Texas", 2D) = "white" {}
        _BloodColor ("BloodColor", Color) = (0,0,0,0)
        _Decal1("Decal 1", 2D) = "white" {}
        [Toggle] _ShowDecal1 ("Show Decal 1", float) = 0
        //_Decal2("Decal 2", 2D) = "white" {}
        //[Toggle] _ShowDecal2 ("Show Decal 2", float) = 0
        _Shininess ("Shininess", Range(0,1)) = 0

    }
    SubShader
    {
        Tags { "Queue"="Geometry" }

        CGPROGRAM
        #pragma surface surf Standard 


        sampler2D _MainTex;
        sampler2D _Decal1;
        float _ShowDecal1;
        half _Shininess;
        float4 _BloodColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 a = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 b = tex2D(_Decal1, IN.uv_MainTex) * _ShowDecal1 * _BloodColor;
            

            o.Albedo = b.r > 0.9 ? b.rgb : a.rgb;
            o.Smoothness = _Shininess;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
