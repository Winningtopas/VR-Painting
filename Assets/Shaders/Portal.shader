Shader "Unlit/Portal"
{
    Properties
    {
        _NoiseTex ("Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        Blend OneMinusDstColor One
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 noiseScale = float2(0.5, 0.5);
                float2 timeScale = float2(_Time.x / 2, _Time.x / 2);
                fixed4 col0 = tex2D(_NoiseTex, i.uv);
                fixed4 col1 = tex2D(_NoiseTex, i.uv * noiseScale + timeScale);
                fixed4 col2 = tex2D(_NoiseTex, i.uv * noiseScale - timeScale);
                float4 UVOffset = col0 + col1 + col2 / 3;
                fixed4 col4 = tex2D(_NoiseTex, i.uv + UVOffset.xy);

                fixed4 returnCol = col4 * UVOffset;
                return (returnCol + fixed4(0.8, 0.6, 0.9, 1)) / 2;
            }
            ENDCG
        }
    }
}
