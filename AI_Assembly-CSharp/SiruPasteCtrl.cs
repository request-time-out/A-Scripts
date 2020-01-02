// Decompiled with JetBrains decompiler
// Type: SiruPasteCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SiruPasteCtrl : MonoBehaviour
{
  private ExcelData excelData;
  private List<string> row;
  [SerializeField]
  private List<SiruPasteCtrl.PasteInfo> lstPaste;
  [DisabledGroup("女クラス")]
  [SerializeField]
  private ChaControl chaFemale;
  private float oldFrame;
  private int oldHash;
  private SiruPasteCtrl.PasteInfo p;
  private ChaFileDefine.SiruParts sp;
  private SiruPasteCtrl.Timing ti;
  private StringBuilder abName;
  private StringBuilder asset;
  private string[] astr;

  public SiruPasteCtrl()
  {
    base.\u002Ector();
  }

  public bool Init(ChaControl _female)
  {
    this.abName = new StringBuilder();
    this.asset = new StringBuilder();
    this.Release();
    this.chaFemale = _female;
    return true;
  }

  public void Release()
  {
    this.lstPaste.Clear();
  }

  public bool Load(string _assetpath, string _file, int _id)
  {
    this.lstPaste.Clear();
    if (_file == string.Empty)
      return false;
    List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_assetpath, false);
    nameListFromPath.Sort();
    this.asset.Clear();
    this.asset.Append(_file);
    this.excelData = (ExcelData) null;
    for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
    {
      this.abName.Clear();
      this.abName.Append(nameListFromPath[index1]);
      if (GlobalMethod.AssetFileExist(this.abName.ToString(), this.asset.ToString(), string.Empty))
      {
        this.excelData = CommonLib.LoadAsset<ExcelData>(this.abName.ToString(), this.asset.ToString(), false, string.Empty);
        Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(this.abName.ToString());
        if (!Object.op_Equality((Object) this.excelData, (Object) null))
        {
          int num1 = 1;
          while (num1 < this.excelData.MaxCell)
          {
            this.row = this.excelData.list[num1++].list;
            int num2 = 1;
            SiruPasteCtrl.PasteInfo pasteInfo1 = new SiruPasteCtrl.PasteInfo();
            SiruPasteCtrl.PasteInfo pasteInfo2 = pasteInfo1;
            List<string> row1 = this.row;
            int index2 = num2;
            int num3 = index2 + 1;
            string element = row1.GetElement<string>(index2);
            pasteInfo2.anim = element;
            pasteInfo1.timing = new SiruPasteCtrl.Timing();
            SiruPasteCtrl.Timing timing = pasteInfo1.timing;
            List<string> row2 = this.row;
            int index3 = num3;
            int num4 = index3 + 1;
            double num5 = (double) float.Parse(row2.GetElement<string>(index3));
            timing.normalizeTime = (float) num5;
            List<string> row3 = this.row;
            int index4 = num4;
            int num6 = index4 + 1;
            this.astr = row3.GetElement<string>(index4).Split(',');
            for (int index5 = 0; index5 < this.astr.Length; ++index5)
            {
              int result = 0;
              if (!int.TryParse(this.astr[index5], out result))
                result = 0;
              pasteInfo1.timing.lstWhere.Add(result);
            }
            this.lstPaste.Add(pasteInfo1);
          }
        }
      }
    }
    this.oldFrame = 0.0f;
    this.oldHash = 0;
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai)
  {
    if (this.oldHash != ((AnimatorStateInfo) ref _ai).get_shortNameHash())
      this.oldFrame = 0.0f;
    this.oldHash = ((AnimatorStateInfo) ref _ai).get_shortNameHash();
    for (int index1 = 0; index1 < this.lstPaste.Count; ++index1)
    {
      this.p = this.lstPaste[index1];
      if (((AnimatorStateInfo) ref _ai).IsName(this.p.anim))
      {
        float num = ((AnimatorStateInfo) ref _ai).get_normalizedTime() % 1f;
        this.ti = this.p.timing;
        if ((double) this.oldFrame <= (double) this.ti.normalizeTime && (double) this.ti.normalizeTime < (double) num)
        {
          for (int index2 = 0; index2 < this.ti.lstWhere.Count; ++index2)
          {
            this.sp = (ChaFileDefine.SiruParts) this.ti.lstWhere[index2];
            this.chaFemale.SetSiruFlag(this.sp, (byte) Mathf.Clamp((int) this.chaFemale.GetSiruFlag(this.sp) + 1, 0, 2));
          }
        }
      }
    }
    this.oldFrame = ((AnimatorStateInfo) ref _ai).get_normalizedTime();
    return true;
  }

  [Serializable]
  public class Timing
  {
    public List<int> lstWhere = new List<int>();
    [Label("タイミング")]
    public float normalizeTime;
  }

  [Serializable]
  public class PasteInfo
  {
    [Label("アニメーション名")]
    public string anim = string.Empty;
    public SiruPasteCtrl.Timing timing;
  }
}
