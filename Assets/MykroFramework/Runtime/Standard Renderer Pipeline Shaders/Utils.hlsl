float mapRange(float value, float min1, float max1, float min2, float max2)
{
    float perc = (value - min1) / (max1 - min1);
    return perc * (max2 - min2) + min2;
}

float smoothstep (float edge0, float edge1, float x)
{
    if (x < edge0)
        return 0;
    if (x >= edge1)
        return 1;
    // Scale/bias into [0..1] range
    x = (x - edge0) / (edge1 - edge0);
    return x * x * (3 - 2 * x);
}

float2 rotateCoordinates(float2 coords, float rotation)
{
    float sinX = sin (rotation);
    float cosX = cos (rotation);
    float sinY = sin (rotation);
    float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
    return mul (coords, rotationMatrix);
}