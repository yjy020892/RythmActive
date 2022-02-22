using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ScreenEffect : MonoBehaviour
{
	public Image _kiosk_screen;
	public Image _screen;
	public Canvas _canvas;
	public FadeEffectData _lastFadeData;

	public class FadeEffectData
	{
		public Color _color;
		public float _delay;
		public float _duration;
		public float _alphaRate;

		public void SetData(Color color, float delay, float duration, float alphaRate)
		{
			_color = color;
			_delay = delay;
			_duration = duration;
			_alphaRate = alphaRate;
		}
	}

	private void Awake()
	{
		if (_kiosk_screen == null)
			_kiosk_screen = GetComponent<Image>();

		if (_canvas == null)
			_canvas = transform.parent.GetComponent<Canvas>();

		if (_lastFadeData == null)
		{
			_lastFadeData = new FadeEffectData();
			_lastFadeData.SetData(Color.black, 0.0f, 0.5f, 1.0f);
		}

		_kiosk_screen.rectTransform.sizeDelta = new Vector2(Screen.width * 2.0f, Screen.height * 2.0f);
		_screen.rectTransform.sizeDelta = new Vector2(Screen.width * 2.0f, Screen.height * 2.0f);
	}

	public void ScreenFadeEffectByLastSetting(bool bFadeIn, Action<bool> callback = null)
	{
		ScreenFadeEffect(_lastFadeData._color, bFadeIn, _lastFadeData._delay, _lastFadeData._duration, _lastFadeData._alphaRate, callback);
	}

	public void ScreenFadeEffect(Color color, bool bFadeIn = false, float delay = 0.0f, float duration = 0.5f, float alphaRate = 1.0f, Action<bool> callback = null)
	{
		if (_lastFadeData == null)
		{
			_lastFadeData = new FadeEffectData();
			_lastFadeData.SetData(Color.black, 0.0f, 0.5f, 1.0f);
		}

		_lastFadeData.SetData(color, delay, duration, alphaRate);

		if (!_canvas.gameObject.activeSelf)
		{
			_canvas.gameObject.SetActive(true);
		}

		if (!_kiosk_screen.gameObject.activeSelf)
		{
			_kiosk_screen.gameObject.SetActive(true);
		}

		if(!_screen.gameObject.activeSelf)
        {
			_screen.gameObject.SetActive(true);
		}

		StartCoroutine(LaunchFadeEffect(color, bFadeIn, delay, duration, alphaRate, callback));
	}

	private IEnumerator LaunchFadeEffect(Color color, bool bFadeIn, float delay, float duration, float alphaRate = 1.0f, Action<bool> callback = null)
	{
		yield return new WaitForSeconds(delay);

		if (_kiosk_screen == null)
		{
			_kiosk_screen = GetComponent<Image>();

			if (_kiosk_screen == null)
			{
				callback(false);

				//if (UIManager.Instance.UISystemCount == 0)
				//{
				//	_canvas.gameObject.SetActive(false);
				//}

				yield break;
			}
		}

		color.a = bFadeIn ? alphaRate : 0.0f;
		_kiosk_screen.color = color;
		_screen.color = color;

		float timer = 0.0f;

		while (true)
		{
			yield return null;

			timer += Time.deltaTime;

			if (bFadeIn)
			{
				color.a -= Time.deltaTime / (duration * 3f);
			}
			else
			{
				color.a += Time.deltaTime / (duration * 3f);
			}
			_kiosk_screen.color = color;
			_screen.color = color;

			if (timer >= duration*3f)
			{
				break;
			}
		}

		if (callback != null)
		{
			callback(true);
		}

		if (bFadeIn)
		{
			//if (UIManager.Instance.UISystemCount == 0)
			//{
			//	_canvas.gameObject.SetActive(false);
			//}

			_kiosk_screen.gameObject.SetActive(false);
			_screen.gameObject.SetActive(false);
		}
	}
}