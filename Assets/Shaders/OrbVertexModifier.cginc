float _NoiseFrequency;
float _NoiseOffset;
float _NoiseMagnitude;
float _BaseRadius;
float _RadiusCurve;
float4 _OrbRotation;

#include "Assets/Shaders/SimplexNoise3D.cginc"

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
float4 qmul(float4 q1, float4 q2)
{
    return float4(
        q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz),
        q1.w * q2.w - dot(q1.xyz, q2.xyz)
    );
}

float4 quat_conj(float4 q)
{ 
	return float4(-q.x, -q.y, -q.z, q.w); 
}

float4 qinv(float q)
{
	float4 inv = quat_conj(q);
	inv /= dot(q, q);
	return inv;
}

float3 rotate_point(float3 position, float4 qr)
{
	float4 qr_conj = quat_conj(qr);
	float4 q_pos = float4(position.x, position.y, position.z, 0);

	float4 q_tmp = qmul(qr, q_pos);
	qr = qmul(q_tmp, qr_conj);

	return float3(qr.x, qr.y, qr.z);
}

void orb_vert(inout appdata_full v) {
	fixed3 dir = normalize(v.vertex.xyz);
	fixed r = _BaseRadius;
	fixed3 no = fixed3(0, 0, _NoiseOffset);
	fixed rad = _BaseRadius + _NoiseMagnitude * 0.5 + _NoiseMagnitude * snoise(dir * _NoiseFrequency + no);

	v.vertex.xyz = dir * rad;

	float len = length(v.vertex.xyz);
	float len32 = pow(len, 3.0);
	float yz = v.vertex.y * v.vertex.y + v.vertex.z * v.vertex.z;
	float xz = v.vertex.x * v.vertex.x + v.vertex.z * v.vertex.z;
	float xy = v.vertex.x * v.vertex.x + v.vertex.y * v.vertex.y;

	fixed3 gradn = snoise_grad(dir * _NoiseFrequency + no);

	v.normal.xyz = normalize(fixed3(
		v.vertex.x / len - gradn.x * _NoiseMagnitude * _NoiseFrequency * yz / len32,
		v.vertex.y / len - gradn.y * _NoiseMagnitude * _NoiseFrequency * xz / len32,
		v.vertex.z / len - gradn.z * _NoiseMagnitude * _NoiseFrequency * xy / len32));
	v.tangent.xyz = cross(v.normal.xyz, fixed3(1, 0, 0));

	v.vertex.xyz = rotate_point(v.vertex.xyz, _OrbRotation);
	v.normal.xyz = rotate_point(v.normal.xyz, _OrbRotation);
	v.tangent.xyz = rotate_point(v.tangent.xyz, _OrbRotation);

	float phi = 3.1415926535 * _RadiusCurve / 180 * rad / r;
	fixed3x3 rot = fixed3x3(
		cos(phi), -sin(phi), 0,
		sin(phi), cos(phi), 0,
		0, 0, 1);

	v.vertex.xyz = mul(v.vertex.xyz, rot);
	v.normal.xyz = mul(v.normal.xyz, rot);
	v.tangent.xyz = mul(v.tangent.xyz, rot);
	v.texcoord.xy = float2(v.vertex.x / len / 2 - 0.5, v.vertex.y / len / 2 - 0.5);
}
