using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums_Game;

public class UIController : MonoBehaviour
{
    public UIType uiType = UIType.None;

    public Image barImg;
    public RectTransform button;

    //public float sliderValue = 0;

    public bool b_Scale;
    private bool b_Repeat = false;

    private Vector3 _preValue;
    private float _value = 1;
    private float _maxValue;
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        switch(uiType)
        {
            case UIType.Scale:
                _preValue = transform.localScale;

                _maxValue = DataManager.instance.uiValue._MaxValue;
                _speed = DataManager.instance.uiValue._Speed;

                break;

            case UIType.Game:
                _preValue = transform.localScale;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(uiType)
        {
            case UIType.Scale:
                if(b_Scale)
                {
                    if(!b_Repeat)
                    {
                        _value += _speed;
                        transform.localScale = new Vector3(_value, _value, _value);
                        //Debug.Log("_value : " + _value);
                        if(_value >= _maxValue)
                        {
                            b_Repeat = true;
                        }
                    }
                    else
                    {
                        _value -= _speed;
                        transform.localScale = new Vector3(_value, _value, _value);

                        if(_value <= 1.0f)
                        {
                            b_Repeat = false;
                        }
                    }
                }
                break;

            case UIType.Slider:
                //SliderValueChange(sliderValue);
                SliderValueChange(barImg.fillAmount);
                break;
        }
    }

    void SliderValueChange(float value)
    {
        //float amount = (value / 100.0f) * 180.0f / 360;
        //barImg.fillAmount = amount;
        //Debug.Log(value);
        //float buttonAngle = amount * 360;

        float buttonAngle = value * 360;
        button.localEulerAngles = new Vector3(0, 0, -buttonAngle);
    }
}
