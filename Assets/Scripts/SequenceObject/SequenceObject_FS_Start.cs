using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RGYB
{
    public class SequenceObject_FS_Start : SequenceObject
    {
        [SerializeField] private Transform maskDestWorldPosition;

        public override IEnumerator SequenceJob()
        {
            // Roll Game Scroll
            SoundManager.Instance.PlaySFX(SFXType.GameBoard_Scroll);
            Vector3 origPos = GameManager.Instance.GameBoardMask.transform.position;
            Vector3 destPos = maskDestWorldPosition.position;
            while (GameManager.Instance.GameBoardMask.transform.position.x < destPos.x)
            {
                GameManager.Instance.GameBoardMask.transform.position = new Vector3(
                    GameManager.Instance.GameBoardMask.transform.position.x + (destPos.x - origPos.x) / 100, origPos.y, origPos.z
                    );
                yield return ScrollWait;
            }

            // FadeIn Cards
            SoundManager.Instance.PlaySFX(SFXType.Card_Create);
            List<SpriteRenderer> spr = new List<SpriteRenderer>();

            for (int i = 0; i < GameManager.Instance.FrontCards.Length; i++)
            {
                spr.Add(GameManager.Instance.FrontCards[i].GetComponent<SpriteRenderer>());
                spr.Add(GameManager.Instance.BackCards[i].GetComponent<SpriteRenderer>());
                spr[i].color = new Vector4(spr[i].color.r, spr[i].color.g, spr[i].color.b, 0);
                spr[i + 1].color = new Vector4(spr[i + 1].color.r, spr[i + 1].color.g, spr[i + 1].color.b, 0);
            }

            while (spr[0].color.a < 1)
            {
                foreach (SpriteRenderer s in spr)
                {
                    s.color = new Vector4(s.color.r, s.color.g, s.color.b, s.color.a + 0.05f);
                }
                yield return FadeWait;
            }

            // Call Next Sequence
            EndMySequence(new object[] { });
        }
    }
}