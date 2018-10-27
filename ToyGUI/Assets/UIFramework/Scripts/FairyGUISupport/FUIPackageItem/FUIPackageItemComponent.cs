using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIPackageItemComponent : FUIPackageItemBase
    {
        public int m_width;
        public int m_height;
        public string m_file;
        public bool m_exported;
        public FUIByteBuffer m_rawData;
        public bool translated;
        public ObjectType m_objectType;
        public FUIObjectFactory.GComponentCreator extensionCreator;
    }
}