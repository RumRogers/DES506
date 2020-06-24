Shader "Custom/Outline"
{
    Properties
    {
        _MainTex("Main Texture",2D) = "black"{}
    }

    SubShader
    {
      //  Tags { "RenderType" = "Opaque" }
      //  LOD 200

        pass
        {
            CGPROGRAM
            sampler2D _MainTex;
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos: SV_POSITION;
                float2 uvs: TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f output;
                output.pos = UnityObjectToClipPos(v.vertex);
                output.uvs = output.pos.xy / 2 + 0.5;
                return output;
            }

            half4 frag(v2f input) : COLOR
            {
                return tex2D(_MainTex, input.uvs.xy);
            }
            ENDCG
        }
    }
   // FallBack "Diffuse"
}
