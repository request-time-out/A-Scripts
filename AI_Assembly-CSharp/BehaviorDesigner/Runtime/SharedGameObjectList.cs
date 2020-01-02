// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedGameObjectList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedGameObjectList : SharedVariable<List<GameObject>>
  {
    public SharedGameObjectList()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedGameObjectList(List<GameObject> value)
    {
      SharedGameObjectList sharedGameObjectList = new SharedGameObjectList();
      sharedGameObjectList.mValue = (__Null) value;
      return sharedGameObjectList;
    }
  }
}
