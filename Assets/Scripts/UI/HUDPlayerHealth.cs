using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HUDPlayerHealth : MonoBehaviour
{
    private List<Image> _hearts = new List<Image>();

    void Start()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            _hearts.Add(child.GetComponent<Image>());
        }

        GameEvents.OnPlayerHealthChangeEvent += OnPlayerHealthChange;
    }

    private void OnDestroy()
    {
        GameEvents.OnPlayerHealthChangeEvent -= OnPlayerHealthChange;
    }
    
    private void OnPlayerHealthChange(int health)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            if (i < health)
            {
                _hearts[i].color = Color.white;
            }
            else
            {   
                _hearts[i].color = new Color(0.3f, 0.3f, 0.3f, 1);
                //AnimateDamage(_hearts[i]);
            }
        }
    }

/* 
    private void AnimateDamage(Image heart)
    {
        Color c = new Color(0.3f, 0.3f, 0.3f, 1);
        Sequence s = DOTween.Sequence();
        s.Insert(0,heart.DOColor(c, 0.5f).SetEase(Ease.InCubic));
        s.Insert(0, heart.rectTransform.DOScale(Vector3.one * 1.3f, .3f).SetEase(Ease.InCubic));
        s.Append(heart.rectTransform.DOScale(Vector3.one, .4f).SetEase(Ease.OutBounce));
    }
*/

}
