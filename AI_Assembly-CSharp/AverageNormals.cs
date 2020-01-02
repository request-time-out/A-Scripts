// Decompiled with JetBrains decompiler
// Type: AverageNormals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AverageNormals : MonoBehaviour
{
  public GameObject[] objUpdate;
  public GameObject ObjRange;
  public float Range;
  private Mesh[] meshUpdate;
  public int[] calcIndexA;
  public int[] calcIndexB;
  private bool meshInit;

  public AverageNormals()
  {
    base.\u002Ector();
  }

  public void Init()
  {
    if (Object.op_Equality((Object) null, (Object) this.objUpdate[0]) || Object.op_Equality((Object) null, (Object) this.objUpdate[1]) || Object.op_Equality((Object) null, (Object) this.ObjRange))
      return;
    Mesh[] meshArray = new Mesh[2];
    List<int>[] intListArray1 = new List<int>[2];
    for (int index = 0; index < 2; ++index)
      intListArray1[index] = new List<int>();
    List<int>[] intListArray2 = new List<int>[2];
    for (int index1 = 0; index1 < 2; ++index1)
    {
      intListArray2[index1] = new List<int>();
      MeshFilter meshFilter = new MeshFilter();
      MeshFilter component1 = this.objUpdate[index1].GetComponent(typeof (MeshFilter)) as MeshFilter;
      if (Object.op_Implicit((Object) component1))
      {
        meshArray[index1] = component1.get_sharedMesh();
      }
      else
      {
        SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
        SkinnedMeshRenderer component2 = this.objUpdate[index1].GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        meshArray[index1] = component2.get_sharedMesh();
      }
      if (Object.op_Inequality((Object) null, (Object) meshArray[index1]))
      {
        if (Object.op_Implicit((Object) this.ObjRange))
        {
          for (int index2 = 0; index2 < meshArray[index1].get_vertexCount(); ++index2)
          {
            if ((double) Vector3.Distance(this.ObjRange.get_transform().get_position(), this.objUpdate[index1].get_transform().TransformPoint(meshArray[index1].get_vertices()[index2])) < (double) this.Range)
              intListArray2[index1].Add(index2);
          }
        }
        else
        {
          for (int index2 = 0; index2 < meshArray[index1].get_vertexCount(); ++index2)
            intListArray2[index1].Add(index2);
        }
      }
    }
    if (!Object.op_Inequality((Object) null, (Object) meshArray[0]) || !Object.op_Inequality((Object) null, (Object) meshArray[1]))
      return;
    for (int index1 = 0; index1 < intListArray2[0].Count; ++index1)
    {
      for (int index2 = 0; index2 < intListArray2[1].Count; ++index2)
      {
        int index3 = intListArray2[0][index1];
        int index4 = intListArray2[1][index2];
        if (Vector3.op_Equality(this.objUpdate[0].get_transform().TransformPoint(meshArray[0].get_vertices()[index3]), this.objUpdate[1].get_transform().TransformPoint(meshArray[1].get_vertices()[index4])))
        {
          intListArray1[0].Add(index3);
          intListArray1[1].Add(index4);
          break;
        }
      }
    }
    this.calcIndexA = new int[intListArray1[0].Count];
    this.calcIndexB = new int[intListArray1[1].Count];
    for (int index = 0; index < intListArray1[0].Count; ++index)
    {
      this.calcIndexA[index] = intListArray1[0][index];
      this.calcIndexB[index] = intListArray1[1][index];
    }
  }

  private void Awake()
  {
  }

  private void Start()
  {
  }

  private void Update()
  {
    if (this.meshInit)
    {
      this.GetUpdateMesh();
      this.meshInit = false;
    }
    this.Average();
  }

  public void GetUpdateMesh()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (!Object.op_Equality((Object) null, (Object) this.objUpdate[index]))
      {
        MeshFilter meshFilter = new MeshFilter();
        MeshFilter component1 = this.objUpdate[index].GetComponent(typeof (MeshFilter)) as MeshFilter;
        if (Object.op_Implicit((Object) component1))
        {
          this.meshUpdate[index] = component1.get_sharedMesh();
        }
        else
        {
          SkinnedMeshRenderer skinnedMeshRenderer = new SkinnedMeshRenderer();
          SkinnedMeshRenderer component2 = this.objUpdate[index].GetComponent(typeof (SkinnedMeshRenderer)) as SkinnedMeshRenderer;
          this.meshUpdate[index] = component2.get_sharedMesh();
        }
      }
    }
  }

  public void Average()
  {
    if (this.calcIndexA.Length == 0 || this.calcIndexB.Length == 0 || (Object.op_Equality((Object) null, (Object) this.meshUpdate[0]) || Object.op_Equality((Object) null, (Object) this.meshUpdate[1])))
      return;
    Vector3[] vector3Array1 = new Vector3[this.calcIndexA.Length];
    for (int index1 = 0; index1 < this.calcIndexA.Length; ++index1)
    {
      int index2 = this.calcIndexA[index1];
      int index3 = this.calcIndexB[index1];
      vector3Array1[index1] = Vector3.Lerp(this.meshUpdate[0].get_normals()[index2], this.meshUpdate[1].get_normals()[index3], 0.5f);
    }
    Vector3[] vector3Array2 = new Vector3[this.meshUpdate[0].get_vertexCount()];
    Vector3[] normals1 = this.meshUpdate[0].get_normals();
    for (int index = 0; index < this.calcIndexA.Length; ++index)
      normals1[this.calcIndexA[index]] = vector3Array1[index];
    this.meshUpdate[0].set_normals(normals1);
    Vector3[] vector3Array3 = new Vector3[this.meshUpdate[1].get_vertexCount()];
    Vector3[] normals2 = this.meshUpdate[1].get_normals();
    for (int index = 0; index < this.calcIndexB.Length; ++index)
      normals2[this.calcIndexB[index]] = vector3Array1[index];
    this.meshUpdate[1].set_normals(normals2);
  }

  private void OnDrawGizmos()
  {
    if (Object.op_Equality((Object) null, (Object) this.ObjRange))
      return;
    Gizmos.set_color(Color.get_red());
    Gizmos.DrawWireSphere(this.ObjRange.get_transform().get_position(), this.Range);
  }
}
