// Decompiled with JetBrains decompiler
// Type: MotionIK
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Correct;
using Correct.Process;
using IllusionUtility.GetUtility;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MotionIK
{
  public bool MapIK = true;
  private Transform[] DefBone = new Transform[8];
  private Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>>[] calcBlend = new Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>>[4]
  {
    new Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>>(),
    new Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>>(),
    new Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>>(),
    new Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>>()
  };
  private Dictionary<int, List<MotionIKData.BlendWeightInfo>>[] calcBlendBend = new Dictionary<int, List<MotionIKData.BlendWeightInfo>>[4]
  {
    new Dictionary<int, List<MotionIKData.BlendWeightInfo>>(),
    new Dictionary<int, List<MotionIKData.BlendWeightInfo>>(),
    new Dictionary<int, List<MotionIKData.BlendWeightInfo>>(),
    new Dictionary<int, List<MotionIKData.BlendWeightInfo>>()
  };
  private BaseData tmpData;
  private BaseProcess tmpProcess;
  private List<GameObject> tmpBones;
  private float nowKeyFrame;
  public int layerNo;
  private MotionIKData tmpMapData;

  public MotionIK(ChaControl info, bool isHscene = false, MotionIKData data = null)
  {
    this.info = info;
    this.data = data ?? new MotionIKData();
    Animator animBody = info.animBody;
    this.ik = (FullBodyBipedIK) ((Component) animBody).GetComponent<FullBodyBipedIK>();
    if (Object.op_Inequality((Object) this.ik, (Object) null))
    {
      this.frameCorrect = (FrameCorrect) ((Component) animBody).GetComponent<FrameCorrect>();
      this.ikCorrect = (IKCorrect) ((Component) animBody).GetComponent<IKCorrect>();
    }
    this.SetPartners((MotionIK[]) Array.Empty<MotionIK>());
    this.Reset();
    if (isHscene)
      return;
    this.tmpBones = new List<GameObject>();
    ((Component) animBody).get_transform().FindLoopPrefix(this.tmpBones, "f_pv_");
    this.DefBone[0] = this.ListFindBone(this.tmpBones, "f_pv_arm_L");
    this.DefBone[1] = this.ListFindBone(this.tmpBones, "f_pv_elbo_L");
    this.DefBone[2] = this.ListFindBone(this.tmpBones, "f_pv_arm_R");
    this.DefBone[3] = this.ListFindBone(this.tmpBones, "f_pv_elbo_R");
    this.DefBone[4] = this.ListFindBone(this.tmpBones, "f_pv_leg_L");
    this.DefBone[5] = this.ListFindBone(this.tmpBones, "f_pv_knee_L");
    this.DefBone[6] = this.ListFindBone(this.tmpBones, "f_pv_leg_R");
    this.DefBone[7] = this.ListFindBone(this.tmpBones, "f_pv_knee_R");
  }

  public static List<MotionIK> Setup(List<ChaControl> infos)
  {
    List<MotionIK> partners = new List<MotionIK>();
    for (int index1 = 0; index1 < infos.Count; ++index1)
    {
      int index2 = index1;
      partners.Add(new MotionIK(infos[index2], false, (MotionIKData) null));
    }
    for (int index = 0; index < partners.Count; ++index)
      partners[index].SetPartners(partners);
    return partners;
  }

  public static Vector3 GetShapeLerpPositionValue(
    float shape,
    Vector3 min,
    Vector3 med,
    Vector3 max,
    bool MapIK = false)
  {
    return !MapIK ? ((double) shape < 0.5 ? Vector3.Lerp(min, med, Mathf.InverseLerp(0.0f, 0.5f, shape)) : Vector3.Lerp(med, max, Mathf.InverseLerp(0.5f, 1f, shape))) : ((double) shape < 0.5 ? Vector3.Lerp(min, Vector3.get_zero(), Mathf.InverseLerp(0.0f, 0.5f, shape)) : Vector3.Lerp(Vector3.get_zero(), max, Mathf.InverseLerp(0.5f, 1f, shape)));
  }

  public static Vector3 GetShapeLerpAngleValue(
    float shape,
    Vector3 min,
    Vector3 med,
    Vector3 max,
    bool MapIK = false)
  {
    Vector3 zero = Vector3.get_zero();
    if ((double) shape >= 0.5)
    {
      float num = Mathf.InverseLerp(0.5f, 1f, shape);
      for (int index = 0; index < 3; ++index)
      {
        if (MapIK)
          ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(0.0f, ((Vector3) ref max).get_Item(index), num));
        else
          ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(((Vector3) ref med).get_Item(index), ((Vector3) ref max).get_Item(index), num));
      }
    }
    else
    {
      float num = Mathf.InverseLerp(0.0f, 0.5f, shape);
      for (int index = 0; index < 3; ++index)
      {
        if (MapIK)
          ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(((Vector3) ref min).get_Item(index), 0.0f, num));
        else
          ((Vector3) ref zero).set_Item(index, Mathf.LerpAngle(((Vector3) ref min).get_Item(index), ((Vector3) ref med).get_Item(index), num));
      }
    }
    return zero;
  }

  public void SetPartners(params MotionIK[] partners)
  {
    this.partners = new MotionIK[partners.Length != 0 ? partners.Length : 1];
    this.partners[0] = this;
    int[] numArray = new int[2]{ 1, 0 };
    for (int index = 0; index < partners.Length; ++index)
    {
      numArray[1] = index;
      if (this.partners[0] != partners[numArray[1]])
      {
        this.partners[numArray[0]] = partners[numArray[1]];
        ++numArray[0];
      }
    }
  }

  public void SetPartners(List<MotionIK> partners)
  {
    MotionIK[] motionIkArray = new MotionIK[partners.Count];
    for (int index1 = 0; index1 < motionIkArray.Length; ++index1)
    {
      int index2 = index1;
      motionIkArray[index2] = partners[index2];
    }
    this.SetPartners(motionIkArray);
  }

  public void SetPartners(List<Tuple<int, int, MotionIK>> partners)
  {
    MotionIK[] motionIkArray = new MotionIK[partners.Count];
    for (int index1 = 0; index1 < motionIkArray.Length; ++index1)
    {
      int index2 = index1;
      motionIkArray[index2] = partners[index2].Item3;
    }
    this.SetPartners(motionIkArray);
  }

  public void SetItems(MotionIK motionIK, GameObject[] items)
  {
    for (int index1 = 0; index1 < this.partners.Length; ++index1)
    {
      for (int index2 = 0; index2 < items.Length; ++index2)
      {
        if (this.partners[index1].items == null)
          this.partners[index1].items = new List<GameObject>();
        if (!this.partners[index1].items.Contains(items[index2]))
          this.partners[index1].items.Add(items[index2]);
      }
    }
  }

  public void Reset()
  {
    this.InitFrameCalc();
    this.enabled = false;
  }

  public void Release()
  {
    this.data.Release();
  }

  public void LoadData(string abName, string assetName, bool add = false)
  {
    if (this.data == null)
      this.data = new MotionIKData();
    this.data.AIRead(abName, assetName, add);
  }

  public ChaControl info { get; private set; }

  public List<GameObject> items { get; private set; }

  public FullBodyBipedIK ik { get; private set; }

  public MotionIK[] partners { get; private set; }

  public MotionIK.IKTargetPair[] ikTargetPairs
  {
    get
    {
      return Object.op_Equality((Object) this.ik, (Object) null) ? (MotionIK.IKTargetPair[]) null : MotionIK.IKTargetPair.GetPairs((IKSolverFullBodyBiped) this.ik.solver);
    }
  }

  public MotionIKData data { get; private set; }

  public FrameCorrect frameCorrect { get; private set; }

  public IKCorrect ikCorrect { get; private set; }

  public bool enabled
  {
    get
    {
      return Object.op_Inequality((Object) this.ik, (Object) null) && ((Behaviour) this.ik).get_enabled();
    }
    set
    {
      if (Object.op_Equality((Object) this.ik, (Object) null))
        return;
      ((Behaviour) this.ik).set_enabled(value);
      this.ikCorrect.isEnabled = value;
    }
  }

  public string[] stateNames
  {
    get
    {
      string[] strArray = new string[this.data.states.Length];
      int index1 = 0;
      for (int index2 = 0; index2 < strArray.Length; ++index2)
        strArray[index1] = this.data.states[index1].name;
      return this.data.states == null ? new string[0] : strArray;
    }
  }

  public void InitFrameCalc()
  {
    if (Object.op_Inequality((Object) this.frameCorrect, (Object) null))
    {
      foreach (BaseCorrect.Info info in this.frameCorrect.list)
      {
        info.enabled = false;
        info.pos = Vector3.get_zero();
        info.ang = Vector3.get_zero();
      }
    }
    if (!Object.op_Inequality((Object) this.ikCorrect, (Object) null))
      return;
    foreach (BaseCorrect.Info info in this.ikCorrect.list)
    {
      info.enabled = false;
      info.pos = Vector3.get_zero();
      info.ang = Vector3.get_zero();
      info.bone = (Transform) null;
    }
  }

  public MotionIKData.State InitState(string stateName, int sex)
  {
    return this.data.InitState(stateName, sex);
  }

  public MotionIKData.State GetNowState(string stateName)
  {
    if (this.data.states == null)
      return (MotionIKData.State) null;
    int index1 = -1;
    for (int index2 = 0; index2 < this.data.states.Length; ++index2)
    {
      if (!(this.data.states[index2].name != stateName))
      {
        index1 = index2;
        break;
      }
    }
    return index1 == -1 ? (MotionIKData.State) null : this.data.states[index1];
  }

  public MotionIKData.State GetNowState(int hashName)
  {
    if (this.data == null || this.data.states == null)
      return (MotionIKData.State) null;
    int index1 = -1;
    for (int index2 = 0; index2 < this.data.states.Length; ++index2)
    {
      if (Animator.StringToHash(this.data.states[index2].name) == hashName)
      {
        index1 = index2;
        break;
      }
    }
    return index1 == -1 ? (MotionIKData.State) null : this.data.states[index1];
  }

  public MotionIKData.Frame[] GetNowFrames(string stateName)
  {
    return this.GetNowState(stateName)?.frames;
  }

  public void SetMapIK(string AnimatorName)
  {
    if (!Singleton<Resources>.Instance.MapIKData.TryGetValue(AnimatorName, out this.tmpMapData))
      this.tmpMapData = (MotionIKData) null;
    this.data = this.tmpMapData != null ? this.tmpMapData.Copy() : (MotionIKData) null;
  }

  public void Calc(string stateName)
  {
    if (Object.op_Equality((Object) this.frameCorrect, (Object) null))
      return;
    this.InitFrameCalc();
    MotionIKData.State nowState = this.GetNowState(stateName);
    if (nowState != null)
    {
      int ikTargetLength = MotionIK.IKTargetPair.IKTargetLength;
      foreach (MotionIKData.Frame frame in nowState.frames)
      {
        int index = frame.frameNo - ikTargetLength;
        if (index >= 0)
        {
          BaseCorrect.Info info = this.frameCorrect.list[index];
          info.enabled = true;
          Vector3[] correctShapeValues = this.GetCorrectShapeValues(this.partners[frame.editNo].info, frame.shapes);
          info.pos = correctShapeValues[0];
          info.ang = correctShapeValues[1];
        }
      }
    }
    this.enabled = nowState != null;
    for (int index1 = 0; index1 < this.ikTargetPairs.Length; ++index1)
    {
      int index2 = index1;
      this.LinkIK(index2, nowState, this.ikTargetPairs[index2]);
    }
  }

  public void Calc(int hashName)
  {
    if (Object.op_Equality((Object) this.frameCorrect, (Object) null))
      return;
    this.InitFrameCalc();
    MotionIKData.State nowState = this.GetNowState(hashName);
    if (nowState != null)
    {
      int ikTargetLength = MotionIK.IKTargetPair.IKTargetLength;
      foreach (MotionIKData.Frame frame in nowState.frames)
      {
        int index = frame.frameNo - ikTargetLength;
        if (index >= 0)
        {
          BaseCorrect.Info info = this.frameCorrect.list[index];
          info.enabled = true;
          Vector3[] correctShapeValues = this.GetCorrectShapeValues(this.partners[frame.editNo].info, frame.shapes);
          info.pos = correctShapeValues[0];
          info.ang = correctShapeValues[1];
        }
      }
    }
    this.enabled = this.MapIK || nowState != null;
    if (!this.MapIK)
    {
      for (int index1 = 0; index1 < this.ikTargetPairs.Length; ++index1)
      {
        int index2 = index1;
        this.LinkIK(index2, nowState, this.ikTargetPairs[index2]);
      }
    }
    else if (nowState != null)
    {
      for (int index1 = 0; index1 < this.ikTargetPairs.Length; ++index1)
      {
        int index2 = index1;
        this.LinkIK(index2, nowState, this.ikTargetPairs[index2]);
      }
    }
    else
    {
      for (int index1 = 0; index1 < this.ikTargetPairs.Length; ++index1)
      {
        int index2 = index1;
        this.tmpData = (BaseData) ((Component) this.ikTargetPairs[index2].effector.target).GetComponent<BaseData>();
        if (Object.op_Equality((Object) this.tmpData.bone, (Object) null))
        {
          this.tmpData.bone = this.DefBone[index2 * 2];
          ((Behaviour) ((Component) this.tmpData).GetComponent<BaseProcess>()).set_enabled(true);
          this.ikTargetPairs[index2].effector.positionWeight = (__Null) 1.0;
          this.ikTargetPairs[index2].effector.rotationWeight = (__Null) 1.0;
        }
        this.tmpData = (BaseData) ((Component) this.ikTargetPairs[index2].bend.bendGoal).GetComponent<BaseData>();
        if (Object.op_Equality((Object) this.tmpData.bone, (Object) null))
        {
          ((Behaviour) ((Component) this.tmpData).GetComponent<BaseProcess>()).set_enabled(true);
          this.tmpData.bone = this.DefBone[index2 * 2 + 1];
          this.ikTargetPairs[index2].bend.weight = (__Null) 1.0;
        }
      }
    }
  }

  private Vector3[] GetCorrectShapeValues(ChaControl chara, MotionIKData.Shape[] shapes)
  {
    Vector3[] vector3Array = new Vector3[2]
    {
      Vector3.get_zero(),
      Vector3.get_zero()
    };
    foreach (MotionIKData.Shape shape in shapes)
    {
      float shapeBodyValue = chara.GetShapeBodyValue(shape.shapeNo);
      for (int index = 0; index < vector3Array.Length; ++index)
      {
        if (index == 0)
        {
          ref Vector3 local = ref vector3Array[index];
          local = Vector3.op_Addition(local, MotionIK.GetShapeLerpPositionValue(shapeBodyValue, shape.small[index], shape.mediam[index], shape.large[index], this.MapIK));
        }
        else
        {
          ref Vector3 local = ref vector3Array[index];
          local = Vector3.op_Addition(local, MotionIK.GetShapeLerpAngleValue(shapeBodyValue, shape.small[index], shape.mediam[index], shape.large[index], this.MapIK));
        }
      }
    }
    return vector3Array;
  }

  private Vector3[] GetCorrectShapeValues(
    ChaControl chara,
    float nowKeyFrame,
    Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>> calcBlend)
  {
    Vector3[] vector3Array = new Vector3[2]
    {
      Vector3.get_zero(),
      Vector3.get_zero()
    };
    for (int index1 = 0; index1 < 2; ++index1)
    {
      foreach (KeyValuePair<int, List<MotionIKData.BlendWeightInfo>> keyValuePair in calcBlend[index1])
      {
        int index2 = -1;
        for (int index3 = 0; index3 < keyValuePair.Value.Count; ++index3)
        {
          int index4 = index3;
          if ((double) keyValuePair.Value[index4].StartKey <= (double) nowKeyFrame)
            index2 = index3;
        }
        if (index2 < 0)
          index2 = 0;
        MotionIKData.BlendWeightInfo blendWeightInfo = keyValuePair.Value[index2];
        float shapeBodyValue = chara.GetShapeBodyValue(blendWeightInfo.shape.shapeNo);
        float num = Mathf.InverseLerp(blendWeightInfo.StartKey, blendWeightInfo.EndKey, nowKeyFrame);
        if (index1 == 0)
        {
          Vector3 vector3 = MotionIK.GetShapeLerpPositionValue(shapeBodyValue, blendWeightInfo.shape.small[index1], blendWeightInfo.shape.mediam[index1], blendWeightInfo.shape.large[index1], this.MapIK);
          if (blendWeightInfo.pattern == 1)
            vector3 = Vector3.Lerp(Vector3.get_zero(), vector3, num);
          else if (blendWeightInfo.pattern == 2)
            vector3 = Vector3.Lerp(vector3, Vector3.get_zero(), num);
          ref Vector3 local = ref vector3Array[index1];
          local = Vector3.op_Addition(local, vector3);
        }
        else
        {
          Vector3 vector3 = MotionIK.GetShapeLerpAngleValue(shapeBodyValue, blendWeightInfo.shape.small[index1], blendWeightInfo.shape.mediam[index1], blendWeightInfo.shape.large[index1], this.MapIK);
          if (blendWeightInfo.pattern == 1)
            vector3 = Vector3.Lerp(Vector3.get_zero(), vector3, num);
          else if (blendWeightInfo.pattern == 2)
            vector3 = Vector3.Lerp(vector3, Vector3.get_zero(), num);
          ref Vector3 local = ref vector3Array[index1];
          local = Vector3.op_Addition(local, vector3);
        }
      }
    }
    return vector3Array;
  }

  private Vector3[] GetCorrectShapeValues(
    ChaControl chara,
    float nowKeyFrame,
    Dictionary<int, List<MotionIKData.BlendWeightInfo>> calcBlend)
  {
    Vector3[] vector3Array = new Vector3[2]
    {
      Vector3.get_zero(),
      Vector3.get_zero()
    };
    foreach (KeyValuePair<int, List<MotionIKData.BlendWeightInfo>> keyValuePair in calcBlend)
    {
      int index1 = -1;
      for (int index2 = 0; index2 < keyValuePair.Value.Count; ++index2)
      {
        int index3 = index2;
        if ((double) keyValuePair.Value[index3].StartKey <= (double) nowKeyFrame)
          index1 = index2;
      }
      if (index1 < 0)
        index1 = 0;
      MotionIKData.BlendWeightInfo blendWeightInfo = keyValuePair.Value[index1];
      float shapeBodyValue = chara.GetShapeBodyValue(blendWeightInfo.shape.shapeNo);
      float num = Mathf.InverseLerp(blendWeightInfo.StartKey, blendWeightInfo.EndKey, nowKeyFrame);
      Vector3 vector3_1 = MotionIK.GetShapeLerpPositionValue(shapeBodyValue, blendWeightInfo.shape.small[0], blendWeightInfo.shape.mediam[0], blendWeightInfo.shape.large[0], this.MapIK);
      if (blendWeightInfo.pattern == 1)
        vector3_1 = Vector3.Lerp(Vector3.get_zero(), vector3_1, num);
      else if (blendWeightInfo.pattern == 2)
        vector3_1 = Vector3.Lerp(vector3_1, Vector3.get_zero(), num);
      ref Vector3 local1 = ref vector3Array[0];
      local1 = Vector3.op_Addition(local1, vector3_1);
      Vector3 vector3_2 = MotionIK.GetShapeLerpAngleValue(shapeBodyValue, blendWeightInfo.shape.small[1], blendWeightInfo.shape.mediam[1], blendWeightInfo.shape.large[1], this.MapIK);
      if (blendWeightInfo.pattern == 1)
        vector3_2 = Vector3.Lerp(Vector3.get_zero(), vector3_2, num);
      else if (blendWeightInfo.pattern == 2)
        vector3_2 = Vector3.Lerp(vector3_2, Vector3.get_zero(), num);
      ref Vector3 local2 = ref vector3Array[1];
      local2 = Vector3.op_Addition(local2, vector3_2);
    }
    return vector3Array;
  }

  private void LinkIK(int index, MotionIKData.State state, MotionIK.IKTargetPair pair)
  {
    this.nowKeyFrame = 0.0f;
    MotionIKData.Parts parts = state?[index];
    Transform transform1 = (Transform) null;
    MotionIKData.Param2 obj1 = parts?.param2;
    IKEffector effector = pair.effector;
    this.tmpData = (BaseData) ((Component) effector.target).GetComponent<BaseData>();
    if (Object.op_Inequality((Object) this.tmpData, (Object) null))
    {
      this.tmpData.bone = obj1 != null ? this.getTarget(obj1.sex, obj1.target) : (Transform) null;
      transform1 = this.tmpData.bone;
      this.tmpProcess = (BaseProcess) ((Component) this.tmpData).GetComponent<BaseProcess>();
      if (Object.op_Inequality((Object) this.tmpProcess, (Object) null))
        ((Behaviour) this.tmpProcess).set_enabled(Object.op_Inequality((Object) this.tmpData.bone, (Object) null));
      MotionIKData.Frame frame = this.FindFrame(state, index * 2);
      if (frame.frameNo == -1)
      {
        this.tmpData.pos = Vector3.get_zero();
        this.tmpData.rot = Quaternion.get_identity();
        return;
      }
      Vector3[] vector3Array = new Vector3[2];
      Vector3[] correctShapeValues;
      if (!this.MapIK)
      {
        correctShapeValues = this.GetCorrectShapeValues(this.partners[frame.editNo].info, frame.shapes);
      }
      else
      {
        vector3Array[0] = this.tmpData.pos;
        ref Vector3 local = ref vector3Array[1];
        Quaternion rot = this.tmpData.rot;
        Vector3 eulerAngles = ((Quaternion) ref rot).get_eulerAngles();
        local = eulerAngles;
        this.CalcBlendSet(ref this.calcBlend[index], parts.param2.blendInfos);
        correctShapeValues = this.GetCorrectShapeValues(this.partners[0].info, this.nowKeyFrame, this.calcBlend[index]);
      }
      this.tmpData.pos = correctShapeValues[0];
      this.tmpData.rot = Quaternion.Euler(correctShapeValues[1]);
    }
    if (Object.op_Equality((Object) transform1, (Object) null) || obj1 == null)
    {
      effector.positionWeight = (__Null) 0.0;
      effector.rotationWeight = (__Null) 0.0;
    }
    else
    {
      effector.positionWeight = (__Null) (double) obj1.weightPos;
      effector.rotationWeight = (__Null) (double) obj1.weightAng;
    }
    Transform transform2 = (Transform) null;
    MotionIKData.Param3 obj2 = parts?.param3;
    IKConstraintBend bend = pair.bend;
    this.tmpData = (BaseData) ((Component) bend.bendGoal).GetComponent<BaseData>();
    if (Object.op_Inequality((Object) this.tmpData, (Object) null))
    {
      this.tmpData.bone = obj2 != null ? this.getTarget(0, obj2.chein) : (Transform) null;
      transform2 = this.tmpData.bone;
      this.tmpProcess = (BaseProcess) ((Component) this.tmpData).GetComponent<BaseProcess>();
      if (Object.op_Inequality((Object) this.tmpProcess, (Object) null))
        ((Behaviour) this.tmpProcess).set_enabled(Object.op_Inequality((Object) this.tmpData.bone, (Object) null));
      MotionIKData.Frame frame = this.FindFrame(state, index * 2 + 1);
      if (frame.frameNo == -1)
      {
        this.tmpData.pos = Vector3.get_zero();
        this.tmpData.rot = Quaternion.get_identity();
        return;
      }
      Vector3[] vector3Array = new Vector3[2];
      Vector3[] correctShapeValues;
      if (!this.MapIK)
      {
        correctShapeValues = this.GetCorrectShapeValues(this.partners[frame.editNo].info, frame.shapes);
      }
      else
      {
        vector3Array[0] = this.tmpData.pos;
        ref Vector3 local = ref vector3Array[1];
        Quaternion rot = this.tmpData.rot;
        Vector3 eulerAngles = ((Quaternion) ref rot).get_eulerAngles();
        local = eulerAngles;
        this.CalcBlendBendSet(ref this.calcBlendBend[index], parts.param3.blendInfos);
        correctShapeValues = this.GetCorrectShapeValues(this.partners[0].info, this.nowKeyFrame, this.calcBlendBend[index]);
      }
      this.tmpData.pos = correctShapeValues[0];
      this.tmpData.rot = Quaternion.Euler(correctShapeValues[1]);
    }
    if (Object.op_Equality((Object) transform2, (Object) null) || obj2 == null)
      bend.weight = (__Null) 0.0;
    else
      bend.weight = (__Null) (double) obj2.weight;
  }

  private Transform getTarget(int sex, string frameName)
  {
    if (frameName.IsNullOrEmpty())
      return (Transform) null;
    Transform transform = (Transform) null;
    Transform[] transformArray = sex >= this.partners.Length ? (Transform[]) this.partners[0].items[sex - this.partners.Length].GetComponentsInChildren<Transform>() : (Transform[]) ((Component) this.partners[sex].info).GetComponentsInChildren<Transform>();
    if (transformArray == null)
      return (Transform) null;
    for (int index1 = 0; index1 < transformArray.Length; ++index1)
    {
      int index2 = index1;
      if (!(((Object) transformArray[index2]).get_name() != frameName))
      {
        transform = transformArray[index2];
        break;
      }
    }
    return transform;
  }

  private MotionIKData.Frame FindFrame(MotionIKData.State state, int no)
  {
    MotionIKData.Frame frame = new MotionIKData.Frame();
    frame.frameNo = -1;
    if (state == null)
      return frame;
    for (int index1 = 0; index1 < state.frames.Length; ++index1)
    {
      int index2 = index1;
      if (state.frames[index2].frameNo == no)
        frame = state.frames[index2];
    }
    return frame;
  }

  private Transform ListFindBone(List<GameObject> list, string name)
  {
    Transform transform = (Transform) null;
    using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        GameObject current = enumerator.Current;
        if (!(((Object) current).get_name() != name))
        {
          transform = current.get_transform();
          break;
        }
      }
    }
    return transform;
  }

  public void ChangeWeight(int nameHash, AnimatorStateInfo info)
  {
    if (this.data == null)
      return;
    int num = -1;
    for (int index = 0; index < this.data.states.Length; ++index)
    {
      if (Animator.StringToHash(this.data.states[index].name) == nameHash)
      {
        num = index;
        break;
      }
    }
    if (num < 0)
      return;
    this.nowKeyFrame = !((AnimatorStateInfo) ref info).get_loop() ? Mathf.Clamp01(((AnimatorStateInfo) ref info).get_normalizedTime()) : ((AnimatorStateInfo) ref info).get_normalizedTime() % 1f;
    for (int index = 0; index < 4; ++index)
    {
      this.tmpData = (BaseData) ((Component) this.ikTargetPairs[index].effector.target).GetComponent<BaseData>();
      Vector3[] vector3Array1 = new Vector3[2]
      {
        this.tmpData.pos,
        null
      };
      ref Vector3 local1 = ref vector3Array1[1];
      Quaternion rot1 = this.tmpData.rot;
      Vector3 eulerAngles1 = ((Quaternion) ref rot1).get_eulerAngles();
      local1 = eulerAngles1;
      Vector3[] vector3Array2 = vector3Array1;
      Vector3[] correctShapeValues1 = this.GetCorrectShapeValues(this.partners[0].info, this.nowKeyFrame, this.calcBlend[index]);
      this.tmpData.pos = correctShapeValues1[0];
      this.tmpData.rot = Quaternion.Euler(correctShapeValues1[1]);
      this.tmpData = (BaseData) ((Component) this.ikTargetPairs[index].bend.bendGoal).GetComponent<BaseData>();
      Vector3[] vector3Array3 = new Vector3[2]
      {
        this.tmpData.pos,
        null
      };
      ref Vector3 local2 = ref vector3Array3[1];
      Quaternion rot2 = this.tmpData.rot;
      Vector3 eulerAngles2 = ((Quaternion) ref rot2).get_eulerAngles();
      local2 = eulerAngles2;
      vector3Array2 = vector3Array3;
      Vector3[] correctShapeValues2 = this.GetCorrectShapeValues(this.partners[0].info, this.nowKeyFrame, this.calcBlendBend[index]);
      this.tmpData.pos = correctShapeValues2[0];
      this.tmpData.rot = Quaternion.Euler(correctShapeValues2[1]);
    }
  }

  public void CalcBlendSet(
    ref Dictionary<int, Dictionary<int, List<MotionIKData.BlendWeightInfo>>> calcBlend,
    params List<MotionIKData.BlendWeightInfo>[] binfo)
  {
    calcBlend[0] = new Dictionary<int, List<MotionIKData.BlendWeightInfo>>();
    calcBlend[1] = new Dictionary<int, List<MotionIKData.BlendWeightInfo>>();
    for (int index = 0; index < binfo.Length; ++index)
    {
      foreach (MotionIKData.BlendWeightInfo blendWeightInfo in binfo[index])
      {
        if (!calcBlend[index].ContainsKey(blendWeightInfo.shape.shapeNo))
          calcBlend[index].Add(blendWeightInfo.shape.shapeNo, new List<MotionIKData.BlendWeightInfo>());
        calcBlend[index][blendWeightInfo.shape.shapeNo].Add(blendWeightInfo);
        List<MotionIKData.BlendWeightInfo> blendWeightInfoList = calcBlend[index][blendWeightInfo.shape.shapeNo];
        // ISSUE: reference to a compiler-generated field
        if (MotionIK.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MotionIK.\u003C\u003Ef__mg\u0024cache0 = new Comparison<MotionIKData.BlendWeightInfo>(MotionIK.Compare);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<MotionIKData.BlendWeightInfo> fMgCache0 = MotionIK.\u003C\u003Ef__mg\u0024cache0;
        blendWeightInfoList.Sort(fMgCache0);
      }
    }
  }

  public void CalcBlendBendSet(
    ref Dictionary<int, List<MotionIKData.BlendWeightInfo>> calcBlendBend,
    params List<MotionIKData.BlendWeightInfo>[] binfo)
  {
    calcBlendBend = new Dictionary<int, List<MotionIKData.BlendWeightInfo>>();
    for (int index = 0; index < binfo.Length; ++index)
    {
      foreach (MotionIKData.BlendWeightInfo blendWeightInfo in binfo[index])
      {
        if (!calcBlendBend.ContainsKey(blendWeightInfo.shape.shapeNo))
          calcBlendBend.Add(blendWeightInfo.shape.shapeNo, new List<MotionIKData.BlendWeightInfo>());
        calcBlendBend[blendWeightInfo.shape.shapeNo].Add(blendWeightInfo);
        List<MotionIKData.BlendWeightInfo> blendWeightInfoList = calcBlendBend[blendWeightInfo.shape.shapeNo];
        // ISSUE: reference to a compiler-generated field
        if (MotionIK.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MotionIK.\u003C\u003Ef__mg\u0024cache1 = new Comparison<MotionIKData.BlendWeightInfo>(MotionIK.Compare);
        }
        // ISSUE: reference to a compiler-generated field
        Comparison<MotionIKData.BlendWeightInfo> fMgCache1 = MotionIK.\u003C\u003Ef__mg\u0024cache1;
        blendWeightInfoList.Sort(fMgCache1);
      }
    }
  }

  private static int Compare(MotionIKData.BlendWeightInfo a, MotionIKData.BlendWeightInfo b)
  {
    if ((double) a.StartKey - (double) b.StartKey > 0.0)
      return 1;
    return (double) a.StartKey - (double) b.StartKey < 0.0 ? -1 : 0;
  }

  public class IKTargetPair
  {
    private static int IKTargetNum = 4;
    private static MotionIK.IKTargetPair[] ret = new MotionIK.IKTargetPair[MotionIK.IKTargetPair.IKTargetNum];

    public IKTargetPair(MotionIK.IKTargetPair.IKTarget target, IKSolverFullBodyBiped solver)
    {
      switch (target)
      {
        case MotionIK.IKTargetPair.IKTarget.LeftHand:
          this.effector = solver.get_leftHandEffector();
          this.bend = (IKConstraintBend) solver.get_leftArmChain().bendConstraint;
          break;
        case MotionIK.IKTargetPair.IKTarget.RightHand:
          this.effector = solver.get_rightHandEffector();
          this.bend = (IKConstraintBend) solver.get_rightArmChain().bendConstraint;
          break;
        case MotionIK.IKTargetPair.IKTarget.LeftFoot:
          this.effector = solver.get_leftFootEffector();
          this.bend = (IKConstraintBend) solver.get_leftLegChain().bendConstraint;
          break;
        case MotionIK.IKTargetPair.IKTarget.RightFoot:
          this.effector = solver.get_rightFootEffector();
          this.bend = (IKConstraintBend) solver.get_rightLegChain().bendConstraint;
          break;
      }
    }

    public IKEffector effector { get; private set; }

    public IKConstraintBend bend { get; private set; }

    public static MotionIK.IKTargetPair[] GetPairs(IKSolverFullBodyBiped solver)
    {
      for (int index1 = 0; index1 < MotionIK.IKTargetPair.ret.Length; ++index1)
      {
        int index2 = index1;
        MotionIK.IKTargetPair.ret[index2] = new MotionIK.IKTargetPair((MotionIK.IKTargetPair.IKTarget) index2, solver);
      }
      return MotionIK.IKTargetPair.ret;
    }

    public static int IKTargetLength
    {
      get
      {
        return MotionIK.IKTargetPair.IKTargetNum * 2;
      }
    }

    public enum IKTarget
    {
      LeftHand,
      RightHand,
      LeftFoot,
      RightFoot,
      TotalNum,
    }
  }
}
