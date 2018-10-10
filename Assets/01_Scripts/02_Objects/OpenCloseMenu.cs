using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Permet de faire défiler un panel dans un sens
/// </summary>
public class OpenCloseMenu : MonoBehaviour
{
	public enum Direction
    {
		LeftToRight,
		RightToLeft,
		TopToBottom,
		BottomToTop
	};

    [Tooltip("Panel à faire défiler")]
    public GameObject panel;

    [Tooltip("Fonction gérant la vitesse de défilement")]
    public AnimationCurve curveToOpenMenu;

    public Direction directionOpen;

    private float _openPosition;
    private float _closePosition;

    private float _openDuration;
    private float _closeDuration;
    
    void Start()
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        
		switch (directionOpen) {
		case Direction.LeftToRight:
			_closePosition = rect.anchoredPosition.x;
			_openPosition = _closePosition + rect.rect.width;
			break;
		case Direction.RightToLeft:
			_closePosition = rect.anchoredPosition.x;
			_openPosition = _closePosition - rect.rect.width;
			break;
		case Direction.TopToBottom:
			_closePosition = rect.anchoredPosition.y;
			_openPosition = _closePosition - rect.rect.height;
			break;
		case Direction.BottomToTop:
			_closePosition = rect.anchoredPosition.y;
			_openPosition = _closePosition + rect.rect.height;
			break;

		}

        _openDuration = 0.5f;
        _closeDuration = 0.2f;

    }

    public void Open()
    {
        if (directionOpen == Direction.LeftToRight || directionOpen == Direction.RightToLeft)
            StartCoroutine(OpenMenuHorizontal());
        else
            StartCoroutine(OpenMenuVertical());
    }

    public void Close()
    {
        if (directionOpen == Direction.LeftToRight || directionOpen == Direction.RightToLeft)
            StartCoroutine(CloseMenuHorizontal());
        else
            StartCoroutine(CloseMenuVertical());
    }


    private IEnumerator OpenMenuVertical()
	{
		var time = 0f;

		Vector2 positionPanel = panel.GetComponent<RectTransform>().anchoredPosition;

		while (Mathf.Abs(panel.GetComponent<RectTransform>().anchoredPosition.y  - _openPosition) > 0)
		{
			panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				positionPanel.x,
                _closePosition
                    + ((directionOpen == Direction.BottomToTop) ? 1 : -1)
                    * (Mathf.Abs(_openPosition - _closePosition))
                    * curveToOpenMenu.Evaluate(time));

			time +=  Time.deltaTime / _openDuration;

			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator CloseMenuVertical()
	{
		var time = 0f;

		Vector2 positionPanel = panel.GetComponent<RectTransform>().anchoredPosition;

		while (Mathf.Abs(panel.GetComponent<RectTransform>().anchoredPosition.y  - _closePosition) > 0)
		{
			panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				positionPanel.x,
                _openPosition
                    - ((directionOpen == Direction.BottomToTop) ? 1 : -1)
                    * (Mathf.Abs(_openPosition - _closePosition))
                    * curveToOpenMenu.Evaluate(time));

			time += Time.deltaTime / _closeDuration;

			yield return new WaitForEndOfFrame();
		}
	}


    private IEnumerator OpenMenuHorizontal()
    {
        var time = 0f;

        Vector2 positionPanel = panel.GetComponent<RectTransform>().anchoredPosition;

        while (Mathf.Abs(panel.GetComponent<RectTransform>().anchoredPosition.x  - _openPosition) > 0)
        {
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				_closePosition
                    + ((directionOpen == Direction.LeftToRight) ? 1 : -1)
                    * (Mathf.Abs(_openPosition - _closePosition))
                    * curveToOpenMenu.Evaluate(time),
                positionPanel.y);

            time += Time.deltaTime / _openDuration;

            yield return new WaitForEndOfFrame();
        }
    }

	private IEnumerator CloseMenuHorizontal()
    {
        var time = 0f;

        Vector2 positionPanel = panel.GetComponent<RectTransform>().anchoredPosition;

        while (Mathf.Abs(panel.GetComponent<RectTransform>().anchoredPosition.x  - _closePosition) > 0)
        {
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				_openPosition
                    - ((directionOpen == Direction.LeftToRight) ? 1 : -1)
                    * (Mathf.Abs(_openPosition - _closePosition))
                    * curveToOpenMenu.Evaluate(time),
                positionPanel.y);

            time += Time.deltaTime / _closeDuration;

            yield return new WaitForEndOfFrame();
        }
    }
}