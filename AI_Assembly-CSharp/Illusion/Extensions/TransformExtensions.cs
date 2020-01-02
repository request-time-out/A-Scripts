// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.TransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Illusion.Extensions
{
  public static class TransformExtensions
  {
    public static List<Transform> Children(this Transform self)
    {
      List<Transform> transformList = new List<Transform>();
      for (int index = 0; index < self.get_childCount(); ++index)
        transformList.Add(self.GetChild(index));
      return transformList;
    }

    public static void ChildrenAction(this Transform self, Action<Transform> act)
    {
      for (int index = 0; index < self.get_childCount(); ++index)
        act(self.GetChild(index));
    }
  }
}
