// Decompiled with JetBrains decompiler
// Type: MetaballShoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MetaballShoot
{
  public string comment = string.Empty;
  public bool isEnable = true;
  [Header("PARAM")]
  public float drag = 1f;
  public float dragDropDown = 1f;
  public int maxMetaball = 30;
  public float timeDropDown = 1f;
  public float speedSMin = 1f;
  public float speedSMax = 1f;
  public float speedMMin = 1f;
  public float speedMMax = 1f;
  public float speedLMin = 1f;
  public float speedLMax = 1f;
  public Vector2 randomRotationS = Vector2.get_zero();
  public Vector2 randomRotationM = Vector2.get_zero();
  public Vector2 randomRotationL = Vector2.get_zero();
  [SerializeField]
  private List<MetaballShoot.MetaInfo> lstMeta = new List<MetaballShoot.MetaInfo>();
  private float timeInterval = 9999999f;
  [Tooltip("発射軸")]
  public GameObject ShootAxis;
  [Tooltip("生成する弾")]
  public GameObject ShootObj;
  [Tooltip("SourceRoot")]
  public GameObject objSourceRoot;
  [Tooltip("止まった時に親子付する場所")]
  public Transform parentTransform;
  public float[] aInterval;
  public ForceMode shootForce;
  public ChaControl chaCustom;
  private const float timeConstMax = 9999999f;
  private int numInterval;

  public bool isConstMetaMesh { get; private set; }

  public bool IsCreate()
  {
    for (int index1 = 0; index1 < this.lstMeta.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.lstMeta[index1].objMeta.aRigid.Length; ++index2)
      {
        if (!this.lstMeta[index1].objMeta.aRigid[index2].get_isKinematic() && !this.lstMeta[index1].objMeta.aRigid[index2].IsSleeping())
          return true;
      }
    }
    return false;
  }

  private void OnValidate()
  {
  }

  public bool ShootMetaBallStart()
  {
    this.numInterval = -1;
    this.timeInterval = 0.0f;
    if (Object.op_Implicit((Object) this.parentTransform))
      this.isConstMetaMesh = true;
    return true;
  }

  public void MetaBallClear()
  {
    for (int index = 0; index < this.lstMeta.Count; ++index)
      Object.Destroy((Object) ((Component) this.lstMeta[index].objMeta).get_gameObject());
    this.lstMeta.Clear();
    this.isConstMetaMesh = false;
  }

  public bool ShootMetaBall()
  {
    if ((double) this.timeInterval == 9999999.0)
      return false;
    if (this.numInterval != -1)
    {
      this.timeInterval += Time.get_deltaTime();
      if ((double) this.timeInterval < (double) this.aInterval[this.numInterval])
        return false;
    }
    this.MetaBallInstantiate();
    ++this.numInterval;
    this.timeInterval = 0.0f;
    if (this.aInterval.Length <= this.numInterval)
      this.timeInterval = 9999999f;
    return true;
  }

  private bool MetaBallInstantiate()
  {
    if (!Object.op_Implicit((Object) this.ShootAxis) || !Object.op_Implicit((Object) this.ShootObj) || !Object.op_Implicit((Object) this.objSourceRoot))
      return false;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.ShootObj);
    if (!Object.op_Implicit((Object) gameObject))
      return false;
    ((Object) gameObject).set_name(((Object) this.ShootObj).get_name());
    gameObject.get_transform().SetParent(this.objSourceRoot.get_transform(), false);
    gameObject.get_transform().set_position(this.ShootAxis.get_transform().get_position());
    gameObject.get_transform().set_rotation(this.ShootAxis.get_transform().get_rotation());
    metaballinfo component = (metaballinfo) gameObject.GetComponent<metaballinfo>();
    if (!Object.op_Implicit((Object) component))
    {
      Object.Destroy((Object) gameObject);
      return false;
    }
    if (Object.op_Implicit((Object) component.rigidBeginning))
    {
      component.rigidBeginning.set_drag(this.drag);
      float num1 = 0.5f;
      if (Object.op_Inequality((Object) this.chaCustom, (Object) null))
        num1 = this.chaCustom.GetShapeBodyValue(0);
      Vector3 vector3_1 = ((Component) component.rigidBeginning).get_transform().InverseTransformDirection(this.ShootAxis.get_transform().get_forward());
      float num2 = (double) num1 < 0.5 ? Mathf.InverseLerp(0.0f, 0.5f, num1) : Mathf.InverseLerp(0.5f, 1f, num1);
      Vector2 vector2 = (double) num1 < 0.5 ? Vector2.Lerp(this.randomRotationS, this.randomRotationM, num2) : Vector2.Lerp(this.randomRotationM, this.randomRotationL, num2);
      Vector3 vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(Random.Range((float) -vector2.x, (float) vector2.x), Random.Range((float) -vector2.y, (float) vector2.y), 0.0f), vector3_1);
      float num3 = (double) num1 < 0.5 ? Mathf.Lerp(this.speedSMin, this.speedMMin, num2) : Mathf.Lerp(this.speedMMin, this.speedLMin, num2);
      float num4 = (double) num1 < 0.5 ? Mathf.Lerp(this.speedSMax, this.speedMMax, num2) : Mathf.Lerp(this.speedMMax, this.speedLMax, num2);
      component.rigidBeginning.AddRelativeForce(Vector3.op_Multiply(vector3_2, Random.Range(num3, num4)), this.shootForce);
    }
    this.lstMeta.Add(new MetaballShoot.MetaInfo()
    {
      objMeta = component,
      nowDrag = this.drag,
      timeLerpDragRand = this.timeDropDown
    });
    if (this.lstMeta.Count > this.maxMetaball)
    {
      Object.Destroy((Object) ((Component) this.lstMeta[0].objMeta).get_gameObject());
      this.lstMeta.RemoveAt(0);
    }
    return true;
  }

  [Serializable]
  public class MetaInfo
  {
    public metaballinfo objMeta;
    public float timeLerpDrag;
    public float timeLerpDragRand;
    public float nowDrag;
  }
}
