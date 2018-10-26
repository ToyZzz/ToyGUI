﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIPackageItemAtlas : FUIPackageItemBase
    {
        public int m_width;
        public int m_height;
        public string m_file;
        public bool m_exported;

        public FUITexture m_fTexture;

        private void LoadAtlas()
        {
            string ext = Path.GetExtension(this.m_file);
            string fileName = m_file.Substring(0, m_file.Length - ext.Length);
            Texture tex = null;
            Texture alphaTex = null;

            tex = (Texture) m_owner.Loadfunc(fileName, ext, typeof(Texture));
            if (tex == null)
            {
                Debug.LogWarning("FairyGUI: texture '" + m_file + "' not found in " + m_name);
            }

            fileName = fileName + "!a";
            alphaTex = (Texture2D)m_owner.Loadfunc(fileName, ext, typeof(Texture2D));

            if (tex == null)
            {

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