// Decompiled with JetBrains decompiler
// Type: ADV.BackupPosRot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV
{
  public class BackupPosRot
  {
    public BackupPosRot(Transform transform)
    {
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      this.position = transform.get_localPosition();
      this.rotation = transform.get_localRotation();
    }

    public Vector3 position { get; private set; }

    public Quaternion rotation { get; private set; }

    public void Set(Transform transform)
    {
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      transform.set_localPosition(this.position);
      transform.set_localRotation(this.rotation);
    }
  }
}
