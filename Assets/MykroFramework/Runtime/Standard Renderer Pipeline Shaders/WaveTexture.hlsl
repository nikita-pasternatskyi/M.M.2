float _Constant_PI = 3.1415926;

float noise_perlin(float3 P) {
    float3 Pi0 = floor(P);
    float3 Pi1 = Pi0 + float3(1.0, 1.0, 1.0);
    Pi0 = Pi0 - floor(Pi0 * (1.0 / 289.0)) * 289.0;
    Pi1 = Pi1 - floor(Pi1 * (1.0 / 289.0)) * 289.0;
    float3 Pf0 = frac(P);
    float3 Pf1 = Pf0 - float3(1.0, 1.0, 1.0);
    float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
    float4 iy = float4(Pi0.yy, Pi1.yy);
    float4 iz0 = float4(Pi0.z, Pi0.z, Pi0.z, Pi0.z);
    float4 iz1 = float4(Pi1.z, Pi0.z, Pi0.z, Pi0.z);

    float4 ixy = (((((((ix * 34.0) + 1.0) * ix) - floor(((ix * 34.0) + 1.0) * ix * (1.0 / 289.0)) * 289.0 + iy) * 34.0) + 1.0) *
        ((((ix * 34.0) + 1.0) * ix) - floor(((ix * 34.0) + 1.0) * ix * (1.0 / 289.0)) * 289.0 + iy)) -
        floor(((((((ix * 34.0) + 1.0) * ix) - floor(((ix * 34.0) + 1.0) * ix * (1.0 / 289.0)) * 289.0 + iy) * 34.0) + 1.0) *
            ((((ix * 34.0) + 1.0) * ix) - floor(((ix * 34.0) + 1.0) * ix * (1.0 / 289.0)) * 289.0 + iy) * (1.0 / 289.0)) * 289.0;
    float4 ixy0 = ((((ixy + iz0) * 34.0) + 1.0) * (ixy + iz0)) - floor((((ixy + iz0) * 34.0) + 1.0) * (ixy + iz0) * (1.0 / 289.0)) * 289.0;
    float4 ixy1 = ((((ixy + iz1) * 34.0) + 1.0) * (ixy + iz1)) - floor((((ixy + iz1) * 34.0) + 1.0) * (ixy + iz1) * (1.0 / 289.0)) * 289.0;

    float4 gx0 = ixy0 * (1.0 / 7.0);
    float4 gy0 = frac(floor(gx0) * (1.0 / 7.0)) - 0.5;
    gx0 = frac(gx0);
    float4 gz0 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx0) - abs(gy0);
    float4 sz0 = step(gz0, float4(0.0, 0.0, 0.0, 0.0));
    gx0 -= sz0 * (step(0.0, gx0) - 0.5);
    gy0 -= sz0 * (step(0.0, gy0) - 0.5);

    float4 gx1 = ixy1 * (1.0 / 7.0);
    float4 gy1 = frac(floor(gx1) * (1.0 / 7.0)) - 0.5;
    gx1 = frac(gx1);
    float4 gz1 = float4(0.5, 0.5, 0.5, 0.5) - abs(gx1) - abs(gy1);
    float4 sz1 = step(gz1, float4(0.0, 0.0, 0.0, 0.0));
    gx1 -= sz1 * (step(0.0, gx1) - 0.5);
    gy1 -= sz1 * (step(0.0, gy1) - 0.5);

    float3 g000 = float3(gx0.x, gy0.x, gz0.x);
    float3 g100 = float3(gx0.y, gy0.y, gz0.y);
    float3 g010 = float3(gx0.z, gy0.z, gz0.z);
    float3 g110 = float3(gx0.w, gy0.w, gz0.w);
    float3 g001 = float3(gx1.x, gy1.x, gz1.x);
    float3 g101 = float3(gx1.y, gy1.y, gz1.y);
    float3 g011 = float3(gx1.z, gy1.z, gz1.z);
    float3 g111 = float3(gx1.w, gy1.w, gz1.w);

    float4 norm0 = 1.79284291400159 - 0.85373472095314 * float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110));
    g000 *= norm0.x;
    g010 *= norm0.y;
    g100 *= norm0.z;
    g110 *= norm0.w;
    float4 norm1 = 1.79284291400159 - 0.85373472095314 * float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111));
    g001 *= norm1.x;
    g011 *= norm1.y;
    g101 *= norm1.z;
    g111 *= norm1.w;

    float n000 = dot(g000, Pf0);
    float n100 = dot(g100, float3(Pf1.x, Pf0.yz));
    float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
    float n110 = dot(g110, float3(Pf1.xy, Pf0.z));
    float n001 = dot(g001, float3(Pf0.xy, Pf1.z));
    float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
    float n011 = dot(g011, float3(Pf0.x, Pf1.yz));
    float n111 = dot(g111, Pf1);

    float3 fade_xyz = Pf0 * Pf0 * Pf0 * (Pf0 * (Pf0 * 6.0 - 15.0) + 10.0);
    float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
    float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
    float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
    return 2.2 * n_xyz;
}

