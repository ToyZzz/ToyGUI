using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ToyGUI
{
    public class FUIPackageItemFont : FUIPackageItemBase
    {
        public int width;
        public int height;
        public string file;
        public bool exported;
        public FUITexture m_texture;
        public FUIByteBuffer m_rawData;
        public FUIBitmapFont m_bitmapFont;

        private void LoadFont()
        {
            FUIBitmapFont font = new FUIBitmapFont(this);
            m_bitmapFont = font;
            // m_rawData操作
            bool ttf = true; // m_rawData.ReadBool();

            float texScaleX = 1;
            float texScaleY = 1;
            FUITexture mainTexture = null;
            FUIAtlasSprite mainSprite = null;
            if (ttf &&  m_owner.SpriteDic.TryGetValue(m_id, out mainSprite))
            {
                mainTexture = mainSprite.m_atlas.LoadAsset();
                texScaleX = mainTexture.Root.m_uvRect.width / mainTexture.Width;
                texScaleY = mainTexture.Root.m_uvRect.height / mainTexture.Height;
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