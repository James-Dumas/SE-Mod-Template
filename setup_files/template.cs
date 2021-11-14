using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace MyNamespace
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class MySpaceEngineersMod : MySessionComponentBase
    {
        public override void BeforeStart()
        {
            // Setup code

            MyLog.Default.WriteLineAndConsole("MySpaceEngineersMod: Setup complete");
        }

        public override void UpdateBeforeSimulation()
        {
            // Code to run every frame
        }
    }
}