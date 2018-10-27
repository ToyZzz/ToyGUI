using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIPackageItemMovieClip : FUIPackageItemBase
    {
        public int width;
        public int height;
        public string file;
        public bool exported;
     
        public FUIByteBuffer m_rawData;

        public float interval;
        public float repeatDelay;
        public bool swing;
        public MovieClipFrame[] frames;

    }

    public class MovieClipFrame
    {

    }
}