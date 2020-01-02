// Decompiled with JetBrains decompiler
// Type: CoroutineActivity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class CoroutineActivity
{
  public int seqID;
  public float timestamp;
  public int curFrame;
  public string typeName;

  public CoroutineActivity(int id)
  {
    this.seqID = id;
    this.timestamp = Time.get_realtimeSinceStartup();
    this.typeName = this.GetType().Name.Substring("Coroutine".Length);
  }
}
