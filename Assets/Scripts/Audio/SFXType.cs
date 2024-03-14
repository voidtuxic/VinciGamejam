using System;
using UnityEngine;

public enum SFXType
{
    WaveStart,
    Projectile1,
    Projectile2,
    Projectile3,
    Hit,
    Kill,
}

[Serializable]
public class SFXData
{
    public SFXType Type;
    public AudioClip Clip;
}