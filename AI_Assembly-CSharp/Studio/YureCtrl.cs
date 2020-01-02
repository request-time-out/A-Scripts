// Decompiled with JetBrains decompiler
// Type: Studio.YureCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  [DefaultExecutionOrder(-1)]
  public class YureCtrl : MonoBehaviour
  {
    public List<YureCtrl.Info> lstInfo;
    [Tooltip("動いているかの確認用")]
    public bool[] aIsActive;
    [Tooltip("動いているかの確認用")]
    public YureCtrl.BreastShapeInfo[] aBreastShape;
    private bool[] aYureEnableActive;
    private YureCtrl.BreastShapeInfo[] aBreastShapeEnable;
    private ChaControl _chaControl;

    public YureCtrl()
    {
      base.\u002Ector();
    }

    public int FemaleID { get; private set; }

    public bool IsInit { get; private set; }

    private OCIChar OCIChar { get; set; }

    private ChaControl ChaControl
    {
      get
      {
        return this._chaControl ?? (this._chaControl = this.OCIChar?.charInfo);
      }
    }

    private void LateUpdate()
    {
      if (!this.IsInit || !Object.op_Inequality((Object) this.ChaControl, (Object) null))
        return;
      this.Proc(this.ChaControl.getAnimatorStateInfo(0));
    }

    public void Init(OCIChar _ocic)
    {
      this.OCIChar = _ocic;
      for (int index = 0; index < 2; ++index)
      {
        this.aBreastShape[index].MemberInit();
        this.aBreastShapeEnable[index].MemberInit();
      }
    }

    public bool Load(string _bundle, string _file, int _motionId, int _femaleID)
    {
      this.IsInit = false;
      this.ResetShape(true);
      if (!GlobalMethod.AssetFileExist(_bundle, _file, string.Empty))
        return false;
      this.FemaleID = _femaleID;
      this.lstInfo.Clear();
      ExcelData excelData = CommonLib.LoadAsset<ExcelData>(_bundle, _file, false, string.Empty);
      bool flag = ((IEnumerable<string>) new string[2]
      {
        "ail",
        "ai3p"
      }).Contains<string>(_file.Split('_')[0]);
      int[] numArray = new int[7]{ 2, 3, 4, 5, 6, 7, 8 };
      int work = 0;
      foreach (ExcelData.Param obj in excelData.list.Where<ExcelData.Param>((Func<ExcelData.Param, bool>) (v => int.TryParse(v.list.SafeGet<string>(0), out work) && work == _motionId)))
      {
        int num1 = 1;
        YureCtrl.Info info1 = new YureCtrl.Info();
        List<string> list = obj.list;
        int result = 0;
        if (flag && !int.TryParse(list.GetElement<string>(num1++), out result))
          result = 0;
        info1.nFemale = result;
        YureCtrl.Info info2 = info1;
        List<string> source1 = list;
        int index1 = num1;
        int num2 = index1 + 1;
        string element = source1.GetElement<string>(index1);
        info2.nameAnimation = element;
        bool[] aIsActive1 = info1.aIsActive;
        List<string> source2 = list;
        int index2 = num2;
        int num3 = index2 + 1;
        int num4 = source2.GetElement<string>(index2) == "1" ? 1 : 0;
        aIsActive1[0] = num4 != 0;
        info1.aBreastShape[0].MemberInit();
        for (int index3 = 0; index3 < numArray.Length; ++index3)
          info1.aBreastShape[0].breast[index3] = list.GetElement<string>(num3++) == "1";
        ref YureCtrl.BreastShapeInfo local1 = ref info1.aBreastShape[0];
        List<string> source3 = list;
        int index4 = num3;
        int num5 = index4 + 1;
        int num6 = source3.GetElement<string>(index4) == "1" ? 1 : 0;
        local1.nip = num6 != 0;
        bool[] aIsActive2 = info1.aIsActive;
        List<string> source4 = list;
        int index5 = num5;
        int num7 = index5 + 1;
        int num8 = source4.GetElement<string>(index5) == "1" ? 1 : 0;
        aIsActive2[1] = num8 != 0;
        info1.aBreastShape[1].MemberInit();
        for (int index3 = 0; index3 < numArray.Length; ++index3)
          info1.aBreastShape[1].breast[index3] = list.GetElement<string>(num7++) == "1";
        ref YureCtrl.BreastShapeInfo local2 = ref info1.aBreastShape[1];
        List<string> source5 = list;
        int index6 = num7;
        int num9 = index6 + 1;
        int num10 = source5.GetElement<string>(index6) == "1" ? 1 : 0;
        local2.nip = num10 != 0;
        bool[] aIsActive3 = info1.aIsActive;
        List<string> source6 = list;
        int index7 = num9;
        int num11 = index7 + 1;
        int num12 = source6.GetElement<string>(index7) == "1" ? 1 : 0;
        aIsActive3[2] = num12 != 0;
        bool[] aIsActive4 = info1.aIsActive;
        List<string> source7 = list;
        int index8 = num11;
        int num13 = index8 + 1;
        int num14 = source7.GetElement<string>(index8) == "1" ? 1 : 0;
        aIsActive4[3] = num14 != 0;
        this.lstInfo.Add(info1);
      }
      this.IsInit = true;
      return true;
    }

    public bool Proc(AnimatorStateInfo _ai)
    {
      if (!this.IsInit)
        return false;
      YureCtrl.Info info = (YureCtrl.Info) null;
      if (this.lstInfo != null)
      {
        for (int index = 0; index < this.lstInfo.Count; ++index)
        {
          if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[index].nameAnimation) && this.lstInfo[index].nFemale == this.FemaleID)
          {
            info = this.lstInfo[index];
            break;
          }
        }
      }
      if (info != null)
      {
        this.Active(info.aIsActive);
        this.Shape(info.aBreastShape);
        return true;
      }
      this.Active(this.aYureEnableActive);
      this.Shape(this.aBreastShapeEnable);
      return false;
    }

    private void Active(bool[] _aIsActive)
    {
      for (int _kind = 0; _kind < this.aIsActive.Length; ++_kind)
      {
        if (this.aIsActive[_kind] != _aIsActive[_kind])
        {
          switch (_kind)
          {
            case 0:
              this.OCIChar.DynamicAnimeBustL = _aIsActive[_kind];
              break;
            case 1:
              this.OCIChar.DynamicAnimeBustR = _aIsActive[_kind];
              break;
            default:
              this.OCIChar.EnableDynamicBonesBustAndHip(_aIsActive[_kind], _kind);
              break;
          }
          this.aIsActive[_kind] = _aIsActive[_kind];
        }
      }
    }

    private void Shape(YureCtrl.BreastShapeInfo[] _shapeInfo)
    {
      for (int index1 = 0; index1 < 2; ++index1)
      {
        int LR = index1;
        YureCtrl.BreastShapeInfo breastShapeInfo1 = _shapeInfo[index1];
        YureCtrl.BreastShapeInfo breastShapeInfo2 = this.aBreastShape[index1];
        if (breastShapeInfo1.breast != breastShapeInfo2.breast)
        {
          for (int index2 = 0; index2 < ChaFileDefine.cf_BustShapeMaskID.Length - 1; ++index2)
          {
            int id = index2;
            if (breastShapeInfo1.breast[id] != breastShapeInfo2.breast[id])
            {
              if (breastShapeInfo1.breast[id])
                this.ChaControl.DisableShapeBodyID(LR, id, false);
              else
                this.ChaControl.DisableShapeBodyID(LR, id, true);
            }
          }
          breastShapeInfo2.breast = breastShapeInfo1.breast;
        }
        if (breastShapeInfo1.nip != breastShapeInfo2.nip)
        {
          if (breastShapeInfo1.nip)
            this.ChaControl.DisableShapeBodyID(LR, 7, false);
          else
            this.ChaControl.DisableShapeBodyID(LR, 7, true);
          breastShapeInfo2.nip = breastShapeInfo1.nip;
        }
        this.aBreastShape[index1] = breastShapeInfo2;
      }
    }

    public void ResetShape(bool _dynamicBone = true)
    {
      if (Object.op_Equality((Object) this.ChaControl, (Object) null))
        return;
      for (int id = 0; id < ChaFileDefine.cf_BustShapeMaskID.Length; ++id)
        this.ChaControl.DisableShapeBodyID(2, id, false);
      for (int index = 0; index < 2; ++index)
        this.aBreastShape[index].MemberInit();
      if (_dynamicBone)
      {
        for (int index = 0; index < this.aIsActive.Length; ++index)
          this.aIsActive[index] = true;
        this.OCIChar.DynamicAnimeBustL = true;
        this.OCIChar.DynamicAnimeBustR = true;
        this.OCIChar.EnableDynamicBonesBustAndHip(true, 2);
        this.OCIChar.EnableDynamicBonesBustAndHip(true, 3);
      }
      this.IsInit = false;
      if (this.lstInfo == null)
        return;
      this.lstInfo.Clear();
    }

    [Serializable]
    public struct BreastShapeInfo
    {
      public bool[] breast;
      public bool nip;

      public void MemberInit()
      {
        this.breast = new bool[7]
        {
          true,
          true,
          true,
          true,
          true,
          true,
          true
        };
        this.nip = true;
      }
    }

    [Serializable]
    public class Info
    {
      public string nameAnimation = string.Empty;
      public bool[] aIsActive = new bool[4];
      public YureCtrl.BreastShapeInfo[] aBreastShape = new YureCtrl.BreastShapeInfo[2];
      public int nFemale;
    }
  }
}
