Shader "Unlit/Oscilloscope"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _BGColor("BGColor", Color) = (0,0,0,1)
        _Color("Color", Color) = (0.458823529,0.588235294,0.525490196,1)
        _Thickness("Thickness",float) = 1
        _Phase("Phase",float) = 0
        _Amplitude("Amplitude",float) = 0.1
        _Zoom("Zoom",float) = 99
        _Origin("Origin",float) = 0.5
        _Speed("Speed",float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            float4 _BGColor;
            float4 _Color;
            float _Thickness;
            float _Phase;
            float _Amplitude;
            float _Zoom;
			float _Origin;
            float _Speed;

            float rand(float3 myVector)  {
                return frac(sin( dot(myVector ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
            }

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                float4 sample = sin(_Time[1] * _Speed + i.uv[0] * _Zoom + _Phase) * _Amplitude + _Origin; //+ rand((i.uv[0], i.uv[1],_Time[0])) * 10;
                if(sample[0] >= i.uv[1] && sample[0] - _MainTex_TexelSize.y * _Thickness < i.uv[1]){
                    col = _Color;
                }else{
                    col = _BGColor;
                }
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
