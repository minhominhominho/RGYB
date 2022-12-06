using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RGYB
{
    public class SequenceObject_SS_Result : SequenceObject
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private GameObject Card;
        [SerializeField] private GameObject OpponentCard;

        public override IEnumerator SequenceJob()
        {
            GameManager.Instance.TimerImage.fillAmount = 0;

            OpponentCard.GetComponent<SpriteRenderer>().sprite
               = GameManager.Instance.CardSprites[GameManager.Instance.FirstSelctedCard];
            OpponentCard.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 255);

            // Card Open
            SoundManager.Instance.PlaySFX(SFXType.Card_Open);
            float rot = 0;
            while (rot < 180)
            {
                rot += 180 / 100;
                Card.transform.rotation = Quaternion.Euler(0, rot, 0);
                yield return new WaitForSecondsRealtime(0.01f * FadingTime);
            }
            Card.transform.rotation = Quaternion.Euler(0, 180, 0);

            yield return new WaitForSecondsRealtime(1f);

            GameResult gs = GameManager.Instance.GetResult();
            if (gs == GameResult.WinAlone) resultText.text = "�ܵ� �¸�!";
            else if (gs == GameResult.WinTogether) resultText.text = "���� �¸�!";
            else if (gs == GameResult.LoseAlone) resultText.text = "�ܵ� �й�..";
            else if (gs == GameResult.LoseTogether) resultText.text = "���� �й�..";

            if (gs == GameResult.WinAlone || gs == GameResult.WinTogether)
                SoundManager.Instance.PlaySFX(SFXType.Sequece_Victory);

            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup(false);

            yield return null;
        }
    }
}