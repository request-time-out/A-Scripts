// Decompiled with JetBrains decompiler
// Type: LegacyAnimationSpeedSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class LegacyAnimationSpeedSet : MonoBehaviour
{
  public float[] AnmSpeed;
  private Animation anim;
  private string[] name_a;
  private int AnmCount;

  public LegacyAnimationSpeedSet()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.anim = (Animation) ((Component) ((Component) this).get_transform()).GetComponent<Animation>();
    this.AnmCount = this.anim.GetClipCount();
    this.name_a = new string[this.AnmCount];
    int num = 0;
    IEnumerator enumerator = this.anim.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        AnimationState current = (AnimationState) enumerator.Current;
        this.name_a[num++] = current.get_name();
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  private void Update()
  {
    for (int index = 0; index < this.AnmCount && this.AnmSpeed.Length > index; ++index)
      this.anim.get_Item(this.name_a[index]).set_speed(this.AnmSpeed[index]);
  }
}
