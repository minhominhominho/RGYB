using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RGYB
{
    public enum CardState { None, Selective, Selected }

    public class CardEffect : MonoBehaviour
    {
        public int CardNum;
        [HideInInspector] public CardState MyState;
        [HideInInspector] public Sprite NoneSprite;
        public Sprite SelectedSprite;
        private SpriteRenderer MyImage;
        private Vector3 origScale;
        private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);

        private void Awake()
        {
            MyState = CardState.None;
            MyImage = GetComponent<SpriteRenderer>();
            NoneSprite = MyImage.sprite;
            origScale = transform.localScale;
        }

        public void SetCardState(CardState cardState)
        {
            MyState = cardState;
        }

        public void NoneEffect()
        {
            MyImage.sprite = NoneSprite;
            transform.localScale = origScale;
        }

        public void SelectedEffect()
        {
            MyImage.sprite = SelectedSprite;
            transform.localScale = selectedScale;
        }

        private void OnMouseUpAsButton()
        {
            if (MyState == CardState.Selective)
            {
                SoundManager.Instance.PlaySFX(SFXType.Card_Select);
                MyState = CardState.Selected;
                SelectedEffect();
                GameManager.Instance.CardSelected(CardNum);
            }
            else if (MyState == CardState.Selected)
            {
                SoundManager.Instance.PlaySFX(SFXType.Card_Select);
                MyState = CardState.Selective;
                NoneEffect();
                GameManager.Instance.CardSelected(-1);
            }
        }
    }
}