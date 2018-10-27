using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
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

        public void Reload(Texture nativeTexture, Texture alphaTexture)
        {

        }

        public static Texture2D CreateEmptyTexture()
        {
            return null;
        }

        public static FUITexture Empty()
        {
            return null;
        }

        public FUITexture Root
        {
            get { return _root; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get { return _region.width; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get { return _region.height; }
        }

        public int WidthInt
        {
            get { return (int)_region.width; }
        }

        public int HeightInt
        {
            get { return (int)_region.height; }
        }
    }
}