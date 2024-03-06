Shader "UI/TutorialFadeCircle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Aspect ("Aspect", Float) = 1
        _Smoothness ("Smoothness", Range(0, 1)) = 0.01
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 uv       : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
 
            int _HolesLength;
            float _Aspect;
            float _Smoothness;
            float4 _Holes[5];

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.uv       = v.uv;
                o.color    = v.color;

                return o;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float4 color = tex2D(_MainTex, IN.uv) * IN.color;

                for (int i = 0; i < _HolesLength; i++)
                {
                    float4 rect = _Holes[i];

                    const float width = rect.z - rect.x;
                    const float height = rect.w - rect.y;
                    const float radius = max(width, height) / 2;
                    
                    float2 center = float2(rect.x + width/2, rect.y + height/2);

                    const float dx = center.x - IN.uv.x;
                    const float dy = (center.y - IN.uv.y) / _Aspect;

                    const float dist = sqrt(dx * dx + dy * dy) - radius;
                    
                    if (dist < 0)
                        discard;

                    color.a *= smoothstep(dist, 0, _Smoothness);
                }
                
                return color;
            }
            
            ENDCG
        }
    }
}
