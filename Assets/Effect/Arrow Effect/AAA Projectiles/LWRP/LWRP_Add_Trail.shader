Shader "ErbGameArt/LWRP/Particles/Add_Trail"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_StartColor("StartColor", Color) = (1,0,0,1)
		_EndColor("EndColor", Color) = (1,1,0,1)
		_Colorpower("Color power", Float) = 1
		_Colorrange("Color range", Float) = 1
		_Noise("Noise", 2D) = "white" {}
		[Toggle(_USEDEPTH_ON)] _Usedepth("Use depth?", Float) = 0
		_Depthpower("Depth power", Float) = 1
		_Emission("Emission", Float) = 2
		[Toggle(_USEDARK_ON)] _Usedark("Use dark", Float) = 0
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="LightweightPipeline" "PreviewType"="Plane"}
		Cull Off
		HLSLINCLUDE
		#pragma target 3.0
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
			#pragma shader_feature _USEDARK_ON
			#pragma shader_feature _USEDEPTH_ON
		    #include "LWRP/ShaderLibrary/Core.hlsl"
		    #include "LWRP/ShaderLibrary/Lighting.hlsl"
		    #include "CoreRP/ShaderLibrary/Color.hlsl"
		    #include "ShaderGraphLibrary/Functions.hlsl"	
			uniform float4 _StartColor;
			uniform float4 _EndColor;
			uniform float _Colorrange;
			uniform float _Colorpower;
			uniform float _Emission;
			uniform sampler2D _MainTexture;
			uniform float4 _SpeedMainTexUVNoiseZW;
			uniform float4 _MainTexture_ST;
			uniform sampler2D _Noise;
			uniform float4 _Noise_ST;
			uniform sampler2D _CameraDepthTexture;
			uniform float _Depthpower;
					
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
				float2 uv1 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float U6 = uv1.x;
				float clampResult70 = clamp( pow( abs( ( U6 * _Colorrange ) ) , _Colorpower ) , 0.0 , 1.0 );
				float4 lerpResult3 = lerp( _StartColor , _EndColor , clampResult70);
				float4 temp_cast_0 = (1.0).xxxx;
				float2 appendResult32 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
				float2 uv_MainTexture = IN.ase_texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 Main57 = tex2D( _MainTexture, ( ( appendResult32 * _Time.y ) + uv_MainTexture ) );
				float2 uv_Noise = IN.ase_texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 appendResult29 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
				float clampResult44 = clamp( ( pow( abs( ( 1.0 - U6 ) ) , 0.8 ) * 1.0 ) , 0.2 , 0.6 );
				float4 temp_cast_1 = (U6).xxxx;
				float4 clampResult48 = clamp( ( ( tex2D( _Noise, ( uv_Noise + ( _Time.y * appendResult29 ) ) ) + clampResult44 ) - temp_cast_1 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 Dissolve49 = clampResult48;
				float V17 = uv1.y;
				float clampResult24 = clamp( ( (1.0 + (U6 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) * (1.0 + (V17 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) * V17 * 6.0 ) , 0.0 , 1.0 );
				float4 temp_output_51_0 = ( IN.ase_color.a * Main57 * Dissolve49 * clampResult24 );
				#ifdef _USEDARK_ON
				float4 staticSwitch55 = temp_output_51_0;
				#else
				float4 staticSwitch55 = temp_cast_0;
				#endif
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth62 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth62 = abs( ( screenDepth62 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult63 = clamp( distanceDepth62 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float4 staticSwitch65 = ( temp_output_51_0 * clampResult63 );
				#else
				float4 staticSwitch65 = temp_output_51_0;
				#endif
		        float3 Color = ( ( lerpResult3 * IN.ase_color * _Emission ) * staticSwitch55 ).rgb;
		        float Alpha = staticSwitch65.r;
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

			uniform sampler2D _MainTexture;
			uniform float4 _SpeedMainTexUVNoiseZW;
			uniform float4 _MainTexture_ST;
			uniform sampler2D _Noise;
			uniform float4 _Noise_ST;
			uniform sampler2D _CameraDepthTexture;
			uniform float _Depthpower;

			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float4 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct GraphVertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
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
				o.ase_color = v.ase_color;
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_texcoord.zw = 0;
				v.vertex.xyz +=  float3(0,0,0) ;
				v.ase_normal =  v.ase_normal ;
				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag (GraphVertexOutput IN ) : SV_Target
		    {
		    	UNITY_SETUP_INSTANCE_ID(IN);
				float2 appendResult32 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
				float2 uv_MainTexture = IN.ase_texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 Main57 = tex2D( _MainTexture, ( ( appendResult32 * _Time.y ) + uv_MainTexture ) );
				float2 uv_Noise = IN.ase_texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 appendResult29 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
				float2 uv1 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float U6 = uv1.x;
				float clampResult44 = clamp( ( pow( abs( ( 1.0 - U6 ) ) , 0.8 ) * 1.0 ) , 0.2 , 0.6 );
				float4 temp_cast_0 = (U6).xxxx;
				float4 clampResult48 = clamp( ( ( tex2D( _Noise, ( uv_Noise + ( _Time.y * appendResult29 ) ) ) + clampResult44 ) - temp_cast_0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 Dissolve49 = clampResult48;
				float V17 = uv1.y;
				float clampResult24 = clamp( ( (1.0 + (U6 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) * (1.0 + (V17 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) * V17 * 6.0 ) , 0.0 , 1.0 );
				float4 temp_output_51_0 = ( IN.ase_color.a * Main57 * Dissolve49 * clampResult24 );
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth62 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth62 = abs( ( screenDepth62 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult63 = clamp( distanceDepth62 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float4 staticSwitch65 = ( temp_output_51_0 * clampResult63 );
				#else
				float4 staticSwitch65 = temp_output_51_0;
				#endif				
				float Alpha = staticSwitch65.r;
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
	FallBack "Hidden/InternalErrorShader"
}