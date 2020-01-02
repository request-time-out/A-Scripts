// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedTransformList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedTransformList : SharedVariable<List<Transform>>
  {
    public SharedTransformList()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedTransformList(List<Transform> value)
    {
      SharedTransformList sharedTransformList = new SharedTransformList();
      sharedTransformList.mValue = (__Null) value;
      return sharedTransformList;
    }
  }
}
