using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIPackageItemImage : FUIPackageItemBase
    {
        public int m_width;
        public int m_height;
        public string m_file;
        public bool m_exported;

        public Rect? scale9Grid;
        public bool scaleByTile;
        public int tileGridIndice;
        // public PixelHitTestData pixelHitTestData;

        private void LoadImage()
        {

        }

        public override object Load()
        {
            return base.Load();
        }
    }
}