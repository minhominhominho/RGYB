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
        [SerializeField] private TextMeshProUGUI scoreText;
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
                rot += 180 / 20;
                Card.transform.rotation = Quaternion.Euler(0, rot, 0);
                yield return FadeWait;
            }
            Card.transform.rotation = Quaternion.Euler(0, 180, 0);

            yield return new WaitForSeconds(1f);

            GameResult gs = GameManager.Instance.GetResult();
            int preScore = DataManager.Instance.GetScore();
            int getScore = 0;
            if (gs == GameResult.WinAlone)
            {
                resultText.text = "단독 승리!";
                getScore = 100;
            }
            else if (gs == GameResult.WinTogether)
            {
                resultText.text = "공동 승리!";
                getScore = 50;
            }
            else if (gs == GameResult.LoseAlone)
            {
                resultText.text = "단독 패배..";
                getScore = -100;
            }
            else if (gs == GameResult.LoseTogether)
            {
                resultText.text = "공동 패배..";
                getScore = -50;
            }
            DataManager.Instance.SetScore(preScore + getScore);

            scoreText.text =
                "점수 : " + DataManager.Instance.GetScore() + " (" + (getScore > 0 ? "+" : "") + getScore + ")";

            if (gs == GameResult.WinAlone || gs == GameResult.WinTogether)
                SoundManager.Instance.PlaySFX(SFXType.Sequece_Victory);

            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup(false);

            yield return null;
        }
    }
}