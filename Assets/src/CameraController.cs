using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    [SerializeField]
    private Transform _target;

    private Camera _camera;

    private float _cameraZPosition;

    void Awake() {
        _camera = GetComponent<Camera>();
    }

	// Use this for initialization
	void Start () {
        _cameraZPosition = transform.localPosition.z;
    }
	
	// Update is called once per frame
	void Update () {
		if (_target != null) {
            _camera.transform.localPosition = new Vector3(_target.localPosition.x, _target.localPosition.y, _cameraZPosition);
        }
	}
}
