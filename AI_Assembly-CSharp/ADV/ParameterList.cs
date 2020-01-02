// Decompiled with JetBrains decompiler
// Type: ADV.ParameterList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV
{
  public static class ParameterList
  {
    private static List<SceneParameter> list { get; } = new List<SceneParameter>();

    public static void Add(SceneParameter param)
    {
      ParameterList.list.Add(param);
    }

    public static void Remove(IData data)
    {
      ParameterList.list.RemoveAll((Predicate<SceneParameter>) (p => p.data == null || p.data == data));
    }

    public static void Init()
    {
      ParameterList.list.LastOrDefault<SceneParameter>()?.Init();
    }

    public static void Release()
    {
      ParameterList.list.LastOrDefault<SceneParameter>()?.Release();
    }

    public static void WaitEndProc()
    {
      ParameterList.list.LastOrDefault<SceneParameter>()?.WaitEndProc();
    }
  }
}
