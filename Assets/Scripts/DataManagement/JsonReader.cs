using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGYB {
    public class JsonReader
    {
        public static GameSequence[] ReadSequence()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Sequences");
            GameSequence[] gameSequences = JsonUtility.FromJson<SequenceReader>(textAsset.ToString()).GameSequences;
            foreach (GameSequence g in gameSequences)
            {
                g.Mapping();
            }
            return gameSequences;
        }
    }

    class SequenceReader
    {
        public GameSequence[] GameSequences;
    }
}