float fresnel(float fresnelExponent, float3 worldNormal, float3 viewDir)
{
    float fresnel = dot(worldNormal, viewDir);
    fresnel = saturate(1 - fresnel);
    fresnel = pow(fresnel, _FresnelExponent);
    return fresnel;
}