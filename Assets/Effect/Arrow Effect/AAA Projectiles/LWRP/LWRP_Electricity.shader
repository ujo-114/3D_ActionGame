Shader "ErbGameArt/LWRP/Particles/Blend_Electricity"
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_Dissolveamount("Dissolve amount", Range( 0 , 1)) = 0.332
		_Mask("Mask", 2D) = "white" {}
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		_Emission("Emission", Float) = 6
		_RemapXYFresnelZW("Remap XY/Fresnel ZW", Vector) = (-10,10,2,2)
		_Speed("Speed", Vector) = (0.189,0.225,-0.2,-0.05)
		_Opacity("Opacity", Range( 0 , 1)) = 1
		[Toggle(_USEDEPTH_ON)] _Usedepth("Use depth?", Float) = 0
		_Depthpower("Depth power", Float) = 1
	}
	
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="LightweightPipeline" "PreviewType"="Plane"}
		Cull Back
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
			uniform sampler2D _Mask; uniform float4 _Mask_ST;
			uniform float _Dissolveamount;
			uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
			uniform float4 _Speed;
			uniform float4 _RemapXYFresnelZW;
			uniform float4 _Color;
			uniform float _Emission;
			uniform float _Opacity;
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
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
		        UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
		    };
		
		    GraphVertexOutput vert (GraphVertexInput v )
			{
		        GraphVertexOutput o = (GraphVertexOutput)0;
		        UNITY_SETUP_INSTANCE_ID(v);
		        UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal.xyz);
				o.ase_texcoord2.xyz = ase_worldNormal;				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				v.vertex.xyz +=  float3( 0, 0, 0 ) ;
				v.ase_normal =  v.ase_normal ;
		        o.position = TransformObjectToHClip(v.vertex.xyz);
		        return o;
			}
		
		    half4 frag( GraphVertexOutput IN  ) : SV_Target
		    {
		        UNITY_SETUP_INSTANCE_ID(IN);
				float temp_output_66_0 = (-0.65 + ((1.0 + (_Dissolveamount - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) - 0.0) * (0.65 - -0.65) / (1.0 - 0.0));
				float2 appendResult21 = (float2(_Speed.x , _Speed.y));
				float2 uv29 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult22 = (float2(_Speed.z , _Speed.w));
				float4 texVar1 = tex2D( _MainTexture,  TRANSFORM_TEX((( appendResult21 * _Time.y ) + uv29 ) ,_MainTexture ));
				float4 texVar2 = tex2D( _MainTexture,  TRANSFORM_TEX(( uv29 + ( _Time.y * appendResult22 )) ,_MainTexture ));
				float clampResult72 = clamp( (_RemapXYFresnelZW.x + (( ( temp_output_66_0 + texVar1.r ) * ( temp_output_66_0 + texVar2.r ) ) - 0.0) * (_RemapXYFresnelZW.y - _RemapXYFresnelZW.x) / (1.0 - 0.0)) , 0.0 , 1.0 );
				float2 appendResult74 = (float2((1.0 + (clampResult72 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) , 0.0));
				float clampResult75 = clamp( tex2D( _Mask,TRANSFORM_TEX( appendResult74 ,_Mask )).r , 0.0 , 1.0 );
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float fresnelNdotV83 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode83 = ( 0.0 + 1.0 * pow( abs(1.0 - fresnelNdotV83), _RemapXYFresnelZW.z ) );
				float clampResult78 = clamp( ( _RemapXYFresnelZW.w * fresnelNode83 ) , 0.0 , 1.0 );		
				float temp_output_61_0 = ( clampResult75 * _Color.a * IN.ase_color.a * clampResult78 * _Opacity );
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth49 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth49 = abs( ( screenDepth49 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult53 = clamp( distanceDepth49 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float staticSwitch47 = ( temp_output_61_0 * clampResult53 );
				#else
				float staticSwitch47 = temp_output_61_0;
				#endif
		        float3 Color = ( clampResult75 * _Color * IN.ase_color * clampResult78 * _Emission ).rgb;
		        float Alpha = staticSwitch47;
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
			uniform sampler2D _Mask;
			uniform float _Dissolveamount;
			uniform sampler2D _MainTexture;
			uniform float4 _Speed;
			uniform float4 _RemapXYFresnelZW;
			uniform float4 _Color;
			uniform float _Opacity;
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
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			GraphVertexOutput vert (GraphVertexInput v)
			{
				GraphVertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal.xyz);
				o.ase_texcoord2.xyz = ase_worldNormal;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				v.vertex.xyz +=  float3(0,0,0) ;
				v.ase_normal =  v.ase_normal ;
				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag (GraphVertexOutput IN ) : SV_Target
		    {
		    	UNITY_SETUP_INSTANCE_ID(IN);
				float temp_output_66_0 = (-0.65 + ((1.0 + (_Dissolveamount - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) - 0.0) * (0.65 - -0.65) / (1.0 - 0.0));
				float2 appendResult21 = (float2(_Speed.x , _Speed.y));
				float2 uv29 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult22 = (float2(_Speed.z , _Speed.w));
				float clampResult72 = clamp( (_RemapXYFresnelZW.x + (( ( temp_output_66_0 + tex2D( _MainTexture, ( ( appendResult21 * _Time.y ) + uv29 ) ).r ) * ( temp_output_66_0 + tex2D( _MainTexture, ( uv29 + ( _Time.y * appendResult22 ) ) ).r ) ) - 0.0) * (_RemapXYFresnelZW.y - _RemapXYFresnelZW.x) / (1.0 - 0.0)) , 0.0 , 1.0 );
				float2 appendResult74 = (float2((1.0 + (clampResult72 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) , 0.0));
				float clampResult75 = clamp( tex2D( _Mask, appendResult74 ).r , 0.0 , 1.0 );
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float fresnelNdotV83 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode83 = ( 0.0 + 1.0 * pow( abs(1.0 - fresnelNdotV83), _RemapXYFresnelZW.z ) );
				float clampResult78 = clamp( ( _RemapXYFresnelZW.w * fresnelNode83 ) , 0.0 , 1.0 );
				float temp_output_61_0 = ( clampResult75 * _Color.a * IN.ase_color.a * clampResult78 * _Opacity );
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth49 = LinearEyeDepth(tex2Dproj( _CameraDepthTexture, screenPos ).r,_ZBufferParams);
				float distanceDepth49 = abs( ( screenDepth49 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Depthpower ) );
				float clampResult53 = clamp( distanceDepth49 , 0.0 , 1.0 );
				#ifdef _USEDEPTH_ON
				float staticSwitch47 = ( temp_output_61_0 * clampResult53 );
				#else
				float staticSwitch47 = temp_output_61_0;
				#endif				
				float Alpha = staticSwitch47;
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