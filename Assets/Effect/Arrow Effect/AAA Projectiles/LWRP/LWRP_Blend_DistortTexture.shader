Shader "ErbGameArt/LWRP/Particles/Blend_DistortTexture"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		_Emission("Emission", Float) = 2
		_Noise("Noise", 2D) = "white" {}
		_NoisespeedUV("Noise speed U/V", Vector) = (0,0,0,0)
		_Mask("Mask", 2D) = "white" {}
		_Distortionpower("Distortion power", Float) = 1
		[Toggle(_USEDEPTH_ON)] _Usedepth("Use depth?", Float) = 0
		_Depthpower("Depth power", Float) = 1
		_Opacity("Opacity", Range( 0 , 1)) = 1
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="LightweightPipeline" "PreviewType"="Plane"}
		Cull Off
		HLSLINCLUDE
		//#pragma target 3.0
		ENDHLSL		
		Pass
		{
			Tags { "LightMode"="LightweightForward" }
			Name "Base"		
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA		
		    HLSLPROGRAM
		    #pragma prefer_hlslcc gles
		    #pragma vertex vert
		    #pragma fragment frag	
			#pragma shader_feature _USEDEPTH_ON
		    #include "LWRP/ShaderLibrary/Core.hlsl"
		    #include "LWRP/ShaderLibrary/Lighting.hlsl"
		    #include "CoreRP/ShaderLibrary/Color.hlsl"
		    #include "ShaderGraphLibrary/Functions.hlsl"			
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform sampler2D _Mask; uniform float4 _Mask_ST;
			uniform sampler2D _Noise; uniform float4 _Noise_ST;
			uniform float4 _NoisespeedUV;
			uniform float _Distortionpower;
			uniform float4 _Color;
			uniform float _Emission;
			uniform sampler2D _CameraDepthTexture;
			uniform float _Depthpower;
			uniform float _Opacity;
					
			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float4 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
	
		    struct GraphVertexOutput
		    {
		        float4 position : POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
		        UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
		    };
		
		    GraphVertexOutput vert (GraphVertexInput v )
			{
		        GraphVertexOutput o = (GraphVertexOutput)0;
		        UNITY_SETUP_INSTANCE_ID(v);
		        UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				o.ase_texcoord.zw = 0;
				v.vertex.xyz +=  float3( 0, 0, 0 ) ;
				v.ase_normal =  v.ase_normal ;
		        o.position = TransformObjectToHClip(v.vertex.xyz);
		        return o;
			}
		
		    half4 frag( GraphVertexOutput IN  ) : SV_Target
		    {
		        UNITY_SETUP_INSTANCE_ID(IN);
				float2 uv_Mask = IN.ase_texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
				float2 appendResult21 = (float2(_NoisespeedUV.x , _NoisespeedUV.y));
				float2 uv29 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode13 = tex2D( _MainTex, TRANSFORM_TEX(( ( (( tex2D( _Mask, TRANSFORM_TEX(uv_Mask , _Mask)) * tex2D( _Noise, TRANSFORM_TEX(( ( appendResult21 * _Time.y ) + uv29 ),_Noise) ))).rg * uv29 * _Distortionpower ) + uv29 ) ,_MainTex));
				float temp_output_62_0 = ( tex2DNode13.a * _Color.a * IN.ase_color.a );
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth49 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth49 = abs( ( screenDepth49 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult53 = clamp( distanceDepth49 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float staticSwitch47 = ( temp_output_62_0 * clampResult53 );
				#else
				float staticSwitch47 = temp_output_62_0;
				#endif	
		        float3 Color = ( ( tex2DNode13 * _Color * IN.ase_color * tex2DNode13.a * _Color.a * IN.ase_color.a ) * _Emission ).rgb;
		        float Alpha = ( staticSwitch47 * _Opacity );
		        float AlphaClipThreshold = 0;
		#if _AlphaClip
		        clip(Alpha - AlphaClipThreshold);
		#endif
		    	return half4(Color, Alpha);
		    }
		    ENDHLSL
		}
		
		Pass
		{		
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }
			ZWrite On
			ColorMask 0
			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma multi_compile_instancing
			#pragma vertex vert
			#pragma fragment frag
			#include "LWRP/ShaderLibrary/Core.hlsl"
			#pragma shader_feature _USEDEPTH_ON
			uniform sampler2D _MainTex;
			uniform sampler2D _Mask;
			uniform float4 _Mask_ST;
			uniform sampler2D _Noise;
			uniform float4 _NoisespeedUV;
			uniform float _Distortionpower;
			uniform float4 _Color;
			uniform sampler2D _CameraDepthTexture;
			uniform float _Depthpower;
			uniform float _Opacity;

			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float4 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct GraphVertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			GraphVertexOutput vert (GraphVertexInput v)
			{
				GraphVertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;				
				o.ase_texcoord.zw = 0;
				v.vertex.xyz +=  float3(0,0,0);
				v.ase_normal =  v.ase_normal;
				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag (GraphVertexOutput IN ) : SV_Target
		    {
		    	UNITY_SETUP_INSTANCE_ID(IN);
				float2 uv_Mask = IN.ase_texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
				float2 appendResult21 = (float2(_NoisespeedUV.x , _NoisespeedUV.y));
				float2 uv29 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode13 = tex2D( _MainTex, ( ( (( tex2D( _Mask, uv_Mask ) * tex2D( _Noise, ( ( appendResult21 * _Time.y ) + uv29 ) ) )).rg * uv29 * _Distortionpower ) + uv29 ) );
				float temp_output_62_0 = ( tex2DNode13.a * _Color.a * IN.ase_color.a );
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth49 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth49 = abs( ( screenDepth49 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult53 = clamp( distanceDepth49 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float staticSwitch47 = ( temp_output_62_0 * clampResult53 );
				#else
				float staticSwitch47 = temp_output_62_0;
				#endif
				float Alpha = ( staticSwitch47 * _Opacity );
				float AlphaClipThreshold = AlphaClipThreshold;				
				#if _AlphaClip
					clip(Alpha - AlphaClipThreshold);
				#endif
				return Alpha;
				return 0;
		    }
			ENDHLSL
		}
	}		
}