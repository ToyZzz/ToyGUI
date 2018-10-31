using System.Collections;
using System.Collections.Generic;
using System.Text;
using FairyGUI;
using FairyGUI.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class UIPackageCheckTool : Editor
{
    public enum NeedCheckReasonType
    {
        UseOtherPackageAssets, // 引用了其他的包          
        UseGray, // 使用了置灰
        UseColorFilter, // 使用了滤镜
        ErrorBlendMode, // 使用了错误的混合模式
        UseGraphMask, // 使用了自定义遮罩
        UseOverflowTypeHidden, // 使用了隐藏的溢出处理
        AssetsMissing// 资源丢失
    }

    public static string[] FairyGUIPackagePath = new[]
    {
        "Assets/ResourcesAssets/GUI"
    };

    public static string[] CommonUINames = new[]
    {
        "CommonUI",
        "CommonBg",
        "CommonIcon",
        "ComponentUI",
        "CollegeIconPage1",
        "CollegeIconPage2",
        "CollegeIconPage3",
        "CollegeIconPage4",
        "CollegeIconPage5",
        "CollegeIconPage6",
        "CollegeIconPage7",
        "CollegeIconPage8",
        "CommanderCard",

    };

    [MenuItem("KaneTempTest/UIPackageCheck")]
    public static void UIPackageCheck()
    {
        UIPackage.RemoveAllPackages();
        string[] assetGUIDs = AssetDatabase.FindAssets("_fui", FairyGUIPackagePath);
        foreach (var assetGUID in assetGUIDs)
        {
            // Debug.Log(assetGUID);
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID).Replace("_fui.bytes", "");
            // Debug.Log(assetPath);
            UIPackage.AddPackage(assetPath);
        }
        m_UIPacakgeAnalysisInfoList = new List<UIPackageAnalysisInfo>();
        List<UIPackage> loadPackageList = UIPackage.GetPackages();
        for (int i = 0; i < loadPackageList.Count; i++)
        {
            AnalysisPackage(loadPackageList[i]);
        }

         OutputAnalysisFile();

        UIPackage.RemoveAllPackages();
    }

    public static UIPackageAnalysisInfo m_curPackageAnalysisInfo = null;

    public static UIPackage m_curAnalysisPackage = null;

    public static List<NeedCheckComponentInfo> m_needCheckComponentList = new List<NeedCheckComponentInfo>();

    public static string m_curCheckGObjectName;

    public static List<UIPackageAnalysisInfo> m_UIPacakgeAnalysisInfoList = new List<UIPackageAnalysisInfo>();

    public static void OutputAnalysisFile()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (UIPackageAnalysisInfo analysisInfo in m_UIPacakgeAnalysisInfoList)
        {
            stringBuilder.AppendLine(analysisInfo.m_name);
            stringBuilder.AppendLine("    ├─── Atlas Infos:");
            foreach (KeyValuePair<NTexture, AtlasAnalysisInfo> kvp in analysisInfo.m_atlasAnalysisInfoDic)
            {
                string atlasName = kvp.Value.m_id.PadRight(10, ' ');
                string atlasWidth = kvp.Value.m_width.ToString().PadRight(4, ' ');
                string atlasHeight = kvp.Value.m_height.ToString().PadRight(4, ' ');
                string atlasFillingRate = string.Format("{0:P}", kvp.Value.m_fillingRate);
                stringBuilder.AppendLine(string.Format("    │   ├─── {0} Size: {1} * {2} FillingRate: {3}", atlasName, atlasWidth, atlasHeight, atlasFillingRate));
            }
            stringBuilder.AppendLine("    ├─── Need Check:");
            foreach (var needCheckItem in analysisInfo.m_needCheckComponentInfoList)
            {
                stringBuilder.AppendLine(string.Format("    │   ├─── {0}", needCheckItem.ToString()));
            }

            stringBuilder.AppendLine("    └───────");
        }

        string filePath = Application.dataPath + "/a.txt";
        FileStream fs = new FileStream(filePath, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.Write(stringBuilder.ToString());
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }

    public static void Clean()
    {

    }

    public static void AnalysisPackage(UIPackage uiPackage)
    {
        m_curPackageAnalysisInfo = new UIPackageAnalysisInfo(uiPackage.name);
        m_curAnalysisPackage = uiPackage;
        List<PackageItem> packageItemList = uiPackage.GetItems();
        foreach (var packageItem in packageItemList)
        {
            switch (packageItem.type)
            {
                case PackageItemType.Atlas:
                    AnalysisItemAtlas(packageItem);
                    break;
                case PackageItemType.Image:
                    AnalysisItemImage(packageItem);
                    break;
                case PackageItemType.Component:
                    AnalysisItemComponent(packageItem);
                    break;
                case PackageItemType.Font:

                    break;
                case PackageItemType.MovieClip:
                    AnalysisItemMovieClip(packageItem);
                    break;
                default:
                    
                    break;
            }
        }

        m_curPackageAnalysisInfo.DoAnalysis();
    }

    public static void AnalysisItemMovieClip(PackageItem packageItem)
    {
        packageItem.Load();
        m_curPackageAnalysisInfo.m_movieClipItemList.Add(packageItem);
    }

    public static void AnalysisItemAtlas(PackageItem packageItem)
    {
        NTexture tex = packageItem.owner.GetItemAsset(packageItem) as NTexture;
        m_curPackageAnalysisInfo.m_atlasItemList.Add(packageItem);
    }

    public static void AnalysisItemImage(PackageItem packageItem)
    {
        NTexture tex = packageItem.owner.GetItemAsset(packageItem) as NTexture;
        m_curPackageAnalysisInfo.AddImagePackageItem(packageItem);
    }

    public static bool IsUseCommonAsset(string packageName)
    {
        for (int i = 0; i < CommonUINames.Length; i++)
        {
            if (packageName == CommonUINames[i])
            {
                return true;
            }
        }

        return false;
    }

    public static void AnalysisItemComponent(PackageItem packageItem)
    {
        ByteBuffer buffer = packageItem.owner.GetItemAsset(packageItem) as ByteBuffer;
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
            // 使用了scrollPanel
            buffer.position = savedPos;
        }
        else
        {
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

        int childCount = buffer.ReadShort();
        for (int i = 0; i < childCount; i++)
        {
            m_needCheckComponentList.Clear();
            int dataLen = buffer.ReadShort();
            int curPos = buffer.position;

            buffer.Seek(curPos, 0);

            ObjectType type = (ObjectType) buffer.ReadByte();
            string src = buffer.ReadS();
            string pkgId = buffer.ReadS();

            PackageItem pi = null;
            if (src != null)
            {
                UIPackage pkg;
                if (pkgId != null)
                {
                    pkg = UIPackage.GetById(pkgId);
                }
                else
                {
                    pkg = packageItem.owner;
                }

                if (pkg == null)
                {
                    m_curPackageAnalysisInfo.m_needCheckComponentInfoList.Add(
                        new NeedCheckComponentInfo(packageItem, pkg, NeedCheckReasonType.AssetsMissing));
                }
                else
                {
                    if (pkg != m_curAnalysisPackage && !IsUseCommonAsset(pkg.name))
                    {
                        // Debug.Log(pkg.name);
                        m_curPackageAnalysisInfo.m_needCheckComponentInfoList.Add(
                            new NeedCheckComponentInfo(packageItem, pkg));
                    }
                }
                pi = pkg != null ? pkg.GetItem(src) : null;
            }
            if (pi != null)
            {
                // Debug.Log("pi的名字是  " + pi.name);
                // Debug.Log("pi的类型是  " + pi.objectType);
                // child = UIObjectFactory.NewObject(pi);
                // child.packageItem = pi;
                // child.ConstructFromResource();
            }
            else
            {
                // child = UIObjectFactory.NewObject(type);
                // Debug.Log("child的类型是  " + type);
            }

            // }

            // child.underConstruct = true;
            AnalysisGObjectInfo(packageItem, buffer, curPos);
            // child.InternalSetParent(this);
            // _children.Add(child);
            if (m_needCheckComponentList.Count > 0)
            {
                foreach (var needCheckChild in m_needCheckComponentList)
                {
                    needCheckChild.m_needCheckChildName = m_curCheckGObjectName;
                    m_curPackageAnalysisInfo.m_needCheckComponentInfoList.Add(needCheckChild);
                }

                m_needCheckComponentList.Clear();
            }

            buffer.position = curPos + dataLen;
        }

        /*
        buffer.Seek(0, 3);
        // this.relations.Setup(buffer, true);

        buffer.Seek(0, 2);
        buffer.Skip(2);

        for (int i = 0; i < childCount; i++)
        {
            int nextPos = buffer.ReadShort();
            nextPos += buffer.position;

            buffer.Seek(buffer.position, 3);
            _children[i].relations.Setup(buffer, false);

            buffer.position = nextPos;
        }

        buffer.Seek(0, 2);
        buffer.Skip(2);

        for (int i = 0; i < childCount; i++)
        {
            int nextPos = buffer.ReadShort();
            nextPos += buffer.position;

            child = _children[i];
            child.Setup_AfterAdd(buffer, buffer.position);
            child.underConstruct = false;
            if (child.displayObject != null)
                ToolSet.SetParent(child.displayObject.cachedTransform, this.displayObject.cachedTransform);

            buffer.position = nextPos;
        }
        */
        buffer.Seek(0, 4);

        buffer.Skip(2); //customData
        buffer.ReadBool();
        int maskId = buffer.ReadShort();
        if (maskId != -1)
        {
            m_curPackageAnalysisInfo.m_needCheckComponentInfoList.Add(new NeedCheckComponentInfo(packageItem, NeedCheckReasonType.UseGraphMask));
            // Debug.Log(packageItem.name);
            // Debug.Log("该Gcomponent使用了自定义遮罩");
            //使用了遮罩
            // this.mask = GetChildAt(maskId).displayObject;
            // if (buffer.ReadBool())
            //     this.reversedMask = true;
        }
      
    }

    public static void AnalysisGObjectInfo(PackageItem packageItem, ByteBuffer buffer, int beginPos)
    {
        buffer.Seek(beginPos, 0);
        buffer.Skip(5);

        string id = buffer.ReadS();
        m_curCheckGObjectName = buffer.ReadS();
        // SetXY(f1, f2);
        buffer.ReadInt();
        buffer.ReadInt();


        if (buffer.ReadBool())
        {
            buffer.ReadInt();
            buffer.ReadInt();
            // SetSize(initWidth, initHeight, true);
        }

        if (buffer.ReadBool())
        {
            buffer.ReadInt();
            buffer.ReadInt();
            buffer.ReadInt();
            buffer.ReadInt();
            // 最大最小宽高
        }

        if (buffer.ReadBool())
        {
            buffer.ReadFloat();
            buffer.ReadFloat();
            // SetScale(f1, f2);
        }

        if (buffer.ReadBool())
        {
            buffer.ReadFloat();
            buffer.ReadFloat();
            // this.skew = new Vector2(f1, f2);
        }

        if (buffer.ReadBool())
        {
            buffer.ReadFloat();
            buffer.ReadFloat();
            buffer.ReadBool();
            // SetPivot(f1, f2, buffer.ReadBool());
        }

        buffer.ReadFloat();
        // if (f1 != 1)
        // this.alpha = f1;
        // alpha

        buffer.ReadFloat();
        // if (f1 != 0)
        //    this.rotation = f1;
        // rotation
        if (!buffer.ReadBool())
        {

        }

        // this.visible = false;
        if (!buffer.ReadBool())
        {

        }

        //this.touchable = false;
        if (buffer.ReadBool())
        {
            m_needCheckComponentList.Add(new NeedCheckComponentInfo(packageItem, NeedCheckReasonType.UseGray));
        }
        // this.grayed = true;

        var blendMode = (BlendMode) buffer.ReadByte();
        if (blendMode!= BlendMode.Normal)
        {
            m_needCheckComponentList.Add(new NeedCheckComponentInfo(packageItem, NeedCheckReasonType.ErrorBlendMode));
        }

        int filter = buffer.ReadByte();
        if (filter == 1)
        {
            ColorFilter cf = new ColorFilter();
            cf.AdjustBrightness(buffer.ReadFloat());
            cf.AdjustContrast(buffer.ReadFloat());
            cf.AdjustSaturation(buffer.ReadFloat());
            cf.AdjustHue(buffer.ReadFloat());
            // Debug.Log(m_curCheckGObjectName);
            // Debug.Log(id);
            // Debug.Log("该对象有一个滤镜");
            m_needCheckComponentList.Add(new NeedCheckComponentInfo(packageItem, NeedCheckReasonType.UseColorFilter));
        }

        // string str = buffer.ReadS();
        //if (str != null)
        //    this.data = str;
    }
}

