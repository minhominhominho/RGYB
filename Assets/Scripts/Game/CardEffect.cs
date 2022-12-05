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


        private void Awake()
        {
            MyState = CardState.None;
            MyImage = GetComponent<SpriteRenderer>();
            NoneSprite = MyImage.sprite;
        }

        public void SetCardState(CardState cardState)
        {
            MyState = cardState;
        }

        public void NoneEffect()
        {
            MyImage.sprite = NoneSprite;
        }

        public void SelectedEffect()
        {
            MyImage.sprite = SelectedSprite;
        }

        private void OnMouseUpAsButton()
        {
            if (MyState == CardState.Selective)
            {
                MyState = CardState.Selected;
                SelectedEffect();
                GameManager.Instance.CardSelected(CardNum);
            }
            else if (MyState == CardState.Selected)
            {
                MyState = CardState.Selective;
                NoneEffect();
                GameManager.Instance.CardSelected(-1);
            }
        }
    }
}