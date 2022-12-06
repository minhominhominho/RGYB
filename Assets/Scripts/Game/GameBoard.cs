using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGYB
{
    public class GameBoard : MonoBehaviour
    {
        private void OnMouseUpAsButton()
        {
            if (GameManager.Instance.CheckPanelClosed())
                SoundManager.Instance.PlaySFX(SFXType.GameBoard_ClickAnywhere);
        }
    }
}