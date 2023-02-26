Shader "Alexander/ComplexOutlineShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline Width", Range(.002,1.0)) = .005
    }
        SubShader
        {
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
            Pass{
                Cull Front

                CGPROGRAM
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
