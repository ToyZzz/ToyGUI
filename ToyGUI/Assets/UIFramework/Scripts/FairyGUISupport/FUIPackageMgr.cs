using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIPackageMgr
    {
        private static FUIPackageMgr _instance = null;
        public static FUIPackageMgr Instance()
        {
            if (_instance == null)
            {
                _instance = new FUIPackageMgr();
            }
            return _instance;
        }

        public FUIPackage AddPackage(string descFilePath)
        {
#if UNITY_EDITOR
            if (descFilePath.StartsWith("Assets/"))
            {
                
            }
#else

#endif
        }

        public FUIPackage AddPackage(string descFilePath,FUIPackage.LoadResource loadFunc)
        {

        }
    }
}