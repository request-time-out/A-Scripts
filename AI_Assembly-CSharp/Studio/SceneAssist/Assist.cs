// Decompiled with JetBrains decompiler
// Type: Studio.SceneAssist.Assist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio.SceneAssist
{
  public class Assist
  {
    public static Transform PlayDecisionSE()
    {
      return Singleton<Sound>.Instance.Play(Sound.Type.SystemSE, Assist.AssetBundleSystemSE, "sse_00_02", 0.0f, 0.0f, true, true, -1, true);
    }

    public static Transform PlayCancelSE()
    {
      return Singleton<Sound>.Instance.Play(Sound.Type.SystemSE, Assist.AssetBundleSystemSE, "sse_00_04", 0.0f, 0.0f, true, true, -1, true);
    }

    public static string AssetBundleSystemSE
    {
      get
      {
        return "sound/data/systemse.unity3d";
      }
    }
  }
}
