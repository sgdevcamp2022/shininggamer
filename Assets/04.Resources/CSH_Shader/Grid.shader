Shader "Custom/Grid"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Grass", 2D) = "white" {}
		_GridTex ("Grid", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.5

		sampler2D _MainTex;
		sampler2D _GridTex;

		struct Input {
			float4 color : COLOR;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 uv = IN.worldPos.xz * 0.02;
			fixed4 c = tex2D(_MainTex, float3(uv, 0));
			
			float2 gridUV = IN.worldPos.xz;
			gridUV.x *= 1 / (4 * 8.66025404);
			gridUV.y *= 1 / (2 * 15.0);
			fixed4 grid = tex2D(_GridTex, gridUV);

			o.Albedo = c.rgb * grid * _Color;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
