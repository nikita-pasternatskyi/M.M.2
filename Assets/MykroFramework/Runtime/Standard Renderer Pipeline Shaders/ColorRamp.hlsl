float ramp(float fac, sampler2D ramp)
{
    return tex2D(ramp, float2(fac, 1)).r;
}