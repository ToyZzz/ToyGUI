using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIPackage
    {
        public const string URL_PREFIX = "ui://";

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string AssetPath { get; set; }

        private List<FUIPackageItemBase> _itemList;

        private Dictionary<string, FUIPackageItemBase> _itemsByIdDic;

        private Dictionary<string, FUIPackageItemBase> _itemsByNameDic;

        private Dictionary<string, FUIAtlasSprite> _spriteDic; 

        public delegate object LoadResource(string name, string extension, System.Type type);

        public LoadResource m_loadFunc;

        public FUIPackage()
        {
            _itemList = new List<FUIPackageItemBase>();
            _itemsByIdDic = new Dictionary<string, FUIPackageItemBase>();
            _itemsByNameDic = new Dictionary<string, FUIPackageItemBase>();
            _spriteDic = new Dictionary<string, FUIAtlasSprite>();
        }

        public Dictionary<string, FUIAtlasSprite> SpriteDic
        {
            get { return _spriteDic; }
        }

        public bool LoadPackage(FUIByteBuffer buffer, string packageSource, string assetNamePrefix)
        {
            return false;
        }

        public bool CreateObject(string resName)
        {
            return false;
        }

        public object GetItemAsset(string resName)
        {
            return null;
        }

        public List<FUIPackageItemBase> GetItems()
        {
            return _itemList;
        }
    }
}