public class UIPackageAnalysisInfo
{
    public UIPackageAnalysisInfo(string name)
    {
        m_name = name;
    }

    public string m_name;

    // public List<AtlasAnalysisInfo> m_atlasAnalysisInfoList = new List<AtlasAnalysisInfo>();

    public Dictionary<NTexture, AtlasAnalysisInfo> m_atlasAnalysisInfoDic = new Dictionary<NTexture, AtlasAnalysisInfo>();

    public Dictionary<NTexture, List<PackageItem>> m_ImageItemDic = new Dictionary<NTexture, List<PackageItem>>();

    public List<PackageItem> m_atlasItemList = new List<PackageItem>();

    public List<PackageItem> m_movieClipItemList = new List<PackageItem>();

    public List<NeedCheckComponentInfo> m_needCheckComponentInfoList = new List<NeedCheckComponentInfo>();

    public void AddImagePackageItem(PackageItem packageItem)
    {
        List<PackageItem> spriteItemList = null;
        if (!m_ImageItemDic.TryGetValue(packageItem.texture.root, out spriteItemList))
        {
            spriteItemList = new List<PackageItem>();
            m_ImageItemDic.Add(packageItem.texture.root, spriteItemList);
        }

        spriteItemList.Add(packageItem);
    }

    public void DoAnalysis()
    {
        // Debug.Log("Cur Package Name  " + m_name);
        for (int i = 0; i < m_atlasItemList.Count; i++)
        {
            AtlasAnalysisInfo aai = new AtlasAnalysisInfo(m_atlasItemList[i]);
            List<PackageItem> spriteList = null;
            if (!m_ImageItemDic.TryGetValue(m_atlasItemList[i].texture, out spriteList))
            {
                m_atlasAnalysisInfoDic.Add(m_atlasItemList[i].texture,aai);
                continue;
            }

            foreach (var imagePackageItem in spriteList)
            {
                aai.m_spriteTotalSize += imagePackageItem.texture.width * imagePackageItem.texture.height;
            }
            m_atlasAnalysisInfoDic.Add(m_atlasItemList[i].texture, aai);
        }

        for (int i = 0; i < m_movieClipItemList.Count; i++)
        {
            NTexture targetNTexture = m_movieClipItemList[i].texture.root;
            AtlasAnalysisInfo targetAtlasAnalysisInfo = null;
            if (m_atlasAnalysisInfoDic.TryGetValue(targetNTexture, out targetAtlasAnalysisInfo))
            {
                foreach (var frame in m_movieClipItemList[i].frames)
                {
                    int frameSize = Mathf.CeilToInt(frame.rect.width * frame.rect.height);
                    targetAtlasAnalysisInfo.m_spriteTotalSize += frameSize;
                }
            } 
        }
        /*
        foreach (KeyValuePair<NTexture, AtlasAnalysisInfo> kvp in m_atlasAnalysisInfoDic)
        {
            Debug.Log(kvp.Value.m_id + string.Format(" 尺寸： {0} * {1}", kvp.Value.m_width, kvp.Value.m_height) + " 填充率： " + kvp.Value.m_fillingRate);
        }

        for (int i = 0; i < m_needCheckComponentInfoList.Count; i++)
        {
            var needCheckTarget = m_needCheckComponentInfoList[i];
            Debug.Log(needCheckTarget.ToString());
        }
        */
        UIPackageCheckTool.m_UIPacakgeAnalysisInfoList.Add(this);
    }
}


