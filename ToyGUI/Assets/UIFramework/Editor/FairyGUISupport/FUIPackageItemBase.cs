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
        public FUIPackageItem m_owner;

        public PackageItemType m_type;

        public ObjectType m_objectType;

        public string id;
        public string name;
        public int width;
        public int height;
        public string file;
        public bool exported;
        // public NTexture texture;
        // public ByteBuffer rawData;
    }
}