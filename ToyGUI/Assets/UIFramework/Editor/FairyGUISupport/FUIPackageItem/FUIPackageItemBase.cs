using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public enum PackageItemType
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

    public enum ObjectType
    {
        Image,
        MovieClip,
        Swf,
        Graph,
        Loader,
        Group,
        Text,
        RichText,
        InputText,
        Component,
        List,
        Label,
        Button,
        ComboBox,
        ProgressBar,
        Slider,
        ScrollBar
    }

    public class FUIPackageItemBase
    {
        public FUIPackage m_owner;

        public PackageItemType m_type;

        public string m_id;
        public string m_name;
        // public NTexture texture;
        // public ByteBuffer rawData;

        public virtual object Load()
        {
            return null;
        }
    }
}