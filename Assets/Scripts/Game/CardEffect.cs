using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGYB
{
    public enum CardState { None, Selective, Selected }

    public class CardEffect : MonoBehaviour
    {
        public int CardNum;
        [HideInInspector] public CardState MyState;
        private Vector3 noneScale = new Vector3(0.93f, 0.93f, 0.93f);
        private Vector3 hoverScale = new Vector3(1.08f, 1.08f, 1.08f);
        private Vector3 selectedScale = new Vector3(1.23f, 1.23f, 1.23f);


        private void Awake()
        {
            MyState = CardState.None;
            this.transform.localScale = noneScale;
        }

        public void SetCardState(CardState cardState)
        {
            MyState = cardState;
        }

        public void NoneEffect()
        {
            this.gameObject.transform.localScale = noneScale;
        }

        public void HoverEffect()
        {
            this.gameObject.transform.localScale = hoverScale;
        }

        public void SelectedEffect()
        {
            this.gameObject.transform.localScale = selectedScale;
        }

        private void OnMouseEnter()
        {
            if (MyState == CardState.Selective)
            {
                HoverEffect();
            }
        }

        private void OnMouseExit()
        {
            if (MyState == CardState.Selective)
            {
                NoneEffect();
            }
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
                HoverEffect();
                GameManager.Instance.CardSelected(-1);
            }
        }
    }
}