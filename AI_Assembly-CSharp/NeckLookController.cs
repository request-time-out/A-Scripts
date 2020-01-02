// Decompiled with JetBrains decompiler
// Type: NeckLookController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.IO;
using UnityEngine;

public class NeckLookController : MonoBehaviour
{
  public NeckLookCalc neckLookScript;
  public int ptnNo;
  public Transform target;
  public float rate;

  public NeckLookController()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Implicit((Object) this.target) || !Object.op_Implicit((Object) Camera.get_main()))
      return;
    this.target = ((Component) Camera.get_main()).get_transform();
  }

  private void Update()
  {
  }

  private void LateUpdate()
  {
    if (Object.op_Equality((Object) this.neckLookScript, (Object) null))
      return;
    this.neckLookScript.UpdateCall(this.ptnNo);
    if (Object.op_Inequality((Object) this.target, (Object) null))
    {
      if (!Object.op_Inequality((Object) null, (Object) this.neckLookScript))
        return;
      Vector3 position1 = ((Component) this).get_transform().get_position();
      Vector3 position2 = this.target.get_position();
      for (int index = 0; index < 2; ++index)
        ((Vector3) ref position2).set_Item(index, Mathf.Lerp(((Vector3) ref position1).get_Item(index), ((Vector3) ref position2).get_Item(index), this.rate));
      this.neckLookScript.NeckUpdateCalc(position2, this.ptnNo);
    }
    else
      this.neckLookScript.NeckUpdateCalc(this.neckLookScript.backupPos, this.ptnNo);
  }

  public void SaveNeckLookCtrl(BinaryWriter writer)
  {
    writer.Write(this.ptnNo);
    Quaternion fixAngle = this.neckLookScript.fixAngle;
    writer.Write((float) fixAngle.x);
    writer.Write((float) fixAngle.y);
    writer.Write((float) fixAngle.z);
    writer.Write((float) fixAngle.w);
  }

  public void LoadNeckLookCtrl(BinaryReader reader)
  {
    this.ptnNo = reader.ReadInt32();
    Quaternion angle;
    ((Quaternion) ref angle).\u002Ector(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    this.neckLookScript.SetFixAngle(angle);
  }
}
