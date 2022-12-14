using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_SecondSelect : SequenceObject
    {
        private bool isSent = false;

        public override IEnumerator SequenceJob()
        {
            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup();

            while (!GameManager.Instance.CheckPanelClosed())
            {
                yield return NormalWait;
            }

            GameManager.Instance.SetActiveFakeSelectButton(false);
            
            // Set card state
            GameManager.Instance.SetAllFrontCardsState(CardState.Selective);

            GameManager.Instance.CannotBeSelectedEffect(false);

            // Turn off turn sign
            GameObject tSignMask = GameManager.Instance.TurnSignMask[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
            GameObject tSignMaskDest = GameManager.Instance.TurnSignMaskDestPosition[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
            Vector3 startPos = tSignMask.transform.localPosition;
            Vector3 destPos = tSignMaskDest.transform.localPosition;
            while (tSignMask.transform.localPosition.y > tSignMaskDest.transform.localPosition.y + 0.01f || tSignMask.transform.localPosition.y < tSignMaskDest.transform.localPosition.y - 0.01f)
            {
                tSignMask.transform.localPosition = new Vector3(
                     tSignMask.transform.localPosition.x,
                      tSignMask.transform.localPosition.y + (destPos.y - startPos.y) / 20,
                       tSignMask.transform.localPosition.z
                    );
                yield return FadeWait;
            }
            tSignMask.transform.localPosition = destPos;
            tSignMaskDest.transform.localPosition = startPos;

            // Turn on turn sign
            SoundManager.Instance.PlaySFX(SFXType.Sequece_TurnSign);
            tSignMask = GameManager.Instance.TurnSignMask[GameManager.Instance.IsFirstSelectPlayer ? 1 : 0];
            tSignMaskDest = GameManager.Instance.TurnSignMaskDestPosition[GameManager.Instance.IsFirstSelectPlayer ? 1 : 0];
            startPos = tSignMask.transform.localPosition;
            destPos = tSignMaskDest.transform.localPosition;
            while (tSignMask.transform.localPosition.y > tSignMaskDest.transform.localPosition.y + 0.01f || tSignMask.transform.localPosition.y < tSignMaskDest.transform.localPosition.y - 0.01f)
            {
                tSignMask.transform.localPosition = new Vector3(
                     tSignMask.transform.localPosition.x,
                      tSignMask.transform.localPosition.y + (destPos.y - startPos.y) / 20,
                       tSignMask.transform.localPosition.z
                    );
                yield return FadeWait;
            }
            tSignMask.transform.localPosition = destPos;
            tSignMaskDest.transform.localPosition = startPos;

            // Wait for selecting
            StartCoroutine(timer());

            yield return null;
        }

        private IEnumerator timer()
        {
            SoundManager.Instance.PlaySFX(SFXType.Sequece_Timer, true);
            GameManager.Instance.TimerImage.fillAmount = 0;
            while (PassedTime < GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
            {
                GameManager.Instance.TimerImage.fillAmount +=
                   0.001f / GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds;
                PassedTime += 0.001f;
                yield return ServerWait;
            }

            // If not selected until timeout
            if (!isSent)
            {
                SoundManager.Instance.StopSFX();
                SoundManager.Instance.PlaySFX(SFXType.Sequece_TimeOver);
                isSent = true;
                
                if (GameManager.Instance.SecondSelectedCard == -1)
                {
                    GameManager.Instance.SecondSelectedCard = GameManager.Instance.PickRandomCard();
                }

                StartCoroutine(endCoroutine());
            }
        }

        // Attached to "End turn Button"
        public void EndSelection()
        {
            if (!isSent)
            {
                isSent = true;
                Debug.Log("EndSelection()");

                if (GameManager.Instance.SecondSelectedCard == -1)
                {
                    Debug.Log("Card not selected");
                    isSent = false;
                    GameManager.Instance.OpenWrongSelectCanvasGroup();
                    return;
                }

                SoundManager.Instance.StopSFX();
                SoundManager.Instance.PlaySFX(SFXType.Sequece_Select);
                StopCoroutine(timer());
                StartCoroutine(endCoroutine());
            }
        }

        // Called by button or timeout
        private IEnumerator endCoroutine()
        {
            GameManager.Instance.SetActiveFakeSelectButton(true);
            GameManager.Instance.CannotBeSelectedEffect(true);

            // Set card state
            GameManager.Instance.SetAllFrontCardsState(CardState.None);
            GameManager.Instance.ResetFrontCards();

            GameManager.Instance.SetSubmit(3, GameManager.Instance.SecondSelectedCard);

            yield return new WaitForSeconds(0.5f);

            EndMySequence(new object[] { GameManager.Instance.SecondSelectedCard });
            yield return null;
        }
    }
}