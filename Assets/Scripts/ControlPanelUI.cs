using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPanelUI : MonoBehaviour {
    public ParticleSystem HumanResources;
    public SpriteRenderer Planet;
    public Text Score;
    public Text GameOverScore;
    public GameObject GameOverPopup;
    public GaugeUI Gauge;
    public Slider PowerSlide;
    public Toggle ChannelButton1, ChannelButton2, ChannelButton3, ChannelButton4;
    public Image ChannelScreen1, ChannelScreen2, ChannelScreen3, ChannelScreen4;
    public float AmplitudeRange = 0.8f;
    public Button EndButton;
    
    private Dictionary<Toggle, Image> _screenMap = new Dictionary <Toggle, Image>();
    private List<Toggle> _choosenSwitches = new List<Toggle>();

    private Vector3 _planetStart;
    private Vector3 _planetScale;
    private Vector3 _planetDestination = new Vector3(2.33f, 1.061f, 0f) - new Vector3(-1.29f, -0.82f, 0.0f);
    private float _score = 0;
    private bool _isGameOver = false;

    // Use this for initialization
    void Start () {
        _planetStart = Planet.transform.position;
        _planetScale = Planet.transform.localScale;
        Gauge.OnCompleted += OnGameOver; 
        PowerSlide.onValueChanged.AddListener(OnSliderValueChange);

        ChannelButton1.onValueChanged.AddListener((b)=> { OnValueChange(ChannelButton1, b);});
        ChannelButton2.onValueChanged.AddListener((b) => { OnValueChange(ChannelButton2, b); });
        ChannelButton3.onValueChanged.AddListener((b) => { OnValueChange(ChannelButton3, b); });
        ChannelButton4.onValueChanged.AddListener((b) => { OnValueChange(ChannelButton4, b); });

        _screenMap.Add(ChannelButton1, ChannelScreen1);
        _screenMap.Add(ChannelButton2, ChannelScreen2);
        _screenMap.Add(ChannelButton3, ChannelScreen3);
        _screenMap.Add(ChannelButton4, ChannelScreen4);

        foreach (Toggle t in _screenMap.Keys)
        {
            if (_choosenSwitches.Contains(t))
                continue;
            Image cs = _screenMap[t];
            cs.material.SetFloat("_Amplitude", 0.0f);
        }

        EndButton.onClick.AddListener(OnResetGame);
    }

    private void OnResetGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnGameOver()
    {
        // Game Over
        GameOverPopup.SetActive(true);
        GameOverScore.text = ((int)_score).ToString() + " HUMANS SAVED";
        _isGameOver = true;
    }

    private void OnSliderValueChange(float currentNormalizedValue)
    {
        foreach(Toggle button in _choosenSwitches)
        {
            Image screen = _screenMap[button];
            screen.material.SetFloat("_Amplitude", currentNormalizedValue * AmplitudeRange - AmplitudeRange / 2);
        }
    }

    private void OnValueChange(Toggle choosen, bool b)
    {
        Debug.Log("OnValueChange" + choosen.ToString() + " " + b);
        if (b)
        {
            _choosenSwitches.Add(choosen);
            float amip = _screenMap[choosen].material.GetFloat("_Amplitude");
            PowerSlide.value = (amip + AmplitudeRange / 2) / AmplitudeRange;
        }
        else
        {
            _choosenSwitches.Remove(choosen);
        }
    }

    // Update is called once per frame
    void Update () {
        foreach (Toggle t in _screenMap.Keys)
        {
            if (_choosenSwitches.Contains(t))
                continue;
            Image cs = _screenMap[t];
            float amip = cs.material.GetFloat("_Amplitude");
            
            float r = (float) (new System.Random().NextDouble()) / 1000f;

            if (amip < 0)
                r = -1.0f * r;
            cs.material.SetFloat("_Amplitude", amip + r);
        }


        float totalVal = 0.0f;
        foreach (Toggle t in _screenMap.Keys)
        {
            Image cs = _screenMap[t];
            float amip = cs.material.GetFloat("_Amplitude");
            totalVal += Math.Abs(amip);
        }

        Gauge.Value = totalVal;

        Vector3 diff = _planetDestination - _planetStart;

        Planet.transform.position = _planetStart + diff * Gauge.Value;
        Planet.transform.localScale = Gauge.Value * _planetScale;

        if (!_isGameOver)
        {
            _score += HumanResources.emissionRate * Time.deltaTime;
            Score.text = ((int)_score).ToString();
        }
    }
}
