using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using OscJack;
using System;

public class OSCToggleControl : MonoBehaviour {

	[SerializeField] string _path = "path";

	[SerializeField] string _acceptValue = "value";

	[SerializeField] Texture _object = null;

	[SerializeField] Text _label;

	[Serializable] public class ToggleEvent : UnityEvent<Texture> {};
	[SerializeField] ToggleEvent _onValueChanged = new ToggleEvent();

	private Toggle _toggle;

	void OnEnable () {
		_toggle = GetComponent<Toggle>();
		_toggle.onValueChanged.AddListener(this.Listener);
		_toggle.isOn = _toggle.isOn;
	}

#if UNITY_EDITOR
	protected void OnValidate() {
		_label.text = _acceptValue;
	}
#endif
	
	// Update is called once per frame
	void Update () {
		var data = OscMaster.GetData(_path);
		if (data != null) {
			var val = (string)data[0];
			if (val.Equals(_acceptValue)) {
				_toggle.isOn = true;
				OscMaster.ClearData(_path);
			}
		}
	}

	private void Listener(bool on) {
		if (on) {
			_onValueChanged.Invoke(_object);
		}
	}
}
