using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FairyGUIPlus
{
    public enum UIPackageItemType
    {
        Image,
        MovieClip,
        Sound,
        Component,
        Atlas,
        Font,
        Swf,
        Misc,
        Unknown
    }

    public class UIPackageItemBase
    {
        public UIPackage m_owner;

        public UIPackageItemType m_type;

        public string m_id;

        public virtual object Load()
        {
            return null;
        }
    }
}

