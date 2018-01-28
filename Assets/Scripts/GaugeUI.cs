using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour {
    public RectTransform Handle;
    public float MinAngle;
    public float MaxAngle;
    [Range(0,1)]
    public float Value;
	
	// Update is called once per frame
	void Update () {
        Handle.localRotation = Quaternion.Euler(0, 0, Value * (MaxAngle - MinAngle) + MinAngle);
	}
}
