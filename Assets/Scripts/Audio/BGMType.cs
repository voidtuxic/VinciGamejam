using System;
using UnityEngine;

namespace Void.Audio
{
    public enum BGMType
    {
        Menu,
        Peace,
        Battle,
        GameOver
    }

    [Serializable]
    public class BGMData
    {
        public BGMType Type;
        public AudioClip Clip;
    }
}