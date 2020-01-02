// Decompiled with JetBrains decompiler
// Type: AssetBundleLoadOperation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;

public abstract class AssetBundleLoadOperation : IEnumerator
{
  public object Current
  {
    get
    {
      return (object) null;
    }
  }

  public bool MoveNext()
  {
    return !this.IsDone();
  }

  public void Reset()
  {
  }

  public abstract bool Update();

  public abstract bool IsDone();
}
