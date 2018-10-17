using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestDealItem : Editor
{
    [MenuItem("Test/Test Load")]
    public static void TestDealPackageDesc()
    {
        string descPath = "Assets/Resources/Package1";
        TestDealItem.DealPackageDesc(descPath);
    }

    public static void DealPackageDesc(string descFilePath)
    {
        string assetPath = string.Format("{0}{1}{2}", descFilePath, "_fui", ".bytes");
        TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
        if (asset == null)
        {
            Debug.Log("Asset is Null");
        }
        ByteBuffer buffer = new ByteBuffer(asset.bytes);
        FUIPackage package = new FUIPackage();
        package.LoadPackage(buffer);
    }
}
