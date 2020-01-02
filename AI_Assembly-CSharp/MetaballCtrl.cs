// Decompiled with JetBrains decompiler
// Type: MetaballCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using IllusionUtility.GetUtility;
using Manager;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MetaballCtrl
{
  private List<ExcelData.Param> infolist = new List<ExcelData.Param>();
  private MetaballCtrl.ShootInfo info = new MetaballCtrl.ShootInfo();
  public MetaballShoot[] ctrlMetaball = new MetaballShoot[7];
  private MetaballCtrl.ShootCtrl[] actrlShoot = new MetaballCtrl.ShootCtrl[7];
  private GameObject[] aobjBody = new GameObject[4];
  private MetaballCtrl.MetaballParameter[] aParam = new MetaballCtrl.MetaballParameter[7];
  private const int cMetaBallNum = 7;
  private UpdateMeta[] ctrlUpdate;
  private HParticleCtrl particle;

  public MetaballCtrl(
    GameObject _objBase,
    GameObject _objMale,
    GameObject _objFemale,
    ChaControl _customFemale)
  {
    if (Object.op_Implicit((Object) _objBase))
    {
      this.ctrlUpdate = (UpdateMeta[]) _objBase.GetComponentsInChildren<UpdateMeta>();
      foreach (UpdateMeta updateMeta in this.ctrlUpdate)
      {
        foreach (MetaballShoot metaballShoot in updateMeta.lstShoot)
        {
          if (metaballShoot.comment.Contains("中出し1"))
            this.ctrlMetaball[3] = metaballShoot;
          else if (metaballShoot.comment.Contains("外出し1"))
          {
            this.ctrlMetaball[4] = metaballShoot;
            this.ctrlMetaball[4].chaCustom = _customFemale;
          }
          else if (metaballShoot.comment.Contains("外出し2"))
          {
            this.ctrlMetaball[5] = metaballShoot;
            this.ctrlMetaball[5].chaCustom = _customFemale;
          }
          else if (metaballShoot.comment.Contains("吐く2"))
          {
            this.ctrlMetaball[6] = metaballShoot;
            this.ctrlMetaball[6].chaCustom = _customFemale;
          }
          else if (metaballShoot.comment.Contains("中出し"))
          {
            this.ctrlMetaball[0] = metaballShoot;
            this.ctrlMetaball[0].chaCustom = _customFemale;
          }
          else if (metaballShoot.comment.Contains("外出し"))
          {
            this.ctrlMetaball[1] = metaballShoot;
            this.ctrlMetaball[1].chaCustom = _customFemale;
          }
          else if (metaballShoot.comment.Contains("吐く"))
          {
            this.ctrlMetaball[2] = metaballShoot;
            this.ctrlMetaball[2].chaCustom = _customFemale;
          }
        }
      }
    }
    this.aobjBody[0] = _objMale;
    this.aobjBody[1] = _objFemale;
    for (int index = 0; index < 7; ++index)
    {
      this.actrlShoot[index] = new MetaballCtrl.ShootCtrl();
      this.aParam[index] = new MetaballCtrl.MetaballParameter();
    }
  }

  public void SetParticle(HParticleCtrl _particle)
  {
    this.particle = _particle;
  }

  public bool Load(
    string _strAssetPath,
    string _nameFile,
    GameObject _objMale,
    GameObject _objMale1,
    GameObject _objFemale1 = null,
    ChaControl _customFemale1 = null)
  {
    this.aobjBody[0] = _objMale;
    this.aobjBody[2] = _objMale1;
    if (Object.op_Inequality((Object) _objFemale1, (Object) null))
    {
      this.aobjBody[3] = _objFemale1;
      if (this.ctrlMetaball[4] != null)
        this.ctrlMetaball[4].chaCustom = _customFemale1;
    }
    foreach (MetaballCtrl.ShootCtrl shootCtrl in this.actrlShoot)
      shootCtrl.lstShoot.Clear();
    string empty1 = string.Empty;
    string _strFileName = _nameFile.ContainsAny("f2", "f1") ? (_nameFile.Contains("ai3p") ? _nameFile.Remove(4, 3) : _nameFile.Remove(3, 3)) : _nameFile.Remove(3, 2);
    this.infolist = GlobalMethod.LoadExcelDataAlFindlFile(_strAssetPath, _strFileName, 1, 1, 3, 1, (List<string>) null, true);
    if (this.infolist == null)
    {
      GlobalMethod.DebugLog("excel : [" + _strFileName + "]は読み込めなかった　警告無しでここに来たら[" + _strAssetPath + "]自体がない", 1);
      return false;
    }
    int count = this.infolist.Count;
    if (count < 3)
    {
      GlobalMethod.DebugLog("excel : [" + _strFileName + "]は必ず3行必要", 1);
      return false;
    }
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    StringBuilder stringBuilder3 = new StringBuilder();
    string empty2 = string.Empty;
    for (int index1 = 0; index1 < count; ++index1)
    {
      int num1 = 0;
      if (this.ctrlMetaball[index1] != null)
      {
        MetaballShoot metaballShoot = this.ctrlMetaball[index1];
        List<string> list1 = this.infolist[index1].list;
        int index2 = num1;
        int num2 = index2 + 1;
        int num3 = !(list1[index2] == "1") ? 0 : 1;
        metaballShoot.isEnable = num3 != 0;
        if (this.ctrlMetaball[index1].isEnable)
        {
          List<string> list2 = this.infolist[index1].list;
          int index3 = num2;
          int num4 = index3 + 1;
          int intTryParse1 = GlobalMethod.GetIntTryParse(list2[index3], 0);
          this.ctrlMetaball[index1].ShootAxis = !Object.op_Implicit((Object) this.aobjBody[intTryParse1]) ? (GameObject) null : this.aobjBody[intTryParse1].get_transform().FindLoop(this.infolist[index1].list[num4++]);
          if (Object.op_Equality((Object) this.aobjBody[intTryParse1], (Object) null))
            ++num4;
          List<string> list3 = this.infolist[index1].list;
          int index4 = num4;
          int num5 = index4 + 1;
          int intTryParse2 = GlobalMethod.GetIntTryParse(list3[index4], 0);
          GameObject gameObject1 = !Object.op_Implicit((Object) this.aobjBody[intTryParse2]) ? (GameObject) null : this.aobjBody[intTryParse2].get_transform().FindLoop(this.infolist[index1].list[num5++]);
          this.ctrlMetaball[index1].parentTransform = !Object.op_Implicit((Object) gameObject1) ? (Transform) null : gameObject1.get_transform();
          this.aParam[index1].param[0].objParent = gameObject1;
          if (Object.op_Equality((Object) this.aobjBody[intTryParse2], (Object) null))
            ++num5;
          List<string> list4 = this.infolist[index1].list;
          int index5 = num5;
          int num6 = index5 + 1;
          int intTryParse3 = GlobalMethod.GetIntTryParse(list4[index5], 0);
          GameObject gameObject2 = !Object.op_Implicit((Object) this.aobjBody[intTryParse3]) ? (GameObject) null : this.aobjBody[intTryParse3].get_transform().FindLoop(this.infolist[index1].list[num6++]);
          this.aParam[index1].param[1].objParent = gameObject2;
          if (Object.op_Equality((Object) this.aobjBody[intTryParse3], (Object) null))
            ++num6;
          stringBuilder1.Clear();
          StringBuilder stringBuilder4 = stringBuilder1;
          List<string> list5 = this.infolist[index1].list;
          int index6 = num6;
          int num7 = index6 + 1;
          string str1 = list5[index6];
          stringBuilder4.Append(str1);
          stringBuilder2.Clear();
          StringBuilder stringBuilder5 = stringBuilder2;
          List<string> list6 = this.infolist[index1].list;
          int index7 = num7;
          int num8 = index7 + 1;
          string str2 = list6[index7];
          stringBuilder5.Append(str2);
          stringBuilder3.Clear();
          StringBuilder stringBuilder6 = stringBuilder3;
          List<string> list7 = this.infolist[index1].list;
          int index8 = num8;
          int num9 = index8 + 1;
          string str3 = list7[index8];
          stringBuilder6.Append(str3);
          this.aParam[index1].param[0].objShoot = this.LoadSiruObject(stringBuilder1.ToString(), stringBuilder2.ToString(), stringBuilder3.ToString());
          this.ctrlMetaball[index1].ShootObj = this.aParam[index1].param[0].objShoot;
          AssetBundleManager.UnloadAssetBundle(stringBuilder1.ToString(), false, stringBuilder3.ToString(), false);
          stringBuilder1.Clear();
          StringBuilder stringBuilder7 = stringBuilder1;
          List<string> list8 = this.infolist[index1].list;
          int index9 = num9;
          int num10 = index9 + 1;
          string str4 = list8[index9];
          stringBuilder7.Append(str4);
          stringBuilder2.Clear();
          StringBuilder stringBuilder8 = stringBuilder2;
          List<string> list9 = this.infolist[index1].list;
          int index10 = num10;
          int num11 = index10 + 1;
          string str5 = list9[index10];
          stringBuilder8.Append(str5);
          stringBuilder3.Clear();
          StringBuilder stringBuilder9 = stringBuilder3;
          List<string> list10 = this.infolist[index1].list;
          int index11 = num11;
          int num12 = index11 + 1;
          string str6 = list10[index11];
          stringBuilder9.Append(str6);
          this.aParam[index1].param[1].objShoot = this.LoadSiruObject(stringBuilder1.ToString(), stringBuilder2.ToString(), stringBuilder3.ToString());
          AssetBundleManager.UnloadAssetBundle(stringBuilder1.ToString(), false, stringBuilder3.ToString(), false);
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo1 = this.aParam[index1].param[0];
          List<string> list11 = this.infolist[index1].list;
          int index12 = num12;
          int num13 = index12 + 1;
          double num14 = (double) float.Parse(list11[index12]);
          List<string> list12 = this.infolist[index1].list;
          int index13 = num13;
          int num15 = index13 + 1;
          double num16 = (double) float.Parse(list12[index13]);
          Vector2 vector2_1 = new Vector2((float) num14, (float) num16);
          metaballParameterInfo1.speedS = vector2_1;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo2 = this.aParam[index1].param[0];
          List<string> list13 = this.infolist[index1].list;
          int index14 = num15;
          int num17 = index14 + 1;
          double num18 = (double) float.Parse(list13[index14]);
          List<string> list14 = this.infolist[index1].list;
          int index15 = num17;
          int num19 = index15 + 1;
          double num20 = (double) float.Parse(list14[index15]);
          Vector2 vector2_2 = new Vector2((float) num18, (float) num20);
          metaballParameterInfo2.rotationS = vector2_2;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo3 = this.aParam[index1].param[0];
          List<string> list15 = this.infolist[index1].list;
          int index16 = num19;
          int num21 = index16 + 1;
          double num22 = (double) float.Parse(list15[index16]);
          List<string> list16 = this.infolist[index1].list;
          int index17 = num21;
          int num23 = index17 + 1;
          double num24 = (double) float.Parse(list16[index17]);
          Vector2 vector2_3 = new Vector2((float) num22, (float) num24);
          metaballParameterInfo3.speedM = vector2_3;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo4 = this.aParam[index1].param[0];
          List<string> list17 = this.infolist[index1].list;
          int index18 = num23;
          int num25 = index18 + 1;
          double num26 = (double) float.Parse(list17[index18]);
          List<string> list18 = this.infolist[index1].list;
          int index19 = num25;
          int num27 = index19 + 1;
          double num28 = (double) float.Parse(list18[index19]);
          Vector2 vector2_4 = new Vector2((float) num26, (float) num28);
          metaballParameterInfo4.rotationM = vector2_4;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo5 = this.aParam[index1].param[0];
          List<string> list19 = this.infolist[index1].list;
          int index20 = num27;
          int num29 = index20 + 1;
          double num30 = (double) float.Parse(list19[index20]);
          List<string> list20 = this.infolist[index1].list;
          int index21 = num29;
          int num31 = index21 + 1;
          double num32 = (double) float.Parse(list20[index21]);
          Vector2 vector2_5 = new Vector2((float) num30, (float) num32);
          metaballParameterInfo5.speedL = vector2_5;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo6 = this.aParam[index1].param[0];
          List<string> list21 = this.infolist[index1].list;
          int index22 = num31;
          int num33 = index22 + 1;
          double num34 = (double) float.Parse(list21[index22]);
          List<string> list22 = this.infolist[index1].list;
          int index23 = num33;
          int num35 = index23 + 1;
          double num36 = (double) float.Parse(list22[index23]);
          Vector2 vector2_6 = new Vector2((float) num34, (float) num36);
          metaballParameterInfo6.rotationL = vector2_6;
          this.ctrlMetaball[index1].speedSMin = (float) this.aParam[index1].param[0].speedS.x;
          this.ctrlMetaball[index1].speedSMax = (float) this.aParam[index1].param[0].speedS.y;
          this.ctrlMetaball[index1].randomRotationS = this.aParam[index1].param[0].rotationS;
          this.ctrlMetaball[index1].speedMMin = (float) this.aParam[index1].param[0].speedM.x;
          this.ctrlMetaball[index1].speedMMax = (float) this.aParam[index1].param[0].speedM.y;
          this.ctrlMetaball[index1].randomRotationM = this.aParam[index1].param[0].rotationM;
          this.ctrlMetaball[index1].speedLMin = (float) this.aParam[index1].param[0].speedL.x;
          this.ctrlMetaball[index1].speedLMax = (float) this.aParam[index1].param[0].speedL.y;
          this.ctrlMetaball[index1].randomRotationL = this.aParam[index1].param[0].rotationL;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo7 = this.aParam[index1].param[1];
          List<string> list23 = this.infolist[index1].list;
          int index24 = num35;
          int num37 = index24 + 1;
          double num38 = (double) float.Parse(list23[index24]);
          List<string> list24 = this.infolist[index1].list;
          int index25 = num37;
          int num39 = index25 + 1;
          double num40 = (double) float.Parse(list24[index25]);
          Vector2 vector2_7 = new Vector2((float) num38, (float) num40);
          metaballParameterInfo7.speedS = vector2_7;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo8 = this.aParam[index1].param[1];
          List<string> list25 = this.infolist[index1].list;
          int index26 = num39;
          int num41 = index26 + 1;
          double num42 = (double) float.Parse(list25[index26]);
          List<string> list26 = this.infolist[index1].list;
          int index27 = num41;
          int num43 = index27 + 1;
          double num44 = (double) float.Parse(list26[index27]);
          Vector2 vector2_8 = new Vector2((float) num42, (float) num44);
          metaballParameterInfo8.rotationS = vector2_8;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo9 = this.aParam[index1].param[1];
          List<string> list27 = this.infolist[index1].list;
          int index28 = num43;
          int num45 = index28 + 1;
          double num46 = (double) float.Parse(list27[index28]);
          List<string> list28 = this.infolist[index1].list;
          int index29 = num45;
          int num47 = index29 + 1;
          double num48 = (double) float.Parse(list28[index29]);
          Vector2 vector2_9 = new Vector2((float) num46, (float) num48);
          metaballParameterInfo9.speedM = vector2_9;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo10 = this.aParam[index1].param[1];
          List<string> list29 = this.infolist[index1].list;
          int index30 = num47;
          int num49 = index30 + 1;
          double num50 = (double) float.Parse(list29[index30]);
          List<string> list30 = this.infolist[index1].list;
          int index31 = num49;
          int num51 = index31 + 1;
          double num52 = (double) float.Parse(list30[index31]);
          Vector2 vector2_10 = new Vector2((float) num50, (float) num52);
          metaballParameterInfo10.rotationM = vector2_10;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo11 = this.aParam[index1].param[1];
          List<string> list31 = this.infolist[index1].list;
          int index32 = num51;
          int num53 = index32 + 1;
          double num54 = (double) float.Parse(list31[index32]);
          List<string> list32 = this.infolist[index1].list;
          int index33 = num53;
          int num55 = index33 + 1;
          double num56 = (double) float.Parse(list32[index33]);
          Vector2 vector2_11 = new Vector2((float) num54, (float) num56);
          metaballParameterInfo11.speedL = vector2_11;
          MetaballCtrl.MetaballParameterInfo metaballParameterInfo12 = this.aParam[index1].param[1];
          List<string> list33 = this.infolist[index1].list;
          int index34 = num55;
          int num57 = index34 + 1;
          double num58 = (double) float.Parse(list33[index34]);
          List<string> list34 = this.infolist[index1].list;
          int index35 = num57;
          int num59 = index35 + 1;
          double num60 = (double) float.Parse(list34[index35]);
          Vector2 vector2_12 = new Vector2((float) num58, (float) num60);
          metaballParameterInfo12.rotationL = vector2_12;
          while (this.infolist[index1].list.Count > num59)
          {
            List<string> list35 = this.infolist[index1].list;
            int index36 = num59;
            int num61 = index36 + 1;
            string str7 = list35[index36];
            if (!(str7 == string.Empty))
            {
              this.info = new MetaballCtrl.ShootInfo();
              this.info.isInside = str7 == "1";
              ref MetaballCtrl.ShootInfo local = ref this.info;
              List<string> list36 = this.infolist[index1].list;
              int index37 = num61;
              int num62 = index37 + 1;
              string str8 = list36[index37];
              local.nameAnimation = str8;
              List<string> list37 = this.infolist[index1].list;
              int index38 = num62;
              num59 = index38 + 1;
              float result;
              if (!float.TryParse(list37[index38], out result))
                result = 0.0f;
              this.info.timeShoot = result;
              this.info.timeOld = 0.0f;
              this.actrlShoot[index1].lstShoot.Add(this.info);
            }
            else
              break;
          }
        }
      }
    }
    return true;
  }

  public bool SetParameterFromState(int _state)
  {
    if (!GlobalMethod.RangeOn<int>(_state, 0, 1))
      return false;
    for (int index = 0; index < this.ctrlMetaball.Length; ++index)
    {
      if (this.ctrlMetaball[index] != null)
      {
        this.ctrlMetaball[index].speedSMin = (float) this.aParam[index].param[_state].speedS.x;
        this.ctrlMetaball[index].speedSMax = (float) this.aParam[index].param[_state].speedS.y;
        this.ctrlMetaball[index].randomRotationS = this.aParam[index].param[_state].rotationS;
        this.ctrlMetaball[index].speedMMin = (float) this.aParam[index].param[_state].speedM.x;
        this.ctrlMetaball[index].speedMMax = (float) this.aParam[index].param[_state].speedM.y;
        this.ctrlMetaball[index].randomRotationM = this.aParam[index].param[_state].rotationM;
        this.ctrlMetaball[index].speedLMin = (float) this.aParam[index].param[_state].speedL.x;
        this.ctrlMetaball[index].speedLMax = (float) this.aParam[index].param[_state].speedL.y;
        this.ctrlMetaball[index].randomRotationL = this.aParam[index].param[_state].rotationL;
        this.ctrlMetaball[index].ShootObj = this.aParam[index].param[_state].objShoot;
        this.ctrlMetaball[index].parentTransform = !Object.op_Implicit((Object) this.aParam[index].param[_state].objParent) ? (Transform) null : this.aParam[index].param[_state].objParent.get_transform();
      }
    }
    return true;
  }

  public bool Proc(AnimatorStateInfo _stateInfo, bool _isInside = false)
  {
    if (Singleton<HSceneFlagCtrl>.Instance.semenType == 0)
      return true;
    for (int index = 0; index < this.actrlShoot.Length; ++index)
    {
      if ((Singleton<HSceneFlagCtrl>.Instance.semenType != 2 || index == 1 || (index == 4 || index == 5)) && this.actrlShoot[index].IsAnimation(_isInside, _stateInfo, this.info))
      {
        if (Singleton<HSceneFlagCtrl>.Instance.semenType == 2)
        {
          if (this.particle != null)
          {
            if (index == 1)
            {
              this.particle.Play(3);
              this.particle.Play(8);
            }
            if (index == 4)
            {
              this.particle.Play(7);
              this.particle.Play(9);
            }
            if (index == 5)
              this.particle.Play(10);
          }
        }
        else if (this.ctrlMetaball[index] != null)
          this.ctrlMetaball[index].ShootMetaBallStart();
      }
    }
    return true;
  }

  public bool Clear()
  {
    foreach (UpdateMeta updateMeta in this.ctrlUpdate)
      updateMeta.MetaBallClear();
    return true;
  }

  private GameObject LoadSiruObject(
    string _pathAsset,
    string _nameFile,
    string _manifest)
  {
    if (_nameFile == string.Empty)
      return (GameObject) null;
    GameObject gameObject = CommonLib.LoadAsset<GameObject>(_pathAsset, _nameFile, false, _manifest);
    Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(_pathAsset);
    return gameObject;
  }

  public struct ShootInfo
  {
    public bool isInside;
    public string nameAnimation;
    public float timeShoot;
    public float timeOld;
  }

  public class ShootCtrl
  {
    public List<MetaballCtrl.ShootInfo> lstShoot = new List<MetaballCtrl.ShootInfo>();

    public bool IsAnimation(
      bool _isInside,
      AnimatorStateInfo _stateInfo,
      MetaballCtrl.ShootInfo info)
    {
      for (int index = 0; index < this.lstShoot.Count; ++index)
      {
        info = this.lstShoot[index];
        if (!((AnimatorStateInfo) ref _stateInfo).IsName(info.nameAnimation))
        {
          info.timeOld = 0.0f;
          this.lstShoot[index] = info;
        }
        else if (!info.isInside || _isInside)
        {
          if ((double) info.timeOld > (double) info.timeShoot || (double) info.timeShoot >= (double) ((AnimatorStateInfo) ref _stateInfo).get_normalizedTime())
          {
            info.timeOld = ((AnimatorStateInfo) ref _stateInfo).get_normalizedTime();
            this.lstShoot[index] = info;
          }
          else
          {
            info.timeOld = ((AnimatorStateInfo) ref _stateInfo).get_normalizedTime();
            this.lstShoot[index] = info;
            return true;
          }
        }
      }
      return false;
    }
  }

  public class MetaballParameterInfo
  {
    public Vector2 speedS;
    public Vector2 speedM;
    public Vector2 speedL;
    public Vector2 rotationS;
    public Vector2 rotationM;
    public Vector2 rotationL;
    public GameObject objShoot;
    public GameObject objParent;
  }

  public class MetaballParameter
  {
    public MetaballCtrl.MetaballParameterInfo[] param = new MetaballCtrl.MetaballParameterInfo[2];

    public MetaballParameter()
    {
      this.param[0] = new MetaballCtrl.MetaballParameterInfo();
      this.param[1] = new MetaballCtrl.MetaballParameterInfo();
    }
  }
}
