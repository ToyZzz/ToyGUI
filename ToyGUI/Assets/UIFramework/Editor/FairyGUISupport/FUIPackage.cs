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

        public string AssetPath { get; private set; }

        private List<FUIPackageItem> _itemList;

        private Dictionary<string, FUIPackageItem> _itemsByIdDic;

        private Dictionary<string, FUIPackageItem> _itemsByNameDic;

        private Dictionary<string, FUIAtlasSprite> _spriteDic;

        public FUIPackage()
        {
            _itemList = new List<FUIPackageItem>();
            _itemsByIdDic = new Dictionary<string, FUIPackageItem>();
            _itemsByNameDic = new Dictionary<string, FUIPackageItem>();
            _spriteDic = new Dictionary<string, FUIAtlasSprite>();
        }

        public bool LoadPackage()
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

        public List<FUIPackageItem> GetItems()
        {
            return _itemList;
        }
    }
}