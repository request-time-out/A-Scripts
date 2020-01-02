// Decompiled with JetBrains decompiler
// Type: AIProject.OptionPositionChangeEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject
{
  [Serializable]
  public class OptionPositionChangeEvent : UnityEvent<int, GameObject>
  {
    public OptionPositionChangeEvent()
    {
      base.\u002Ector();
    }
  }
}
