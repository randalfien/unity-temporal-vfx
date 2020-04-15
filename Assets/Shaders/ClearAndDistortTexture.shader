Shader "CharlesGames/ClearAndDistortTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DisplAmount("Amount of displacement", Range (0, 1)) = 0.4
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "PreviewType"="Plane"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        //Blend Zero Zero
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            
             half _DisplAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 noise = tex2D(_NoiseTex, i.uv+fixed2(0,_Time.x*8));
                fixed2 displ = (noise.rg - 0.5)*_DisplAmount*0.01;
                fixed4 tx = tex2D(_MainTex, i.uv+displ);
                tx.a *= 0.96;
                if( tx.a < 0.1)
                {
                   tx *= 0;
                }
                return tx;
            }
            ENDCG
        }
    }
}
