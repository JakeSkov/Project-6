Shader "Lambert_Dark"
{
	Properties // Interface between shaders and unity/the inspector
	{
		//Name ("Display Name", Type) = Default()
		_Color("Color",Color) = (1.0, 1.0, 1.0, 1.0)
	}
	Subshader //Start of our shader
	{
		Pass //Layering effects ontop of each other
		{
			tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vertexFunction
			#pragma fragment fragmentFunction
			
			//user defined variables
			uniform float4 _Color;
			
			//Unity fedined variables
			uniform float3 _LightColor0;
			
			//structs
			struct inputInformation //vertex input
			{
				float4 vertexPos : POSITION;
				float3 vertexNormal : NORMAL;
			};
			
			struct passToFragment //vertex output
			{
				float4 position: SV_POSITION;
				float4 colour : COLOR;
			};
			
			//vertex function
			passToFragment vertexFunction (inputInformation input)
			{

				passToFragment output;
				float3 lightDirection;
				float attenuation = 1.0;
				
				float3 ambientLight = UNITY_LIGHTMODEL_AMBIENT.rgb;
				
				//get the direction of the light from unity and normalize
				lightDirection = normalize(_WorldSpaceLightPos0.xyx);
				
				//Grab the normal from input
				float3 tempNorm = input.vertexNormal;
				
				//convert from world to object space
				float3 objectNorm = normalize(mul(float4(tempNorm, 1.0), _World2Object).xyz);
				//float4(float3, alpha) <- how to cast in a shader
				
				//dot product between light direciton and the normal
				float3 diffuseRefleciton = attenuation * _LightColor0.xyz * _Color.rgb * max(0.0, dot(objectNorm, lightDirection));
				
				//calc final light
				float3 finalLight = diffuseRefleciton + ambientLight;
				
				//r,g,b,a
				//output.colour = float4(input.vertexNormal, 1.0);
				output.colour = float4(diffuseRefleciton, 1.0);
				
				output.position = mul(UNITY_MATRIX_MVP, input.vertexPos);
				return output;
			}
			
			//fragment function
			float4 fragmentFunction (passToFragment input) : COLOR
			{
				return input.colour;
			}
			
			ENDCG
		}
	}
	
	//fallback
}
