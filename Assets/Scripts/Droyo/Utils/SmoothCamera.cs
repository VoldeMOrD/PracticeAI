using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {
    Transform _target;
    float _distance = 10f;
    float _height= 5f;
    float _heightDamping = 2f;
    float _rotationDamping = 3f;

    private void LateUpdate() {
        
        if(_target == null){
            return;
        }
        float wantedRotationAngle = _target.eulerAngles.y;
        float wantedHeight = _target.position.y + _height;
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;
        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);
        // Damp the height
	    currentHeight = Mathf.Lerp (currentHeight, wantedHeight, _heightDamping * Time.deltaTime);
        // Convert the angle into a rotation
	    var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = _target.position;
        transform.position -= currentRotation * Vector3.forward * _distance;
        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        // Always look at the target
        transform.LookAt (_target);
    }

    public Transform Target {
        get { return _target; }
        set { _target = value; }
    }
    public float Distance {
        get { return _distance; }
        set { _distance = value; }
    }
    public float Height {
        get { return _height; }
        set { _height = value; }
    }
    public float HeightDamping {
        get { return _heightDamping; }
        set { _heightDamping = value; }
    }
    public float RotationDamping {
        get { return _rotationDamping; }
        set { _rotationDamping = value; }
    }

    public void SetValueFromBounds(Bounds bounds, float multiply = 1f){
        this._distance = ((bounds.extents.x + bounds.extents.y) / 2) * multiply;
        this._height = ((bounds.extents.y + bounds.extents.x) / 4) * multiply;
    }
}