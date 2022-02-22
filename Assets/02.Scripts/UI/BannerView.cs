using System;
using UnityEngine;
namespace EasyBanner
{
    /// <summary>
    /// The banner view data
    /// </summary>
    public class BannerView : MonoBehaviour
    {
        // Control the item's scale curve
        public AnimationCurve ScaleCurve;

        // Control the position curve
        public AnimationCurve PositionCurve;

        /// <summary>
        /// Auto play
        /// </summary>
        public bool Loop;

        /// <summary>
        /// Direction of banner
        /// </summary>
        public LoopDirection Direction = LoopDirection.Forward;

        public float _loopStep = 1.0f;
        /// <summary>
        /// The time of banner change to the next page
        /// </summary>
        public float LoopStep
        {
            set
            {
                if (Mathf.Abs(value) < float.Epsilon)
                {
                    return;
                }

                _loopStep = Mathf.Abs(value);
            }

            get
            {
                return _loopStep;
            }

        }

        private int _ttemOffset = 70;
        /// <summary>
        ///  Offset width between item
        /// </summary>
        public int ItemOffset
        {

            set
            {
                if (value == 0)
                {
                    return;
                }

                _ttemOffset = Mathf.Abs(value);
            }

            get
            {
                return _ttemOffset;
            }
        }

        private float _lerpDuration = 0.2f;
        /// <summary>
        /// Lerp duration
        /// </summary>
        public float LerpDuration
        {

            set
            {
                if (Math.Abs(value) < 0.01f)
                {
                    return;
                }

                _lerpDuration = Mathf.Abs(value);
            }

            get
            {
                return _lerpDuration;
            }
        }



        private bool _highLightCenter;
        private Action HighLighAction = null;

        public bool HighLightCenter
        {

            set
            {
                if (_highLightCenter != value && HighLighAction != null)
                {
                    HighLighAction();
                }

                _highLightCenter = value;
            }

            get
            {
                return _highLightCenter;
            }
        }

        public AxisType Axis = AxisType.Horizontal;
        private Action _tweenViewToTarget = null;
        public bool EnableLerpTween = true;

        public float CurOffsetValue = 0.5f;

        /// <summary>
        /// Check the curve value
        /// </summary>
        private void Awake()
        {
            if (ScaleCurve == null || ScaleCurve.length == 0)
            {
                ScaleCurve = ScaleCurve = new AnimationCurve(new Keyframe(0, 0.3f, 0, 1f), new Keyframe(0.5f, 1)); ;
                ScaleCurve.postWrapMode = WrapMode.PingPong;
                ScaleCurve.preWrapMode = WrapMode.PingPong;
            }

            if (PositionCurve == null || PositionCurve.length == 0)
            {
                PositionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                PositionCurve.postWrapMode = WrapMode.Loop;
                PositionCurve.preWrapMode = WrapMode.Loop;
            }
        }

        public void SetTweenViewToTarget(Action act, Action highLighAction)
        {
            _tweenViewToTarget = act;
            HighLighAction = highLighAction;
        }

        private void Update()
        {
            if (EnableLerpTween && _tweenViewToTarget != null)
            {
                _tweenViewToTarget();
            }
        }
    }

}