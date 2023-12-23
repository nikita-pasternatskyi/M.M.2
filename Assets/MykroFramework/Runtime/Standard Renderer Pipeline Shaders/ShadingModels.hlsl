float Specular_Simple(float3 lightDir, float3 viewDir, float3 normal, float specularPower, float specularIntensity, float specularSmoothness, float atten)
{
    float3 reflectDirection = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDirection), 0.0), specularPower);
	spec *= specularIntensity;
    spec = clamp(pow(spec, specularSmoothness), 0,1) * atten;
    return spec;
}

float Diffuse_Light_Simple(float3 normal, float3 lightDir)
{
    float towardsLight = dot(normal, lightDir);
    float rangedLight = towardsLight * 0.5 + 0.5;
    return clamp(rangedLight, 0, 1.0);
}