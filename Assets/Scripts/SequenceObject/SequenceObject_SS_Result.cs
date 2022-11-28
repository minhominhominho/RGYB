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

        public override IEnumerator SequenceJob()
        {
            // TODO : Effect

            resultText.text = Enum.GetName(typeof(GameResult), GameManager.Instance.GetResult());

            yield return new WaitForSecondsRealtime(GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds);

            GameManager.Instance.ExitButton();
        }
    }
}