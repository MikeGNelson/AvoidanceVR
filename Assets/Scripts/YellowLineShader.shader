Shader "Custom/FourCornersBlackEdgesRandomLines"
{
    Properties
    {
        _YellowThickness ("Yellow Segment Thickness", Float) = 0.05
        _BlackThickness ("Black Segment Thickness", Float) = 0.02
        _CornerLength ("Corner Length", Float) = 0.2
        _YellowColor ("Yellow Color", Color) = (1, 1, 0, 1)
        _BlackColor ("Black Color", Color) = (0, 0, 0, 1)
        _LineColor ("Line Color", Color) = (1, 0, 0, 1)
        _LineThickness ("Line Thickness", Float) = 0.01
        _LineDensity ("Line Density (Number of Lines)", Float) = 5.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _YellowThickness;
            float _BlackThickness;
            float _CornerLength;
            fixed4 _YellowColor;
            fixed4 _BlackColor;

            float _LineThickness;
            fixed4 _LineColor;
            float _LineDensity;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Yellow segments for all four corners
                float isYellowHorizontal = ((uv.y <= _YellowThickness) * (uv.x <= _CornerLength)) +  // Top-left corner
                                            ((uv.y <= _YellowThickness) * (uv.x >= 1.0 - _CornerLength)) +  // Top-right corner
                                            ((uv.y >= 1.0 - _YellowThickness) * (uv.x <= _CornerLength)) +  // Bottom-left corner
                                            ((uv.y >= 1.0 - _YellowThickness) * (uv.x >= 1.0 - _CornerLength)); // Bottom-right corner

                float isYellowVertical = ((uv.x <= _YellowThickness) * (uv.y <= _CornerLength)) +  // Top-left corner
                                          ((uv.x >= 1.0 - _YellowThickness) * (uv.y <= _CornerLength)) +  // Top-right corner
                                          ((uv.x <= _YellowThickness) * (uv.y >= 1.0 - _CornerLength)) +  // Bottom-left corner
                                          ((uv.x >= 1.0 - _YellowThickness) * (uv.y >= 1.0 - _CornerLength)); // Bottom-right corner

                float isYellow = saturate(isYellowHorizontal + isYellowVertical);

                // Black edge segments between yellow corners
                float isBlackHorizontal = ((uv.y <= _BlackThickness) * (uv.x > _CornerLength) * (uv.x < 1.0 - _CornerLength)) + // Top edge
                                           ((uv.y >= 1.0 - _BlackThickness) * (uv.x > _CornerLength) * (uv.x < 1.0 - _CornerLength)); // Bottom edge

                float isBlackVertical = ((uv.x <= _BlackThickness) * (uv.y > _CornerLength) * (uv.y < 1.0 - _CornerLength)) + // Left edge
                                         ((uv.x >= 1.0 - _BlackThickness) * (uv.y > _CornerLength) * (uv.y < 1.0 - _CornerLength)); // Right edge

                float isBlack = saturate(isBlackHorizontal + isBlackVertical) * (1.0 - isYellow);

                // Random lines
                // float2 randomSeed = floor(uv * _LineDensity);
                // float linePosition = random(randomSeed); // Random line position
                // float isHorizontalLine = step(linePosition - _LineThickness * 0.5, uv.y) *
                //                          step(uv.y, linePosition + _LineThickness * 0.5); // Horizontal line
                // float isVerticalLine = step(linePosition - _LineThickness * 0.5, uv.x) *
                //                        step(uv.x, linePosition + _LineThickness * 0.5); // Vertical line
                // float isLine = saturate(isHorizontalLine + isVerticalLine);

                // Combine colors
                fixed4 color = _YellowColor * isYellow +
                               _BlackColor * isBlack; 
                               // +  _LineColor * isLine;

                // Apply transparency for non-edge areas
                float alpha = saturate(isYellow + isBlack);// + isLine);
                return fixed4(color.rgb, alpha);
            }
            ENDCG
        }
    }
}
