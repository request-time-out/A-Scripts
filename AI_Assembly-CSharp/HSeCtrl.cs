// Decompiled with JetBrains decompiler
// Type: HSeCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class HSeCtrl
{
  private List<HSeCtrl.Info> lstInfo = new List<HSeCtrl.Info>();
  private Dictionary<string, Dictionary<string, Transform>> dicLoop = new Dictionary<string, Dictionary<string, Transform>>();
  private string abName = string.Empty;
  private string assetName = string.Empty;
  private List<string> row = new List<string>();
  private bool[] dicloopContainKey = new bool[2];
  private HSceneFlagCtrl hFlag;
  private int oldnameHash;
  private float oldNormalizeTime;
  private ExcelData excelData;
  private HSeCtrl.KeyInfo infoKey;
  private HSeCtrl.Info info;
  private string nameAnim;
  private StringBuilder procNameSE;

  public HSeCtrl()
  {
    this.hFlag = (HSceneFlagCtrl) Singleton<Resources>.Instance.HSceneTable.HSceneSet.GetComponentInChildren<HSceneFlagCtrl>(true);
  }

  public bool Load(
    string _strAssetFolder,
    string _file,
    GameObject _objBoneMale,
    params GameObject[] _objBoneFemale)
  {
    this.lstInfo.Clear();
    this.dicLoop.Clear();
    this.oldnameHash = 0;
    this.oldNormalizeTime = 0.0f;
    this.excelData = (ExcelData) null;
    Singleton<Manager.Sound>.Instance.Stop(Manager.Sound.Type.GameSE3D);
    if (_file == string.Empty)
      return false;
    GameObject[] gameObjectArray;
    if (_objBoneFemale.Length > 1)
      gameObjectArray = new GameObject[3]
      {
        _objBoneMale,
        _objBoneFemale[0],
        _objBoneFemale[1]
      };
    else
      gameObjectArray = new GameObject[2]
      {
        _objBoneMale,
        _objBoneFemale[0]
      };
    List<string> nameListFromPath = CommonLib.GetAssetBundleNameListFromPath(_strAssetFolder, false);
    nameListFromPath.Sort();
    this.assetName = _file;
    for (int index1 = 0; index1 < nameListFromPath.Count; ++index1)
    {
      this.abName = nameListFromPath[index1];
      if (GlobalMethod.AssetFileExist(this.abName, this.assetName, string.Empty))
      {
        this.excelData = CommonLib.LoadAsset<ExcelData>(this.abName, this.assetName, false, string.Empty);
        Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(this.abName);
        if (!Object.op_Equality((Object) this.excelData, (Object) null))
        {
          int num1 = 1;
          while (num1 < this.excelData.MaxCell)
          {
            this.row = this.excelData.list[num1++].list;
            int num2 = 0;
            List<string> row1 = this.row;
            int index2 = num2;
            int num3 = index2 + 1;
            this.nameAnim = row1.GetElement<string>(index2);
            List<string> row2 = this.row;
            int index3 = num3;
            int num4 = index3 + 1;
            int index4 = int.Parse(row2.GetElement<string>(index3));
            ref HSeCtrl.KeyInfo local1 = ref this.infoKey;
            List<string> row3 = this.row;
            int index5 = num4;
            int num5 = index5 + 1;
            string element1 = row3.GetElement<string>(index5);
            local1.objParentName = element1;
            gameObjectArray[index4].SafeProc<GameObject>((Action<GameObject>) (o =>
            {
              if (!Object.op_Inequality((Object) o, (Object) null))
                return;
              this.infoKey.objParent = o.get_transform().FindLoop(this.infoKey.objParentName);
            }));
            ref HSeCtrl.KeyInfo local2 = ref this.infoKey;
            List<string> row4 = this.row;
            int index6 = num5;
            int num6 = index6 + 1;
            double num7 = (double) float.Parse(row4.GetElement<string>(index6));
            local2.key = (float) num7;
            ref HSeCtrl.KeyInfo local3 = ref this.infoKey;
            List<string> row5 = this.row;
            int index7 = num6;
            int num8 = index7 + 1;
            int num9 = row5.GetElement<string>(index7) == "1" ? 1 : 0;
            local3.isLoop = num9 != 0;
            ref HSeCtrl.KeyInfo local4 = ref this.infoKey;
            List<string> row6 = this.row;
            int index8 = num8;
            int num10 = index8 + 1;
            int num11 = !(row6.GetElement<string>(index8) == "1") ? 0 : 1;
            local4.loopSwitch = num11;
            ref HSeCtrl.KeyInfo local5 = ref this.infoKey;
            List<string> row7 = this.row;
            int index9 = num10;
            int num12 = index9 + 1;
            int num13 = row7.GetElement<string>(index9) == "1" ? 1 : 0;
            local5.bChangeVol = num13 != 0;
            ref HSeCtrl.KeyInfo local6 = ref this.infoKey;
            List<string> row8 = this.row;
            int index10 = num12;
            int num14 = index10 + 1;
            string element2 = row8.GetElement<string>(index10);
            local6.assetPath = element2;
            List<string> row9 = this.row;
            int index11 = num14;
            int num15 = index11 + 1;
            string element3 = row9.GetElement<string>(index11);
            if (element3 != string.Empty)
            {
              string[] strArray = element3.Split('/');
              this.infoKey.nameSE = new List<string>();
              for (int index12 = 0; index12 < strArray.Length; ++index12)
                this.infoKey.nameSE.Add(strArray[index12]);
            }
            ref HSeCtrl.KeyInfo local7 = ref this.infoKey;
            List<string> row10 = this.row;
            int index13 = num15;
            int num16 = index13 + 1;
            int num17 = row10.GetElement<string>(index13) == "1" ? 1 : 0;
            local7.isShorts = num17 != 0;
            List<string> row11 = this.row;
            int index14 = num16;
            int index15 = index14 + 1;
            string element4 = row11.GetElement<string>(index14);
            if (element4 != string.Empty)
            {
              string[] strArray = element4.Split('/');
              this.infoKey.nameShortsSE = new List<string>();
              for (int index12 = 0; index12 < strArray.Length; ++index12)
                this.infoKey.nameShortsSE.Add(strArray[index12]);
            }
            this.infoKey.nFemale = 0;
            if (this.row.Count > 11)
            {
              ref HSeCtrl.KeyInfo local8 = ref this.infoKey;
              int num18;
              if (this.row.GetElement<string>(index15) == string.Empty)
              {
                num18 = 0;
              }
              else
              {
                List<string> row12 = this.row;
                int index12 = index15;
                int num19 = index12 + 1;
                num18 = GlobalMethod.GetIntTryParse(row12.GetElement<string>(index12), 0);
              }
              local8.nFemale = num18;
            }
            this.info = this.lstInfo.Find((Predicate<HSeCtrl.Info>) (i => i.nameAnimation == this.nameAnim));
            if (this.info == null)
            {
              this.info = new HSeCtrl.Info();
              this.info.nameAnimation = this.nameAnim;
              this.info.key.Add(this.infoKey);
              this.lstInfo.Add(this.info);
            }
            else
              this.info.key.Add(this.infoKey);
          }
        }
      }
    }
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai, ChaControl[] _females)
  {
    if (_females == null)
      return false;
    if (this.oldnameHash != ((AnimatorStateInfo) ref _ai).get_shortNameHash())
      this.oldNormalizeTime = 0.0f;
    float _now = ((AnimatorStateInfo) ref _ai).get_normalizedTime() % 1f;
    this.procNameSE = StringBuilderPool.Get();
    string empty = string.Empty;
    string key = string.Empty;
    for (int index1 = 0; index1 < this.lstInfo.Count; ++index1)
    {
      if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[index1].nameAnimation))
      {
        List<HSeCtrl.KeyInfo> keyInfoList = this.lstInfo[index1].IsKey(this.oldNormalizeTime, _now);
        for (int index2 = 0; index2 < keyInfoList.Count; ++index2)
        {
          this.procNameSE.Clear();
          this.procNameSE.Append(this.GetSEName(keyInfoList[index2].nameSE));
          if (keyInfoList[index2].isLoop)
          {
            if (keyInfoList[index2].loopSwitch == 0)
            {
              bool flag = false;
              for (int index3 = 0; index3 < keyInfoList[index2].nameSE.Count; ++index3)
              {
                int index4 = index3;
                if (this.dicLoop.ContainsKey(keyInfoList[index2].nameSE[index4]))
                {
                  empty = keyInfoList[index2].nameSE[index4];
                  if (this.dicLoop[empty].ContainsKey(keyInfoList[index2].objParentName))
                  {
                    flag = true;
                    key = keyInfoList[index2].objParentName;
                    break;
                  }
                }
              }
              if (flag)
              {
                Singleton<Manager.Sound>.Instance.Stop(this.dicLoop[empty][key]);
                this.dicLoop[empty].Remove(key);
              }
            }
            else
            {
              this.dicloopContainKey[0] = this.dicLoop.ContainsKey(this.procNameSE.ToString());
              this.dicloopContainKey[1] = this.dicloopContainKey[0] && this.dicLoop[this.procNameSE.ToString()].ContainsKey(keyInfoList[index2].objParentName);
              if (!this.dicloopContainKey[0] || !this.dicloopContainKey[1])
              {
                Transform trans = Illusion.Game.Utils.Sound.Play(new Illusion.Game.Utils.Sound.Setting()
                {
                  type = Manager.Sound.Type.GameSE3D,
                  assetBundleName = keyInfoList[index2].assetPath,
                  assetName = this.procNameSE.ToString()
                });
                trans.SafeProcObject<Transform>((Action<Transform>) (_ => ((AudioSource) ((Component) _).GetComponent<AudioSource>()).SafeProcObject<AudioSource>((Action<AudioSource>) (a => a.set_loop(true)))));
                ((AudioSource) ((Component) trans).GetComponent<AudioSource>()).set_rolloffMode((AudioRolloffMode) 1);
                GameObject parent = keyInfoList[index2].objParent;
                DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Action<M0>) (_ =>
                {
                  Vector3 pos = !Object.op_Implicit((Object) parent) ? Vector3.get_zero() : parent.get_transform().get_position();
                  Quaternion rot = !Object.op_Implicit((Object) parent) ? Quaternion.get_identity() : parent.get_transform().get_rotation();
                  trans.SafeProcObject<Transform>((Action<Transform>) (o => o.SetPositionAndRotation(pos, rot)));
                })), (Component) trans);
                if (!this.dicloopContainKey[0])
                  this.dicLoop.Add(this.procNameSE.ToString(), new Dictionary<string, Transform>());
                if (!this.dicloopContainKey[1])
                  this.dicLoop[this.procNameSE.ToString()].Add(keyInfoList[index2].objParentName, (Transform) null);
                this.dicLoop[this.procNameSE.ToString()][keyInfoList[index2].objParentName] = trans;
              }
            }
          }
          else
          {
            Illusion.Game.Utils.Sound.Setting s = new Illusion.Game.Utils.Sound.Setting()
            {
              type = Manager.Sound.Type.GameSE3D,
              assetBundleName = keyInfoList[index2].assetPath,
              assetName = this.procNameSE.ToString()
            };
            if (keyInfoList[index2].isShorts && keyInfoList[index2].nameShortsSE.Count > 0 && _females[keyInfoList[index2].nFemale].IsKokanHide())
              s.assetName = this.GetSEName(keyInfoList[index2].nameShortsSE);
            Transform trans = Illusion.Game.Utils.Sound.Play(s);
            ((AudioSource) ((Component) trans).GetComponent<AudioSource>()).set_rolloffMode((AudioRolloffMode) 1);
            GameObject parent = keyInfoList[index2].objParent;
            DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Action<M0>) (_ =>
            {
              Vector3 pos = !Object.op_Implicit((Object) parent) ? Vector3.get_zero() : parent.get_transform().get_position();
              Quaternion rot = !Object.op_Implicit((Object) parent) ? Quaternion.get_identity() : parent.get_transform().get_rotation();
              trans.SafeProcObject<Transform>((Action<Transform>) (o => o.SetPositionAndRotation(pos, rot)));
            })), (Component) trans);
          }
        }
        break;
      }
    }
    this.oldNormalizeTime = _now;
    this.oldnameHash = ((AnimatorStateInfo) ref _ai).get_shortNameHash();
    StringBuilderPool.Release(this.procNameSE);
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai, ChaControl _female, int main = 0)
  {
    if (Object.op_Equality((Object) _female, (Object) null))
      return false;
    if (this.oldnameHash != ((AnimatorStateInfo) ref _ai).get_shortNameHash())
      this.oldNormalizeTime = 0.0f;
    float _now = ((AnimatorStateInfo) ref _ai).get_normalizedTime() % 1f;
    this.procNameSE = StringBuilderPool.Get();
    string empty = string.Empty;
    string key = string.Empty;
    for (int index1 = 0; index1 < this.lstInfo.Count; ++index1)
    {
      if (((AnimatorStateInfo) ref _ai).IsName(this.lstInfo[index1].nameAnimation))
      {
        List<HSeCtrl.KeyInfo> keyInfoList = this.lstInfo[index1].IsKey(this.oldNormalizeTime, _now);
        for (int index2 = 0; index2 < keyInfoList.Count; ++index2)
        {
          this.procNameSE.Clear();
          this.procNameSE.Append(this.GetSEName(keyInfoList[index2].nameSE));
          if (keyInfoList[index2].isLoop)
          {
            if (keyInfoList[index2].loopSwitch == 0)
            {
              bool flag = false;
              for (int index3 = 0; index3 < keyInfoList[index2].nameSE.Count; ++index3)
              {
                int index4 = index3;
                if (this.dicLoop.ContainsKey(keyInfoList[index2].nameSE[index4]))
                {
                  empty = keyInfoList[index2].nameSE[index4];
                  if (this.dicLoop[empty].ContainsKey(keyInfoList[index2].objParentName))
                  {
                    flag = true;
                    key = keyInfoList[index2].objParentName;
                    break;
                  }
                }
              }
              if (flag)
              {
                Singleton<Manager.Sound>.Instance.Stop(this.dicLoop[empty][key]);
                this.dicLoop[empty].Remove(key);
              }
            }
            else
            {
              this.dicloopContainKey[0] = this.dicLoop.ContainsKey(this.procNameSE.ToString());
              this.dicloopContainKey[1] = this.dicloopContainKey[0] && this.dicLoop[this.procNameSE.ToString()].ContainsKey(keyInfoList[index2].objParentName);
              if (!this.dicloopContainKey[0] || !this.dicloopContainKey[1])
              {
                Transform trans = Illusion.Game.Utils.Sound.Play(new Illusion.Game.Utils.Sound.Setting()
                {
                  type = Manager.Sound.Type.GameSE3D,
                  assetBundleName = keyInfoList[index2].assetPath,
                  assetName = this.procNameSE.ToString()
                });
                trans.SafeProcObject<Transform>((Action<Transform>) (_ => ((AudioSource) ((Component) _).GetComponent<AudioSource>()).SafeProcObject<AudioSource>((Action<AudioSource>) (a => a.set_loop(true)))));
                GameObject parent = keyInfoList[index2].objParent;
                DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Action<M0>) (_ =>
                {
                  Vector3 pos = !Object.op_Implicit((Object) parent) ? Vector3.get_zero() : parent.get_transform().get_position();
                  Quaternion rot = !Object.op_Implicit((Object) parent) ? Quaternion.get_identity() : parent.get_transform().get_rotation();
                  trans.SafeProcObject<Transform>((Action<Transform>) (o => o.SetPositionAndRotation(pos, rot)));
                })), (Component) trans);
                if (!this.dicloopContainKey[0])
                  this.dicLoop.Add(this.procNameSE.ToString(), new Dictionary<string, Transform>());
                if (!this.dicloopContainKey[1])
                  this.dicLoop[this.procNameSE.ToString()].Add(keyInfoList[index2].objParentName, (Transform) null);
                this.dicLoop[this.procNameSE.ToString()][keyInfoList[index2].objParentName] = trans;
              }
            }
          }
          else
          {
            Illusion.Game.Utils.Sound.Setting s = new Illusion.Game.Utils.Sound.Setting()
            {
              type = Manager.Sound.Type.GameSE3D,
              assetBundleName = keyInfoList[index2].assetPath,
              assetName = this.procNameSE.ToString()
            };
            if (keyInfoList[index2].isShorts && (keyInfoList[index2].nameShortsSE.Count > 0 && _female.IsKokanHide()))
              s.assetName = this.GetSEName(keyInfoList[index2].nameShortsSE);
            Transform trans = Illusion.Game.Utils.Sound.Play(s);
            GameObject parent = keyInfoList[index2].objParent;
            DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Action<M0>) (_ =>
            {
              Vector3 pos = !Object.op_Implicit((Object) parent) ? Vector3.get_zero() : parent.get_transform().get_position();
              Quaternion rot = !Object.op_Implicit((Object) parent) ? Quaternion.get_identity() : parent.get_transform().get_rotation();
              trans.SafeProcObject<Transform>((Action<Transform>) (o => o.SetPositionAndRotation(pos, rot)));
            })), (Component) trans);
          }
        }
        break;
      }
    }
    this.oldNormalizeTime = _now;
    this.oldnameHash = ((AnimatorStateInfo) ref _ai).get_shortNameHash();
    StringBuilderPool.Release(this.procNameSE);
    return true;
  }

  public void InitOldMember(int _init = -1)
  {
    if (_init == -1 || _init == 0)
      this.oldNormalizeTime = 0.0f;
    if (_init != -1 && _init != 1)
      return;
    this.oldnameHash = 0;
  }

  private string GetSEName(List<string> list)
  {
    if (list.Count < 1)
      return string.Empty;
    int index = Random.Range(0, list.Count);
    return list[index];
  }

  private struct KeyInfo
  {
    public GameObject objParent;
    public string objParentName;
    public float key;
    public bool isLoop;
    public int loopSwitch;
    public string assetPath;
    public List<string> nameSE;
    public bool isShorts;
    public List<string> nameShortsSE;
    public int nFemale;
    public bool bChangeVol;
  }

  private class Info
  {
    public List<HSeCtrl.KeyInfo> key = new List<HSeCtrl.KeyInfo>();
    public string nameAnimation;

    public List<HSeCtrl.KeyInfo> IsKey(float _old, float _now)
    {
      HSeCtrl.Info.IsCheck<float, float, float>[] isCheckArray = new HSeCtrl.Info.IsCheck<float, float, float>[2]
      {
        new HSeCtrl.Info.IsCheck<float, float, float>(this.IsKeyLoop),
        new HSeCtrl.Info.IsCheck<float, float, float>(this.IsKeyNormal)
      };
      List<HSeCtrl.KeyInfo> keyInfoList = new List<HSeCtrl.KeyInfo>();
      int index1 = (double) _old <= (double) _now ? 1 : 0;
      for (int index2 = 0; index2 < this.key.Count; ++index2)
      {
        if (isCheckArray[index1](_old, _now, this.key[index2].key))
          keyInfoList.Add(this.key[index2]);
      }
      return keyInfoList;
    }

    private bool IsKeyLoop(float _old, float _now, float _key)
    {
      return (double) _old < (double) _key || (double) _now > (double) _key;
    }

    private bool IsKeyNormal(float _old, float _now, float _key)
    {
      return (double) _old <= (double) _key && (double) _key < (double) _now;
    }

    public delegate bool IsCheck<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2);
  }
}
