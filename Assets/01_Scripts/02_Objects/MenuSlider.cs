using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* DOC
*
* Mathf.Clamp(float x, float y, float z) :
*   x if y <= x <= z
*   y if x < y
*   z if x > z
* OnBeginDrag
*   Called before a drag is started (unity doc)
*
*/

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
[RequireComponent(typeof(ScrollRect))]
public class MenuSlider : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Tooltip("Index of the default slide")]
    public int indexDefaultSlide;
    [Tooltip("How fast will page lerp to the slide")]
    public float decelerationRate = 10f;

    [Tooltip("Icon for unselected slide")]
    public Sprite unselectedIcon;
    [Tooltip("Icon for selected slide")]
    public Sprite selectedIcon;
    [Tooltip("Icons")]
    public Transform icons;
    [Tooltip("Liste des panels cachés au début")]
    public List<GameObject> hiddenPanelAtStart;

    private int _slideCount;
    private int _currentSlide;

    private bool _lerpping;
    private Vector2 _lerpTo;

    private List<Vector2> _slidePositions = new List<Vector2>();

    private ScrollRect _scrollRectComponent;
    private RectTransform _scrollRect;
    private RectTransform _slides;

    private bool _showIcons;
    private int _currentIndexIcon;
    private List<Image> _iconImages;

    void Start ()
    {
        _scrollRectComponent = GetComponent<ScrollRect>();
        _scrollRect = GetComponent<RectTransform>();
        _slides = _scrollRectComponent.content;

        _slideCount = _slides.childCount;

        SetSlidePositions();
        SetSlide(indexDefaultSlide);
        _lerpping = false;

        InitIcons();
        SetIcon(indexDefaultSlide);

        foreach (GameObject panel in hiddenPanelAtStart)
            panel.SetActive(false);
	}
	
	void Update () {
		if (_lerpping)
        {
            _slides.anchoredPosition = Vector2.Lerp(
                _slides.anchoredPosition, _lerpTo,
                decelerationRate * Time.deltaTime);
            if (Vector2.SqrMagnitude(_slides.anchoredPosition - _lerpTo) < 0.1f)
            {
                _slides.anchoredPosition = _lerpTo;
                _scrollRectComponent.velocity = Vector2.zero;
                _lerpping = false;
            }
        }
	}

    /// <summary>
    /// Positionne toutes les pages dans le containers
    /// </summary>
    private void SetSlidePositions()
    {
        int width = (int)_scrollRect.rect.width;
        int slidesWidth = width * _slideCount;
        int offsetX = width / 2;

        _slidePositions.Clear();

        // set width of container
        Vector2 newSize = new Vector2(slidesWidth, 0);
        _slides.sizeDelta = newSize;
        Vector2 newPosition = new Vector2(slidesWidth / 2, 0);
        _slides.anchoredPosition = newPosition;

        // iterate through all container childern and set their positions
        for (int i = 0; i < _slideCount; i++)
        {
            RectTransform child = _slides.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition;

            childPosition = new Vector2(i * width - slidesWidth / 2 + offsetX, 0f);
            child.anchoredPosition = childPosition;
            _slidePositions.Add(childPosition);
        }
    }

    /// <summary>
    /// Se positionne à la slide slideIndex.
    /// Set currentSlide à slideIndex.
    /// </summary>
    /// <param name="slideIndex">Indice du slide</param>
    private void SetSlide(int slideIndex)
    {
        slideIndex = Mathf.Clamp(slideIndex, 0, _slideCount - 1);
        _slides.anchoredPosition = _slidePositions[slideIndex];
        _currentSlide = slideIndex;
        if (_showIcons) SetIcon(_currentSlide);
    }

    /// <summary>
    /// Se déplace de façon smooth vers la slide slideIndex
    /// </summary>
    /// <param name="slideIndex">Indice du slide</param>
    private void LerpToSlide(int slideIndex)
    {
        slideIndex = Mathf.Clamp(slideIndex, 0, _slideCount - 1);
        _lerpTo = _slidePositions[slideIndex];
        _currentSlide = slideIndex;
        _lerpping = true;
        if (_showIcons) SetIcon(_currentSlide);
    }

    /// <summary>
    /// Renvoie l'index de la page la plus proche de la position actuelle
    /// </summary>
    private int GetNearestSlide()
    {
        Vector2 currentPosition = _slides.anchoredPosition;

        float minDistance = float.MaxValue;
        int nearestSlide = _currentSlide;

        for (int i = 0; i < _slideCount; i++)
        {
            if (i == _currentSlide)
                continue;
            float distance = Vector2.SqrMagnitude(currentPosition - _slidePositions[i]);
            if (distance < minDistance)
            {
                nearestSlide = i;
                minDistance = distance;
            }
        }
        return nearestSlide;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lerpping = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        LerpToSlide(GetNearestSlide());
    }

    private void InitIcons()
    {
        _showIcons = unselectedIcon != null && selectedIcon != null;
        if (_showIcons)
        {
            if (icons == null || icons.childCount != _slideCount)
                _showIcons = false;
            else
            {
                _currentIndexIcon = indexDefaultSlide;
                _iconImages = new List<Image>();

                // cache all Image components into list
                for (int i = 0; i < icons.childCount; i++)
                    _iconImages.Add(icons.GetChild(i).GetComponent<Image>());
            }
        }
    }

    private void SetIcon(int indexIcon)
    {
        _iconImages[_currentIndexIcon].sprite = unselectedIcon;
        _iconImages[indexIcon].sprite = selectedIcon;

        _currentIndexIcon = indexIcon;
    }
}
