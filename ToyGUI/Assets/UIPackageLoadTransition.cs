using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FairyGUI;
using FairyGUI.Utils;

public class UIPackageLoadTransition : Editor
{
    public static string LoadTargetPackagePath = "Assets/UIres/TestPackageTran";

    [MenuItem("KaneTempTest/UIPackageTransition")]
    public static void UIPackageCheck()
    {
        UIPackage.RemoveAllPackages();
        UIPackage testPackage = UIPackage.AddPackage(LoadTargetPackagePath);


        UIPackage.RemoveAllPackages();
    }

    public static void AnalysisPackage(UIPackage uiPackage)
    {
        List<PackageItem> packageItemList = new List<PackageItem>();
        foreach (var packageItem in packageItemList)
        {
            if (packageItem.type == PackageItemType.Component)
            {

            }
        }
    }

    public static void AnalysisComponent(PackageItem packageItem)
    {
        ByteBuffer buffer = packageItem.rawData;
        buffer.Seek(0, 5);

        int transitionCount = buffer.ReadShort();
        for (int i = 0; i < transitionCount; i++)
        {
            int nextPos = buffer.ReadShort();
            nextPos += buffer.position;

            // Transition trans = new Transition(this);
            // trans.Setup(buffer);
            // _transitions.Add(trans);
            TransitionDeal(buffer);

            buffer.position = nextPos;
        }
    }

    public static void TransitionDeal(ByteBuffer buffer)
    {
        string name = buffer.ReadS();
        int _options = buffer.ReadInt();
        bool _autoPlay = buffer.ReadBool();
        int _autoPlayTimes = buffer.ReadInt();
        float _autoPlayDelay = buffer.ReadFloat();

        int cnt = buffer.ReadShort();
        TransitionItemInfo[] _items = new TransitionItemInfo[cnt];
        for (int i = 0; i < cnt; i++)
        {
            int dataLen = buffer.ReadShort();
            int curPos = buffer.position;
            buffer.Seek(curPos, 0);

        }
    }

    
}

public class TransitionItemInfo
{

}

public class TweenConfig
{

}