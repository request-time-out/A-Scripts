// Decompiled with JetBrains decompiler
// Type: MetaballNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class MetaballNode : MonoBehaviour
{
  public float baseRadius;
  public bool bSubtract;
  private MetaballSeedBase _seed;
  private Mesh _boneMesh;

  public MetaballNode()
  {
    base.\u002Ector();
  }

  public virtual float Density
  {
    get
    {
      return this.bSubtract ? -1f : 1f;
    }
  }

  public float Radius
  {
    get
    {
      return this.baseRadius;
    }
  }

  private void OnDrawGizmosSelected()
  {
    if (Object.op_Equality((Object) this._seed, (Object) null))
      this._seed = Utils.FindComponentInParents<MetaballSeedBase>(((Component) this).get_transform());
    if ((double) this.Density == 0.0 || Object.op_Inequality((Object) this._seed, (Object) null) && Object.op_Inequality((Object) this._seed.sourceRoot, (Object) null) && Object.op_Equality((Object) ((Component) this._seed.sourceRoot).get_gameObject(), (Object) ((Component) this).get_gameObject()))
      return;
    Gizmos.set_color(!this.bSubtract ? Color.get_white() : Color.get_red());
    float radius = this.Radius;
    if (Object.op_Inequality((Object) this._seed, (Object) null))
      radius *= 1f - Mathf.Sqrt(this._seed.powerThreshold);
    Matrix4x4 matrix = Gizmos.get_matrix();
    Gizmos.set_matrix(((Component) this).get_transform().get_localToWorldMatrix());
    Gizmos.DrawWireSphere(Vector3.get_zero(), radius);
    MetaballNode component = (MetaballNode) ((Component) ((Component) this).get_transform().get_parent()).GetComponent<MetaballNode>();
    if (Object.op_Inequality((Object) component, (Object) null) && (double) component.Density != 0.0 && (Object.op_Inequality((Object) this._seed, (Object) null) && this._seed.IsTreeShape))
    {
      if (Object.op_Equality((Object) this._boneMesh, (Object) null))
      {
        this._boneMesh = new Mesh();
        Vector3[] vector3Array1 = new Vector3[5];
        Vector3[] vector3Array2 = new Vector3[5];
        int[] numArray = new int[6];
        vector3Array1[0] = new Vector3(0.1f, 0.0f, 0.0f);
        vector3Array1[1] = new Vector3(-0.1f, 0.0f, 0.0f);
        vector3Array1[2] = new Vector3(0.0f, 0.1f, 0.0f);
        vector3Array1[3] = new Vector3(0.0f, -0.1f, 0.0f);
        vector3Array1[4] = new Vector3(0.0f, 0.0f, 1f);
        vector3Array2[0] = new Vector3(0.0f, 0.0f, 1f);
        vector3Array2[1] = new Vector3(0.0f, 0.0f, 1f);
        vector3Array2[2] = new Vector3(0.0f, 0.0f, 1f);
        vector3Array2[3] = new Vector3(0.0f, 0.0f, 1f);
        vector3Array2[4] = new Vector3(0.0f, 0.0f, 1f);
        numArray[0] = 0;
        numArray[1] = 1;
        numArray[2] = 4;
        numArray[3] = 2;
        numArray[4] = 3;
        numArray[5] = 4;
        this._boneMesh.set_vertices(vector3Array1);
        this._boneMesh.set_normals(vector3Array2);
        this._boneMesh.SetIndices(numArray, (MeshTopology) 0, 0);
      }
      Vector3 one = Vector3.get_one();
      Vector3 position1 = ((Component) this).get_transform().get_position();
      Vector3 position2 = ((Component) this).get_transform().get_parent().get_position();
      Vector3 vector3_1 = Vector3.op_Subtraction(position2, position1);
      if ((double) ((Vector3) ref vector3_1).get_sqrMagnitude() >= 1.40129846432482E-45)
      {
        Vector3 vector3_2 = one;
        Vector3 vector3_3 = Vector3.op_Subtraction(position2, position1);
        double magnitude = (double) ((Vector3) ref vector3_3).get_magnitude();
        Vector3 vector3_4 = Vector3.op_Multiply(vector3_2, (float) magnitude);
        Matrix4x4 matrix4x4 = Matrix4x4.TRS(position2, Quaternion.LookRotation(Vector3.op_Subtraction(position1, position2)), vector3_4);
        Gizmos.set_color(Color.get_blue());
        Gizmos.set_matrix(matrix4x4);
        Gizmos.DrawWireMesh(this._boneMesh);
      }
    }
    Gizmos.set_color(Color.get_white());
    Gizmos.set_matrix(matrix);
  }
}
