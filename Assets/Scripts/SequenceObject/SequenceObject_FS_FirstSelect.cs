using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_FirstSelect : SequenceObject
    {
        private bool isSent = false;
        private const float fadingTime = 0.5f;

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

            // Turn on turn sign
            GameObject tSign = GameManager.Instance.TurnSign[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
            tSign.SetActive(true);
            SpriteRenderer spriteRenderer = tSign.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            while (spriteRenderer.color.a < 1)
            {
                spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.01f);
                yield return new WaitForSecondsRealtime(0.01f * fadingTime);
            }

            // Wait for selecting
            StartCoroutine(timer());

            yield return null;
        }

        private IEnumerator timer()
        {
            while (PassedTime < GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
            {
                PassedTime += 0.001f;
                yield return new WaitForSecondsRealtime(0.001f);
            }

            // If not selected until timeout
            if (!isSent)
            {
                isSent = true;

                if (GameManager.Instance.FirstSelctedCard == -1)
                {
                    GameManager.Instance.FirstSelctedCard = GameManager.Instance.PickRandomCard();
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

                if (GameManager.Instance.FirstSelctedCard == -1)
                {
                    Debug.Log("Card not selected");
                    isSent = false;
                    GameManager.Instance.OpenWrongSelectCanvasGroup();
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

            // Set card state
            GameManager.Instance.SetAllFrontCardsState(CardState.None);
            GameManager.Instance.ResetFrontCards();

            GameManager.Instance.SetSubmit(3, GameManager.Instance.FirstSelctedCard);

            yield return new WaitForSecondsRealtime(0.5f);

            EndMySequence(new object[] { GameManager.Instance.FirstSelctedCard });
            yield return null;
        }
    }
}