public class AtlasAnalysisInfo
{
    public AtlasAnalysisInfo(PackageItem item)
    {
        m_id = item.id;
        m_width = item.texture.width;
        m_height = item.texture.height;
    }

    public string m_id;

    public int m_width;

    public int m_height;

    public int m_spriteTotalSize = 0;

    public float m_fillingRate
    {
        get { return (float) m_spriteTotalSize / (m_height * m_width); }
    }
}

public class NeedCheckComponentInfo
{
    public NeedCheckComponentInfo(PackageItem item, UIPackage otherPackage, UIPackageCheckTool.NeedCheckReasonType type = UIPackageCheckTool.NeedCheckReasonType.UseOtherPackageAssets)
    {
        m_type = item.type;
        m_name = item.name;
        if (otherPackage != null)
        {
            m_useOtherAssetPackageName = otherPackage.name;
        }
        m_needCheckReasonType = type;
    }

    public NeedCheckComponentInfo(PackageItem item, UIPackageCheckTool.NeedCheckReasonType reasonType)
    {
        m_type = item.type;
        m_name = item.name;
        m_needCheckReasonType = reasonType;
    }

    public PackageItemType m_type;

    public string m_name;

    public string m_needCheckChildName;

    public UIPackageCheckTool.NeedCheckReasonType m_needCheckReasonType;

    public string m_useOtherAssetPackageName;

    public override string ToString()
    {
        switch (m_type)
        {
            case PackageItemType.Image:
                return string.Format("{0} -> {1} 检查原因 {2}",
                    m_type.ToString(), m_name, m_needCheckReasonType.ToString());
                break;
            case PackageItemType.MovieClip:
                return string.Format("{0} -> {1} 检查原因 {2}",
                    m_type.ToString(), m_name, m_needCheckReasonType.ToString());
                break;
            case PackageItemType.Component:
                return string.Format("{0} -> {1} 检查原因 {2}",
                    m_name, m_needCheckChildName, m_needCheckReasonType.ToString());
                break;
        }
        Debug.Log("Error");
        return base.ToString();
    }
}
