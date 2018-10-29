using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using FairyGUI.Utils;
using UnityEngine;
using UnityEditor;

public class TestPackageLoad : Editor
{
    [MenuItem("FUITest/Analysis PackageData")]
    public static void TestAnalysisPackageData()
    {
        Debug.Log("Analysis PackageData");
        string testPackageDescPath = "Assets/UIres/TestPackage1";
        UIPackage package = UIPackage.AddPackage(testPackageDescPath);
        if (package == null)
        {
            Debug.Log("Package Load Fail");
            return;
        }

        AnalysisPackage(package);
    }

    public static void AnalysisPackage(UIPackage uiPackage)
    {
        List<PackageItem> packageItemList = uiPackage.GetItems();
        foreach (var item in packageItemList)
        {
            switch (item.type)
            {
                case PackageItemType.Atlas:
                    CheckAtlas(item);
                    break;

                case PackageItemType.Component:
                    CheckComponent(item);
                    break;

                case PackageItemType.Font:

                    break;

                case PackageItemType.Image:
                    CheckImage(item);
                    break;

                case PackageItemType.Misc:

                    break;

                case PackageItemType.MovieClip:
                    CheckMovieClip(item);
                    break;

                case PackageItemType.Sound:

                    break;

                default:
                    Debug.Log("PackageItem Error Type");
                    break;
            }
        }
    }

    public static void CheckAtlas(PackageItem packageItem)
    {
        NTexture tex = packageItem.owner.GetItemAsset(packageItem) as NTexture;
        //    Debug.Log("Atlas Size -> " + tex.width + " * " + tex.height);

    }

    public static void CheckImage(PackageItem packageItem)
    {
        NTexture tex = packageItem.owner.GetItemAsset(packageItem) as NTexture;
        //    Debug.Log("Image Size -> " + tex.width + " * " + tex.height);
        if (packageItem.scale9Grid != null)
        {
            if (packageItem.tileGridIndice != 0)
            {
                // Debug.Log("Use tileGridIndice");
            }
        }
    }

    public static void CheckFont(PackageItem packageItem)
    {

    }

    public static void CheckSound(PackageItem packageItem)
    {

    }

    public static void CheckMovieClip(PackageItem packageItem)
    {

    }

    public static void CheckMisc(PackageItem packageItem)
    {

    }

    public static PackageItem m_curPackageItem = null;

    public static void CheckComponent(PackageItem packageItem)
    {
        m_curPackageItem = packageItem;
        ByteBuffer byteBuffer = packageItem.owner.GetItemAsset(packageItem) as ByteBuffer;
        DealCompRawData(byteBuffer);
        /*
        Debug.Log(packageItem.objectType.ToString());
        GComponent comp = UIPackage.CreateObject(packageItem.owner.name, packageItem.name) as GComponent;
        if (comp != null)
        {
            if (comp.mask != null)
            {
                Debug.Log(packageItem.name + "设置了一个遮罩");
            }
        }

        Debug.Log("Child Num: " + comp.GetChildren().Length);
        foreach (var child in comp.GetChildren())
        {
            Debug.Log(child.name);
            if (child.filter != null)
            {
                Debug.Log(child.parent.name + "设置了滤镜");
            }

            if (child.asCom != null)
            {

            }
        }
        */

    }

    public static void DealCompRawData(ByteBuffer buffer)
    {
        buffer.Seek(0, 0);
        buffer.ReadInt();
        buffer.ReadInt();

        if (buffer.ReadBool())
        {
            buffer.ReadInt();
            buffer.ReadInt();
            buffer.ReadInt();
            buffer.ReadInt();
        }

        if (buffer.ReadBool())
        {
            buffer.ReadFloat();
            buffer.ReadFloat();
        }

        if (buffer.ReadBool())
        {
            buffer.ReadInt();
            buffer.ReadInt();
            buffer.ReadInt();
            buffer.ReadInt();
        }

        OverflowType overflow = (OverflowType) buffer.ReadByte();
        if (overflow == OverflowType.Scroll)
        {
            int savedPos = buffer.position;
            buffer.Seek(0, 7);
            // SetupScroll(buffer);
            // 使用了scrollPanel
            buffer.position = savedPos;
        }
        else
        {
            // SetupOverflow(overflow);
            if (overflow == OverflowType.Hidden)
            {
                // 使用了隐藏
            }
        }

        if (buffer.ReadBool())
        {
            buffer.ReadInt();
            buffer.ReadInt();
            // clipSoftness设置
        }

        // 初始化控制器组件
        buffer.Seek(0, 1);
        int controllerCount = buffer.ReadShort();
        for (int i = 0; i < controllerCount; i++)
        {
            int nextPos = buffer.ReadShort();
            nextPos += buffer.position;
            /*
            Controller controller = new Controller();
            _controllers.Add(controller);
            controller.parent = this;
            controller.Setup(buffer);
            */
            buffer.position = nextPos;
        }

        buffer.Seek(0, 2);
        GObject child;
        int childCount = buffer.ReadShort();
        for (int i = 0; i < childCount; i++)
        {
            int dataLen = buffer.ReadShort();
            int curPos = buffer.position;

            // if (objectPool != null)
            //     child = objectPool[poolIndex + i];
            // else
            //{
            buffer.Seek(curPos, 0);

            ObjectType type = (ObjectType) buffer.ReadByte();
            string src = buffer.ReadS();
            string pkgId = buffer.ReadS();

            PackageItem pi = null;
            if (src != null)
            {
                UIPackage pkg;
                if (pkgId != null)
                    pkg = UIPackage.GetById(pkgId);
                else
                    pkg = m_curPackageItem.owner;

                pi = pkg != null ? pkg.GetItem(src) : null;
            }

            if (pi != null)
            {
                Debug.Log("pi的名字是  " + pi.name);
                Debug.Log("pi的类型是  " + pi.objectType);
                // child = UIObjectFactory.NewObject(pi);
                // child.packageItem = pi;
                // child.ConstructFromResource();
            }
            else
            {
                // child = UIObjectFactory.NewObject(type);
                Debug.Log("child的类型是  " + type);
            }

            // }

            // child.underConstruct = true;
             child.Setup_BeforeAdd(buffer, curPos);
            // child.InternalSetParent(this);
            // _children.Add(child);

            buffer.position = curPos + dataLen;
        }
    }
}
