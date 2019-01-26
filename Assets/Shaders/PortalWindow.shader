Shader "Portal/PortalWindow"
{
	SubShader
	{
	    ZWrite Off
	    ColorMask 0
	    Cull Off
	    
	    
	    
		Pass
		{
			Stencil{
	             Ref 1
	             Comp always
	             Pass replace
	        }
			
		}
	}
}
