using UnityEngine;

namespace EasyBanner
{
    /// <summary>
    /// Base class of item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ItemBase<T> : MonoBehaviour
    {
        [HideInInspector]
        public int Index = 0;

        // Curve center offset 
        [HideInInspector]
        public float CenterOffset { private set; get; }

        [HideInInspector]
        public GameObject obj { private set; get; }
        public Transform Trs { private set; get; }

        [HideInInspector]
        public bool HighLightEffect;

        void Awake()
        {
            obj = gameObject;
            Trs = transform;
            CenterOffset = 0;
            HighLightEffect = true;
        }

        public void Init(float dFactor, int centerIndex, int index)
        {
            OnInit();
            Index = index;
            CenterOffset = dFactor * (centerIndex - index);
        }

        public abstract void UpdateView(T data, bool result);

        public abstract void OnInit();


        // Update Item's status
        // 1. position
        // 2. scale
        // 3. "depth" is 2D or z Position in 3D to set the front and back item
        public void UpdateItemStatus(float pos, float itemCount, float scale, float depth, AxisType axis, bool start)
        {
            var targetScale = new Vector3(scale, scale, 0);
            var targetPos = (axis == AxisType.Horizontal) ? new Vector3(pos, 0, 0) : new Vector3(0, pos, 0);

            // position     
            Trs.localPosition = targetPos;
            Trs.localScale = targetScale;

            SetSiblingIndex((int)(depth * itemCount), start);
        }

        public void SetSiblingIndex(int depth, bool start)
        {
            //eoprotec
            if(!start)
            {
                Trs.SetSiblingIndex(depth);
            }
        }

        // eoprotec
        public void SetSongData(int center/*, int itemCount, Transform bannerView, Transform centerPanel, Transform leftPanel, Transform rightPanel*/)
        {
            if(center.Equals(Index))
            {
                Trs.SetAsLastSibling();
                //Debug.Log(Trs.GetSiblingIndex());
            }
        }


        public void SetHighLightEffectBase(bool center)
        {
            HighLightEffect = center;
            SetHighLightEffect(center);
        }

        public virtual void SetHighLightEffect(bool center)
        {

        }
    }
}
