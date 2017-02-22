using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Orb : MonoBehaviour {

	public Light _light;

	public float speed = 1f;

	public float minFreq = 0.31f;
	public float maxFreq = 6f;
	public float freqPeriod = 25f;

	public float curvePeriod = 25f;

	[Range(0, 1)]
	public float lightHue = 0.0f;
	[Range(0, 1)]
	public float lightSat = 0.5f;
	[Range(0, 1)]
	public float lightBright = 1.0f;

	public float lightHuePeriod = 20f;

	public float rotationY = 0f;
	public float rotationX = 0f;
	public float rotationZ = 0f;

	public float maxRotationSpeed = 60.0f;
	public float rotationPeriod = 30f;

	private float offset = 0f;
	private Material mat;

	public float freq;

	private Color lightColor;

	private Quaternion orbRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().sharedMaterial;
		offset = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		offset += speed * Time.deltaTime;
//		freq = (minFreq + maxFreq) / 2 - Mathf.Cos(Time.time * 2 * Mathf.PI / freqPeriod) * (maxFreq - minFreq) / 2;
//		float curve = 90 * Mathf.Sin(Time.time * 2 * Mathf.PI / curvePeriod);
		mat.SetFloat("_NoiseOffset", offset);
		mat.SetFloat("_NoiseFrequency", freq);
//		mat.SetFloat("_RadiusCurve", curve);


//		lightHue = Mathf.Repeat(Time.time / lightHuePeriod, 1.0f);
		lightColor = Color.HSVToRGB(lightHue, lightSat, lightBright);
		_light.color = lightColor;
		mat.SetColor("_V_WIRE_Color", lightColor);

//		rotationY = maxRotationSpeed * Mathf.Sin(Time.time * 2 * Mathf.PI / rotationPeriod);
//		rotationX = maxRotationSpeed * Mathf.Cos(Time.time * 2 * Mathf.PI / rotationPeriod);
		orbRotation = Quaternion.Euler(new Vector3(rotationX, rotationY, rotationZ) * Time.deltaTime) * orbRotation;

		mat.SetVector("_OrbRotation", new Vector4(orbRotation.x, orbRotation.y, orbRotation.z, orbRotation.w));
	}
}
