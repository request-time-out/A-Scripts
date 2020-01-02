// Decompiled with JetBrains decompiler
// Type: AIProject.GameUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIProject
{
  public static class GameUtil
  {
    public static void OpenConfig()
    {
      if (Object.op_Inequality((Object) Singleton<Game>.Instance.Config, (Object) null))
        return;
      Singleton<Game>.Instance.LoadConfig();
    }

    public static void GameEnd(bool isCheck = true)
    {
      Singleton<Scene>.Instance.GameEnd(isCheck);
    }
  }
}
