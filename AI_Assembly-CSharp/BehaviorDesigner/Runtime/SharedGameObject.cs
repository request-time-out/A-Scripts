﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedGameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedGameObject : SharedVariable<GameObject>
  {
    public SharedGameObject()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedGameObject(GameObject value)
    {
      SharedGameObject sharedGameObject = new SharedGameObject();
      sharedGameObject.mValue = (__Null) value;
      return sharedGameObject;
    }
  }
}