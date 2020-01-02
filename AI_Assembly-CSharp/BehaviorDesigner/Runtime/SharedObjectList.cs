// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedObjectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedObjectList : SharedVariable<List<Object>>
  {
    public SharedObjectList()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedObjectList(List<Object> value)
    {
      SharedObjectList sharedObjectList = new SharedObjectList();
      sharedObjectList.mValue = (__Null) value;
      return sharedObjectList;
    }
  }
}
