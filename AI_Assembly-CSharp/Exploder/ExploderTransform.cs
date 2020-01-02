// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderTransform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public struct ExploderTransform
  {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public Transform parent;

    public ExploderTransform(Transform unityTransform)
    {
      this.position = unityTransform.get_position();
      this.rotation = unityTransform.get_rotation();
      this.localScale = unityTransform.get_localScale();
      this.parent = unityTransform.get_parent();
    }

    public Vector3 InverseTransformDirection(Vector3 dir)
    {
      return Quaternion.op_Multiply(Quaternion.Inverse(this.rotation), dir);
    }

    public Vector3 InverseTransformPoint(Vector3 pnt)
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) (1.0 / this.localScale.x), (float) (1.0 / this.localScale.y), (float) (1.0 / this.localScale.z));
      return Vector3.Scale(vector3, Quaternion.op_Multiply(Quaternion.Inverse(this.rotation), Vector3.op_Subtraction(pnt, this.position)));
    }

    public Vector3 TransformPoint(Vector3 pnt)
    {
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(this.position, this.rotation, this.localScale);
      return ((Matrix4x4) ref matrix4x4).MultiplyPoint3x4(pnt);
    }
  }
}
