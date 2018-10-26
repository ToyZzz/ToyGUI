using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUITexture
{
    /// <summary>
    /// UV坐标
    /// </summary>
    public Rect m_uvRect;

    /// <summary>
    /// 是否旋转
    /// </summary>
    public bool m_rotated;

    public int m_refCount;

    public float m_lastActive;

    private Texture _nativeTexture;

    private Texture _alphaTexture;

    private Rect _region;

    private FUITexture _root;

    private static FUITexture _empty;

    public FUITexture(Texture texture)
    {

    }

    public FUITexture(Texture texture, Texture alphaTexture, float xScale, float yScale)
    {

    }

    public FUITexture(Texture texture, Rect region)
    {

    }

    public FUITexture(FUITexture root, Rect region, bool rotated)
    {

    }

    public static FUITexture CreateEmptyTexture()
    {
        return null;
    }
}
