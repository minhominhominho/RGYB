using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_OpenNonSelected : SequenceObject
    {
        private bool isSent = false;

        public override IEnumerator SequenceJob()
        {
            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup();

            while (!GameManager.Instance.CheckPanelClosed())
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }

            GameManager.Instance.SetActiveFakeSelectButton(false);

            // Set card state
            GameManager.Instance.SetAllFrontCardsState(CardState.Selective);

            GameManager.Instance.CannotBeSelectedEffect(false);

            // Wait for selecting
            StartCoroutine(timer());

            yield return null;
        }

        private IEnumerator timer()
        {
            GameManager.Instance.TimerImage.fillAmount = 0;
            while (PassedTime < GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
            {
                GameManager.Instance.TimerImage.fillAmount +=
                    0.001f / GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds;
                PassedTime += 0.001f;
                yield return new WaitForSecondsRealtime(0.001f);
            }

            // If not selected until timeout
            if (!isSent)
            {
                isSent = true;

                if (GameManager.Instance.OpenedCard == -1)
                {
                    GameManager.Instance.OpenedCard = GameManager.Instance.PickRandomCard();
                }

                StartCoroutine(endCoroutine());
            }
        }

        // Attached to "End turn Button" or Called when sequence timeout occured
        public void EndSelection()
        {
            if (!isSent)
            {
                isSent = true;
                Debug.Log("EndSelection()");

                if (GameManager.Instance.OpenedCard == -1)
                {
                    Debug.Log("Card not selected");
                    isSent = false;
                    // TODO : Effect or Popup
                    return;
                }

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

            GameManager.Instance.SetSubmit(2, GameManager.Instance.OpenedCard);
            yield return new WaitForSecondsRealtime(0.5f);
            GameManager.Instance.OpenSignEffect();
            yield return new WaitForSecondsRealtime(0.5f);

            EndMySequence(new object[] { GameManager.Instance.OpenedCard });
            yield return null;
        }
    }
}