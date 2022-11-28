using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_OpenNonSelected : SequenceObject
    {
        //private bool isSent = false;
        //[SerializeField] private Transform selectedCardDest;
        //[SerializeField] private GameObject openEffect;
        //private const float fadingTime = 0.5f;

        //public override IEnumerator SequenceJob()
        //{
        //    // Wait for selecting
        //    StartCoroutine(Timer());

        //    yield return null;
        //}

        //private IEnumerator Timer()
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
        //        // Not select until timeout
        //        if (GameManager.Instance.OpenedCard == -1)
        //        {
        //            GameManager.Instance.OpenedCard = GameManager.Instance.PickRandomCard();
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

        //        if (GameManager.Instance.OpenedCard == -1)
        //        {
        //            Debug.Log("Card not selected");
        //            isSent = false;
        //            // TODO : Effect or Popup
        //            return;
        //        }

        //        StopCoroutine(Timer());
        //        StartCoroutine(endCoroutine());
        //    }
        //}

        //// Called by button or timeout
        //private IEnumerator endCoroutine()
        //{
        //    // No Lock

        //    // Move selected card using fadein & fadeout
        //    GameObject g = GameManager.Instance.FrontCards[GameManager.Instance.OpenedCard];
        //    SpriteRenderer spr = g.GetComponent<SpriteRenderer>();

        //    while (spr.color.a > 0)
        //    {
        //        spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, spr.color.a - 0.01f);
        //        yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    }
        //    g.transform.position = selectedCardDest.position;
        //    g.transform.rotation = selectedCardDest.rotation;
        //    g.transform.localScale = selectedCardDest.localScale;
        //    while (spr.color.a < 1)
        //    {
        //        spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, spr.color.a + 0.01f);
        //        yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    }

        //    // '+' fadein
        //    openEffect.SetActive(true);
        //    spr = openEffect.GetComponent<SpriteRenderer>();
        //    spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, 0);
        //    while (spr.color.a < 1)
        //    {
        //        spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, spr.color.a + 0.01f);
        //        yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    }

        //    EndMySequence(new object[] {
        //        GameManager.Instance.OpenedCard,
        //        GameManager.Instance.GetBackCardNum(GameManager.Instance.OpenedCard)
        //    });

        //    yield return null;
        //}
    }
}