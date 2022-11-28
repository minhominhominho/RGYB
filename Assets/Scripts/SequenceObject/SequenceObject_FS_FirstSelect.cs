using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_FirstSelect : SequenceObject
    {
        //private bool isSent = false;
        //[SerializeField] private Transform selectedEffect;
        //private const float fadingTime = 0.5f;

        //public override IEnumerator SequenceJob()
        //{
        //    // Set card state
        //    GameManager.Instance.SetAllFrontCardsState(CardState.Selective);
        //    GameManager.Instance.SetAllBackCardsState(CardState.None);

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

        //    // Wait for selecting
        //    StartCoroutine(timer());

        //    yield return null;
        //}

        //private IEnumerator timer()
        //{
        //    while (PassedTime < GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
        //    {
        //        PassedTime += 0.001f;
        //        yield return new WaitForSecondsRealtime(0.001f);
        //    }

        //    // If not selected until timeout
        //    if (!isSent)
        //    {
        //        isSent = true;

        //        if (GameManager.Instance.FirstSelctedCard == -1)
        //        {
        //            GameManager.Instance.FirstSelctedCard = GameManager.Instance.PickRandomCard();
        //        }

        //        StartCoroutine(endCoroutine());
        //    }
        //}

        //// Attached to "End turn Button" or Called when sequence timeout occured
        //public void EndSelection()
        //{
        //    if (!isSent)
        //    {
        //        isSent = true;
        //        Debug.Log("EndSelection()");

        //        if (GameManager.Instance.FirstSelctedCard == -1)
        //        {
        //            Debug.Log("Card not selected");
        //            isSent = false;
        //            // TODO : Effect or Popup
        //            return;
        //        }

        //        StopCoroutine(timer());
        //        StartCoroutine(endCoroutine());
        //    }
        //}

        //// Called by button or timeout
        //private IEnumerator endCoroutine()
        //{
        //    // Lock
        //    GameManager.Instance.SetFrontCardState(GameManager.Instance.FirstSelctedCard, CardState.NotBeChanged);

        //    // Move selected card using fadein & fadeout
        //    //GameObject g = GameManager.Instance.FrontCards[GameManager.Instance.FirstSelctedCard];
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

        //    EndMySequence(new object[] { 
        //        GameManager.Instance.FirstSelctedCard, 
        //        GameManager.Instance.GetBackCardNum(GameManager.Instance.FirstSelctedCard)
        //    });
        //    yield return null;
        //}
    }
}