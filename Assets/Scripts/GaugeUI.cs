using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour {
    public Graphic TargetGraphic;
    public RectTransform Handle;
    public float MinAngle;
    public float MaxAngle;
    [Range(0,1)]
    private float _value;

    public delegate void GaugeCompleted();
    public event GaugeCompleted OnCompleted;

    public float Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = Mathf.Clamp01(value);

            if(TargetGraphic != null)
            {
                TargetGraphic.color = new Color(1, 1, 1, _value);
            }


            if(_value >= 1)
            {
                if(OnCompleted!= null)
                {
                    OnCompleted();
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        Handle.localRotation = Quaternion.Euler(0, 0, Value * (MaxAngle - MinAngle) + MinAngle);
	}
}
