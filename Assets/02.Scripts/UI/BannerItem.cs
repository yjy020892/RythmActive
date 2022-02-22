using UnityEngine;
using UnityEngine.UI;

namespace EasyBanner
{
    /// <summary>
    /// The item of Banner
    /// </summary>
    public class BannerItem : ItemBase<Sprite>
    {
        private Image _img;
        //private Text _text;
        /// <summary>
        /// Update the data to the sprite of item.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="result"></param>
        public override void UpdateView(Sprite data, bool result)
        {
            if (result)
            {
                _img.sprite = data;
            }
            else
            {
                _img.sprite = null;
            }

            //_text.text = Index.ToString();
        }

        public override void OnInit()
        {
            _img = GetComponent<Image>();
            //_text = Trs.Find("Text").GetComponent<Text>();
        }

        //public override void SetHighLightEffect(bool center)
        //{
        //    _img.color = center ? Color.white : Color.gray;
        //}
    }
}

