using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {

	[Range(0, 5)]
	public float speed = 1f;

	public float minFreq = 0.31f;
	public float maxFreq = 6f;
	public float freqPeriod = 15f;

	private float offset = 0f;
	private Material mat;

	private float freq;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().sharedMaterial;
		offset = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		offset += speed * Time.deltaTime;
		freq = (minFreq + maxFreq) / 2 - Mathf.Cos(Time.time * 2 * Mathf.PI / freqPeriod) * (maxFreq - minFreq) / 2;
		mat.SetFloat("_NoiseOffset", offset);
		mat.SetFloat("_NoiseFrequency", freq);
	}
}
