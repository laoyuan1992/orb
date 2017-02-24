using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Orb : MonoBehaviour {

	[SerializeField]
	private Light _light;

	[SerializeField]
	private float _evolution = 1f;
	public float evolution {
		get { return _evolution; }
		set { _evolution = value; }
	}

	private float _magnitude = 1.67f;
	public float magnitude {
		get { return _magnitude; }
		set { _magnitude = value; }
	}

	private float _radius = 6.7f;
	public float radius {
		get { return _radius; }
		set { _radius = value; }
	}

	[SerializeField, Range(0, 1)]
	private float _lightHue = 0.0f;

	public float lightHue {
		get { return _lightHue; }
		set { _lightHue = value % 1.0f; }
	}

	[SerializeField, Range(0, 1)]
	private float _lightSat = 0.5f;

	public float lightSat {
		get { return _lightSat; }
		set { _lightSat = Mathf.Clamp01(value); }
	}

	[SerializeField, Range(0, 1)]
	private float _lightBright = 1.0f;
	public float lightBright {
		get { return _lightBright; }
		set { _lightBright = Mathf.Clamp01(value); }
	}

	[SerializeField, Range(-90, 90)]
	private float _rotationY = 0f;
	public float rotationY {
		get { return _rotationY; }
		set { _rotationY = value; }
	}

	[SerializeField, Range(-90, 90)]
	private float _rotationX = 0f;
	public float rotationX {
		get { return _rotationX; }
		set { _rotationX = value; }
	}

	[SerializeField, Range(-90, 90)]
	private float _rotationZ = 0f;
	public float rotationZ {
		get { return _rotationZ; }
		set { _rotationZ = value; }
	}

	[SerializeField, Range(0, 6)]
	private float _frequency = 0f;
	public float frequency {
		get { return _frequency; }
		set { _frequency = value; }
	}

	[SerializeField, Range(-90, 90)]
	private float _curve = 0f;
	public float curve {
		get { return _curve; }
		set { _curve = value; }
	}

	[SerializeField, Range(0, 1)]
	private float _textureCrossfade = 0f;
	public float textureCrossfade {
		get { return _textureCrossfade; }
		set { _textureCrossfade = value; }
	}

	private float _scaleX = 1f;
	public float scaleX {
		get { return _scaleX; }
		set { _scaleX = value; UpdateScale(); }
	}

	private float _scaleY = 1f;
	public float scaleY {
		get { return _scaleY; }
		set { _scaleY = value; UpdateScale(); }
	}

	private float _scaleZ = 1f;
	public float scaleZ {
		get { return _scaleZ; }
		set { _scaleZ = value; UpdateScale(); }
	}

	private float _translateX = 0f;
	public float translateX {
		get { return _translateX; }
		set { _translateX = value; UpdateTranslate(); }
	}

	private float _translateY = 0f;
	public float translateY {
		get { return _translateY; }
		set { _translateY = value; UpdateTranslate(); }
	}

	private float _translateZ = 0f;
	public float translateZ {
		get { return _translateZ; }
		set { _translateZ = value; UpdateTranslate(); }
	}

	private float _smoothness = 0.5f;
	public float smoothness {
		get { return _smoothness; }
		set { _smoothness = value; }
	}

	private float _metallic = 0.0f;
	public float metallic {
		get { return _metallic; }
		set { _metallic = value; }
	}

	private float offset = 0f;
	private Material mat;

	private Color lightColor;

	private Quaternion orb_rotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().sharedMaterial;
		offset = 0f;
	}

	void UpdateScale() {
		transform.localScale = new Vector3(_scaleX, _scaleY, _scaleZ);
	}

	void UpdateTranslate() {
		transform.position = new Vector3(_translateX, _translateY, _translateZ);
	}
	
	// Update is called once per frame
	void Update () {
		offset += _evolution * Time.deltaTime;

		mat.SetFloat("_NoiseOffset", offset);
		mat.SetFloat("_NoiseFrequency", _frequency);
		mat.SetFloat("_NoiseMagnitude", _magnitude);
		mat.SetFloat("_BaseRadius", _radius);
		mat.SetFloat("_RadiusCurve", _curve);

		lightColor = Color.HSVToRGB(_lightHue, _lightSat * (1f - _textureCrossfade), _lightBright);
		_light.color = lightColor;
		mat.SetColor("_V_WIRE_Color", lightColor);

		orb_rotation = Quaternion.Euler(new Vector3(_rotationX, _rotationY, _rotationZ) * Time.deltaTime) * orb_rotation;
		mat.SetVector("_OrbRotation", new Vector4(orb_rotation.x, orb_rotation.y, orb_rotation.z, orb_rotation.w));

		mat.SetFloat("_Crossfade", _textureCrossfade);

		mat.SetFloat("_Glossiness", _smoothness);
		mat.SetFloat("_Metallic", _metallic);
	}
}
