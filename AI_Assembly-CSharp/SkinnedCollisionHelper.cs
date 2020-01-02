// Decompiled with JetBrains decompiler
// Type: SkinnedCollisionHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

public class SkinnedCollisionHelper : MonoBehaviour
{
  public bool forceUpdate;
  public bool updateOncePerFrame;
  public bool calcNormal;
  private bool IsInit;
  private SkinnedCollisionHelper.CWeightList[] nodeWeights;
  private SkinnedMeshRenderer skinnedMeshRenderer;
  private MeshCollider meshCollider;
  private Mesh meshCalc;

  public SkinnedCollisionHelper()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.Init();
  }

  public bool Init()
  {
    if (this.IsInit)
      return true;
    this.skinnedMeshRenderer = (SkinnedMeshRenderer) ((Component) this).GetComponent<SkinnedMeshRenderer>();
    this.meshCollider = (MeshCollider) ((Component) this).GetComponent<MeshCollider>();
    if (!Object.op_Inequality((Object) this.meshCollider, (Object) null) || !Object.op_Inequality((Object) this.skinnedMeshRenderer, (Object) null))
      return false;
    this.meshCalc = (Mesh) Object.Instantiate<Mesh>((M0) this.skinnedMeshRenderer.get_sharedMesh());
    ((Object) this.meshCalc).set_name(((Object) this.skinnedMeshRenderer.get_sharedMesh()).get_name() + "_calc");
    this.meshCollider.set_sharedMesh(this.meshCalc);
    this.meshCalc.MarkDynamic();
    Vector3[] vertices = this.skinnedMeshRenderer.get_sharedMesh().get_vertices();
    Vector3[] normals = this.skinnedMeshRenderer.get_sharedMesh().get_normals();
    Matrix4x4[] bindposes = this.skinnedMeshRenderer.get_sharedMesh().get_bindposes();
    BoneWeight[] boneWeights = this.skinnedMeshRenderer.get_sharedMesh().get_boneWeights();
    this.nodeWeights = new SkinnedCollisionHelper.CWeightList[this.skinnedMeshRenderer.get_bones().Length];
    for (int index = 0; index < this.skinnedMeshRenderer.get_bones().Length; ++index)
    {
      this.nodeWeights[index] = new SkinnedCollisionHelper.CWeightList();
      this.nodeWeights[index].transform = this.skinnedMeshRenderer.get_bones()[index];
    }
    for (int i = 0; i < vertices.Length; ++i)
    {
      BoneWeight boneWeight = boneWeights[i];
      if ((double) ((BoneWeight) ref boneWeight).get_weight0() != 0.0)
      {
        Vector3 p = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex0()]).MultiplyPoint3x4(vertices[i]);
        Vector3 n = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex0()]).MultiplyPoint3x4(normals[i]);
        this.nodeWeights[((BoneWeight) ref boneWeight).get_boneIndex0()].weights.Add((object) new SkinnedCollisionHelper.CVertexWeight(i, p, n, ((BoneWeight) ref boneWeight).get_weight0()));
      }
      if ((double) ((BoneWeight) ref boneWeight).get_weight1() != 0.0)
      {
        Vector3 p = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex1()]).MultiplyPoint3x4(vertices[i]);
        Vector3 n = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex1()]).MultiplyPoint3x4(normals[i]);
        this.nodeWeights[((BoneWeight) ref boneWeight).get_boneIndex1()].weights.Add((object) new SkinnedCollisionHelper.CVertexWeight(i, p, n, ((BoneWeight) ref boneWeight).get_weight1()));
      }
      if ((double) ((BoneWeight) ref boneWeight).get_weight2() != 0.0)
      {
        Vector3 p = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex2()]).MultiplyPoint3x4(vertices[i]);
        Vector3 n = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex2()]).MultiplyPoint3x4(normals[i]);
        this.nodeWeights[((BoneWeight) ref boneWeight).get_boneIndex2()].weights.Add((object) new SkinnedCollisionHelper.CVertexWeight(i, p, n, ((BoneWeight) ref boneWeight).get_weight2()));
      }
      if ((double) ((BoneWeight) ref boneWeight).get_weight3() != 0.0)
      {
        Vector3 p = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex3()]).MultiplyPoint3x4(vertices[i]);
        Vector3 n = ((Matrix4x4) ref bindposes[((BoneWeight) ref boneWeight).get_boneIndex3()]).MultiplyPoint3x4(normals[i]);
        this.nodeWeights[((BoneWeight) ref boneWeight).get_boneIndex3()].weights.Add((object) new SkinnedCollisionHelper.CVertexWeight(i, p, n, ((BoneWeight) ref boneWeight).get_weight3()));
      }
    }
    this.UpdateCollisionMesh(false);
    this.IsInit = true;
    return true;
  }

  public bool Release()
  {
    Object.Destroy((Object) this.meshCalc);
    return true;
  }

  public void UpdateCollisionMesh(bool _bRelease = true)
  {
    Vector3[] vertices = this.meshCalc.get_vertices();
    for (int index = 0; index < vertices.Length; ++index)
      vertices[index] = Vector3.get_zero();
    foreach (SkinnedCollisionHelper.CWeightList nodeWeight in this.nodeWeights)
    {
      Matrix4x4 localToWorldMatrix = nodeWeight.transform.get_localToWorldMatrix();
      foreach (SkinnedCollisionHelper.CVertexWeight weight in nodeWeight.weights)
      {
        ref Vector3 local = ref vertices[weight.index];
        local = Vector3.op_Addition(local, Vector3.op_Multiply(((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(weight.localPosition), weight.weight));
      }
    }
    for (int index = 0; index < vertices.Length; ++index)
      vertices[index] = ((Component) this).get_transform().InverseTransformPoint(vertices[index]);
    this.meshCalc.set_vertices(vertices);
    ((Collider) this.meshCollider).set_enabled(false);
    ((Collider) this.meshCollider).set_enabled(true);
  }

  private void Update()
  {
  }

  private void LateUpdate()
  {
    if (!this.IsInit || !this.forceUpdate)
      return;
    if (this.updateOncePerFrame)
      this.forceUpdate = false;
    this.UpdateCollisionMesh(true);
  }

  private class CVertexWeight
  {
    public int index;
    public Vector3 localPosition;
    public Vector3 localNormal;
    public float weight;

    public CVertexWeight(int i, Vector3 p, Vector3 n, float w)
    {
      this.index = i;
      this.localPosition = p;
      this.localNormal = n;
      this.weight = w;
    }
  }

  private class CWeightList
  {
    public Transform transform;
    public ArrayList weights;

    public CWeightList()
    {
      this.weights = new ArrayList();
    }
  }
}
