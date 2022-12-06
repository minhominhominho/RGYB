using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RGYB
{
    public class SequenceObject_FS_Result : SequenceObject
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private GameObject Card;
        [SerializeField] private GameObject OpponentCard;

        public override IEnumerator SequenceJob()
        {
            OpponentCard.GetComponent<SpriteRenderer>().sprite
                = GameManager.Instance.CardSprites[GameManager.Instance.SecondSelectedCard];
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
            if (gs == GameResult.WinAlone) resultText.text = "단독 승리!";
            else if (gs == GameResult.WinTogether) resultText.text = "공동 승리!";
            else if (gs == GameResult.LoseAlone) resultText.text = "단독 패배..";
            else if (gs == GameResult.LoseTogether) resultText.text = "공동 패배..";

            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup(false);

            yield return null;
        }
    }
}