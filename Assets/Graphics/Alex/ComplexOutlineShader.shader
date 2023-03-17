Shader "Alexander/ComplexOutlineShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline Width", Range(.002,1.0)) = .005
        _BloodColor ("BloodColor", Color) = (0,0,0,0)
        _Decal1("Decal 1", 2D) = "white" {}
        _DecalStr("Decal Strength", Range(0.4,0.5)) = 0
        _Shininess ("Shininess", Range(0,1)) = 0
        _BloodShininess ("Blood Shininess", Range(0,1)) = 0

    }
        SubShader
        {
        CGPROGRAM //first pass
            #pragma surface surf Standard //creating basic lambert shader
            struct Input
            {
            float2 uv_MainTex;
            };
        sampler2D _MainTex;
        sampler2D _Decal1;
        float _ShowDecal1;
        half _Shininess;
        half _BloodShininess;
        float4 _BloodColor;
        float _DecalStr;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            
            fixed4 a = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 b = tex2D(_Decal1, IN.uv_MainTex) * _DecalStr * _BloodColor;
            

            o.Albedo = b.r > 0.4 ? b.rgb : a.rgb;
            o.Smoothness = b.r > 0.4 ? _BloodShininess : _Shininess;
        }
        ENDCG
            Pass{
                Cull Front

                CGPROGRAM // second pass
            //access vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag    

            #include "UnityCG.cginc"

            struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL; //collecting normals of model
            };

        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR; //color of normals
        };

        float _Outline;
        float4 _OutlineColor;

        v2f vert(appdata v) { //vertex shader
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex); //sets position of outline from clip space

            float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal)); //mutiply world postiion by unity model view matrix
            float2 offset = TransformViewToProjection(norm.xy);

            o.pos.xy += offset * o.pos.z * _Outline; //takes in offset and multiplies to extrude vertices
            o.color = _OutlineColor; //colors extruded vertices and normals
            return o;
        }

        fixed4 frag(v2f i) : SV_Target
        {
            return i.color;
        }
            ENDCG
        }
        }
}
