using System.Collections.Generic;
using UnityEngine;


namespace EasyBanner
{
    /// <summary>
    /// Manager the banner
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BannerManager<T>
    {
        private float _totalSize = 500.0f;
        private float _currentDuration = 0.0f;

        /// <summary>
        /// The interval between item，Use with Positioncurve in BannerView
        /// </summary>
        private float _factor = 0.2f;

        /// <summary>
        /// originValue Lerp to TargetValue
        /// </summary>
        private float _originValue = 0.1f;
        private float _centerOffset;
        private float _factorTime = 0.1f;

        private List<ItemBase<T>> _itemList = new List<ItemBase<T>>();
        private List<T> _datas;

        // Control the item's depth curve
        private AnimationCurve _depthCurve;
        private ItemBase<T> _itemTemplet;
        private BannerView _view;
        private MoveDirection _direction;

        private int _needChangeIndex = -1;
        private int _halfCount = 0;
        private int _itemCount;
        private int _centerIndex = 0;
        private int _itemIndex = 0;

        /// <summary>
        /// if we can change the target item
        /// </summary>
        private bool _canChangeItem;
        private bool _fullList;
        private bool _inited;
        private bool _single;
        private bool b_Start = false;

        private System.Action OnTweenOver;

        public BannerManager(BannerView view, ItemBase<T> item)
        {
            _inited = false;
            _view = view;

            _view.SetTweenViewToTarget(TweenViewToTarget, HighLightEffect);
            _itemTemplet = item;
            OnTweenOver = null;

            _depthCurve = AnimationCurve.Linear(0, 0, 0.5f, 1);
            _depthCurve.postWrapMode = WrapMode.PingPong;
            _depthCurve.preWrapMode = WrapMode.PingPong;

            _factorTime = 6f * Time.deltaTime;
        }

        /// <summary>
        /// Initialize the banner
        /// </summary>
        /// <param name="count">the pages you want to show</param>
        /// <param name="datas">the data of picture that you want to loop </param>
        public void Init(int count, List<T> datas)
        {
            _inited = false;
            if (count <= 0 || datas.Count <= 0)
            {
                return;
            }

            _itemIndex = 0;
            _single = (count == 1);
            _itemCount = count;
            _datas = datas;
            _fullList = datas.Count > _itemCount;
            _itemList.Clear();
            _itemTemplet.gameObject.SetActive(true);
            _canChangeItem = true;
            RemoveAllChild();

            _factor = (Mathf.RoundToInt((1f / _itemCount) * 10000f)) * 0.0001f;
            _halfCount = Mathf.FloorToInt(_itemCount / 2f);
            _centerIndex = _halfCount;
            _centerOffset = -_view.ItemOffset * _itemCount * 0.5f;

            for (int i = 0; i < _itemCount; i++)
            {
                var item = AddChild(_itemTemplet);
                if (item)
                {
                    item.name = i.ToString();
                    _itemList.Add(item);
                    item.Init(_factor, _centerIndex, i);
                    if (i < datas.Count)
                    {
                        item.UpdateView(datas[i], true);
                    }
                    else
                    {
                        item.UpdateView(default(T), false);
                    }
                }
            }

            _totalSize = _view.ItemOffset * _itemCount;
            _view.CurOffsetValue = 0.5f;
            LerpTweenToTarget(0f, _view.CurOffsetValue, false);
            _itemTemplet.gameObject.SetActive(false);
            _inited = true;
            b_Start = true;
        }

        /// <summary>
        /// Seek to the target Item ,index start from zero
        /// </summary>
        /// <param name="idx"></param>
        public void SeekToTargetItem(int idx)
        {
            if (!_inited || !_canChangeItem)
            {
                return;
            }

            if (_centerIndex == idx || idx < 0)
            {
                return;
            }


            var moveIndexCount = NormalizeIndex(idx - _centerIndex, _datas.Count);
            var targetValue = _view.CurOffsetValue + moveIndexCount * _factor;

            _centerIndex = idx;
            _view.CurOffsetValue = targetValue;
            UpdateitemListStatus(targetValue, true);

            if (_view.Axis == AxisType.Horizontal)
            {
                _itemList.Sort((l, r) => r.transform.localPosition.x.CompareTo(l.transform.localPosition.x));
            }
            else
            {
                _itemList.Sort((l, r) => r.transform.localPosition.y.CompareTo(l.transform.localPosition.y));
            }

            int depth = -1;
            var currIndex = _centerIndex - _halfCount;
            foreach (var item in _itemList)
            {
                Updateitem(item, currIndex);
                if (currIndex < _centerIndex)
                {
                    depth++;
                }
                else if (currIndex == _centerIndex)
                {
                    depth = _itemCount - 1;
                }
                else
                {
                    depth--;
                }

                item.SetSiblingIndex(depth, b_Start);
                currIndex++;
            }

            if (OnTweenOver != null)
            {
                OnTweenOver();
            }
        }

        // Click the NextPage button to select the next item.
        public void OnNextPageClick()
        {
            if (!_inited || !_canChangeItem)
            {
                return;
            }

            _direction = _view.Axis == AxisType.Horizontal ? MoveDirection.LeftToRight : MoveDirection.DownToUp;
            _needChangeIndex = NormalizeIndex(_centerIndex - _halfCount, _datas.Count);
            _centerIndex = NormalizeIndex(_centerIndex + 1, _datas.Count);

            foreach (var item in _itemList)
            {
                item.SetSongData(_centerIndex/*, _itemCount, bannerViewTrans, centerPanelSetTrans, leftPanelSetTrans, rightPanelSetTrans*/);
            }

            LerpTweenToTarget(_view.CurOffsetValue, _view.CurOffsetValue + _factor, true);
        }

        // Click the PrePage button the select the pre item.
        public void OnPrePageClick()
        {
            if (!_inited || !_canChangeItem)
            {
                return;
            }

            _direction = _view.Axis == AxisType.Horizontal ? MoveDirection.RightToLeft : MoveDirection.UpToDown;
            _needChangeIndex = NormalizeIndex(_centerIndex + _halfCount, _datas.Count);
            _centerIndex = NormalizeIndex(_centerIndex - 1, _datas.Count);

            foreach (var item in _itemList)
            {
                item.SetSongData(_centerIndex/*, _itemCount, bannerViewTrans, centerPanelSetTrans, leftPanelSetTrans, rightPanelSetTrans*/);
            }

            LerpTweenToTarget(_view.CurOffsetValue, _view.CurOffsetValue - _factor, true);
        }

        public void ChangeAxisType(AxisType axis)
        {
            if (_inited && axis != _view.Axis)
            {
                _view.Axis = axis;
                UpdateitemListStatus(_view.CurOffsetValue, true);
            }
        }

        private float _loopTempTime;

        /// <summary>
        /// Loop Banner
        /// </summary>
        public void LoopBanner()
        {
            if (!_view.Loop)
            {
                return;
            }

            if (!(Time.time > _loopTempTime))
            {
                return;
            }

            _loopTempTime = Time.time + _view.LoopStep;
            if (_view.Direction == LoopDirection.Forward)
            {
                OnNextPageClick();
            }
            else
            {
                OnPrePageClick();
            }
        }

        public void SetOnTweenOver(System.Action act)
        {
            OnTweenOver = act;
        }

        #region private
        private void UpdateitemListStatus(float value, bool stop)
        {
            var snap = stop ? GetCenterSnapValue(value) : 0;
            foreach (var item in _itemList)
            {
                float pos = _single ? 0 : GetPosValue(value, item.CenterOffset) + _centerOffset;
                pos -= snap;

                var scale = GetScaleValue(value, item.CenterOffset);
                var depth = _depthCurve.Evaluate(value + item.CenterOffset);
                item.UpdateItemStatus(pos, _itemCount, scale, depth, _view.Axis, b_Start);

                if (_view.HighLightCenter)
                {
                    bool center = (Mathf.Abs(pos) < float.Epsilon);
                    if (center && !item.HighLightEffect)
                    {
                        item.SetHighLightEffectBase(true);
                    }
                    else if (!center && item.HighLightEffect)
                    {
                        item.SetHighLightEffectBase(false);
                    }
                }
                else if (item.HighLightEffect)
                {
                    item.SetHighLightEffectBase(false);
                }
            }
        }


        private ItemBase<T> AddChild(ItemBase<T> prefab)
        {
            var item = Object.Instantiate(prefab);
            if (item != null)
            {
                var t = item.transform;
                t.SetAsLastSibling();
                t.SetParent(_view.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                item.gameObject.layer = _view.gameObject.layer;
            }
            return item;
        }

        private void RemoveAllChild()
        {
            foreach (Transform item in _view.transform)
            {
                Object.Destroy(item.gameObject);
            }
        }


        private void LerpTweenToTarget(float originValue, float targetValue, bool needTween = false)
        {
            if (!needTween)
            {
                UpdateitemListStatus(targetValue, true);
                if (OnTweenOver != null)
                {
                    OnTweenOver();
                }
            }
            else
            {
                _canChangeItem = false;
                _originValue = originValue;
                _view.CurOffsetValue = targetValue;
                _currentDuration = 0.0f;
            }
            _view.EnableLerpTween = needTween;
        }


        /// <summary>
        /// 补间动画
        /// </summary>
        private void TweenViewToTarget()
        {
            _currentDuration += Time.deltaTime;
            if (_currentDuration > _view.LerpDuration)
            {
                _currentDuration = _view.LerpDuration;
            }

            var percent = _currentDuration / _view.LerpDuration;
            var value = Mathf.Lerp(_originValue, _view.CurOffsetValue, percent);
            var stop = _currentDuration >= _view.LerpDuration;

            // eoprotec
            //if(stop)
            //{
            //    leftPanelSetTrans.SetParent(bannerViewTrans.GetChild(2));
            //    leftPanelSetTrans.anchoredPosition = Vector3.zero;
            //    leftPanelSetTrans.localScale = Vector3.one;
            //    //bannerViewTrans.GetChild(2);
            //}

            UpdateitemListStatus(value, stop);

            if (_view.LerpDuration - _currentDuration < _factorTime)
            {
                ChangeBoundaryitemData();
            }

            if (stop)
            {
                _canChangeItem = true;
                _view.EnableLerpTween = false;
                if (OnTweenOver != null)
                {
                    OnTweenOver();
                }
            }
        }

        private void ChangeBoundaryitemData()
        {
            if (_fullList)
            {
                foreach (var item in _itemList)
                {
                    if ((_direction == MoveDirection.RightToLeft || _direction == MoveDirection.UpToDown) && item.Index == _needChangeIndex)
                    {
                        item.Index = NormalizeIndex(_needChangeIndex - _itemCount, _datas.Count);
                        item.UpdateView(_datas[item.Index], true);
                        _needChangeIndex = -1;
                    }
                    else if ((_direction == MoveDirection.LeftToRight || _direction == MoveDirection.DownToUp) && item.Index == _needChangeIndex)
                    {
                        item.Index = NormalizeIndex(_needChangeIndex + _itemCount, _datas.Count);
                        item.UpdateView(_datas[item.Index], true);
                        _needChangeIndex = -1;
                    }
                }
            }
        }


        /// <summary>
        /// 获取浮点误差中心偏移值
        /// </summary>
        private float GetCenterSnapValue(float value)
        {
            if (!_single)
            {
                var snap = _totalSize;
                foreach (var item in _itemList)
                {
                    float pos = GetPosValue(value, item.CenterOffset) + _centerOffset;
                    if (Mathf.Abs(pos) < Mathf.Abs(snap))
                    {
                        snap = pos;
                    }
                }
                return snap;
            }
            else
            {
                return 0;
            }
        }

        // Get the evaluate value to set item's scale
        private float GetScaleValue(float sliderValue, float added)
        {
            float scaleValue = _view.ScaleCurve.Evaluate(sliderValue + added);
            return scaleValue;
        }

        // Get the X value set the Item's position
        private float GetPosValue(float sliderValue, float added)
        {
            float evaluateValue = _view.PositionCurve.Evaluate(sliderValue + added) * _totalSize;
            return evaluateValue;
        }


        private void Updateitem(ItemBase<T> item, int i)
        {
            int index = _fullList ? NormalizeIndex(i, _datas.Count) : NormalizeIndex(i, _itemCount);
            if (index < _datas.Count)
            {
                item.Index = index;
                item.UpdateView(_datas[index], true);
            }
            else
            {
                item.UpdateView(default(T), false);
            }
        }

        private int NormalizeIndex(int idx, int all)
        {
            idx = (idx + all) % all;
            return idx;
        }


        private void HighLightEffect()
        {
            if (!_inited)
            {
                return;
            }

            if (_view.HighLightCenter)
            {
                foreach (var item in _itemList)
                {
                    bool center = (item.Index == _centerIndex);
                    if (item.HighLightEffect != center)
                    {
                        item.SetHighLightEffectBase(center);
                    }
                }
            }
            else
            {
                foreach (var item in _itemList)
                {
                    if (item.HighLightEffect == true)
                    {
                        item.SetHighLightEffectBase(false);
                    }
                }
            }
        }

        #endregion
    }


    public enum AxisType : byte
    {
        Horizontal,
        Vertical
    }

    public enum MoveDirection : byte
    {
        LeftToRight,
        RightToLeft,
        UpToDown,
        DownToUp,
    }

    public enum LoopDirection
    {
        Forward,
        Reverse
    }
}