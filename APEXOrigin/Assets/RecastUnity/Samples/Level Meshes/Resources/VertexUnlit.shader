// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

//UNlit
//Vertex color
//Lightmap support
//Texture
 
Shader "/Unlit/Vertex Color + Lightmap" {

    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
                //AlphaTest Greater 0.001  // uncomment if you have problems like the sprites or 3d text have white quads instead of alpha pixels.
        Tags {Queue=Transparent}
        Pass
        {
 
            CGPROGRAM
                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
 
                struct v2f
                {
                    fixed4 color : COLOR;
                    fixed4 pos : SV_POSITION;
                    fixed2 lmap : TEXCOORD1;
                    fixed2 pack0 : TEXCOORD0;
                    
 
                };
                sampler2D _MainTex;
                fixed4 _MainTex_ST;
               
                 #ifdef LIGHTMAP_ON
                    // sampler2D unity_Lightmap;
                    //fixed4 unity_LightmapST;
                #endif
                
                v2f vert(appdata_full v)
                 {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                    #ifdef LIGHTMAP_ON
                        o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    #endif
                    o.color = v.color;
                    
                    return o;
                 }
                fixed4 frag(v2f i) : COLOR
                {
                     fixed4 c = tex2D(_MainTex, i.pack0)* i.color;
                    #ifdef LIGHTMAP_ON
                        c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
                    #endif
                    return c;
                 }
             ENDCG
         }
    }
}