// Evan Pezent | evanpezent.com | epezent@rice.edu
// 08/2017

// USEFUL LINKS
// http://nitishparadkar.com/post/simple-gradient-shader/
// http://blog.sinclairtarget.com/a-simple-gradient-shader/
// https://docs.unity3d.com/430/Documentation/Components/SL-VertexFragmentShaderExamples.html

// Shader Folder/Name
Shader "ShapeRenderer/FillRadialGradient"
{
    Properties
    {
        [PerRendererData] _MainTex("Texture", 2D) = "white" {}
        [PerRendererData] _TileOff("Tiling/Offset", Vector) = (1, 1, 0, 0)
        [PerRendererData] _Color1("Color 1", Color) = (1, 1, 1, 1)
        [PerRendererData] _Color2("Color 2", Color) = (1, 1, 1, 1)
        [PerRendererData] _Angle("Angle", Float) = 0.0
        [PerRendererData] _Slider1("Slider1", Float) = 0.0
        [PerRendererData] _Slider2("Slider2", Float) = 0.0
    }

    SubShader
    {

        // SORTING LAYERS COMPATIBILITY
        Tags
        {
            "Queue" = "Transparent" // important
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off // important
        Blend One OneMinusSrcAlpha

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            // BEGIN HLSL PROGRAM SNIPPET
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct vertexIn {
                float4 pos : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinates
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f {
                float4 pos : SV_POSITION; // clip space position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // texture to sample
            sampler2D _MainTex;
            float4 _TileOff;

            fixed4 SampleTexture(float2 uv) {
                fixed4 color = tex2D(_MainTex, uv);
                return color;
            }

            // VERTEX SHADER
            // runs on each vertex of mesh and transforms vertex position
            // from object space to "clip space"
            v2f vert(vertexIn input)
            {
                v2f output;

                output.pos = UnityObjectToClipPos(input.pos);
                output.uv = input.uv;
                return output;
            }

            fixed4 _Color1, _Color2;
            float _Slider1, _Slider2;

            // FRAGMENT SHADER
            // runs on each pixel that object occupies on-screen
            // returns a low precision color {r, g, b, a}
            float4 frag(v2f input) : COLOR
            {
                // rotation about the center (0.5, 0.5)
                float x = input.uv.x - 0.5 * (1 + _Slider1);
                float y = input.uv.y - 0.5 * (1 + _Slider2);
                float t = sqrt(x*x + y* y) * 2;
                t = max(0, t);
                t = min(1, t);

                float4 color = float4(0.0,0.0,0.0,1.0);
                color.r = (_Color1.r - _Color2.r) * t + _Color2.r;
                color.g = (_Color1.g - _Color2.g) * t + _Color2.g;
                color.b = (_Color1.b - _Color2.b) * t + _Color2.b;
                color.a = (_Color1.a - _Color2.a) * t + _Color2.a;
                color *= SampleTexture(input.uv *_TileOff.xy + _TileOff.zw);

                return color;
            }
            ENDCG // END HLSL PROGRAM SNIPPET
        }
    }
}