﻿// Decompiled with JetBrains decompiler
// Type: DestroyOnTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
  public string m_Tag;

  public DestroyOnTrigger()
  {
    base.\u002Ector();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!(((Component) other).get_gameObject().get_tag() == this.m_Tag))
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }
}