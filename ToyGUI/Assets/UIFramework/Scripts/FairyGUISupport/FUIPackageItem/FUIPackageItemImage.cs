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

        public FUITexture m_fTexture;

        /// <summary>
        /// 是否是九宫格
        /// </summary>
        public Rect? scale9Grid;
        
        /// <summary>
        /// 九宫格中需要使用平铺的格子
        /// </summary>
        public int tileGridIndice;

        /// 上下两个互斥，一共三种状态，无，九宫格，平铺适应变化

        /// <summary>
        /// 修改使用平铺来适应scale变化
        /// </summary>
        public bool scaleByTile;
        // public PixelHitTestData pixelHitTestData;

        private void LoadImage()
        {
            FUIAtlasSprite sprite;
            if (m_owner.SpriteDic.TryGetValue(m_id, out sprite))
            {
                m_fTexture = new FUITexture(sprite.m_atlas.LoadAsset(),sprite.m_rect,sprite.m_rotated);
            }
            else
            {
                m_fTexture = FUITexture.Empty();
            }
        }

        public FUITexture LoadAsset()
        {
            return null;
        }

        public override object Load()
        {
            return base.Load();
        }
    }
}