// Decompiled with JetBrains decompiler
// Type: PlaceholderSoftware.WetStuff.Demos.Demo_Assets.DemoMenuGui
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlaceholderSoftware.WetStuff.Demos.Demo_Assets
{
  public class DemoMenuGui : MonoBehaviour
  {
    public DemoMenuGui()
    {
      base.\u002Ector();
    }

    private void OnGUI()
    {
      using (new GUILayout.HorizontalScope((GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
      {
        GUILayout.Space(10f);
        using (new GUILayout.VerticalScope((GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        {
          GUILayout.Space(10f);
          DemoMenuGui.SceneButton("1. Puddle");
          DemoMenuGui.SceneButton("2. Timeline");
          DemoMenuGui.SceneButton("3. Rain");
          DemoMenuGui.SceneButton("4. Particles (Splat)");
          DemoMenuGui.SceneButton("5. Particles (Drip Drip Drip)");
          DemoMenuGui.SceneButton("6. Triplanar Mapping");
          DemoMenuGui.SceneButton("7. Dry Decals");
        }
      }
    }

    private static void SceneButton(string scene)
    {
      if (!GUILayout.Button(scene, (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        return;
      SceneManager.LoadScene(scene);
    }
  }
}
