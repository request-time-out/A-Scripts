﻿// Decompiled with JetBrains decompiler
// Type: QuickStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Exploder.Utils;
using UnityEngine;

public class QuickStart : MonoBehaviour
{
  public QuickStart()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    ExploderSingleton.Instance.ExplodeObject(((Component) this).get_gameObject());
  }
}
