shader "Custom/WaterFall"
{
    Properties
    {
        _MainTex("Base (RGB)",2D)="white"{}
        _ScrollXSpeed("X Scroll Speed",Range(-10,10))=2
        _ScrollYSpeed("Y Scroll Speed",Range(-10,10))=2
    }

    SubShader
    {
        Tags{"RenderType"="Opaque"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed _ScrollXSpeed;
        fixed _ScrollYSpeed;

        struct Input{
            float2 uv_MainTex;
        };

        void surf(Input IN,inout SurfaceOutput o)
        {
            fixed2 scrolledUV = IN.uv_MainTex;

            fixed xscrollValue = _ScrollXSpeed*_Time;
            fixed yscrollValue = _ScrollYSpeed*_Time;

            scrolledUV+=fixed2(xscrollValue,yscrollValue);

            half4 c =tex2D(_MainTex,scrolledUV);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Diffuse"
}