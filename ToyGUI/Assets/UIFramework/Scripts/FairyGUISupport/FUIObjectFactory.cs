using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyGUI
{
    public class FUIObjectFactory
    {
        public delegate object GComponentCreator();

        public delegate object GLoaderCreator();
    }
}