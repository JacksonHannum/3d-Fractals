Shader "Unlit/Render Fractal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CamPos ("CamPos" , float) = (0,0,0)


        _CamDirX ("CamDirX" , float) = 0
        _CamDirY ("CamDirY" , float) = 0

        _TimePassed ("TimePassed" , float) = 0


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float3 _CamPos;

            float _CamDirX;
            float _CamDirY;

            float _TimePassed;

            float2 rotate(float2 pt , float2 pv , float ang)
            {
                float2 p = pt - pv;
                float s = sin(ang);
                float c = cos(ang);
                p = float2(p.x * c - p.y * s , p.x * s + p.y * c);
                p += pv;
                return p;
            }

            float3 flip(float3 x , float4 plane)
            {

            }

            float4 dist(float px , float py , float pz)
            {
 
                
                float r;
                int n = 0;
                float3 z = float3 (px ,  py , pz);
                
                float s = 3;
                float2 a;

                while (n < 10) 
                {
         
                    //if (z.x < 0) z.x = -z.x;
                    //if (z.y < 0) z.y = -z.y;
                    //if (z.z < 0) z.z = -z.z;

                    /*
                    if(z.x + z.y < 0) z.xy = -z.yx;
                    if(z.x + z.z < 0) z.xz = -z.zx;
                    if(z.y +  z.z < 0) z.zy = -z.yz;
                    z = z * s - 0.5 * (s - 1.0);

                    if p[0] < p[1]:
                        p[[0,1]] = p[[1,0]]
                    if p[0] < p[2]:
                        p[[0,2]] = p[[2,0]]
                    if p[1] < p[2]:
                        p[[2,1]] = p[[1,2]]
                    */

                    z = abs(z);
                    
                    if (z.x < z.y) z.xy = z.yx;
                    if (z.x < z.z) z.xz = z.zx;
                    if (z.y < z.z) z.yz = z.zy;

                    z = z * 3 + float3(-2 , -2 , 0);
                    if (z.z > 1) z.z = 1 + (1 - z.z);

                    a = rotate(float2(z.x , z.y) , 0 , _TimePassed);
                    z.xy = a.xy;

                    a = rotate(float2(z.x , z.z) , 0 , _TimePassed);
                    z.xz = a.xy;

                    a = rotate(float2(z.y , z.z) , 0 , _TimePassed);
                    z.yz = a.xy;
                    

                    //z = z * s - 0.5 * (s - 1);
                    n++;
                }
                
          
                return (float4(z.x , z.y , z.z , (length(z) - 1) * pow(s, -float(n))));


                //return length(z) * pow(Scale, float(-n));
            }
	

	
            

            float trace(float3 s , float2 p , float4 trig)
            {
                float3 r = s;

                float3 d = float3 (p.x * 300 , p.y * 300 , 150);
                d = float3 (d.x , trig.x * d.z + trig.y * d.y , trig.y * d.z - trig.x * d.y);
                d = float3 (trig.z * d.z + trig.w * d.x , d.y , trig.w * d.z - trig.z * d.x);

                d /= length(d);
                float cdist = 100;
                float cn;
                float4 distance;

                float n;
                while (n < 100)
                {
                    distance = dist(r.x , r.y , r.z);
                    if (distance.w < 0.00001)
                    {
                        cdist = -1;
                        break;
                    }
                    if (distance.w < cdist)
                    {
                        cdist = distance.w;
                        cn = n;
                    }
                    r += d * distance.w;
                    n ++;
                }

                if (cdist > 0)
                {
            
                    return (2000000000 * cdist);
                }
                else
                {
                    return (n);
                }  
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float sinx = sin(_CamDirX);
                float cosx = cos(_CamDirX);
                float siny = sin(_CamDirY);
                float cosy = cos(_CamDirY);
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                float cycles = trace(_CamPos , float2 (i.uv.x , i.uv.y) , float4 (sinx , cosx , siny , cosy));

                float4 col = ((float4(0.1 , 0.4 , 0.1 , 1) - (0.005 * cycles)));
                //col = tex2D(_MainTex , float2(n, _Color));   

                return col;
            }
            ENDCG
        }
    }
}
