Shader "UI/TutorialFade"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Aspect ("Aspect", Float) = 1
        _Smoothness ("Smoothness", Range(0, 1)) = 0.01

        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        LOD 0
        Tags
        {
            "Queue"             = "Transparent" 
            "IgnoreProjector"   = "True" 
            "RenderType"        = "Transparent"
            "PreviewType"       = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Stencil
        {
            Ref        [_Stencil]
            ReadMask   [_StencilReadMask]
            WriteMask  [_StencilWriteMask]
            Comp       [_StencilComp]
            Pass       [_StencilOp]
        }

        Cull       Off
        Lighting   Off
        ZWrite     Off
        ZTest      [unity_GUIZTestMode]
        Blend      SrcAlpha OneMinusSrcAlpha
        ColorMask  [_ColorMask]

        Pass
        {
            Name "Default"
            
            CGPROGRAM
            #ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
            #endif
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target   3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

            struct v2f
			{
				float4 vertex        : SV_POSITION;
				fixed4 color         : COLOR;
				half2 texcoord       : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            uniform sampler2D _MainTex;

            uniform int    _HolesLength;
            uniform float  _Aspect;
            uniform float  _Smoothness;
            uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
            uniform float4 _ClipRect;
            uniform float4 _Holes[5];

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                OUT.worldPosition = IN.vertex;

                OUT.worldPosition.xyz += float3(0, 0, 0);

                OUT.vertex   = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = IN.texcoord;
                OUT.color    = IN.color * _Color;

                return OUT;
            }

            void Discard(float2 uv)
            {
                for (int i = 0; i < _HolesLength; i++)
                {
                    if (uv.x >= _Holes[i].x && uv.x <= _Holes[i].z && uv.y >= _Holes[i].y && uv.y <= _Holes[i].w)
                        discard;
                }
            }

            float GetSqrDistance(float2 uv, float4 rect)
            {
                float width  = rect.z - rect.x;
                float height = rect.w - rect.y;

                float2 center = float2(rect.x + width / 2, rect.y + height / 2);

                float dx = max(abs(uv.x - center.x) - width / 2, 0);
                float dy = max(abs(uv.y - center.y) - height / 2, 0) / _Aspect;

                return dx * dx + dy * dy;
            }

            float4 GetFadeColor(float2 uv, float4 color)
            {
                float alpha = color.a;

                for (int i = 0; i < _HolesLength; i++)
                    alpha *= smoothstep(GetSqrDistance(uv, _Holes[i]), 0, _Smoothness * _Smoothness);

                color.a = alpha;

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
                float4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
                #endif

                Discard(IN.texcoord);

                color = GetFadeColor(IN.texcoord, color);

                return color;
            }
            ENDCG
        }
    }
	Fallback Off
}