Shader "Custom/TriplanarMapping" {
    Properties {
        _MainTexX ("X Texture", 2D) = "white" {}
        _MainTexY ("Y Texture", 2D) = "white" {}
        _MainTexZ ("Z Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float2 uv_MainTexX;
            float2 uv_MainTexY;
            float2 uv_MainTexZ;
            float3 worldPos;
            float3 normal;
        };

        sampler2D _MainTexX;
        sampler2D _MainTexY;
        sampler2D _MainTexZ;

        void surf (Input IN, inout SurfaceOutput o) {
            // Calculate blending factors based on the surface normal
            float blendX = abs(IN.normal.x);
            float blendY = abs(IN.normal.y);
            float blendZ = abs(IN.normal.z);

            // Sample the textures using world-space position
            fixed4 texX = tex2D(_MainTexX, IN.worldPos.xz);
            fixed4 texY = tex2D(_MainTexY, IN.worldPos.yz);
            fixed4 texZ = tex2D(_MainTexZ, IN.worldPos.xy);

            // Blend the textures based on the blending factors
            fixed4 finalColor = blendX * texX + blendY * texY + blendZ * texZ;

            // Assign the final color to the surface output
            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
