using Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSpritesController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private List<Sprite> _normalSprites;
    [SerializeField] private List<Sprite> _highlightedSprites;
    [SerializeField] private GameObject _checkmark;
    [SerializeField] private bool _isFullScreen;

    private Image _checkmarkImage;
    private bool _isOn;

    void Start()
    {
        _checkmarkImage = _checkmark.GetComponent<Image>();
        if (!_isFullScreen)
            UpdateSprites(false);
        else
            UpdateFullscreenSprites(PlayerPrefsManager.IsFullScreen());
    }

    public void UpdateSprites(bool isOn)
    {
        _isOn = isOn;
        if (!isOn)
            _checkmarkImage.sprite = _highlightedSprites[0];
        else
            _checkmarkImage.sprite = _highlightedSprites[1]; 
    }

    private void UpdateFullscreenSprites(bool isOn)
    {
        if (!isOn)
            _checkmarkImage.sprite = _normalSprites[0];
        else
            _checkmarkImage.sprite = _normalSprites[1];
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (_isOn)
            _checkmarkImage.sprite = _highlightedSprites[1];
        else
            _checkmarkImage.sprite = _highlightedSprites[0];
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (_isOn)
            _checkmarkImage.sprite = _normalSprites[1];
        else
            _checkmarkImage.sprite = _normalSprites[0];
    }
}
