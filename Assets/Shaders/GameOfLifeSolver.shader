Shader "Unlit/GameOfLifeSolver"
{
    SubShader
     {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            fixed4 SampleNeighbour(v2f_customrendertexture IN, int x, int y) : COLOR
            {
                return tex2D(_SelfTexture2D, IN.globalTexcoord + float2(x / _CustomRenderTextureWidth, y / _CustomRenderTextureHeight));
            }

            bool IsAlive(float4 color)
            {
                return color.r > 0.5;
            }

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                fixed4 self = SampleNeighbour(IN, 0,0);
                
                int neigbours = SampleNeighbour(IN, -1, -1).r + SampleNeighbour(IN, 0, -1).r + SampleNeighbour(IN, 1, -1).r +
                                SampleNeighbour(IN, -1,  0).r +             0                + SampleNeighbour(IN, 1,  0).r +
                                SampleNeighbour(IN, -1,  1).r + SampleNeighbour(IN, 0,  1).r + SampleNeighbour(IN, 1,  1).r;
                
                if(IsAlive(self))
                {
                    if(neigbours == 2 || neigbours == 3)
                    {
                        self = fixed4(1,1,1,1);
                    }
                    else
                    {
                        self = fixed4(0,0,0,1);
                    }
                }
                else
                {
                    if(neigbours == 3)
                    {
                       self = fixed4(1,1,1,1); 
                    }
                    else
                    {
                        self = fixed4(0,0,0,1);
                    }
                }
            
                return self;
            }
            
            ENDCG
        }
    }
}