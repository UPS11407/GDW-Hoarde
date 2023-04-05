Shader "Alexander/ExtrudeOutline"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _ExtrudeAmount("Extrusion Amount", Range(0,1)) = .001
        _Color("Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline Width", Range(.1,2)) = .005
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
            half _ExtrudeAmount;
            fixed4 _Color;

            void vert(inout appdata v) {
                v.vertex.xyz += v.normal * _ExtrudeAmount;
            }
            void surf(Input IN, inout SurfaceOutput o)
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Emission = _Color;
            }
            ENDCG

            Pass{
                Cull Front
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };
                struct v2f {
                    float4 pos : SV_POSITION;
                    float4 color : COLOR;


                };

                float _Outline;
                float4 _OutlineColor;
                float _ExtrudeAmount;
                
                
                
                v2f vert(appdata v) {
                    v2f o;
                    
                    o.pos = UnityObjectToClipPos(v.vertex);
                    float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                    float2 offset = TransformViewToProjection(norm.xy);
                    if(_ExtrudeAmount > 0){
                        o.pos.xy += offset * o.pos.z * (_Outline + _ExtrudeAmount);
                    } else {
                        o.pos.xy += offset * o.pos.z * _Outline;
                    }
                    
                    o.color = _OutlineColor;
                    
                    return o;
                }
                fixed4 frag(v2f i) : SV_Target{
                    return i.color;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
