// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalTestRoof
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;
using UnityEx;

namespace AIProject.Animal
{
  public class AnimalTestRoof : MonoBehaviour
  {
    private ValueTuple<Vector3, Vector3>[] vertex;

    public AnimalTestRoof()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      ((Component) this).get_transform().get_position();
      Vector3 lossyScale = ((Component) this).get_transform().get_lossyScale();
      this.vertex[0].Item1 = (__Null) Vector3.get_zero();
      this.vertex[1].Item1 = (__Null) new Vector3((float) (lossyScale.x * 0.5), 0.0f, (float) (lossyScale.z * 0.5));
      this.vertex[2].Item1 = (__Null) new Vector3((float) (lossyScale.x * 0.5), 0.0f, (float) (lossyScale.z * -0.5));
      this.vertex[3].Item1 = (__Null) new Vector3((float) (lossyScale.x * -0.5), 0.0f, (float) (lossyScale.z * -0.5));
      this.vertex[4].Item1 = (__Null) new Vector3((float) (lossyScale.x * -0.5), 0.0f, (float) (lossyScale.z * 0.5));
      int num = LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer);
      for (int index = 0; index < this.vertex.Length; ++index)
      {
        this.vertex[index].Item1 = (__Null) Vector3.op_Addition(((Component) this).get_transform().get_position(), Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), (Vector3) this.vertex[index].Item1));
        RaycastHit raycastHit;
        this.vertex[index].Item2 = !Physics.Raycast((Vector3) this.vertex[index].Item1, Vector3.get_down(), ref raycastHit, 1000f, num) ? this.vertex[index].Item1 : (__Null) ((RaycastHit) ref raycastHit).get_point();
      }
    }
  }
}
