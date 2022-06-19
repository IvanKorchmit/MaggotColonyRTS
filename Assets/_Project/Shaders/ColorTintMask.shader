Shader "Custom/ColorTintMask"
{
    Properties
    {
        _TintR ("Red Tint", Color) = (1,1,1,1)
        _TintG ("Green Tint", Color) = (1,1,1,1)
        _TintB ("Blue Tint", Color) = (1,1,1,1)
        _TintA ("Alpha Tint", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
            
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _MaskTex;

        struct Input
        {
            float2 uv_MainTex, uv_MaskTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _TintR;
        fixed4 _TintG;
        fixed4 _TintB;
        fixed4 _TintA;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 mask = tex2D(_MaskTex, IN.uv_MainTex);
            
            float fullMask = saturate(mask.r + mask.g + mask.b + mask.a);
            fixed3 noTint = c.rgb * (1 - fullMask);
            
            fixed3 tintR = c.rgb * saturate(_TintR.rgb * mask.r);
            fixed3 tintG = c.rgb * saturate(_TintG.rgb * mask.g);
            fixed3 tintB = c.rgb * saturate(_TintB.rgb * mask.b);
            fixed3 tintA = c.rgb * saturate(_TintA.rgb * mask.a);

            o.Albedo = saturate(noTint + tintR + tintG + tintB + tintA);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