float snoise(float3 p)
{
    float r = noise_perlin(p);
    return (isinf(r)) ? 0.0 : 0.2500 * r;
}

float noise(float3 p)
{
    return 0.5 * snoise(p) + 0.5;
}

float fractal_noise(float3 p, float octaves, float roughness)
{
    float fscale = 1.0;
    float amp = 1.0;
    float maxamp = 0.0;
    float sum = 0.0;
    octaves = clamp(octaves, 0.0, 15.0);
    int n = int(octaves);
    for (int i = 0; i <= n; i++) {
        float t = noise(fscale * p);
        sum += t * amp;
        maxamp += amp;
        amp *= clamp(roughness, 0.0, 1.0);
        fscale *= 2.0;
    }
    float rmd = octaves - floor(octaves);
    if (rmd != 0.0) {
        float t = noise(fscale * p);
        float sum2 = sum + t * amp;
        sum /= maxamp;
        sum2 /= maxamp + amp;
        return (1.0 - rmd) * sum + rmd * sum2;
    }
    else {
        return sum / maxamp;
    }
}

float calc_wave(float3 p,
    float distortion,
    float detail,
    float detail_scale,
    float detail_roughness,
    float phase,
    int wave_type,
    int bands_dir,
    int rings_dir,
    int wave_profile)
{
    /* Prevent precision issues on unit coordinates. */
    p = (p + 0.000001) * 0.999999;

    float n;

    if (wave_type == 0) {   /* type bands */
        if (bands_dir == 0) { /* X axis */
            n = p.x * 20.0;
        }
        else if (bands_dir == 1) { /* Y axis */
            n = p.y * 20.0;
        }
        else if (bands_dir == 2) { /* Z axis */
            n = p.z * 20.0;
        }
        else { /* Diagonal axis */
            n = (p.x + p.y + p.z) * 10.0;
        }
    }
    else { /* type rings */
        float3 rp = p;
        if (rings_dir == 0) { /* X axis */
            rp *= float3(0.0, 1.0, 1.0);
        }
        else if (rings_dir == 1) { /* Y axis */
            rp *= float3(1.0, 0.0, 1.0);
        }
        else if (rings_dir == 2) { /* Z axis */
            rp *= float3(1.0, 1.0, 0.0);
        }
        /* else: Spherical */

        n = length(rp) * 20.0;
    }

    n += phase;

    if (distortion != 0.0) {
        n += distortion * (fractal_noise(p * detail_scale, detail, detail_roughness) * 2.0 - 1.0);
    }

    if (wave_profile == 0) { /* profile sin */
        return 0.5 + 0.5 * sin(n - (_Constant_PI / 2.0));
    }
    else if (wave_profile == 1) { /* profile saw */
        n /= 2.0 * _Constant_PI;
        return n - floor(n);
    }
    else { /* profile tri */
        n /= 2.0 * _Constant_PI;
        return abs(n - floor(n + 0.5)) * 2.0;
    }
}

void node_tex_wave(float3 co,
    float scale,
    float distortion,
    float detail,
    float detail_scale,
    float detail_roughness,
    float phase,
    float wave_type,
    float bands_dir,
    float rings_dir,
    float wave_profile,
    out float3 color,
    out float fac)
{
    float f;
    f = calc_wave(co * scale,
        distortion,
        detail,
        detail_scale,
        detail_roughness,
        phase,
        int(wave_type),
        int(bands_dir),
        int(rings_dir),
        int(wave_profile));

    color = float3(f, f, f);
    fac = f;
}