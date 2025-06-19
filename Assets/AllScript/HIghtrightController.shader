Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Outline Color", Color) = (1, 0, 0, 1)
        _Thickness ("Outline Thickness", Float) = 0.03
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Front  // 裏面だけ描く（外側から見えるようにする）

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Color;
            float _Thickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                // 法線方向に少し拡大してアウトライン作成
                v.vertex.xyz += v.normal * _Thickness;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}