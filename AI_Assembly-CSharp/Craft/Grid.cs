// Decompiled with JetBrains decompiler
// Type: Craft.Grid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Craft
{
  public class Grid : MonoBehaviour
  {
    [SerializeField]
    private float size;
    [SerializeField]
    private float height;
    [SerializeField]
    private float width;
    [SerializeField]
    private Vector3 center;

    public Grid()
    {
      base.\u002Ector();
    }

    public Vector3 GetNearestPointOnGrid(Vector3 _position)
    {
      _position = ((Component) this).get_transform().InverseTransformPoint(_position);
      int num1 = Mathf.FloorToInt((float) _position.x / this.size);
      int num2 = Mathf.Max(Mathf.FloorToInt((float) _position.y / this.size), 0);
      int num3 = Mathf.FloorToInt((float) _position.z / this.size);
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) num1 * this.size, (float) num2 * this.size, (float) num3 * this.size);
      return ((Component) this).get_transform().TransformPoint(Vector3.op_Addition(vector3, Vector3.op_Multiply(this.center, this.size)));
    }

    public Vector2 GetGridPos(Vector3 _position)
    {
      return this.ConvertPos(this.GetNearestPointOnGrid(_position));
    }

    public Vector2 ConvertPos(Vector3 _position)
    {
      Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().InverseTransformPoint(_position), Vector3.op_Multiply(this.center, this.size));
      float num1 = (float) ((double) this.width - vector3.x - 1.0);
      float num2 = (float) ((double) this.height - vector3.z - 1.0);
      bool flag = 0.0 <= (double) num1 && (double) num1 <= (double) this.width - 1.0 && (0.0 <= (double) num2 && (double) num2 <= (double) this.height - 1.0);
      return new Vector2(!flag ? -1f : num1, !flag ? -1f : num2);
    }
  }
}
