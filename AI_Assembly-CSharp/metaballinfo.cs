// Decompiled with JetBrains decompiler
// Type: metaballinfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class metaballinfo : MonoBehaviour
{
  public Rigidbody[] aRigid;
  public Rigidbody rigidBeginning;

  public metaballinfo()
  {
    base.\u002Ector();
  }

  private void Reset()
  {
    this.aRigid = (Rigidbody[]) ((Component) this).GetComponentsInChildren<Rigidbody>(true);
    if (this.aRigid.Length <= 0)
      return;
    this.rigidBeginning = this.aRigid[0];
  }

  private void Update()
  {
  }
}
