Shader "Custom/GridOverlayShader"
{
    Properties
    {
        _Color ("Color", Color) = (.5,.5,.5,1)
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf(Input IN, inout SurfaceOutput o)
        {
            
            float edgeSize = 0.05; 

            
            float2 edgeDist = fwidth(IN.uv_MainTex);
            float isEdge = step(edgeSize, min(edgeDist.x, edgeDist.y));

            
            fixed4 edgeColor = _Color;

            
            fixed4 fillColor = fixed4(0, 0, 0, 0);

            
            fixed4 finalColor = lerp(fillColor, edgeColor, isEdge);

            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
