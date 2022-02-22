using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyBanner;
using UnityEngine.EventSystems;

/// <summary>
/// demo
/// </summary>
public class Demo : MonoBehaviour
{
    private BannerView _view;
    private BannerItem _item;
    private BannerManager<Sprite> _mgr;
    private bool _isSelect = false;
    public Button LeftBtn;
    public Button RightBtn;
    public Dropdown ShowCountDropdown;
    public Dropdown DirectionDropdown;
    public InputField JumpPageInputField;
    public InputField LoopStepInputField;
    public InputField ItemOffsetField;
    public InputField LerpDurationField;

    public Toggle _autoPlayTg;

    private int _itemCount = 1;

    public List<Sprite> Sprites;

    private void Awake()
    {
        _view = GetComponentInChildren<BannerView>();
        _item = GetComponentInChildren<BannerItem>();
        
        
    }

    void Start()
    {
        Init();

        LeftBtn.onClick.AddListener(OnLeftButtonClick);
        RightBtn.onClick.AddListener(OnRightButtonClick);
        JumpPageInputField.onEndEdit.AddListener(OnTurnToButtonClick);
        LoopStepInputField.onEndEdit.AddListener(OnLoopStepChanged);
        ItemOffsetField.onEndEdit.AddListener(OnItemOffsetChanged);
        LerpDurationField.onEndEdit.AddListener(OnLerpDurationChanged);
        ShowCountDropdown.value = 0;
        ShowCountDropdown.onValueChanged.AddListener(ConsoleResult);

        DirectionDropdown.value = 0;
        DirectionDropdown.onValueChanged.AddListener(OnChangAxisButtonClick);
        _autoPlayTg.onValueChanged.AddListener(OnAutoPlay);
    }

    private void Init()
    {
        _mgr = new BannerManager<Sprite>(_view, _item);
        //_mgr.Init(_itemCount, Sprites);
    }

    void Update()
    {

        //Check current select
        if (Input.GetMouseButtonDown(0))
        {
            _isSelect = EventSystem.current.currentSelectedGameObject == gameObject;
        }

        //mouse opera
        if (_isSelect && Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            OnRightButtonClick();
        }
        if (_isSelect && Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            OnLeftButtonClick();
        }

        _mgr.LoopBanner();

    }

    private void OnAutoPlay(bool isOn)
    {
        _view.Loop = isOn;
    }

    private void OnRightButtonClick()
    {
        _mgr.OnNextPageClick();
    }

    private void OnLeftButtonClick()
    {
        _mgr.OnPrePageClick();
    }

    private void OnChangAxisButtonClick(int value)
    {
        switch (value)
        {
            case 0:
                _mgr.ChangeAxisType(AxisType.Horizontal);
                break;
            case 1:
                _mgr.ChangeAxisType(AxisType.Vertical);
                break;
            default:
                _mgr.ChangeAxisType(AxisType.Horizontal);
                break;
        }
    }

    private void OnTurnToButtonClick(string index)
    {
        int idx;
        if (int.TryParse(index, out idx))
        {
            _mgr.SeekToTargetItem(idx);
        }
    }


    public void ConsoleResult(int value)
    {
        _itemCount = 2 * value + 1;
        //_mgr.Init(_itemCount, Sprites);
    }


    public void OnLoopStepChanged(string value)
    {
        int idx;
        if (int.TryParse(value, out idx))
        {
            _view.LoopStep = idx;
        }
    }

    public void OnItemOffsetChanged(string value)
    {
        int idx;
        if (int.TryParse(value, out idx))
        {
            _view.ItemOffset = idx;
        }
    }

    public void OnLerpDurationChanged(string value)
    {
        float idx;
        if (float.TryParse(value, out idx))
        {
            _view.LerpDuration = idx;
        }
    }



}
