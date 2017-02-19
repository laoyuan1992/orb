float _NoiseFrequency;
float _NoiseOffset;
float _NoiseMagnitude;
float _BaseRadius;

#include "Assets/Shaders/SimplexNoise3D.cginc"

void orb_vert(inout appdata_full v) {
	fixed3 dir = normalize(v.vertex.xyz);
	fixed r = _BaseRadius;
	fixed3 no = fixed3(0, 0, _NoiseOffset);
	fixed rad = _BaseRadius + _NoiseMagnitude + 0.5 + _NoiseMagnitude * snoise(dir * _NoiseFrequency + no);

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
}
