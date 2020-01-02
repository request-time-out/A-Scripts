// Decompiled with JetBrains decompiler
// Type: H_Lookat_dan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEx;

public class H_Lookat_dan : MonoBehaviour
{
  private ChaControl[] females;
  private ChaControl male;
  private StringBuilder assetName;
  public List<H_Lookat_dan.MotionLookAtList> lstLookAt;
  public string strPlayMotion;
  public Transform transLookAtNull;
  public bool bTopStick;
  public bool bManual;
  public H_Lookat_dan.ShapeInfo[] lstShape;
  public int numFemale;
  [SerializeField]
  public H_LookAtDan_Info dan_Info;
  public ValueTuple<GameObject, Transform> DanBase;
  public GameObject DanTop;
  public GameObject DanBaseR;

  public H_Lookat_dan()
  {
    base.\u002Ector();
  }

  public void DankonInit(ChaControl _male, ChaControl[] _females)
  {
    this.females = _females;
    this.male = _male;
    if (Object.op_Equality((Object) this.male.objBodyBone, (Object) null))
      return;
    Transform transform = this.male.objBodyBone.get_transform();
    if (Object.op_Inequality((Object) transform, (Object) null))
    {
      GameObject loop = transform.FindLoop("cm_J_dan101_00");
      this.DanBase = new ValueTuple<GameObject, Transform>(loop, loop.get_transform());
      this.DanTop = transform.FindLoop("cm_J_dan109_00");
      this.DanBaseR = ((Component) ((Transform) this.DanBase.Item2).get_parent()).get_gameObject();
      this.dan_Info.SetUpAxisTransform(this.DanBaseR.get_transform());
    }
    this.dan_Info.SetLookAtTransform((Transform) this.DanBase.Item2);
  }

  public bool LoadList(string _pathFile, int motionId = -1)
  {
    this.Release();
    if (_pathFile == string.Empty)
      return false;
    this.assetName.Append(_pathFile);
    this.assetName.Replace("_m_", "_");
    List<H_Lookat_dan.MotionLookAtList> motionLookAtListList;
    if (!Singleton<Resources>.Instance.HSceneTable.DicLstLookAtDan.TryGetValue(this.assetName.ToString(), out motionLookAtListList))
      motionLookAtListList = new List<H_Lookat_dan.MotionLookAtList>();
    this.lstLookAt = new List<H_Lookat_dan.MotionLookAtList>((IEnumerable<H_Lookat_dan.MotionLookAtList>) motionLookAtListList);
    if (this.lstLookAt.Count != 0)
      this.setInfo(this.lstLookAt[0]);
    return true;
  }

  public void Release()
  {
    this.lstLookAt.Clear();
    this.assetName.Clear();
    this.strPlayMotion = string.Empty;
    this.transLookAtNull = (Transform) null;
    this.bTopStick = false;
    this.dan_Info.SetTargetTransform((Transform) null);
  }

  private void LateUpdate()
  {
    if (Object.op_Equality((Object) this.male, (Object) null) || this.females == null || (Object.op_Equality((Object) this.male.objBodyBone, (Object) null) || Object.op_Equality((Object) this.females[0].objBodyBone, (Object) null)))
      return;
    this.setLookAt();
    if (this.lstShape != null && Object.op_Inequality((Object) this.transLookAtNull, (Object) null))
    {
      Vector3 vector3_1 = this.transLookAtNull.get_position();
      for (int index = 0; index < this.lstShape.Length; ++index)
      {
        if (this.lstShape[index].bUse)
        {
          float shapeBodyValue = this.females[this.numFemale].GetShapeBodyValue(this.lstShape[index].shape);
          Vector3 vector3_2 = this.transLookAtNull.TransformDirection((double) shapeBodyValue < 0.5 ? Vector3.Lerp(this.lstShape[index].minPos, this.lstShape[index].middlePos, Mathf.InverseLerp(0.0f, 0.5f, shapeBodyValue)) : Vector3.Lerp(this.lstShape[index].middlePos, this.lstShape[index].maxPos, Mathf.InverseLerp(0.5f, 1f, shapeBodyValue)));
          vector3_1 = Vector3.op_Addition(vector3_1, vector3_2);
        }
      }
      this.transLookAtNull.set_position(vector3_1);
    }
    H_Lookat_dan.LookAtProc(this);
  }

  private bool setLookAt()
  {
    AnimatorStateInfo animatorStateInfo = this.females[0].getAnimatorStateInfo(0);
    if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.strPlayMotion))
      return true;
    foreach (H_Lookat_dan.MotionLookAtList _list in this.lstLookAt)
    {
      if (((AnimatorStateInfo) ref animatorStateInfo).IsName(_list.strMotion))
      {
        this.setInfo(_list);
        break;
      }
    }
    return true;
  }

  private bool setInfo(H_Lookat_dan.MotionLookAtList _list)
  {
    if (_list == null)
      return false;
    if (Object.op_Equality((Object) this.females[_list.numFemale].objBodyBone, (Object) null))
    {
      this.transLookAtNull = (Transform) null;
      this.lstShape = (H_Lookat_dan.ShapeInfo[]) null;
      return false;
    }
    this.strPlayMotion = _list.strMotion;
    this.numFemale = _list.numFemale;
    if (_list.strLookAtNull == string.Empty)
    {
      this.transLookAtNull = (Transform) null;
      this.lstShape = (H_Lookat_dan.ShapeInfo[]) null;
    }
    else
    {
      GameObject loop = this.females[_list.numFemale].objBodyBone.get_transform().FindLoop(_list.strLookAtNull);
      this.transLookAtNull = !Object.op_Inequality((Object) loop, (Object) null) ? (Transform) null : loop.get_transform();
      this.lstShape = _list.lstShape;
    }
    this.bTopStick = _list.bTopStick;
    this.bManual = _list.bManual;
    Transform transform = (Transform) this.DanBase.Item2;
    this.dan_Info.SetTargetTransform(this.transLookAtNull);
    this.dan_Info.SetOldRotation(!Object.op_Inequality((Object) transform, (Object) null) ? Quaternion.get_identity() : transform.get_rotation());
    return true;
  }

  public static bool LookAtProc(H_Lookat_dan h_Lookat_Dan)
  {
    if (Object.op_Equality((Object) h_Lookat_Dan.DanBase.Item1, (Object) null) || Object.op_Equality((Object) h_Lookat_Dan.transLookAtNull, (Object) null))
      return false;
    if (!h_Lookat_Dan.bManual)
      ((Transform) h_Lookat_Dan.DanBase.Item2).LookAt(h_Lookat_Dan.transLookAtNull);
    else
      h_Lookat_Dan.dan_Info.ManualCalc();
    if (h_Lookat_Dan.bTopStick && Object.op_Inequality((Object) h_Lookat_Dan.DanTop, (Object) null))
      h_Lookat_Dan.DanTop.get_transform().set_position(h_Lookat_Dan.transLookAtNull.get_position());
    return true;
  }

  [Serializable]
  public struct ShapeInfo
  {
    public int shape;
    public Vector3 minPos;
    public Vector3 middlePos;
    public Vector3 maxPos;
    public bool bUse;
  }

  [Serializable]
  public class MotionLookAtList
  {
    public H_Lookat_dan.ShapeInfo[] lstShape = new H_Lookat_dan.ShapeInfo[10];
    public string strMotion;
    public int numFemale;
    public string strLookAtNull;
    public bool bTopStick;
    public bool bManual;

    public MotionLookAtList()
    {
      for (int index = 0; index < this.lstShape.Length; ++index)
        this.lstShape[index].bUse = false;
    }
  }
}
