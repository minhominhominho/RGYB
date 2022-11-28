using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_FirstSelect : SequenceObject
    {
        //private const float fadingTime = 0.5f;

        //public override IEnumerator SequenceJob()
        //{
        //    // Set card state
        //    GameManager.Instance.SetAllFrontCardsState(CardState.None);
        //    GameManager.Instance.SetAllBackCardsState(CardState.Receiving);

        //    // Turn on turn sign
        //    GameObject tSign = GameManager.Instance.TurnSign[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
        //    tSign.SetActive(true);
        //    SpriteRenderer spriteRenderer = tSign.GetComponent<SpriteRenderer>();
        //    spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        //    while (spriteRenderer.color.a < 1)
        //    {
        //        spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.01f);
        //        yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    }

        //    // Wait first select player
        //    while (GameManager.Instance.FirstSelctedCard == -1)
        //    {
        //        PassedTime += 0.001f;
        //        yield return new WaitForSecondsRealtime(0.001f);
        //    }

        //    if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
        //        Debug.LogError("TimeOut : Something bad happens");

        //    // No Lock

        //    // Move selected card using fadein & fadeout
        //    //GameObject g = GameManager.Instance.BackCards[GameManager.Instance.MatchedCardNum];
        //    //SpriteRenderer spr = g.GetComponent<SpriteRenderer>();

        //    //while (spr.color.a > 0)
        //    //{
        //    //    spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, spr.color.a - 0.01f);
        //    //    yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    //}
        //    //g.transform.position = selectedCardDest.position;
        //    //g.transform.rotation = selectedCardDest.rotation;
        //    //g.transform.localScale = selectedCardDest.localScale;
        //    //while (spr.color.a < 1)
        //    //{
        //    //    spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, spr.color.a + 0.01f);
        //    //    yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    //}

        //    EndMySequence(new object[] { });
        //}
    }
}