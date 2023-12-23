float3 BlendMode_LinearLight(float3 base, float3 blend)
{
    float3 color;
    blend = 2 * (blend - 0.5);
    color = min(base + blend, 1);

    return color;
}

float BlendMode_Multiply(float base, float blend, float factor)
{
    return lerp(base, base * blend, factor);
}

float BlendMode_Overlay(float base, float blend)
{
    return (base <= 0.5) ? 2*base*blend : 1 - 2*(1-base)*(1-blend);
}

float3 BlendMode_Overlay(float3 base, float3 blend)
{
    return float3(  BlendMode_Overlay(base.r, blend.r), 
			        BlendMode_Overlay(base.g, blend.g), 
			        BlendMode_Overlay(base.b, blend.b) );
}