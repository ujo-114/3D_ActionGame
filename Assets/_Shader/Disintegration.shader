Shader "LSQ/Geomtry Shader/Disintegration"
{
    Properties
    {
        [Header(Base)]
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Specular("Specular", Color) = (1,1,1,1)
        _Gloss("Gloss", Range(8, 256)) = 20 //高光区域大小

        [Header(Dissolve)]
        _DissolveTexture("Dissolve Texutre", 2D) = "white" {}
        _DissolveBorderColor("Dissolve Border Color", Color) = (1, 1, 1, 1)
        _DissolveBorderWidth("Dissolve Border", float) = 0.05
        _Weight("Weight", Range(0, 1)) = 0

        [Header(Particle)]
        _Direction("Direction", Vector) = (0, 0, 0, 0)
        _FlowMap("Flow (RG)", 2D) = "black" {}
        _Exapnd("Expand", float) = 1
        _ParticleShape("Shape", 2D) = "white" {}
        _ParticleRadius("Radius", float) = .1
        [Toggle]_ParticleLit("Use Lit", int) = 1
        [HDR]_ParticleColor("Particle Color", Color) = (1, 1, 1, 1)
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                Tags
                {
                    "LightMode" = "ForwardBase"
                }
                Cull Off


                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma geometry geom

                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2g
                {
                    float4 objPos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct g2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                    float4 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                fixed4 _Specular;
                float _Gloss;

                sampler2D _DissolveTexture;
                float4 _DissolveBorderColor;
                float _DissolveBorderWidth;
                float _Weight;

                sampler2D _FlowMap;
                float4 _FlowMap_ST;
                float _Exapnd;
                float4 _Direction;

                sampler2D _ParticleShape;
                float _ParticleRadius;
                float4 _ParticleColor;
                int _ParticleLit;

                 v2g vert(appdata v)
                 {
                    v2g o;
                    o.objPos = v.vertex;
                    o.uv = v.uv;
                    o.normal = UnityObjectToWorldNormal(v.normal);
                    return o;
                }

                float random(float2 uv)
                {
                    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
                }

                float remap(float value, float from1, float to1, float from2, float to2)
                {
                    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
                }

                float randomMapped(float2 uv, float from, float to)
                {
                    return remap(random(uv), 0, 1, from, to);
                }

                float4 remapFlowTexture(float4 tex)
                {
                    return float4(
                        remap(tex.x, 0, 1, -1, 1),
                        remap(tex.y, 0, 1, -1, 1),
                        0,
                        remap(tex.w, 0, 1, -1, 1)
                    );
                }

                [maxvertexcount(7)] //粒子的Quad + 原本的Triangle
                void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
                {
                    float2 avgUV = (IN[0].uv + IN[1].uv + IN[2].uv) / 3;
                    float3 avgPos = (IN[0].objPos + IN[1].objPos + IN[2].objPos) / 3;
                    float3 avgNormal = (IN[0].normal + IN[1].normal + IN[2].normal) / 3;

                    //随噪声溶解效果的消散
                    float dissolve_value = tex2Dlod(_DissolveTexture, float4(avgUV, 0, 0)).r;
                    float t = clamp(_Weight * 2 - dissolve_value, 0 , 1);
                    //产生更多的随机性
                    float2 flowUV = TRANSFORM_TEX(mul(unity_ObjectToWorld, avgPos).xz, _FlowMap);
                    float4 flowVector = remapFlowTexture(tex2Dlod(_FlowMap, float4(flowUV, 0, 0)));
                    float3 pseudoRandomPos = (avgPos)+_Direction;
                    pseudoRandomPos += (flowVector.xyz * _Exapnd);
                    //控制位置和大小随消散效果变化
                    float3 p = lerp(avgPos, pseudoRandomPos, t);
                    float radius = lerp(_ParticleRadius, 0, t);
                    //构建一个广告牌QUAD
                    if (t > 0)
                    {
                        //让消散的粒子始终朝向摄像机,使用摄像机空间的坐标轴
                        float3 right = UNITY_MATRIX_IT_MV[0].xyz;
                        float3 up = UNITY_MATRIX_IT_MV[1].xyz;
                        //得到QUAD四个点的位置（沿着摄像机空间的坐标轴偏移）
                        float4 v[4];
                        v[0] = float4(p + radius * right - radius * up, 1.0f);
                        v[1] = float4(p + radius * right + radius * up, 1.0f);
                        v[2] = float4(p - radius * right - radius * up, 1.0f);
                        v[3] = float4(p - radius * right + radius * up, 1.0f);
                        //构建QUAD的顶点：位置和UV一一对应
                        g2f vert;
                        vert.pos = UnityObjectToClipPos(v[0]);
                        vert.uv = float2(1.0f, 0.0f);
                        vert.normal = UnityObjectToWorldNormal(avgNormal);
                        vert.worldPos = mul(unity_ObjectToWorld, v[0]);
                        triStream.Append(vert);

                        vert.pos = UnityObjectToClipPos(v[1]);
                        vert.uv = float2(1.0f, 1.0f);
                        vert.normal = UnityObjectToWorldNormal(avgNormal);
                        vert.worldPos = mul(unity_ObjectToWorld, v[1]);
                        triStream.Append(vert);

                        vert.pos = UnityObjectToClipPos(v[2]);
                        vert.uv = float2(0.0f, 0.0f);
                        vert.normal = UnityObjectToWorldNormal(avgNormal);
                        vert.worldPos = mul(unity_ObjectToWorld, v[2]);
                        triStream.Append(vert);

                        vert.pos = UnityObjectToClipPos(v[3]);
                        vert.uv = float2(0.0f, 1.0f);
                        vert.normal = UnityObjectToWorldNormal(avgNormal);
                        vert.worldPos = mul(unity_ObjectToWorld, v[3]);
                        triStream.Append(vert);

                        triStream.RestartStrip();
                    }

                    //原本的顶点，并把世界坐标的w设置为-1作为区分
                    for (int j = 0; j < 3; j++)
                    {
                        g2f o;
                        o.pos = UnityObjectToClipPos(IN[j].objPos);
                        o.uv = TRANSFORM_TEX(IN[j].uv, _MainTex);
                        o.normal = UnityObjectToWorldNormal(IN[j].normal);
                        o.worldPos = float4(mul(unity_ObjectToWorld, IN[j].objPos).xyz, -1);
                        triStream.Append(o);
                    }

                    triStream.RestartStrip();
                }

                fixed4 frag(g2f i) : SV_Target
                {
                    //光照
                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                    fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
                    fixed3 diffuse = _LightColor0.rgb * saturate(dot(i.normal, lightDir));
                    fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
                    fixed3 halfDir = normalize(lightDir + viewDir);
                    fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(i.normal, halfDir)), _Gloss);

                    fixed4 col = 1;

                    if (i.worldPos.w < 0) //人物
                    {
                        float dissolve = tex2D(_DissolveTexture, i.uv).r;
                        clip(dissolve - 2 * _Weight);

                        col = tex2D(_MainTex, i.uv) * _Color;
                        col.rgb = ambient * col.rgb + diffuse * col.rgb + specular;
                        if (_Weight > 0)
                        {
                            col += _DissolveBorderColor * step(dissolve - 2 * _Weight, _DissolveBorderWidth);
                        }
                    }
                    else //消散粒子
                    {
                        col.rgb = lerp(_ParticleColor.rgb, ambient * _ParticleColor.rgb + diffuse * _ParticleColor.rgb + specular, _ParticleLit);
                        float s = tex2D(_ParticleShape, i.uv).r;
                        clip(s - 0.5);
                    }

                    return col;
                }

                ENDCG
            }
        }
}
