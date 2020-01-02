// Decompiled with JetBrains decompiler
// Type: HVoiceCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using AIProject.Definitions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

public class HVoiceCtrl : MonoBehaviour
{
  public HVoiceCtrl.BreathList[] breathLists;
  private Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>>[] dicdiclstVoiceList;
  private HVoiceCtrl.ShortVoiceList[] ShortBreathLists;
  private ValueDictionary<int, int, int, int, int, string, HVoiceCtrl.BreathPtn> dicBreathPtns;
  private ValueDictionary<int, int, int, int, int, string, HVoiceCtrl.BreathPtn> dicBreathAddPtns;
  private Dictionary<int, List<HVoiceCtrl.VoicePtn>>[] lstLoadVoicePtn;
  private List<HVoiceCtrl.StartVoicePtn>[] lstLoadStartVoicePtn;
  private HVoiceCtrl.ShortBreathPtn[] shortBreathPtns;
  private HVoiceCtrl.ShortBreathPtn[] shortBreathAddPtns;
  private Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceAnimationPlay>> dicdicVoicePlayAnimation;
  public HSceneFlagCtrl ctrlFlag;
  [SerializeField]
  private HVoiceCtrl.BreathList[] breathUseLists;
  [SerializeField]
  private ValueDictionary<int, string, HVoiceCtrl.BreathPtn> dicBreathUsePtns;
  [SerializeField]
  private List<HVoiceCtrl.VoicePtn>[] lstVoicePtn;
  [SerializeField]
  private ValueDictionary<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> dicShortBreathUsePtns;
  public HVoiceCtrl.VoiceAnimationPlay playAnimation;
  [SerializeField]
  private int[] personality;
  [SerializeField]
  private float[] voicePitch;
  [SerializeField]
  private Actor param;
  [SerializeField]
  private Actor param_sub;
  [SerializeField]
  private List<int> lstSystem;
  public HVoiceCtrl.Voice[] nowVoices;
  public int nowMode;
  public int nowKind;
  public int nowId;
  [SerializeField]
  private int merchantType;
  private GlobalMethod.FloatBlend[] blendEyes;
  private GlobalMethod.FloatBlend[] blendMouths;
  private GlobalMethod.FloatBlend[] blendMouthMaxs;
  private HSceneManager hSceneManager;
  public float HBeforeHouchiTime;
  public float HouchiTime;
  private bool[] isPlays;
  private StringBuilder sbLoadFile;
  [SerializeField]
  private int IngoLimit;
  private bool masturbation;
  private bool les;
  public int MapHID;
  private List<string> lst;
  private List<string> lstBreathAbnames;

  public HVoiceCtrl()
  {
    base.\u002Ector();
  }

  public bool SetVoiceList(int _mode, int _kind, int _id, List<int> _lstSystem)
  {
    this.lstSystem = _lstSystem;
    this.nowKind = _kind;
    this.nowId = _id;
    switch (_mode)
    {
      case 3:
        this.nowMode = 4;
        break;
      case 4:
        this.nowMode = 5;
        break;
      case 5:
        this.nowMode = 3;
        break;
      default:
        this.nowMode = _mode;
        break;
    }
    this.lstLoadVoicePtn[0].TryGetValue(this.nowMode, out this.lstVoicePtn[0]);
    if (this.lstLoadVoicePtn[1] != null)
      this.lstLoadVoicePtn[1].TryGetValue(this.nowMode, out this.lstVoicePtn[1]);
    this.playAnimation = new HVoiceCtrl.VoiceAnimationPlay();
    if (this.dicdicVoicePlayAnimation.ContainsKey(this.nowMode))
      this.dicdicVoicePlayAnimation[this.nowMode].TryGetValue(_id, out this.playAnimation);
    else
      this.dicdicVoicePlayAnimation.Add(this.nowMode, new Dictionary<int, HVoiceCtrl.VoiceAnimationPlay>());
    if (this.playAnimation == null)
    {
      this.playAnimation = new HVoiceCtrl.VoiceAnimationPlay();
      this.dicdicVoicePlayAnimation[this.nowMode].Add(this.nowId, this.playAnimation);
    }
    for (int index = 0; index < 2; ++index)
    {
      this.nowVoices[index].animBreath = string.Empty;
      this.nowVoices[index].animVoice = string.Empty;
      this.nowVoices[index].breathGroup = -1;
    }
    return true;
  }

  public bool SetBreathVoiceList(
    ChaControl[] _charas,
    int _mode,
    int _kind,
    bool _isFemaleMain,
    bool _isIyagari,
    bool _isMainFirstPerson = true)
  {
    this.breathUseLists[0] = this.breathLists[0];
    this.breathUseLists[1] = this.breathLists[1];
    int[] numArray = new int[2]{ -1, -1 };
    if (_isIyagari && this.hSceneManager.EventKind != HSceneManager.HEvent.Yobai)
      numArray[0] = numArray[1] = 2;
    else if (_isFemaleMain && _mode != 4)
      numArray[0] = numArray[1] = 3;
    if (_mode < 3)
      _kind = 0;
    for (int index = 0; index < 2; ++index)
    {
      this.dicBreathUsePtns[index] = (ValueDictionary<string, HVoiceCtrl.BreathPtn>) null;
      if (this.dicBreathPtns[index].ContainsKey(_mode) && this.dicBreathPtns[index][_mode].ContainsKey(_kind))
      {
        int key = !_isMainFirstPerson ? index ^ 1 : index;
        if (this.dicBreathPtns[index][_mode][_kind].ContainsKey(key))
        {
          if (numArray[index] == -1)
            numArray[index] = !Object.op_Implicit((Object) _charas[index]) || this.hSceneManager.PersonalPhase[index] <= 1 ? 0 : 1;
          if (this.dicBreathPtns[index][_mode][_kind][key].ContainsKey(numArray[index]))
            this.dicBreathUsePtns[index] = this.dicBreathPtns[index][_mode][_kind][key][numArray[index]];
        }
      }
    }
    for (int index = 0; index < 2; ++index)
    {
      this.dicShortBreathUsePtns[index] = (ValueDictionary<int, List<HVoiceCtrl.BreathVoicePtnInfo>>) null;
      if (this.shortBreathPtns[index].dicInfo.ContainsKey(_mode) && this.shortBreathPtns[index].dicInfo[_mode].ContainsKey(_kind))
      {
        int key = !_isMainFirstPerson ? index ^ 1 : index;
        ValueDictionary<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> valueDictionary = this.shortBreathPtns[index].dicInfo[_mode][_kind];
        if (this.shortBreathPtns[index].dicInfo[_mode][_kind].ContainsKey(key))
          this.dicShortBreathUsePtns[index] = this.shortBreathPtns[index].dicInfo[_mode][_kind][key];
      }
    }
    for (int index = 0; index < 2; ++index)
    {
      if (this.dicBreathUsePtns[index] == null)
        this.dicBreathUsePtns[index] = this.dicBreathUsePtns.New<int, string, HVoiceCtrl.BreathPtn>();
      if (this.dicShortBreathUsePtns[index] == null)
        this.dicShortBreathUsePtns[index] = this.dicShortBreathUsePtns.New<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
    }
    return true;
  }

  public bool AfterFinish()
  {
    if (this.playAnimation == null)
      return false;
    this.playAnimation.AfterFinish();
    return true;
  }

  public bool Proc(AnimatorStateInfo _ai, params ChaControl[] _females)
  {
    this.isPlays[0] = false;
    this.isPlays[1] = false;
    for (int _nFemale = 1; _nFemale > -1; --_nFemale)
    {
      if ((_females.Length != 1 || _nFemale != 1) && !Object.op_Equality((Object) _females[_nFemale], (Object) null))
        this.isPlays[_nFemale] = this.StartVoiceProc(_females, _nFemale);
    }
    for (int _main = 1; _main > -1; --_main)
    {
      if ((_females.Length != 1 || _main != 1) && (!Object.op_Equality((Object) _females[_main], (Object) null) && !this.isPlays[_main]))
        this.isPlays[_main] = this.VoiceProc(_ai, _females[_main], _main);
    }
    for (int _main = 0; _main < 2; ++_main)
    {
      if ((_females.Length != 1 || _main != 1) && (!Object.op_Equality((Object) _females[_main], (Object) null) && !this.isPlays[_main]))
        this.isPlays[_main] = this.ShortBreathProc(_females[_main], _main);
    }
    for (int _main = 0; _main < 2; ++_main)
    {
      if ((_females.Length != 1 || _main != 1) && (!Object.op_Equality((Object) _females[_main], (Object) null) && !this.isPlays[_main]))
        this.BreathProc(_ai, _females[_main], _main, false);
    }
    for (int _main = 0; _main < 2; ++_main)
    {
      if (_females.Length != 1 && !Object.op_Equality((Object) _females[_main], (Object) null))
        this.OpenCtrl(_females[_main], _main);
    }
    this.nowVoices[0].speedStateFast = this.ctrlFlag.nowSpeedStateFast;
    if (_females.Length > 1 && Object.op_Inequality((Object) _females[1], (Object) null) && (_females[1].visibleAll && Object.op_Inequality((Object) _females[1].objTop, (Object) null)))
      this.nowVoices[1].speedStateFast = this.ctrlFlag.nowSpeedStateFast;
    for (int id = 0; id < 2; ++id)
    {
      if ((id != 1 || _females.Length > 1 && !Object.op_Equality((Object) _females[1], (Object) null) && (_females[1].visibleAll && !Object.op_Equality((Object) _females[1].objTop, (Object) null))) && (!this.isVoiceCheck(id) && (this.nowVoices[id].state == HVoiceCtrl.VoiceKind.startVoice || this.nowVoices[id].state == HVoiceCtrl.VoiceKind.voice)))
        this.nowVoices[id].state = HVoiceCtrl.VoiceKind.none;
    }
    if (this.isVoiceCheck(0) && (this.nowVoices[0].state == HVoiceCtrl.VoiceKind.voice || this.nowVoices[0].state == HVoiceCtrl.VoiceKind.startVoice) || this.isVoiceCheck(1) && (this.nowVoices[1].state == HVoiceCtrl.VoiceKind.voice || this.nowVoices[1].state == HVoiceCtrl.VoiceKind.startVoice))
      this.HouchiTime = 0.0f;
    if ((double) this.ctrlFlag.StartHouchiTime < (double) this.HouchiTime)
    {
      if (this.MapHID < 0)
      {
        this.ctrlFlag.voice.playVoices[0] = true;
        this.ctrlFlag.voice.playVoices[1] = true;
      }
      else if (this.ctrlFlag.MapHvoices.ContainsKey(this.MapHID))
      {
        this.ctrlFlag.MapHvoices[this.MapHID].playVoices[0] = true;
        this.ctrlFlag.MapHvoices[this.MapHID].playVoices[1] = true;
      }
    }
    return true;
  }

  public bool OpenCtrl(ChaControl _female, int _main)
  {
    if (!_female.visibleAll || Object.op_Equality((Object) _female.objBody, (Object) null))
      return false;
    float _ans1 = 0.0f;
    if (this.blendEyes[0] == null)
    {
      this.blendEyes[0] = new GlobalMethod.FloatBlend();
      this.blendEyes[1] = new GlobalMethod.FloatBlend();
    }
    if (this.blendMouths[0] == null)
    {
      this.blendMouths[0] = new GlobalMethod.FloatBlend();
      this.blendMouths[1] = new GlobalMethod.FloatBlend();
    }
    if (this.blendMouthMaxs[0] == null)
    {
      this.blendMouthMaxs[0] = new GlobalMethod.FloatBlend();
      this.blendMouthMaxs[1] = new GlobalMethod.FloatBlend();
    }
    if (this.blendEyes[_main].Proc(ref _ans1))
      _female.ChangeEyesOpenMax(_ans1);
    FBSCtrlMouth mouthCtrl = _female.mouthCtrl;
    if (mouthCtrl != null)
    {
      float _ans2 = 0.0f;
      if (this.blendMouths[_main].Proc(ref _ans2))
        mouthCtrl.OpenMin = _ans2;
      float _ans3 = 1f;
      if (this.blendMouthMaxs[_main].Proc(ref _ans3))
        mouthCtrl.OpenMax = _ans3;
    }
    return true;
  }

  public bool FaceReset(ChaControl _female)
  {
    _female.ChangeEyesOpenMax(1f);
    FBSCtrlMouth mouthCtrl = _female.mouthCtrl;
    if (mouthCtrl != null)
      mouthCtrl.OpenMin = 0.0f;
    _female.DisableShapeMouth(false);
    return true;
  }

  public bool BreathProc(
    AnimatorStateInfo _ai,
    ChaControl _female,
    int _main,
    bool _forceSleepIdle = false)
  {
    if (!_female.visibleAll || Object.op_Equality((Object) _female.objBody, (Object) null) || this.breathUseLists[_main].lstVoiceList.Count == 0)
      return false;
    foreach (KeyValuePair<string, HVoiceCtrl.BreathPtn> keyValuePair in (Dictionary<string, HVoiceCtrl.BreathPtn>) this.dicBreathUsePtns[_main])
    {
      if (((AnimatorStateInfo) ref _ai).IsName(keyValuePair.Key) || _forceSleepIdle)
      {
        HVoiceCtrl.BreathPtn _ptn = keyValuePair.Value;
        if (!_forceSleepIdle || !(_ptn.anim != "D_Idle"))
        {
          if (_ptn.onlyOne)
          {
            if (!(_ptn.anim == this.nowVoices[_main].animBreath))
            {
              if (_ptn.anim == this.nowVoices[_main].animVoice)
                break;
            }
            else
              break;
          }
          if (this.nowVoices[_main].state != HVoiceCtrl.VoiceKind.breath)
          {
            if (!_ptn.force)
            {
              if (this.isVoiceCheck(_main))
                break;
            }
          }
          else
          {
            if (this.ctrlFlag.isGaugeHit != this.nowVoices[_main].isGaugeHit)
            {
              if (this.nowVoices[_main].breathInfo != null)
              {
                this.SetBreathFace(_ptn, _main);
                this.SetFace(this.nowVoices[_main].Face, _female, _main);
              }
            }
            else
            {
              this.nowVoices[_main].timeFaceDelta += Time.get_deltaTime();
              if ((double) this.nowVoices[_main].timeFaceDelta >= (double) this.nowVoices[_main].timeFace && this.nowVoices[_main].breathInfo != null)
              {
                this.SetBreathFace(_ptn, _main);
                this.SetFace(this.nowVoices[_main].Face, _female, _main);
              }
            }
            this.nowVoices[_main].isGaugeHit = this.ctrlFlag.isGaugeHit;
          }
          List<int> source = new List<int>();
          for (int index = 0; index < _ptn.lstInfo.Count; ++index)
          {
            HVoiceCtrl.BreathVoicePtnInfo _lst = _ptn.lstInfo[index];
            if (this.IsPlayBreathVoicePtn(_female, _lst, _main))
              source.AddRange((IEnumerable<int>) _lst.lstVoice.OrderBy<int, Guid>((Func<int, Guid>) (inf => Guid.NewGuid())).ToList<int>());
          }
          List<int> list = source.OrderBy<int, Guid>((Func<int, Guid>) (inf => Guid.NewGuid())).ToList<int>();
          if (list.Count != 0)
          {
            int index = list[0];
            HVoiceCtrl.VoiceListInfo lstVoice = this.breathUseLists[_main].lstVoiceList[index];
            if (this.nowVoices[_main].state == HVoiceCtrl.VoiceKind.breath && (this.nowVoices[_main].breathInfo == lstVoice || this.nowVoices[_main].breathGroup == lstVoice.group))
            {
              if (this.isVoiceCheck(_main))
                break;
            }
            HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
            Manager.Voice instance = Singleton<Manager.Voice>.Instance;
            int num1 = this.personality[_main];
            string pathAsset = lstVoice.pathAsset;
            string nameFile = lstVoice.nameFile;
            float num2 = this.voicePitch[_main];
            Transform voiceTr = voiceFlag.voiceTrs[_main];
            int no = num1;
            string assetBundleName = pathAsset;
            string assetName = nameFile;
            double num3 = (double) num2;
            Transform voiceTrans = voiceTr;
            Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, (float) num3, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
            AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
            if (this.masturbation || this.les)
              Singleton<Sound>.Instance.AudioSettingData3DOnly(component, 2);
            else
              component.set_rolloffMode((AudioRolloffMode) 1);
            _female.SetVoiceTransform(trfVoice);
            if (!voiceFlag.lstUseAsset.Contains(lstVoice.pathAsset))
              voiceFlag.lstUseAsset.Add(lstVoice.pathAsset);
            this.nowVoices[_main].breathInfo = lstVoice;
            this.nowVoices[_main].state = HVoiceCtrl.VoiceKind.breath;
            this.nowVoices[_main].animBreath = _ptn.anim;
            this.nowVoices[_main].notOverWrite = this.nowVoices[_main].breathInfo.notOverWrite;
            this.nowVoices[_main].arrayBreath = index;
            this.nowVoices[_main].breathGroup = lstVoice.group;
            this.SetBreathFace(_ptn, _main);
            if (voiceFlag.urines[_main] && lstVoice.urine)
              voiceFlag.urines[_main] = false;
            this.SetFace(this.nowVoices[_main].Face, _female, _main);
            break;
          }
          break;
        }
      }
    }
    return true;
  }

  private bool IsPlayBreathVoicePtn(
    ChaControl _female,
    HVoiceCtrl.BreathVoicePtnInfo _lst,
    int _main)
  {
    return this.IsBreathPtnConditions(_female, _lst.lstConditions, _main) && this.IsBreathAnimationList(_lst.lstAnimeID, this.nowId);
  }

  private bool IsBreathAnimationList(List<int> _lstAnimList, int _idNow)
  {
    return _lstAnimList.Count == 0 || _lstAnimList.Contains(-1) || _lstAnimList.Contains(_idNow);
  }

  private bool IsBreathPtnConditions(ChaControl _female, List<int> _lstConditions, int _main)
  {
    for (int index = 0; index < _lstConditions.Count; ++index)
    {
      switch (_lstConditions[index] + 1)
      {
        case 1:
          if (this.hSceneManager.PersonalPhase[_main] > 1)
            return false;
          break;
        case 2:
          if (this.hSceneManager.PersonalPhase[_main] < 2)
            return false;
          break;
        case 3:
          if (this.hSceneManager.GetFlaverSkillLevel(2) >= this.IngoLimit)
            return false;
          break;
        case 4:
          if (this.hSceneManager.GetFlaverSkillLevel(2) < this.IngoLimit)
            return false;
          break;
        case 5:
          if (this.VoiceFlag().sleep)
            return false;
          break;
        case 6:
          if (!this.VoiceFlag().sleep)
            return false;
          break;
        case 7:
          if (this.ctrlFlag.nowSpeedStateFast)
            return false;
          break;
        case 8:
          if (!this.ctrlFlag.nowSpeedStateFast)
            return false;
          break;
        case 9:
          if (this.VoiceFlag().urines[_main])
            return false;
          break;
        case 10:
          if (!this.VoiceFlag().urines[_main])
            return false;
          break;
      }
    }
    return true;
  }

  private bool SetBreathFace(HVoiceCtrl.BreathPtn _ptn, int _main)
  {
    if (this.ctrlFlag.isGaugeHit)
    {
      if (this.nowVoices[_main].breathInfo.lstHitFace.Count > 0)
        this.nowVoices[_main].Face = this.nowVoices[_main].breathInfo.lstHitFace[Random.Range(0, this.nowVoices[_main].breathInfo.lstHitFace.Count)];
    }
    else if (this.nowVoices[_main].breathInfo.lstNotHitFace.Count > 0)
      this.nowVoices[_main].Face = this.nowVoices[_main].breathInfo.lstNotHitFace[Random.Range(0, this.nowVoices[_main].breathInfo.lstNotHitFace.Count)];
    this.nowVoices[_main].timeFaceDelta = 0.0f;
    this.nowVoices[_main].timeFace = Random.Range(_ptn.timeChangeFaceMin, _ptn.timeChangeFaceMax);
    return true;
  }

  public bool VoiceProc(AnimatorStateInfo _ai, ChaControl _female, int _main)
  {
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    if (Singleton<HSceneManager>.Instance.EventKind == HSceneManager.HEvent.Yobai || !_female.visibleAll || Object.op_Equality((Object) _female.objBody, (Object) null))
    {
      voiceFlag.playVoices[_main] = false;
      return false;
    }
    int num = this.VoiceProcDetail(_ai, _female, true, _main);
    if (num == 3)
      num = this.VoiceProcDetail(_ai, _female, false, _main);
    if (num != 2)
    {
      voiceFlag.playVoices[_main] = false;
      if (num == 1)
        voiceFlag.playShorts[_main] = -1;
    }
    return num == 1;
  }

  public int VoiceProcDetail(AnimatorStateInfo _ai, ChaControl _female, bool _isFirst, int _main)
  {
    if (!_female.visibleAll || Object.op_Equality((Object) _female.objBody, (Object) null))
      return 0;
    int num1 = 0;
    HVoiceCtrl.VoiceAnimationPlayInfo animationPlayInfo = this.playAnimation.GetAnimation(((AnimatorStateInfo) ref _ai).get_shortNameHash());
    if (animationPlayInfo == null)
    {
      animationPlayInfo = new HVoiceCtrl.VoiceAnimationPlayInfo();
      animationPlayInfo.animationHash = ((AnimatorStateInfo) ref _ai).get_shortNameHash();
      this.playAnimation.lstPlayInfo.Add(animationPlayInfo);
    }
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    bool flag = this.ctrlFlag.nowAnimationInfo.ActionCtrl.Item1 == 4 && this.ctrlFlag.nowAnimationInfo.nInitiativeFemale != 0;
    if (this.lstVoicePtn == null || this.lstVoicePtn[_main] == null)
    {
      if (flag && this.nowVoices[0].state != HVoiceCtrl.VoiceKind.voice && this.nowVoices[0].state != HVoiceCtrl.VoiceKind.startVoice)
        voiceFlag.dialog = false;
      return 0;
    }
    if (this.nowVoices[_main].notOverWrite && this.isVoiceCheck(_main))
      return 2;
    if (this.dicdiclstVoiceList[_main] == null || this.dicdiclstVoiceList[_main].Count == 0)
      return 0;
    for (int index = 0; index < this.lstVoicePtn[_main].Count; ++index)
    {
      HVoiceCtrl.VoicePtn voicePtn = this.lstVoicePtn[_main][index];
      if ((this.hSceneManager.bMerchant || this.VoicePtnCondition(voicePtn.condition, _main)) && this.VoicePtnStartEvent(voicePtn.startKind, _female, _main))
      {
        if (voicePtn.howTalk == 0)
        {
          if (((AnimatorStateInfo) ref _ai).IsName(voicePtn.anim))
          {
            List<int> _lstCategory = new List<int>();
            if (voicePtn.LookFlag)
            {
              if (!voiceFlag.playVoices[_main])
                break;
            }
            else if (animationPlayInfo.isPlays[_main])
              break;
            List<HVoiceCtrl.PlayVoiceinfo> list = this.GetPlayListNum(voicePtn.lstInfo, this.dicdiclstVoiceList[_main], ref _lstCategory, _female, this.playAnimation, _main).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
            if (list.Count == 0)
            {
              this.InitListPlayFlag(voicePtn.lstInfo, this.dicdiclstVoiceList[_main], _lstCategory);
              list = this.GetPlayListNum(voicePtn.lstInfo, this.dicdiclstVoiceList[_main], ref _lstCategory, _female, this.playAnimation, _main).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
              if (list.Count == 0)
              {
                num1 = 3;
                continue;
              }
            }
            int voiceId = list[0].voiceID;
            int mode = list[0].mode;
            int kind = list[0].kind;
            if (this.dicdiclstVoiceList[_main].Count != 0 && this.dicdiclstVoiceList[_main].ContainsKey(mode) && (this.dicdiclstVoiceList[_main][mode].ContainsKey(kind) && this.dicdiclstVoiceList[_main][mode][kind].dicdicVoiceList.Count != 0) && this.dicdiclstVoiceList[_main][mode][kind].dicdicVoiceList.ContainsKey(voiceId))
            {
              HVoiceCtrl.VoiceListInfo dicdicVoice = this.dicdiclstVoiceList[_main][mode][kind].dicdicVoiceList[voiceId];
              Manager.Voice instance = Singleton<Manager.Voice>.Instance;
              int num2 = this.personality[_main];
              string pathAsset = dicdicVoice.pathAsset;
              string nameFile = dicdicVoice.nameFile;
              float num3 = this.voicePitch[_main];
              Transform voiceTr = voiceFlag.voiceTrs[_main];
              int no = num2;
              string assetBundleName = pathAsset;
              string assetName = nameFile;
              double num4 = (double) num3;
              Transform voiceTrans = voiceTr;
              Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, (float) num4, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
              AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
              if (this.masturbation || this.les)
                Singleton<Sound>.Instance.AudioSettingData3DOnly(component, 2);
              else
                component.set_rolloffMode((AudioRolloffMode) 1);
              _female.SetVoiceTransform(trfVoice);
              if (!voiceFlag.lstUseAsset.Contains(dicdicVoice.pathAsset))
                voiceFlag.lstUseAsset.Add(dicdicVoice.pathAsset);
              this.nowVoices[_main].voiceInfo = dicdicVoice;
              this.nowVoices[_main].state = HVoiceCtrl.VoiceKind.voice;
              this.nowVoices[_main].animVoice = voicePtn.anim;
              this.nowVoices[_main].notOverWrite = this.nowVoices[_main].voiceInfo.notOverWrite;
              this.nowVoices[_main].arrayVoice = voiceId;
              this.nowVoices[_main].VoiceListID = mode;
              this.nowVoices[_main].VoiceListSheetID = kind;
              this.nowVoices[_main].voiceInfo.isPlay = true;
              if (this.nowVoices[_main].voiceInfo.lstHitFace.Count > 0)
                this.nowVoices[_main].Face = this.nowVoices[_main].voiceInfo.lstHitFace[Random.Range(0, this.nowVoices[_main].voiceInfo.lstHitFace.Count)];
              animationPlayInfo.isPlays[_main] = true;
              num1 = 1;
              this.SetFace(this.nowVoices[_main].Face, _female, _main);
              voiceFlag.dialog = false;
              break;
            }
            break;
          }
        }
        else
        {
          if (flag)
          {
            if (voicePtn.howTalk == 1 && (voiceFlag.dialog || _main == 1) || voicePtn.howTalk == 2 && (!voiceFlag.dialog || _main == 0))
              continue;
          }
          else if (voicePtn.howTalk == 1 && (voiceFlag.dialog || _main == 0) || voicePtn.howTalk == 2 && (!voiceFlag.dialog || _main == 1))
            continue;
          if (((AnimatorStateInfo) ref _ai).IsName(voicePtn.anim))
          {
            List<int> _lstCategory = new List<int>();
            if (voicePtn.LookFlag)
            {
              if (!flag && _main == 1 || flag && _main == 0)
              {
                if (voiceFlag.playVoices[_main])
                {
                  if (animationPlayInfo.isPlays[_main])
                    continue;
                }
                else
                  break;
              }
            }
            else if (animationPlayInfo.isPlays[_main])
              break;
            if (this.nowVoices[_main ^ 1].state != HVoiceCtrl.VoiceKind.voice && this.nowVoices[_main ^ 1].state != HVoiceCtrl.VoiceKind.startVoice)
            {
              List<HVoiceCtrl.PlayVoiceinfo> list = this.GetPlayListNum(voicePtn.lstInfo, this.dicdiclstVoiceList[_main], ref _lstCategory, _female, this.playAnimation, _main).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
              if (list.Count == 0 && list.Count == 0)
              {
                num1 = 3;
              }
              else
              {
                int voiceId = list[0].voiceID;
                int mode = list[0].mode;
                int kind = list[0].kind;
                if (this.dicdiclstVoiceList[_main].Count != 0 && this.dicdiclstVoiceList[_main].ContainsKey(mode) && (this.dicdiclstVoiceList[_main][mode].ContainsKey(kind) && this.dicdiclstVoiceList[_main][mode][kind].dicdicVoiceList.Count != 0) && this.dicdiclstVoiceList[_main][mode][kind].dicdicVoiceList.ContainsKey(voiceId))
                {
                  HVoiceCtrl.VoiceListInfo dicdicVoice = this.dicdiclstVoiceList[_main][mode][kind].dicdicVoiceList[voiceId];
                  Manager.Voice instance = Singleton<Manager.Voice>.Instance;
                  int num2 = this.personality[_main];
                  string pathAsset = dicdicVoice.pathAsset;
                  string nameFile = dicdicVoice.nameFile;
                  float num3 = this.voicePitch[_main];
                  Transform voiceTr = voiceFlag.voiceTrs[_main];
                  int no = num2;
                  string assetBundleName = pathAsset;
                  string assetName = nameFile;
                  double num4 = (double) num3;
                  Transform voiceTrans = voiceTr;
                  Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, (float) num4, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
                  AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
                  if (this.masturbation || this.les)
                    Singleton<Sound>.Instance.AudioSettingData3DOnly(component, 2);
                  else
                    component.set_rolloffMode((AudioRolloffMode) 1);
                  _female.SetVoiceTransform(trfVoice);
                  if (!voiceFlag.lstUseAsset.Contains(dicdicVoice.pathAsset))
                    voiceFlag.lstUseAsset.Add(dicdicVoice.pathAsset);
                  this.nowVoices[_main].voiceInfo = dicdicVoice;
                  this.nowVoices[_main].state = HVoiceCtrl.VoiceKind.voice;
                  this.nowVoices[_main].animVoice = voicePtn.anim;
                  this.nowVoices[_main].notOverWrite = this.nowVoices[_main].voiceInfo.notOverWrite;
                  this.nowVoices[_main].arrayVoice = voiceId;
                  this.nowVoices[_main].VoiceListID = mode;
                  this.nowVoices[_main].VoiceListSheetID = kind;
                  this.nowVoices[_main].voiceInfo.isPlay = true;
                  if (this.nowVoices[_main].voiceInfo.lstHitFace.Count > 0)
                    this.nowVoices[_main].Face = this.nowVoices[_main].voiceInfo.lstHitFace[Random.Range(0, this.nowVoices[_main].voiceInfo.lstHitFace.Count)];
                  animationPlayInfo.isPlays[_main] = true;
                  num1 = 1;
                  this.SetFace(this.nowVoices[_main].Face, _female, _main);
                  voiceFlag.dialog = voicePtn.howTalk == 1;
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
      }
    }
    return num1;
  }

  private List<HVoiceCtrl.PlayVoiceinfo> GetPlayListNum(
    List<HVoiceCtrl.VoicePtnInfo> _lst,
    Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>> _lstVoice,
    ref List<int> _lstCategory,
    ChaControl female,
    HVoiceCtrl.VoiceAnimationPlay nowVoiceInfo,
    int _main)
  {
    List<HVoiceCtrl.PlayVoiceinfo> playVoiceinfoList = new List<HVoiceCtrl.PlayVoiceinfo>();
    for (int index = 0; index < _lst.Count; ++index)
    {
      if (this.VoiceAnimationList(_lst[index].lstAnimList, this.nowId) && (this.ctrlFlag.initiative == 0 || _lst[index].lstPlayConditions.Contains(13)) && this.VoicePtnConditions(_lst[index].lstPlayConditions, female, nowVoiceInfo, _main))
      {
        _lstCategory.Add(index);
        playVoiceinfoList.AddRange((IEnumerable<HVoiceCtrl.PlayVoiceinfo>) this.GetPlayNum(_lst[index], _lstVoice));
      }
    }
    return playVoiceinfoList;
  }

  private bool VoicePtnStartEvent(int EventPtn, ChaControl _female, int _main)
  {
    switch (EventPtn)
    {
      case 0:
        if (Object.op_Inequality((Object) _female, (Object) null) && (double) this.hSceneManager.Mood[_main] < (double) _female.fileGameInfo.moodBound.lower || this.hSceneManager.EventKind != HSceneManager.HEvent.Normal && this.hSceneManager.EventKind != HSceneManager.HEvent.GyakuYobai && this.hSceneManager.EventKind != HSceneManager.HEvent.FromFemale)
          return false;
        break;
      case 1:
        if ((!Object.op_Inequality((Object) _female, (Object) null) || (double) this.hSceneManager.Mood[_main] >= (double) _female.fileGameInfo.moodBound.lower) && (this.hSceneManager.EventKind == HSceneManager.HEvent.Normal || this.hSceneManager.EventKind == HSceneManager.HEvent.GyakuYobai || this.hSceneManager.EventKind == HSceneManager.HEvent.FromFemale))
          return false;
        break;
    }
    return true;
  }

  private bool VoicePtnCondition(int _condition, int _main)
  {
    if (this.ctrlFlag.isFaintness)
      return _condition == 4;
    switch (_condition)
    {
      case 0:
        if (this.hSceneManager.PersonalPhase[_main] >= 2)
          return false;
        break;
      case 1:
        if (this.hSceneManager.PersonalPhase[_main] < 2)
          return false;
        break;
      case 4:
        return false;
    }
    return true;
  }

  private bool VoicePtnConditionAccurate(int _condition, int _main)
  {
    switch (_condition)
    {
      case 0:
        if (this.hSceneManager.PersonalPhase[_main] >= 2 || this.hSceneManager.isForce || this.ctrlFlag.isFaintness)
          return false;
        break;
      case 1:
        if (this.hSceneManager.PersonalPhase[_main] < 2 || this.hSceneManager.isForce || this.ctrlFlag.isFaintness)
          return false;
        break;
      case 2:
        if (!this.hSceneManager.isForce || this.ctrlFlag.isFaintness)
          return false;
        break;
      case 3:
        if (this.ctrlFlag.isFaintness)
          return false;
        break;
      case 4:
        if (!this.ctrlFlag.isFaintness)
          return false;
        break;
    }
    return true;
  }

  private bool VoiceAnimationList(List<int> _lstAnimList, int _idNow)
  {
    if (_lstAnimList.Count == 0)
      return false;
    return _lstAnimList.Contains(-1) || _lstAnimList.Contains(_idNow);
  }

  private bool VoicePtnConditions(
    List<int> _lstConditions,
    ChaControl female,
    HVoiceCtrl.VoiceAnimationPlay nowVoiceInfo,
    int _main)
  {
    if (_lstConditions.Contains(13) && this.ctrlFlag.initiative == 0)
      return false;
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    for (int index = 0; index < _lstConditions.Count; ++index)
    {
      switch (_lstConditions[index] + 1)
      {
        case 1:
          if ((double) this.ctrlFlag.StartHouchiTime > (double) this.HouchiTime)
            return false;
          if (this.nowMode < 4)
          {
            this.HouchiTime = 0.0f;
            break;
          }
          break;
        case 2:
          if (female.GetBustSizeKind() != 0)
            return false;
          break;
        case 3:
          if (female.GetBustSizeKind() != 2)
            return false;
          break;
        case 4:
          if (female.GetBustSizeKind() != 1)
            return false;
          break;
        case 5:
          if (nowVoiceInfo == null || nowVoiceInfo.Count == 0)
            return false;
          break;
        case 6:
          if (nowVoiceInfo == null || nowVoiceInfo.Count < 1)
            return false;
          break;
        case 7:
          if (this.hSceneManager.GetFlaverSkillLevel(2) < this.IngoLimit)
            return false;
          break;
        case 8:
          if (this.hSceneManager.GetFlaverSkillLevel(2) >= this.IngoLimit)
            return false;
          break;
        case 9:
          if (_main != 0)
            return false;
          break;
        case 10:
          if (_main != 1)
            return false;
          break;
        case 11:
          if (voiceFlag.oldFinish != 0)
            return false;
          break;
        case 12:
          if (voiceFlag.oldFinish != 2)
            return false;
          break;
        case 13:
          if (voiceFlag.oldFinish != 3)
            return false;
          break;
        case 15:
          if (voiceFlag.oldFinish != 1)
            return false;
          break;
      }
    }
    return true;
  }

  private List<HVoiceCtrl.PlayVoiceinfo> GetPlayNum(
    HVoiceCtrl.VoicePtnInfo _lstPlay,
    Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>> _lstVoice)
  {
    List<HVoiceCtrl.PlayVoiceinfo> playVoiceinfoList = new List<HVoiceCtrl.PlayVoiceinfo>();
    if (_lstVoice.Count == 0)
      return playVoiceinfoList;
    int loadListmode = _lstPlay.loadListmode;
    int loadListKind = _lstPlay.loadListKind;
    if (!_lstVoice.ContainsKey(loadListmode) || !_lstVoice[loadListmode].ContainsKey(loadListKind))
      return playVoiceinfoList;
    Dictionary<int, HVoiceCtrl.VoiceListInfo> dicdicVoiceList = _lstVoice[loadListmode][loadListKind].dicdicVoiceList;
    for (int index = 0; index < _lstPlay.lstVoice.Count; ++index)
    {
      if (!dicdicVoiceList.ContainsKey(_lstPlay.lstVoice[index]))
        GlobalMethod.DebugLog("再生しようとしている番号がリストにない", 1);
      else if (!dicdicVoiceList[_lstPlay.lstVoice[index]].isPlay)
      {
        HVoiceCtrl.PlayVoiceinfo playVoiceinfo;
        playVoiceinfo.mode = loadListmode;
        playVoiceinfo.kind = loadListKind;
        playVoiceinfo.voiceID = _lstPlay.lstVoice[index];
        playVoiceinfoList.Add(playVoiceinfo);
      }
    }
    return playVoiceinfoList;
  }

  private bool InitListPlayFlag(
    List<HVoiceCtrl.VoicePtnInfo> _lst,
    Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>> _lstVoice,
    List<int> _lstCategory)
  {
    for (int index1 = 0; index1 < _lstCategory.Count; ++index1)
    {
      if (_lst.Count > _lstCategory[index1])
      {
        int loadListmode = _lst[_lstCategory[index1]].loadListmode;
        int loadListKind = _lst[_lstCategory[index1]].loadListKind;
        for (int index2 = 0; index2 < _lst[_lstCategory[index1]].lstVoice.Count; ++index2)
        {
          int key = _lst[_lstCategory[index1]].lstVoice[index2];
          if (_lstVoice.ContainsKey(loadListmode) && _lstVoice[loadListmode].ContainsKey(loadListKind))
          {
            if (!_lstVoice[loadListmode][loadListKind].dicdicVoiceList.ContainsKey(key))
              GlobalMethod.DebugLog("再生しようとしている番号がリストにない", 1);
            else
              _lstVoice[loadListmode][loadListKind].dicdicVoiceList[key].isPlay = false;
          }
        }
      }
    }
    return true;
  }

  private bool StartVoiceProc(ChaControl[] _females, int _nFemale)
  {
    bool flag = false;
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    if (Object.op_Equality((Object) _females[_nFemale], (Object) null) || Object.op_Equality((Object) _females[_nFemale].objBody, (Object) null))
    {
      if (voiceFlag.playStart != -1 && _nFemale == 0)
        voiceFlag.playStart = -1;
      return false;
    }
    List<HVoiceCtrl.StartVoicePtn> startVoicePtnList = this.lstLoadStartVoicePtn[_nFemale];
    if (startVoicePtnList == null)
      return false;
    for (int index = 0; index < 2; ++index)
    {
      if (this.nowVoices[index].notOverWrite && this.isVoiceCheck(_nFemale))
        return flag;
    }
    if (_nFemale == 0 && voiceFlag.playStart > 4 && (this.nowVoices[1].state == HVoiceCtrl.VoiceKind.startVoice || this.nowVoices[1].state == HVoiceCtrl.VoiceKind.voice))
      return flag;
    if (this.dicdiclstVoiceList[_nFemale] == null)
      return false;
    for (int index = 0; index < startVoicePtnList.Count; ++index)
    {
      HVoiceCtrl.StartVoicePtn startVoicePtn = startVoicePtnList[index];
      if (startVoicePtn.nTaii == this.nowMode && (this.hSceneManager.bMerchant || this.VoicePtnCondition(startVoicePtn.condition, _nFemale)) && startVoicePtn.timing == voiceFlag.playStart)
      {
        if (startVoicePtn.nForce != -1)
        {
          if (startVoicePtn.nForce == 0)
          {
            if (this.hSceneManager.isForce || this.hSceneManager.EventKind != HSceneManager.HEvent.Normal && this.hSceneManager.EventKind != HSceneManager.HEvent.GyakuYobai && this.hSceneManager.EventKind != HSceneManager.HEvent.FromFemale)
              continue;
          }
          else if (this.MapHID >= 0 || !this.hSceneManager.isForce || startVoicePtn.nForce == 2 && Singleton<HSceneFlagCtrl>.Instance.nPlace != 15 || (startVoicePtn.nForce == 3 && Singleton<HSceneFlagCtrl>.Instance.nPlace != 12 || startVoicePtn.nForce == 5 && (double) this.hSceneManager.Mood[_nFemale] > (double) _females[_nFemale].fileGameInfo.moodBound.lower) || startVoicePtn.nForce == 4 && this.hSceneManager.Agent[0].BehaviorResources.Mode != Desire.ActionType.EndTaskMasturbation)
            continue;
        }
        List<int> _lstCategory = new List<int>();
        List<HVoiceCtrl.PlayVoiceinfo> list = this.GetPlayListNum(startVoicePtn.lstInfo, this.dicdiclstVoiceList[_nFemale], ref _lstCategory, _nFemale, _females[_nFemale]).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
        if (list.Count == 0)
        {
          this.InitGetPlayListNum(startVoicePtn.lstInfo, this.dicdiclstVoiceList[_nFemale], _lstCategory);
          list = this.GetPlayListNum(startVoicePtn.lstInfo, this.dicdiclstVoiceList[_nFemale], ref _lstCategory, _nFemale, _females[_nFemale]).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
          if (list.Count == 0)
          {
            if (index == startVoicePtnList.Count - 1)
            {
              Debug.Log((object) ("再生するものがない[" + (object) _nFemale + "]"));
              if (_nFemale == 0 && voiceFlag.playStart < 5)
              {
                voiceFlag.playStart = -1;
                break;
              }
              break;
            }
            continue;
          }
        }
        if (startVoicePtn.timing == 4 && this.ctrlFlag.nowAnimationInfo.nInitiativeFemale != 0)
        {
          voiceFlag.playStart = -1;
          break;
        }
        int mode = list[0].mode;
        int kind = list[0].kind;
        int voiceId = list[0].voiceID;
        if (!this.dicdiclstVoiceList[_nFemale][mode][kind].dicdicVoiceList.ContainsKey(voiceId))
        {
          GlobalMethod.DebugLog("配列外してる[" + (object) _nFemale + "]", 1);
          break;
        }
        if (Singleton<HSceneManager>.Instance.EventKind != HSceneManager.HEvent.Yobai)
        {
          HVoiceCtrl.VoiceListInfo dicdicVoice = this.dicdiclstVoiceList[_nFemale][mode][kind].dicdicVoiceList[voiceId];
          Manager.Voice instance = Singleton<Manager.Voice>.Instance;
          int num1 = this.personality[_nFemale];
          string pathAsset = dicdicVoice.pathAsset;
          string nameFile = dicdicVoice.nameFile;
          float num2 = this.voicePitch[_nFemale];
          Transform voiceTr = voiceFlag.voiceTrs[_nFemale];
          int no = num1;
          string assetBundleName = pathAsset;
          string assetName = nameFile;
          double num3 = (double) num2;
          Transform voiceTrans = voiceTr;
          Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, (float) num3, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
          AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
          if (this.masturbation || this.les)
            Singleton<Sound>.Instance.AudioSettingData3DOnly(component, 2);
          else
            component.set_rolloffMode((AudioRolloffMode) 1);
          _females[_nFemale].SetVoiceTransform(trfVoice);
          if (!voiceFlag.lstUseAsset.Contains(dicdicVoice.pathAsset))
            voiceFlag.lstUseAsset.Add(dicdicVoice.pathAsset);
          this.nowVoices[_nFemale].voiceInfo = dicdicVoice;
          if (startVoicePtn.timing != 0 && startVoicePtn.timing < 4)
          {
            HVoiceCtrl.VoiceKind[] voiceKindArray = new HVoiceCtrl.VoiceKind[3]
            {
              HVoiceCtrl.VoiceKind.startVoice,
              HVoiceCtrl.VoiceKind.startVoice,
              HVoiceCtrl.VoiceKind.voice
            };
            this.nowVoices[_nFemale].state = voiceKindArray[startVoicePtn.timing - 1];
            if (_nFemale == 1)
            {
              voiceFlag.playStart += 4;
              if (this.nowVoices[0].state != HVoiceCtrl.VoiceKind.breath && this.nowVoices[0].state != HVoiceCtrl.VoiceKind.breathShort)
                Singleton<Manager.Voice>.Instance.Stop(voiceFlag.voiceTrs[_nFemale ^ 1]);
            }
            else
              voiceFlag.playStart = -1;
          }
          else if (startVoicePtn.timing == 0)
          {
            this.nowVoices[_nFemale].state = HVoiceCtrl.VoiceKind.startVoice;
            voiceFlag.playStart = -1;
          }
          else
          {
            this.nowVoices[_nFemale].state = startVoicePtn.timing == 5 || startVoicePtn.timing == 6 ? HVoiceCtrl.VoiceKind.startVoice : HVoiceCtrl.VoiceKind.voice;
            if (_nFemale == 1 && this.nowVoices[0].state != HVoiceCtrl.VoiceKind.breath && this.nowVoices[0].state != HVoiceCtrl.VoiceKind.breathShort)
              Singleton<Manager.Voice>.Instance.Stop(voiceFlag.voiceTrs[_nFemale ^ 1]);
            voiceFlag.playStart = -1;
          }
          this.nowVoices[_nFemale].animVoice = startVoicePtn.anim;
          this.nowVoices[_nFemale].notOverWrite = this.nowVoices[_nFemale].voiceInfo.notOverWrite;
          this.nowVoices[_nFemale].arrayVoice = voiceId;
          this.nowVoices[_nFemale].VoiceListID = mode;
          this.nowVoices[_nFemale].VoiceListSheetID = kind;
          if (this.nowVoices[_nFemale].voiceInfo.lstHitFace.Count > 0)
            this.nowVoices[_nFemale].Face = this.nowVoices[_nFemale].voiceInfo.lstHitFace[Random.Range(0, this.nowVoices[_nFemale].voiceInfo.lstHitFace.Count)];
          this.nowVoices[_nFemale].voiceInfo.isPlay = true;
          flag = true;
          this.SetFace(this.nowVoices[_nFemale].Face, _females[_nFemale], _nFemale);
          break;
        }
        break;
      }
    }
    return flag;
  }

  private List<HVoiceCtrl.PlayVoiceinfo> GetPlayListNum(
    List<HVoiceCtrl.VoicePtnInfo> _lst,
    Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>> _lstVoice,
    ref List<int> _lstCategory,
    int _main,
    ChaControl female)
  {
    List<HVoiceCtrl.PlayVoiceinfo> playVoiceinfoList = new List<HVoiceCtrl.PlayVoiceinfo>();
    for (int index = 0; index < _lst.Count; ++index)
    {
      if ((_lst[index].lstAnimList[0] == -1 || _lst[index].lstAnimList.Contains(this.ctrlFlag.nowAnimationInfo.id)) && this.StartVoicePtnConditions(_lst[index].lstPlayConditions, female, _main))
      {
        playVoiceinfoList.AddRange((IEnumerable<HVoiceCtrl.PlayVoiceinfo>) this.GetPlayNum(_lst[index], _lstVoice));
        _lstCategory.Add(index);
      }
    }
    return playVoiceinfoList;
  }

  private bool StartVoicePtnConditions(List<int> conditions, ChaControl female, int _main)
  {
    if (conditions.Contains(7) && this.ctrlFlag.initiative == 0)
      return false;
    for (int index = 0; index < conditions.Count; ++index)
    {
      switch (conditions[index] + 1)
      {
        case 1:
          if (female.GetBustSizeKind() != 0)
            return false;
          break;
        case 2:
          if (female.GetBustSizeKind() != 2)
            return false;
          break;
        case 3:
          if (female.GetBustSizeKind() != 1)
            return false;
          break;
        case 4:
          if (this.hSceneManager.GetFlaverSkillLevel(2) < this.IngoLimit)
            return false;
          break;
        case 5:
          if (this.hSceneManager.GetFlaverSkillLevel(2) >= this.IngoLimit)
            return false;
          break;
        case 6:
          if (_main != 0)
            return false;
          break;
        case 7:
          if (_main != 1)
            return false;
          break;
      }
    }
    return true;
  }

  private void InitGetPlayListNum(
    List<HVoiceCtrl.VoicePtnInfo> _lst,
    Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>> _lstVoice,
    List<int> _lstCategory)
  {
    for (int index1 = 0; index1 < _lstCategory.Count; ++index1)
    {
      if (_lst.Count <= _lstCategory[index1])
      {
        GlobalMethod.DebugLog("開始音声のカテゴリ取得に失敗(フラグ再設定時)", 1);
      }
      else
      {
        int loadListmode = _lst[_lstCategory[index1]].loadListmode;
        int loadListKind = _lst[_lstCategory[index1]].loadListKind;
        for (int index2 = 0; index2 < _lst[_lstCategory[index1]].lstVoice.Count; ++index2)
        {
          if (!_lstVoice.ContainsKey(loadListmode) || !_lstVoice[loadListmode].ContainsKey(loadListKind))
          {
            GlobalMethod.DebugLog("開始音声の再生番号取得に失敗(フラグ再設定時)", 1);
          }
          else
          {
            int key = _lst[_lstCategory[index1]].lstVoice[index2];
            if (!_lstVoice[loadListmode][loadListKind].dicdicVoiceList.ContainsKey(key))
              GlobalMethod.DebugLog("開始音声の再生番号取得に失敗(フラグ再設定時)", 1);
            else
              _lstVoice[loadListmode][loadListKind].dicdicVoiceList[key].isPlay = false;
          }
        }
      }
    }
  }

  private List<HVoiceCtrl.PlayVoiceinfo> GetHBeforePlayListNum(
    List<HVoiceCtrl.VoicePtnInfo> _lst,
    Dictionary<int, Dictionary<int, HVoiceCtrl.VoiceList>> _lstVoice,
    ref List<int> _lstCategory,
    int _main,
    ChaControl female)
  {
    List<HVoiceCtrl.PlayVoiceinfo> playVoiceinfoList = new List<HVoiceCtrl.PlayVoiceinfo>();
    for (int index = 0; index < _lst.Count; ++index)
    {
      if (this.StartVoicePtnConditions(_lst[index].lstPlayConditions, female, _main))
      {
        playVoiceinfoList.AddRange((IEnumerable<HVoiceCtrl.PlayVoiceinfo>) this.GetPlayNum(_lst[index], _lstVoice));
        _lstCategory.Add(index);
      }
    }
    return playVoiceinfoList;
  }

  public void HBeforeProc(ChaControl[] _females)
  {
    this.isPlays[0] = false;
    this.isPlays[1] = false;
    for (int index = 1; index > -1; --index)
    {
      int _nFemale = index;
      if (_females.Length != 1 && !Object.op_Equality((Object) _females[_nFemale], (Object) null))
        this.isPlays[_nFemale] = this.HBeforeProc(_females, _nFemale);
    }
  }

  private bool HBeforeProc(ChaControl[] _females, int _nFemale)
  {
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    if (Object.op_Equality((Object) _females[_nFemale], (Object) null) || Object.op_Equality((Object) _females[_nFemale].objBody, (Object) null))
    {
      if (voiceFlag.playStart != -1 && _nFemale == 0)
        voiceFlag.playStart = -1;
      return false;
    }
    List<HVoiceCtrl.StartVoicePtn> startVoicePtnList = this.lstLoadStartVoicePtn[_nFemale];
    if (startVoicePtnList == null)
      return false;
    for (int index = 0; index < 2; ++index)
    {
      if (this.nowVoices[index].notOverWrite && this.isVoiceCheck(_nFemale))
        return false;
    }
    if (_nFemale == 0 && voiceFlag.playStart > 4 && (this.nowVoices[1].state == HVoiceCtrl.VoiceKind.startVoice || this.nowVoices[1].state == HVoiceCtrl.VoiceKind.voice) || this.dicdiclstVoiceList[_nFemale] == null)
      return false;
    for (int index = 0; index < startVoicePtnList.Count; ++index)
    {
      HVoiceCtrl.StartVoicePtn startVoicePtn = startVoicePtnList[index];
      if (startVoicePtn.timing == 0)
      {
        if (startVoicePtn.nForce != -1)
        {
          if (startVoicePtn.nForce == 0)
          {
            if (this.hSceneManager.EventKind != HSceneManager.HEvent.Normal && this.hSceneManager.EventKind != HSceneManager.HEvent.GyakuYobai && this.hSceneManager.EventKind != HSceneManager.HEvent.FromFemale)
              continue;
          }
          else if (!this.hSceneManager.isForce || startVoicePtn.nForce == 2 && Singleton<HSceneFlagCtrl>.Instance.nPlace != 15 || (startVoicePtn.nForce == 3 && Singleton<HSceneFlagCtrl>.Instance.nPlace != 12 || startVoicePtn.nForce == 4 && this.hSceneManager.Agent[0].BehaviorResources.Mode != Desire.ActionType.EndTaskMasturbation))
            continue;
        }
        if ((this.hSceneManager.bMerchant || this.VoicePtnCondition(startVoicePtn.condition, _nFemale)) && startVoicePtn.timing == voiceFlag.playStart)
        {
          List<int> _lstCategory = new List<int>();
          List<HVoiceCtrl.PlayVoiceinfo> list = this.GetHBeforePlayListNum(startVoicePtn.lstInfo, this.dicdiclstVoiceList[_nFemale], ref _lstCategory, _nFemale, _females[_nFemale]).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
          if (list.Count == 0)
          {
            this.InitGetPlayListNum(startVoicePtn.lstInfo, this.dicdiclstVoiceList[_nFemale], _lstCategory);
            list = this.GetHBeforePlayListNum(startVoicePtn.lstInfo, this.dicdiclstVoiceList[_nFemale], ref _lstCategory, _nFemale, _females[_nFemale]).OrderBy<HVoiceCtrl.PlayVoiceinfo, Guid>((Func<HVoiceCtrl.PlayVoiceinfo, Guid>) (inf => Guid.NewGuid())).ToList<HVoiceCtrl.PlayVoiceinfo>();
            if (list.Count == 0)
            {
              if (index == startVoicePtnList.Count - 1)
              {
                Debug.Log((object) ("再生するものがない[" + (object) _nFemale + "]"));
                if (_nFemale == 0 && voiceFlag.playStart < 5)
                {
                  voiceFlag.playStart = -1;
                  break;
                }
                break;
              }
              continue;
            }
          }
          int mode = list[0].mode;
          int kind = list[0].kind;
          int voiceId = list[0].voiceID;
          if (!this.dicdiclstVoiceList[_nFemale][mode][kind].dicdicVoiceList.ContainsKey(voiceId))
          {
            GlobalMethod.DebugLog("配列外してる[" + (object) _nFemale + "]", 1);
            break;
          }
          if (Singleton<HSceneManager>.Instance.EventKind != HSceneManager.HEvent.Yobai)
          {
            HVoiceCtrl.VoiceListInfo dicdicVoice = this.dicdiclstVoiceList[_nFemale][mode][kind].dicdicVoiceList[voiceId];
            Manager.Voice instance = Singleton<Manager.Voice>.Instance;
            int num1 = this.personality[_nFemale];
            string pathAsset = dicdicVoice.pathAsset;
            string nameFile = dicdicVoice.nameFile;
            float num2 = this.voicePitch[_nFemale];
            Transform voiceTr = voiceFlag.voiceTrs[_nFemale];
            int no = num1;
            string assetBundleName = pathAsset;
            string assetName = nameFile;
            double num3 = (double) num2;
            Transform voiceTrans = voiceTr;
            Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, (float) num3, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
            AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
            if (this.masturbation || this.les)
              Singleton<Sound>.Instance.AudioSettingData3DOnly(component, 2);
            else
              component.set_rolloffMode((AudioRolloffMode) 1);
            _females[_nFemale].SetVoiceTransform(trfVoice);
            if (!voiceFlag.lstUseAsset.Contains(dicdicVoice.pathAsset))
              voiceFlag.lstUseAsset.Add(dicdicVoice.pathAsset);
            this.nowVoices[_nFemale].voiceInfo = dicdicVoice;
            this.nowVoices[_nFemale].state = HVoiceCtrl.VoiceKind.startVoice;
            if (_nFemale == 1)
            {
              voiceFlag.playStart += 5;
              if (this.nowVoices[0].state != HVoiceCtrl.VoiceKind.breath && this.nowVoices[0].state != HVoiceCtrl.VoiceKind.breathShort)
                Singleton<Manager.Voice>.Instance.Stop(voiceFlag.voiceTrs[_nFemale ^ 1]);
            }
            else
              voiceFlag.playStart = -1;
            this.nowVoices[_nFemale].animVoice = startVoicePtn.anim;
            this.nowVoices[_nFemale].notOverWrite = this.nowVoices[_nFemale].voiceInfo.notOverWrite;
            this.nowVoices[_nFemale].arrayVoice = voiceId;
            this.nowVoices[_nFemale].VoiceListID = mode;
            this.nowVoices[_nFemale].VoiceListSheetID = kind;
            if (this.nowVoices[_nFemale].voiceInfo.lstHitFace.Count > 0)
              this.nowVoices[_nFemale].Face = this.nowVoices[_nFemale].voiceInfo.lstHitFace[Random.Range(0, this.nowVoices[_nFemale].voiceInfo.lstHitFace.Count)];
            this.nowVoices[_nFemale].voiceInfo.isPlay = true;
            this.SetFace(this.nowVoices[_nFemale].Face, _females[_nFemale], _nFemale);
            this.HBeforeHouchiTime = 0.0f;
            break;
          }
          break;
        }
      }
    }
    return true;
  }

  public bool ShortBreathProc(ChaControl _female, int _main)
  {
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    if (!_female.visibleAll || Object.op_Equality((Object) _female.objBody, (Object) null))
      voiceFlag.playShorts[_main] = -1;
    if (this.nowVoices[_main].notOverWrite && this.isVoiceCheck(_main))
      voiceFlag.playShorts[_main] = -1;
    if (Singleton<HSceneManager>.Instance.EventKind == HSceneManager.HEvent.Yobai)
      voiceFlag.playShorts[_main] = -1;
    if (this.dicShortBreathUsePtns[_main] == null)
      voiceFlag.playShorts[_main] = -1;
    if (voiceFlag.playShorts[_main] == -1 || this.dicShortBreathUsePtns[_main] != null && !this.dicShortBreathUsePtns[_main].ContainsKey(voiceFlag.playShorts[_main]))
      return false;
    List<HVoiceCtrl.BreathVoicePtnInfo> breathVoicePtnInfoList = this.dicShortBreathUsePtns[_main][voiceFlag.playShorts[_main]];
    if (breathVoicePtnInfoList.Count == 0)
    {
      voiceFlag.playShorts[_main] = -1;
      return false;
    }
    List<int> source = new List<int>();
    for (int index = 0; index < breathVoicePtnInfoList.Count; ++index)
    {
      if (this.IsPlayShortBreathVoicePtn(_female, breathVoicePtnInfoList[index], _main))
        source.AddRange((IEnumerable<int>) breathVoicePtnInfoList[index].lstVoice);
    }
    if (source.Count == 0)
      return false;
    int key = source.OrderBy<int, Guid>((Func<int, Guid>) (inf => Guid.NewGuid())).ToList<int>()[0];
    if (!this.ShortBreathLists[_main].dicShortBreathLists.ContainsKey(key))
    {
      voiceFlag.playShorts[_main] = -1;
      return false;
    }
    Dictionary<int, HVoiceCtrl.VoiceListInfo> shortBreathLists = this.ShortBreathLists[_main].dicShortBreathLists;
    Manager.Voice instance = Singleton<Manager.Voice>.Instance;
    int num1 = this.personality[_main];
    string pathAsset = shortBreathLists[key].pathAsset;
    string nameFile = shortBreathLists[key].nameFile;
    float num2 = this.voicePitch[_main];
    Transform voiceTr = voiceFlag.voiceTrs[_main];
    int no = num1;
    string assetBundleName = pathAsset;
    string assetName = nameFile;
    double num3 = (double) num2;
    Transform voiceTrans = voiceTr;
    Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, (float) num3, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
    AudioSource component = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
    if (this.masturbation || this.les)
      Singleton<Sound>.Instance.AudioSettingData3DOnly(component, 2);
    else
      component.set_rolloffMode((AudioRolloffMode) 1);
    _female.SetVoiceTransform(trfVoice);
    if (!voiceFlag.lstUseAsset.Contains(shortBreathLists[key].pathAsset))
      voiceFlag.lstUseAsset.Add(shortBreathLists[key].pathAsset);
    this.nowVoices[_main].shortInfo = shortBreathLists[key];
    this.nowVoices[_main].state = HVoiceCtrl.VoiceKind.breathShort;
    this.nowVoices[_main].notOverWrite = this.nowVoices[_main].shortInfo.notOverWrite;
    this.nowVoices[_main].arrayShort = key;
    this.nowVoices[_main].Face = this.nowVoices[_main].shortInfo.lstHitFace[Random.Range(0, this.nowVoices[_main].shortInfo.lstHitFace.Count)];
    voiceFlag.playShorts[_main] = -1;
    this.SetFace(this.nowVoices[_main].Face, _female, _main);
    return true;
  }

  private bool IsPlayShortBreathVoicePtn(
    ChaControl _female,
    HVoiceCtrl.BreathVoicePtnInfo _lst,
    int _main)
  {
    return this.CheckShortVoiceCondition(_female, _lst.lstConditions, _main) && this.IsShortBreathAnimationList(_lst.lstAnimeID, this.nowId);
  }

  private bool IsShortBreathAnimationList(List<int> _lstAnimList, int _idNow)
  {
    return _lstAnimList.Count == 0 || _lstAnimList.Contains(-1) || _lstAnimList.Contains(_idNow);
  }

  private bool CheckShortVoiceCondition(ChaControl _female, List<int> _lstConditions, int _main)
  {
    HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
    for (int index = 0; index < _lstConditions.Count; ++index)
    {
      switch (_lstConditions[index] + 1)
      {
        case 1:
          if (this.hSceneManager.isForce || this.ctrlFlag.nowAnimationInfo.nInitiativeFemale != 0 || (this.ctrlFlag.isFaintness || this.hSceneManager.PersonalPhase[_main] > 1))
            return false;
          break;
        case 2:
          if (this.hSceneManager.isForce || this.ctrlFlag.nowAnimationInfo.nInitiativeFemale != 0 || (this.ctrlFlag.isFaintness || this.hSceneManager.PersonalPhase[_main] < 2))
            return false;
          break;
        case 3:
          if (!this.hSceneManager.isForce || this.ctrlFlag.isFaintness)
            return false;
          break;
        case 4:
          if (this.ctrlFlag.isFaintness || this.ctrlFlag.nowAnimationInfo.nInitiativeFemale == 0)
            return false;
          break;
        case 5:
          if (!this.ctrlFlag.isFaintness || voiceFlag.sleep)
            return false;
          break;
        case 6:
          if (!this.ctrlFlag.isFaintness || !voiceFlag.sleep)
            return false;
          break;
        case 7:
          if (this.hSceneManager.GetFlaverSkillLevel(2) >= this.IngoLimit)
            return false;
          break;
        case 8:
          if (this.hSceneManager.GetFlaverSkillLevel(2) < this.IngoLimit)
            return false;
          break;
      }
    }
    return true;
  }

  public void PlaySoundETC(string _animName, int _mode, ChaControl _female, int _main, bool _iya = false)
  {
    this.isPlays[_main] = true;
    if (Singleton<HSceneManager>.Instance.EventKind == HSceneManager.HEvent.Yobai && this.ctrlFlag.isFaintness)
    {
      this.isPlays[_main] = false;
    }
    else
    {
      HVoiceCtrl.VoiceAnimationPlayInfo animation = this.playAnimation.GetAnimation(Animator.StringToHash(_animName));
      if (_mode > 2)
      {
        this.isPlays[_main] = false;
      }
      else
      {
        HSceneFlagCtrl.VoiceFlag voiceFlag = this.VoiceFlag();
        int key1;
        switch (_mode)
        {
          case 0:
            int key2 = !this.ctrlFlag.isFaintness ? (this.hSceneManager.Agent[_main].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
            if (!this.dicdiclstVoiceList[_main].ContainsKey(this.personality[_main]) || !this.dicdiclstVoiceList[_main][3].ContainsKey(5) || !this.dicdiclstVoiceList[_main][3][5].dicdicVoiceList.ContainsKey(key2))
            {
              this.isPlays[_main] = false;
              return;
            }
            HVoiceCtrl.VoiceListInfo dicdicVoice = this.dicdiclstVoiceList[_main][3][5].dicdicVoiceList[key2];
            Manager.Voice instance1 = Singleton<Manager.Voice>.Instance;
            int num1 = this.personality[_main];
            string pathAsset1 = dicdicVoice.pathAsset;
            string nameFile1 = dicdicVoice.nameFile;
            float num2 = this.voicePitch[_main];
            Transform voiceTr1 = voiceFlag.voiceTrs[_main];
            int no1 = num1;
            string assetBundleName1 = pathAsset1;
            string assetName1 = nameFile1;
            double num3 = (double) num2;
            Transform voiceTrans1 = voiceTr1;
            instance1.OnecePlayChara(no1, assetBundleName1, assetName1, (float) num3, 0.0f, 0.0f, true, voiceTrans1, Manager.Voice.Type.PCM, -1, true, true, false);
            if (!voiceFlag.lstUseAsset.Contains(dicdicVoice.pathAsset))
              voiceFlag.lstUseAsset.Add(dicdicVoice.pathAsset);
            this.nowVoices[_main].voiceInfo = dicdicVoice;
            this.nowVoices[_main].state = HVoiceCtrl.VoiceKind.voice;
            this.nowVoices[_main].animVoice = _animName;
            this.nowVoices[_main].notOverWrite = this.nowVoices[_main].voiceInfo.notOverWrite;
            this.nowVoices[_main].arrayVoice = key2;
            this.nowVoices[_main].Face = this.nowVoices[_main].voiceInfo.lstHitFace[Random.Range(0, this.nowVoices[_main].voiceInfo.lstHitFace.Count)];
            this.nowVoices[_main].voiceInfo.isPlay = true;
            animation.isPlays[_main] = true;
            this.SetFace(this.nowVoices[_main].Face, _female, _main);
            voiceFlag.dialog = false;
            return;
          case 1:
            key1 = !_iya ? 15 : 13;
            break;
          default:
            key1 = !_iya ? 16 : 14;
            break;
        }
        Dictionary<int, HVoiceCtrl.VoiceListInfo> shortBreathLists = this.ShortBreathLists[_main].dicShortBreathLists;
        if (!shortBreathLists.ContainsKey(key1))
        {
          this.isPlays[_main] = false;
        }
        else
        {
          Manager.Voice instance2 = Singleton<Manager.Voice>.Instance;
          int num4 = this.personality[_main];
          string pathAsset2 = shortBreathLists[key1].pathAsset;
          string nameFile2 = shortBreathLists[key1].nameFile;
          float num5 = this.voicePitch[_main];
          Transform voiceTr2 = voiceFlag.voiceTrs[_main];
          int no2 = num4;
          string assetBundleName2 = pathAsset2;
          string assetName2 = nameFile2;
          double num6 = (double) num5;
          Transform voiceTrans2 = voiceTr2;
          instance2.OnecePlayChara(no2, assetBundleName2, assetName2, (float) num6, 0.0f, 0.0f, true, voiceTrans2, Manager.Voice.Type.PCM, -1, true, true, false);
          if (!voiceFlag.lstUseAsset.Contains(shortBreathLists[key1].pathAsset))
            voiceFlag.lstUseAsset.Add(shortBreathLists[key1].pathAsset);
          this.nowVoices[_main].shortInfo = shortBreathLists[key1];
          this.nowVoices[_main].state = HVoiceCtrl.VoiceKind.breathShort;
          this.nowVoices[_main].notOverWrite = this.nowVoices[_main].shortInfo.notOverWrite;
          this.nowVoices[_main].arrayShort = key1;
          this.nowVoices[_main].Face = this.nowVoices[_main].shortInfo.lstHitFace[Random.Range(0, this.nowVoices[_main].shortInfo.lstHitFace.Count)];
          voiceFlag.playShorts[_main] = -1;
          this.SetFace(this.nowVoices[_main].Face, _female, _main);
        }
      }
    }
  }

  private bool isVoiceCheck(int id)
  {
    return this.MapHID < 0 ? Object.op_Inequality((Object) this.ctrlFlag.voice.voiceTrs[id], (Object) null) && Singleton<Manager.Voice>.Instance.IsVoiceCheck(this.ctrlFlag.voice.voiceTrs[id], true) : this.ctrlFlag.MapHvoices.ContainsKey(this.MapHID) && Object.op_Inequality((Object) this.ctrlFlag.MapHvoices[this.MapHID].voiceTrs[id], (Object) null) && Singleton<Manager.Voice>.Instance.IsVoiceCheck(this.ctrlFlag.MapHvoices[this.MapHID].voiceTrs[id], true);
  }

  private HSceneFlagCtrl.VoiceFlag VoiceFlag()
  {
    if (this.MapHID < 0)
      return this.ctrlFlag.voice;
    return this.ctrlFlag.MapHvoices.ContainsKey(this.MapHID) ? this.ctrlFlag.MapHvoices[this.MapHID] : (HSceneFlagCtrl.VoiceFlag) null;
  }

  private bool SetFace(HVoiceCtrl.FaceInfo _face, ChaControl _female, int _main)
  {
    FBSCtrlEyes eyesCtrl = _female.eyesCtrl;
    if (eyesCtrl != null)
      this.blendEyes[_main].Start(eyesCtrl.OpenMax, _face.openEye, 0.3f);
    FBSCtrlMouth mouthCtrl = _female.mouthCtrl;
    if (mouthCtrl != null)
    {
      this.blendMouths[_main].Start(mouthCtrl.OpenMin, _face.openMouthMin, 0.3f);
      this.blendMouthMaxs[_main].Start(mouthCtrl.OpenMax, _face.openMouthMax, 0.3f);
    }
    _female.ChangeEyebrowPtn(_face.eyeBlow, true);
    _female.ChangeEyesPtn(_face.eye, true);
    _female.ChangeMouthPtn(_face.mouth, true);
    if (_face.mouth == 10 || _face.mouth == 13)
      _female.DisableShapeMouth(true);
    else
      _female.DisableShapeMouth(false);
    _female.ChangeTearsRate(_face.tear);
    _female.ChangeHohoAkaRate(_face.cheek);
    _female.HideEyeHighlight(!_face.highlight);
    _female.ChangeEyesBlinkFlag(_face.blink);
    return true;
  }

  [DebuggerHidden]
  public IEnumerator Init(
    int _personality,
    float _pitch,
    Actor _param,
    int _personality_sub = 0,
    float _pitch_sub = 0.0f,
    Actor _param_sub = null,
    int merchant = -1,
    bool musterbation = false,
    bool les = false)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HVoiceCtrl.\u003CInit\u003Ec__Iterator0()
    {
      _personality = _personality,
      _pitch = _pitch,
      _personality_sub = _personality_sub,
      _pitch_sub = _pitch_sub,
      _param = _param,
      _param_sub = _param_sub,
      musterbation = musterbation,
      les = les,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator LoadBreathList(
    string _pathAssetFolder,
    bool musterbation = false,
    bool les = false)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HVoiceCtrl.\u003CLoadBreathList\u003Ec__Iterator1()
    {
      musterbation = musterbation,
      les = les,
      _pathAssetFolder = _pathAssetFolder,
      \u0024this = this
    };
  }

  private bool LoadBreath(int _personality, int _main, string _pathAssetFolder)
  {
    this.sbLoadFile.Clear();
    this.sbLoadFile.AppendFormat("HBreath_{0:00}", (object) (_personality == -90 ? 5 : _personality));
    this.StartCoroutine(this.LoadBreathBase(GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString()), this.breathLists[_main]));
    return true;
  }

  private bool LoadMapHBreath(int _personality, int _main, string _pathAssetFolder)
  {
    this.sbLoadFile.Clear();
    this.sbLoadFile.AppendFormat("HBreath_{0:00}", (object) (_personality == -90 ? 5 : _personality));
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.LoadBreathBase(GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString()), this.breathLists[_main])), false));
    return true;
  }

  [DebuggerHidden]
  private IEnumerator LoadBreathBase(string _str, HVoiceCtrl.BreathList _breath)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HVoiceCtrl.\u003CLoadBreathBase\u003Ec__Iterator2()
    {
      _str = _str,
      _breath = _breath,
      \u0024this = this
    };
  }

  private void LoadBreathFace(
    string[][] _aastr,
    int _line,
    int _idx,
    List<HVoiceCtrl.FaceInfo> _lst)
  {
    HVoiceCtrl.FaceInfo faceInfo = new HVoiceCtrl.FaceInfo();
    float result;
    if (!float.TryParse(_aastr[_line][_idx++], out result) || (double) result < 0.0)
      return;
    faceInfo.openEye = result;
    faceInfo.openMouthMin = float.Parse(_aastr[_line][_idx++]);
    faceInfo.openMouthMax = float.Parse(_aastr[_line][_idx++]);
    faceInfo.eyeBlow = int.Parse(_aastr[_line][_idx++]);
    faceInfo.eye = int.Parse(_aastr[_line][_idx++]);
    faceInfo.mouth = int.Parse(_aastr[_line][_idx++]);
    faceInfo.tear = float.Parse(_aastr[_line][_idx++]);
    faceInfo.cheek = float.Parse(_aastr[_line][_idx++]);
    faceInfo.highlight = _aastr[_line][_idx++] == "1";
    faceInfo.blink = _aastr[_line][_idx++] == "1";
    _lst.Add(faceInfo);
  }

  private bool LoadBreathPtn(int personal, int mode, int kind)
  {
    this.sbLoadFile.Clear();
    this.sbLoadFile.AppendFormat("HBreathPattern_{0:00}_{1:00}_{2:00}", (object) personal, (object) mode, (object) kind);
    string str1 = GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString());
    if (str1.IsNullOrEmpty())
      return false;
    string[][] data;
    GlobalMethod.GetListString(str1, out data);
    int length = data.Length;
    int num = length == 0 ? 0 : data[0].Length;
    if (!this.dicBreathPtns[personal].ContainsKey(mode))
      this.dicBreathPtns[personal][mode] = this.dicBreathPtns[personal].New<int, int, int, int, string, HVoiceCtrl.BreathPtn>();
    if (!this.dicBreathPtns[personal][mode].ContainsKey(kind))
      this.dicBreathPtns[personal][mode][kind] = this.dicBreathPtns[personal][mode].New<int, int, int, string, HVoiceCtrl.BreathPtn>();
    ValueDictionary<int, int, string, HVoiceCtrl.BreathPtn> dictionary = this.dicBreathPtns[personal][mode][kind];
    for (int index1 = 0; index1 < length; ++index1)
    {
      int key1 = int.Parse(data[index1][0]);
      if (!dictionary.ContainsKey(key1))
        dictionary[key1] = dictionary.New<int, int, string, HVoiceCtrl.BreathPtn>();
      int key2 = int.Parse(data[index1][1]);
      if (!dictionary[key1].ContainsKey(key2))
        dictionary[key1][key2] = dictionary[key1].New<int, string, HVoiceCtrl.BreathPtn>();
      ValueDictionary<string, HVoiceCtrl.BreathPtn> valueDictionary = dictionary[key1][key2];
      string key3 = data[index1][2];
      if (!valueDictionary.ContainsKey(key3))
        valueDictionary.Add(key3, new HVoiceCtrl.BreathPtn());
      HVoiceCtrl.BreathPtn breathPtn = valueDictionary[key3];
      breathPtn.level = key2;
      breathPtn.anim = key3;
      breathPtn.onlyOne = data[index1][3] == "1";
      breathPtn.isPlay = false;
      breathPtn.force = data[index1][4] == "1";
      breathPtn.timeChangeFaceMin = float.Parse(data[index1][5]);
      breathPtn.timeChangeFaceMax = float.Parse(data[index1][6]);
      breathPtn.lstInfo.Clear();
      for (int index2 = 7; index2 < num; index2 += 3)
      {
        string self = data[index1][index2];
        if (!self.IsNullOrEmpty())
        {
          HVoiceCtrl.BreathVoicePtnInfo breathVoicePtnInfo = new HVoiceCtrl.BreathVoicePtnInfo();
          string[] strArray = self.Split(',');
          int result = 0;
          for (int index3 = 0; index3 < strArray.Length; ++index3)
          {
            if (int.TryParse(strArray[index3], out result))
              breathVoicePtnInfo.lstConditions.Add(result);
          }
          string str2 = data[index1][index2 + 1];
          char[] chArray1 = new char[1]{ ',' };
          foreach (string s in str2.Split(chArray1))
          {
            if (int.TryParse(s, out result))
              breathVoicePtnInfo.lstAnimeID.Add(result);
          }
          string str3 = data[index1][index2 + 2];
          char[] chArray2 = new char[1]{ ',' };
          foreach (string s in str3.Split(chArray2))
          {
            if (int.TryParse(s, out result))
              breathVoicePtnInfo.lstVoice.Add(result);
          }
          breathPtn.lstInfo.Add(breathVoicePtnInfo);
        }
        else
          break;
      }
    }
    return true;
  }

  private bool LoadBreathAddPtn(int personal, int mode, int kind)
  {
    for (int index1 = 0; index1 < 50; ++index1)
    {
      this.sbLoadFile.Clear();
      this.sbLoadFile.AppendFormat("HBreathPattern_{0:00}_{1:00}_{2:00}_{3:00}", (object) personal, (object) mode, (object) kind, (object) index1);
      string str1 = GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString());
      if (!str1.IsNullOrEmpty())
      {
        string[][] data;
        GlobalMethod.GetListString(str1, out data);
        int length = data.Length;
        int num = length == 0 ? 0 : data[0].Length;
        if (!this.dicBreathAddPtns[personal].ContainsKey(mode))
          this.dicBreathAddPtns[personal][mode] = this.dicBreathAddPtns[personal].New<int, int, int, int, string, HVoiceCtrl.BreathPtn>();
        if (!this.dicBreathAddPtns[personal][mode].ContainsKey(kind))
          this.dicBreathAddPtns[personal][mode][kind] = this.dicBreathAddPtns[personal][mode].New<int, int, int, string, HVoiceCtrl.BreathPtn>();
        ValueDictionary<int, int, string, HVoiceCtrl.BreathPtn> dictionary = this.dicBreathAddPtns[personal][mode][kind];
        for (int index2 = 0; index2 < length; ++index2)
        {
          int key1 = int.Parse(data[index2][0]);
          if (!dictionary.ContainsKey(key1))
            dictionary[key1] = dictionary.New<int, int, string, HVoiceCtrl.BreathPtn>();
          int key2 = int.Parse(data[index2][1]);
          if (!dictionary[key1].ContainsKey(key2))
            dictionary[key1][key2] = dictionary[key1].New<int, string, HVoiceCtrl.BreathPtn>();
          ValueDictionary<string, HVoiceCtrl.BreathPtn> valueDictionary = dictionary[key1][key2];
          string key3 = data[index2][2];
          if (!valueDictionary.ContainsKey(key3))
            valueDictionary.Add(key3, new HVoiceCtrl.BreathPtn());
          HVoiceCtrl.BreathPtn breathPtn = valueDictionary[key3];
          breathPtn.level = key2;
          breathPtn.anim = key3;
          breathPtn.onlyOne = data[index2][3] == "1";
          breathPtn.isPlay = false;
          breathPtn.force = data[index2][4] == "1";
          breathPtn.timeChangeFaceMin = float.Parse(data[index2][5]);
          breathPtn.timeChangeFaceMax = float.Parse(data[index2][6]);
          for (int index3 = 7; index3 < num; index3 += 3)
          {
            string str2 = data[index2][index3];
            if (!(str2 == string.Empty))
            {
              HVoiceCtrl.BreathVoicePtnInfo bvi = new HVoiceCtrl.BreathVoicePtnInfo();
              string[] strArray = str2.Split(',');
              int result = 0;
              for (int index4 = 0; index4 < strArray.Length; ++index4)
              {
                if (int.TryParse(strArray[index4], out result))
                  bvi.lstConditions.Add(result);
              }
              string str3 = data[index2][index3 + 1];
              char[] chArray1 = new char[1]{ ',' };
              foreach (string s in str3.Split(chArray1))
              {
                if (int.TryParse(s, out result))
                  bvi.lstAnimeID.Add(result);
              }
              string str4 = data[index2][index3 + 2];
              char[] chArray2 = new char[1]{ ',' };
              foreach (string s in str4.Split(chArray2))
              {
                if (int.TryParse(s, out result))
                  bvi.lstVoice.Add(result);
              }
              HVoiceCtrl.BreathVoicePtnInfo breathVoicePtnInfo = breathPtn.lstInfo.Find((Predicate<HVoiceCtrl.BreathVoicePtnInfo>) (f => f.lstConditions.SequenceEqual<int>((IEnumerable<int>) bvi.lstConditions) && f.lstAnimeID.SequenceEqual<int>((IEnumerable<int>) bvi.lstAnimeID)));
              if (breathVoicePtnInfo == null)
                breathPtn.lstInfo.Add(bvi);
              else
                breathVoicePtnInfo.lstVoice = new List<int>((IEnumerable<int>) bvi.lstVoice);
            }
            else
              break;
          }
        }
      }
    }
    this.LoadBreathPtnComposition(personal, mode, kind);
    return true;
  }

  private bool LoadBreathPtnComposition(int personal, int mode, int kind)
  {
    if (!this.dicBreathPtns.ContainsKey(personal) || !this.dicBreathPtns[personal].ContainsKey(mode) || !this.dicBreathPtns[personal][mode].ContainsKey(kind))
      return false;
    ValueDictionary<int, int, string, HVoiceCtrl.BreathPtn> valueDictionary1 = this.dicBreathPtns[personal][mode][kind];
    if (!this.dicBreathAddPtns.ContainsKey(personal) || !this.dicBreathAddPtns[personal].ContainsKey(mode) || !this.dicBreathAddPtns[personal][mode].ContainsKey(kind))
      return false;
    ValueDictionary<int, int, string, HVoiceCtrl.BreathPtn> valueDictionary2 = this.dicBreathAddPtns[personal][mode][kind];
    foreach (KeyValuePair<int, ValueDictionary<int, string, HVoiceCtrl.BreathPtn>> keyValuePair1 in (Dictionary<int, ValueDictionary<int, string, HVoiceCtrl.BreathPtn>>) valueDictionary1)
    {
      if (valueDictionary2.ContainsKey(keyValuePair1.Key))
      {
        foreach (KeyValuePair<int, ValueDictionary<string, HVoiceCtrl.BreathPtn>> keyValuePair2 in (Dictionary<int, ValueDictionary<string, HVoiceCtrl.BreathPtn>>) keyValuePair1.Value)
        {
          if (valueDictionary2[keyValuePair1.Key].ContainsKey(keyValuePair2.Key))
          {
            foreach (KeyValuePair<string, HVoiceCtrl.BreathPtn> keyValuePair3 in (Dictionary<string, HVoiceCtrl.BreathPtn>) keyValuePair2.Value)
            {
              if (valueDictionary2[keyValuePair1.Key][keyValuePair2.Key].ContainsKey(keyValuePair3.Key))
                keyValuePair3.Value.lstInfo.AddRange((IEnumerable<HVoiceCtrl.BreathVoicePtnInfo>) valueDictionary2[keyValuePair1.Key][keyValuePair2.Key][keyValuePair3.Key].lstInfo);
            }
          }
        }
      }
    }
    return true;
  }

  [DebuggerHidden]
  private IEnumerator LoadVoiceList(
    string _pathAssetFolder,
    bool musterbation = false,
    bool les = false)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HVoiceCtrl.\u003CLoadVoiceList\u003Ec__Iterator3()
    {
      musterbation = musterbation,
      les = les,
      _pathAssetFolder = _pathAssetFolder,
      \u0024this = this
    };
  }

  private bool LoadVoice(
    int _personality,
    int _mode,
    int _kind,
    string _pathAssetFolder,
    int _famale)
  {
    this.sbLoadFile.Clear();
    this.sbLoadFile.AppendFormat("HVoiceFace_{0:00}_{1:00}_{2:00}", (object) _personality, (object) _mode, (object) _kind);
    this.lst.Clear();
    GlobalMethod.LoadAllListTextFromList(_pathAssetFolder, this.sbLoadFile.ToString(), ref this.lst, (List<string>) null);
    foreach (string text in this.lst)
    {
      if (!(text == string.Empty))
      {
        string[][] data;
        GlobalMethod.GetListString(text, out data);
        int length = data.Length;
        int index = 0;
        int result = 0;
        if (int.TryParse(data[index][0], out result))
        {
          int line = index + 1;
          if (result != 0 && line + result == length)
          {
            if (!this.dicdiclstVoiceList[_famale].ContainsKey(_mode))
            {
              this.dicdiclstVoiceList[_famale].Add(_mode, new Dictionary<int, HVoiceCtrl.VoiceList>());
              this.dicdiclstVoiceList[_famale][_mode].Add(_kind, new HVoiceCtrl.VoiceList());
            }
            else if (!this.dicdiclstVoiceList[_famale][_mode].ContainsKey(_kind))
              this.dicdiclstVoiceList[_famale][_mode].Add(_kind, new HVoiceCtrl.VoiceList());
            HVoiceCtrl.VoiceList voiceList = this.dicdiclstVoiceList[_famale][_mode][_kind];
            this.LoadVoiceFace(data, result, ref line, voiceList.dicdicVoiceList);
            voiceList.total += result;
          }
        }
      }
    }
    return true;
  }

  private void LoadVoiceFace(
    string[][] aastr,
    int endY,
    ref int line,
    Dictionary<int, HVoiceCtrl.VoiceListInfo> dic)
  {
    int[] numArray = new int[2]{ -1, -1 };
    while (line - 1 < endY)
    {
      int length = aastr[line].Length;
      if (!int.TryParse(aastr[line][0], out numArray[0]))
        numArray[0] = numArray[1] + 1;
      HVoiceCtrl.VoiceListInfo voiceListInfo = new HVoiceCtrl.VoiceListInfo();
      voiceListInfo.pathAsset = aastr[line][1];
      voiceListInfo.nameFile = aastr[line][2];
      if (length < 4)
      {
        ++line;
        ++numArray[1];
      }
      else
      {
        voiceListInfo.notOverWrite = aastr[line][3] == "1";
        if ((length - 4) % 32 != 0)
        {
          ++line;
          ++numArray[1];
        }
        else
        {
          for (int index1 = 4; index1 < length; index1 += 32)
          {
            int num1 = 0;
            HVoiceCtrl.FaceInfo faceInfo1 = new HVoiceCtrl.FaceInfo();
            if (!aastr[line][index1 + num1].IsNullOrEmpty())
            {
              string[] strArray1 = aastr[line];
              int num2 = index1;
              int num3 = num1;
              int num4 = num3 + 1;
              int index2 = num2 + num3;
              float num5 = float.Parse(strArray1[index2]);
              if ((double) num5 >= 0.0)
              {
                faceInfo1.openEye = num5;
                HVoiceCtrl.FaceInfo faceInfo2 = faceInfo1;
                string[] strArray2 = aastr[line];
                int num6 = index1;
                int num7 = num4;
                int num8 = num7 + 1;
                int index3 = num6 + num7;
                double num9 = (double) float.Parse(strArray2[index3]);
                faceInfo2.openMouthMin = (float) num9;
                HVoiceCtrl.FaceInfo faceInfo3 = faceInfo1;
                string[] strArray3 = aastr[line];
                int num10 = index1;
                int num11 = num8;
                int num12 = num11 + 1;
                int index4 = num10 + num11;
                double num13 = (double) float.Parse(strArray3[index4]);
                faceInfo3.openMouthMax = (float) num13;
                HVoiceCtrl.FaceInfo faceInfo4 = faceInfo1;
                string[] strArray4 = aastr[line];
                int num14 = index1;
                int num15 = num12;
                int num16 = num15 + 1;
                int index5 = num14 + num15;
                int num17 = int.Parse(strArray4[index5]);
                faceInfo4.eyeBlow = num17;
                HVoiceCtrl.FaceInfo faceInfo5 = faceInfo1;
                string[] strArray5 = aastr[line];
                int num18 = index1;
                int num19 = num16;
                int num20 = num19 + 1;
                int index6 = num18 + num19;
                int num21 = int.Parse(strArray5[index6]);
                faceInfo5.eye = num21;
                HVoiceCtrl.FaceInfo faceInfo6 = faceInfo1;
                string[] strArray6 = aastr[line];
                int num22 = index1;
                int num23 = num20;
                int num24 = num23 + 1;
                int index7 = num22 + num23;
                int num25 = int.Parse(strArray6[index7]);
                faceInfo6.mouth = num25;
                HVoiceCtrl.FaceInfo faceInfo7 = faceInfo1;
                string[] strArray7 = aastr[line];
                int num26 = index1;
                int num27 = num24;
                int num28 = num27 + 1;
                int index8 = num26 + num27;
                double num29 = (double) float.Parse(strArray7[index8]);
                faceInfo7.tear = (float) num29;
                HVoiceCtrl.FaceInfo faceInfo8 = faceInfo1;
                string[] strArray8 = aastr[line];
                int num30 = index1;
                int num31 = num28;
                int num32 = num31 + 1;
                int index9 = num30 + num31;
                double num33 = (double) float.Parse(strArray8[index9]);
                faceInfo8.cheek = (float) num33;
                HVoiceCtrl.FaceInfo faceInfo9 = faceInfo1;
                string[] strArray9 = aastr[line];
                int num34 = index1;
                int num35 = num32;
                int num36 = num35 + 1;
                int index10 = num34 + num35;
                int num37 = strArray9[index10] == "1" ? 1 : 0;
                faceInfo9.highlight = num37 != 0;
                HVoiceCtrl.FaceInfo faceInfo10 = faceInfo1;
                string[] strArray10 = aastr[line];
                int num38 = index1;
                int num39 = num36;
                int num40 = num39 + 1;
                int index11 = num38 + num39;
                int num41 = strArray10[index11] == "1" ? 1 : 0;
                faceInfo10.blink = num41 != 0;
                HVoiceCtrl.FaceInfo faceInfo11 = faceInfo1;
                string[] strArray11 = aastr[line];
                int num42 = index1;
                int num43 = num40;
                int num44 = num43 + 1;
                int index12 = num42 + num43;
                int num45 = int.Parse(strArray11[index12]);
                faceInfo11.behaviorNeckLine = num45;
                HVoiceCtrl.FaceInfo faceInfo12 = faceInfo1;
                string[] strArray12 = aastr[line];
                int num46 = index1;
                int num47 = num44;
                int num48 = num47 + 1;
                int index13 = num46 + num47;
                int num49 = int.Parse(strArray12[index13]);
                faceInfo12.behaviorEyeLine = num49;
                HVoiceCtrl.FaceInfo faceInfo13 = faceInfo1;
                string[] strArray13 = aastr[line];
                int num50 = index1;
                int num51 = num48;
                int num52 = num51 + 1;
                int index14 = num50 + num51;
                int num53 = int.Parse(strArray13[index14]);
                faceInfo13.targetNeckLine = num53;
                Vector3 zero = Vector3.get_zero();
                if (faceInfo1.targetNeckLine == 7)
                {
                  for (int index15 = 0; index15 < 2; ++index15)
                  {
                    zero = Vector3.get_zero();
                    string[] strArray14 = aastr[line];
                    int num54 = index1;
                    int num55 = num52;
                    int num56 = num55 + 1;
                    int index16 = num54 + num55;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray14[index16], (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    string[] strArray15 = aastr[line];
                    int num57 = index1;
                    int num58 = num56;
                    int num59 = num58 + 1;
                    int index17 = num57 + num58;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray15[index17], (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    string[] strArray16 = aastr[line];
                    int num60 = index1;
                    int num61 = num59;
                    num52 = num61 + 1;
                    int index18 = num60 + num61;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray16[index18], (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    faceInfo1.NeckRot[index15] = zero;
                  }
                  for (int index15 = 0; index15 < 2; ++index15)
                  {
                    zero = Vector3.get_zero();
                    string[] strArray14 = aastr[line];
                    int num54 = index1;
                    int num55 = num52;
                    int num56 = num55 + 1;
                    int index16 = num54 + num55;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray14[index16], (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    string[] strArray15 = aastr[line];
                    int num57 = index1;
                    int num58 = num56;
                    int num59 = num58 + 1;
                    int index17 = num57 + num58;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray15[index17], (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    string[] strArray16 = aastr[line];
                    int num60 = index1;
                    int num61 = num59;
                    num52 = num61 + 1;
                    int index18 = num60 + num61;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray16[index18], (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    faceInfo1.HeadRot[index15] = zero;
                  }
                }
                else
                  num52 += 12;
                HVoiceCtrl.FaceInfo faceInfo14 = faceInfo1;
                string[] strArray17 = aastr[line];
                int num62 = index1;
                int num63 = num52;
                int num64 = num63 + 1;
                int index19 = num62 + num63;
                int num65 = int.Parse(strArray17[index19]);
                faceInfo14.targetEyeLine = num65;
                if (faceInfo1.targetEyeLine == 7)
                {
                  for (int index15 = 0; index15 < 2; ++index15)
                  {
                    zero = Vector3.get_zero();
                    string[] strArray14 = aastr[line];
                    int num54 = index1;
                    int num55 = num64;
                    int num56 = num55 + 1;
                    int index16 = num54 + num55;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray14[index16], (float&) ref zero.x))
                      zero.x = (__Null) 0.0;
                    string[] strArray15 = aastr[line];
                    int num57 = index1;
                    int num58 = num56;
                    int num59 = num58 + 1;
                    int index17 = num57 + num58;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray15[index17], (float&) ref zero.y))
                      zero.y = (__Null) 0.0;
                    string[] strArray16 = aastr[line];
                    int num60 = index1;
                    int num61 = num59;
                    num64 = num61 + 1;
                    int index18 = num60 + num61;
                    // ISSUE: cast to a reference type
                    if (!float.TryParse(strArray16[index18], (float&) ref zero.z))
                      zero.z = (__Null) 0.0;
                    faceInfo1.EyeRot[index15] = zero;
                  }
                }
                else
                {
                  int num66 = num64 + 6;
                }
                voiceListInfo.lstHitFace.Add(faceInfo1);
              }
              else
                break;
            }
            else
              break;
          }
          if (!dic.ContainsKey(numArray[0]))
            dic.Add(numArray[0], voiceListInfo);
          else
            dic[numArray[0]] = voiceListInfo;
          ++line;
          ++numArray[1];
        }
      }
    }
  }

  private bool LoadVoicePtn(
    int taii,
    int kind,
    int charNum,
    string _pathAssetFolder,
    bool merchant)
  {
    this.sbLoadFile.Clear();
    if (!merchant)
      this.sbLoadFile.AppendFormat("HVoicePattern_{0:00}_{1:00}", (object) taii, (object) kind);
    else
      this.sbLoadFile.AppendFormat("HVoicePattern_-90_{0:00}_{1:00}", (object) taii, (object) kind);
    if (!this.lstLoadVoicePtn[charNum].ContainsKey(taii))
      this.lstLoadVoicePtn[charNum].Add(taii, new List<HVoiceCtrl.VoicePtn>());
    this.lst.Clear();
    GlobalMethod.LoadAllListTextFromList(_pathAssetFolder, this.sbLoadFile.ToString(), ref this.lst, (List<string>) null);
    for (int index1 = 0; index1 < this.lst.Count; ++index1)
    {
      string[][] data;
      GlobalMethod.GetListString(this.lst[index1], out data);
      int index2 = 0;
      int length1 = data.Length;
      while (index2 < length1)
      {
        int length2 = data[index2].Length;
        int result = -1;
        if (!int.TryParse(data[index2][0], out result))
        {
          ++index2;
        }
        else
        {
          HVoiceCtrl.CheckVoicePtn ptn;
          ptn.condition = result;
          if (!int.TryParse(data[index2][1], out result))
            result = -1;
          ptn.startKind = result;
          if (!int.TryParse(data[index2][2], out result))
            result = 0;
          ptn.howTalk = result;
          ptn.LookFlag = data[index2][3] == "0";
          ptn.anim = data[index2][4];
          HVoiceCtrl.VoicePtn voicePtn = this.CheckPtn(this.lstLoadVoicePtn[charNum][taii], ptn);
          if (voicePtn == null)
          {
            this.lstLoadVoicePtn[charNum][taii].Add(new HVoiceCtrl.VoicePtn());
            voicePtn = this.lstLoadVoicePtn[charNum][taii][this.lstLoadVoicePtn[charNum][taii].Count - 1];
          }
          else
            voicePtn.lstInfo.Clear();
          voicePtn.condition = ptn.condition;
          voicePtn.startKind = ptn.startKind;
          voicePtn.howTalk = ptn.howTalk;
          voicePtn.LookFlag = ptn.LookFlag;
          voicePtn.anim = ptn.anim;
          int index3 = 5;
          while (index3 < length2)
          {
            HVoiceCtrl.VoicePtnInfo voicePtnInfo1 = new HVoiceCtrl.VoicePtnInfo();
            if (!data[index2][index3].IsNullOrEmpty())
            {
              string[] strArray1 = data[index2];
              int index4 = index3;
              int index5 = index4 + 1;
              string str1 = strArray1[index4];
              char[] chArray1 = new char[1]{ ',' };
              foreach (string s in str1.Split(chArray1))
                voicePtnInfo1.lstAnimList.Add(int.Parse(s));
              if (!data[index2][index5].IsNullOrEmpty())
              {
                string[] strArray2 = data[index2];
                int index6 = index5;
                int index7 = index6 + 1;
                string str2 = strArray2[index6];
                char[] chArray2 = new char[1]{ ',' };
                foreach (string s in str2.Split(chArray2))
                  voicePtnInfo1.lstPlayConditions.Add(int.Parse(s));
                if (!data[index2][index7].IsNullOrEmpty())
                {
                  HVoiceCtrl.VoicePtnInfo voicePtnInfo2 = voicePtnInfo1;
                  string[] strArray3 = data[index2];
                  int index8 = index7;
                  int index9 = index8 + 1;
                  int num1 = int.Parse(strArray3[index8]);
                  voicePtnInfo2.loadListmode = num1;
                  if (!data[index2][index9].IsNullOrEmpty())
                  {
                    HVoiceCtrl.VoicePtnInfo voicePtnInfo3 = voicePtnInfo1;
                    string[] strArray4 = data[index2];
                    int index10 = index9;
                    int index11 = index10 + 1;
                    int num2 = int.Parse(strArray4[index10]);
                    voicePtnInfo3.loadListKind = num2;
                    if (!data[index2][index11].IsNullOrEmpty())
                    {
                      string[] strArray5 = data[index2];
                      int index12 = index11;
                      index3 = index12 + 1;
                      string str3 = strArray5[index12];
                      char[] chArray3 = new char[1]{ ',' };
                      foreach (string s in str3.Split(chArray3))
                        voicePtnInfo1.lstVoice.Add(int.Parse(s));
                      voicePtn.lstInfo.Add(voicePtnInfo1);
                    }
                    else
                      break;
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            }
            else
              break;
          }
          ++index2;
        }
      }
    }
    return true;
  }

  private HVoiceCtrl.VoicePtn CheckPtn(
    List<HVoiceCtrl.VoicePtn> ptns,
    HVoiceCtrl.CheckVoicePtn ptn)
  {
    HVoiceCtrl.VoicePtn voicePtn = (HVoiceCtrl.VoicePtn) null;
    for (int index = 0; index < ptns.Count; ++index)
    {
      HVoiceCtrl.VoicePtn ptn1 = ptns[index];
      if (ptn1 != null && ptn1.condition == ptn.condition && (ptn1.startKind == ptn.startKind && ptn1.howTalk == ptn.howTalk) && (ptn1.LookFlag == ptn.LookFlag && !(ptn1.anim != ptn.anim)))
        voicePtn = ptn1;
    }
    return voicePtn;
  }

  [DebuggerHidden]
  private IEnumerator LoadShortBreathList(string _pathAssetFolder, bool musterbation = false)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HVoiceCtrl.\u003CLoadShortBreathList\u003Ec__Iterator4()
    {
      musterbation = musterbation,
      \u0024this = this
    };
  }

  private bool LoadShortBreath(int _personality, int _main)
  {
    this.sbLoadFile.Clear();
    this.sbLoadFile.AppendFormat("HShort_breath_{0:00}", (object) _personality);
    string str1 = GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString());
    if (str1.IsNullOrEmpty())
      return false;
    string[][] data;
    GlobalMethod.GetListString(str1, out data);
    int length = data.Length;
    int num1 = length == 0 ? 0 : data[0].Length;
    for (int index1 = 0; index1 < length; ++index1)
    {
      int result1 = 0;
      int num2 = 0;
      string[] strArray1 = data[index1];
      int index2 = num2;
      int num3 = index2 + 1;
      if (int.TryParse(strArray1[index2], out result1))
      {
        if (!this.ShortBreathLists[_main].dicShortBreathLists.ContainsKey(result1))
          this.ShortBreathLists[_main].dicShortBreathLists.Add(result1, new HVoiceCtrl.VoiceListInfo());
        HVoiceCtrl.VoiceListInfo dicShortBreathList = this.ShortBreathLists[_main].dicShortBreathLists[result1];
        HVoiceCtrl.VoiceListInfo voiceListInfo1 = dicShortBreathList;
        string[] strArray2 = data[index1];
        int index3 = num3;
        int num4 = index3 + 1;
        string str2 = strArray2[index3];
        voiceListInfo1.pathAsset = str2;
        HVoiceCtrl.VoiceListInfo voiceListInfo2 = dicShortBreathList;
        string[] strArray3 = data[index1];
        int index4 = num4;
        int num5 = index4 + 1;
        string str3 = strArray3[index4];
        voiceListInfo2.nameFile = str3;
        HVoiceCtrl.VoiceListInfo voiceListInfo3 = dicShortBreathList;
        string[] strArray4 = data[index1];
        int index5 = num5;
        int num6 = index5 + 1;
        int num7 = strArray4[index5] == "1" ? 1 : 0;
        voiceListInfo3.notOverWrite = num7 != 0;
        dicShortBreathList.lstHitFace.Clear();
        for (int index6 = num6; index6 < num1; index6 += 10)
        {
          int num8 = 0;
          HVoiceCtrl.FaceInfo faceInfo1 = new HVoiceCtrl.FaceInfo();
          string[] strArray5 = data[index1];
          int num9 = index6;
          int num10 = num8;
          int num11 = num10 + 1;
          int index7 = num9 + num10;
          float result2;
          if (float.TryParse(strArray5[index7], out result2) && (double) result2 >= 0.0)
          {
            faceInfo1.openEye = result2;
            HVoiceCtrl.FaceInfo faceInfo2 = faceInfo1;
            string[] strArray6 = data[index1];
            int num12 = index6;
            int num13 = num11;
            int num14 = num13 + 1;
            int index8 = num12 + num13;
            double num15 = (double) float.Parse(strArray6[index8]);
            faceInfo2.openMouthMin = (float) num15;
            HVoiceCtrl.FaceInfo faceInfo3 = faceInfo1;
            string[] strArray7 = data[index1];
            int num16 = index6;
            int num17 = num14;
            int num18 = num17 + 1;
            int index9 = num16 + num17;
            double num19 = (double) float.Parse(strArray7[index9]);
            faceInfo3.openMouthMax = (float) num19;
            HVoiceCtrl.FaceInfo faceInfo4 = faceInfo1;
            string[] strArray8 = data[index1];
            int num20 = index6;
            int num21 = num18;
            int num22 = num21 + 1;
            int index10 = num20 + num21;
            int num23 = int.Parse(strArray8[index10]);
            faceInfo4.eyeBlow = num23;
            HVoiceCtrl.FaceInfo faceInfo5 = faceInfo1;
            string[] strArray9 = data[index1];
            int num24 = index6;
            int num25 = num22;
            int num26 = num25 + 1;
            int index11 = num24 + num25;
            int num27 = int.Parse(strArray9[index11]);
            faceInfo5.eye = num27;
            HVoiceCtrl.FaceInfo faceInfo6 = faceInfo1;
            string[] strArray10 = data[index1];
            int num28 = index6;
            int num29 = num26;
            int num30 = num29 + 1;
            int index12 = num28 + num29;
            int num31 = int.Parse(strArray10[index12]);
            faceInfo6.mouth = num31;
            HVoiceCtrl.FaceInfo faceInfo7 = faceInfo1;
            string[] strArray11 = data[index1];
            int num32 = index6;
            int num33 = num30;
            int num34 = num33 + 1;
            int index13 = num32 + num33;
            double num35 = (double) float.Parse(strArray11[index13]);
            faceInfo7.tear = (float) num35;
            HVoiceCtrl.FaceInfo faceInfo8 = faceInfo1;
            string[] strArray12 = data[index1];
            int num36 = index6;
            int num37 = num34;
            int num38 = num37 + 1;
            int index14 = num36 + num37;
            double num39 = (double) float.Parse(strArray12[index14]);
            faceInfo8.cheek = (float) num39;
            HVoiceCtrl.FaceInfo faceInfo9 = faceInfo1;
            string[] strArray13 = data[index1];
            int num40 = index6;
            int num41 = num38;
            int num42 = num41 + 1;
            int index15 = num40 + num41;
            int num43 = strArray13[index15] == "1" ? 1 : 0;
            faceInfo9.highlight = num43 != 0;
            HVoiceCtrl.FaceInfo faceInfo10 = faceInfo1;
            string[] strArray14 = data[index1];
            int num44 = index6;
            int num45 = num42;
            int num46 = num45 + 1;
            int index16 = num44 + num45;
            int num47 = strArray14[index16] == "1" ? 1 : 0;
            faceInfo10.blink = num47 != 0;
            dicShortBreathList.lstHitFace.Add(faceInfo1);
          }
          else
            break;
        }
      }
    }
    return true;
  }

  private bool LoadShortBreathPtn(int _main, int _mode, int _kind)
  {
    this.sbLoadFile.Clear();
    this.sbLoadFile.AppendFormat("HShortBreathPattern_{0:00}_{1:00}_{2:00}", (object) _main, (object) _mode, (object) _kind);
    string str1 = GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString());
    if (str1.IsNullOrEmpty())
      return false;
    string[][] data;
    GlobalMethod.GetListString(str1, out data);
    int length = data.Length;
    int num1 = length == 0 ? 0 : data[0].Length;
    ValueDictionary<int, int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> dicInfo = this.shortBreathPtns[_main].dicInfo;
    for (int index1 = 0; index1 < length; ++index1)
    {
      if (!dicInfo.ContainsKey(_mode))
        dicInfo[_mode] = dicInfo.New<int, int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
      if (!dicInfo[_mode].ContainsKey(_kind))
        dicInfo[_mode][_kind] = dicInfo[_mode].New<int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
      int num2 = 0;
      string[] strArray1 = data[index1];
      int index2 = num2;
      int num3 = index2 + 1;
      int key1 = int.Parse(strArray1[index2]);
      if (!dicInfo[_mode][_kind].ContainsKey(key1))
        dicInfo[_mode][_kind][key1] = dicInfo[_mode][_kind].New<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
      string[] strArray2 = data[index1];
      int index3 = num3;
      int num4 = index3 + 1;
      int key2 = int.Parse(strArray2[index3]);
      if (!dicInfo[_mode][_kind][key1].ContainsKey(key2))
        dicInfo[_mode][_kind][key1][key2] = new List<HVoiceCtrl.BreathVoicePtnInfo>();
      dicInfo[_mode][_kind][key1][key2].Clear();
      for (int index4 = num4; index4 < num1; index4 += 3)
      {
        string str2 = data[index1][index4];
        if (!(str2 == string.Empty))
        {
          HVoiceCtrl.BreathVoicePtnInfo breathVoicePtnInfo = new HVoiceCtrl.BreathVoicePtnInfo();
          string[] strArray3 = str2.Split(',');
          int result = 0;
          for (int index5 = 0; index5 < strArray3.Length; ++index5)
          {
            if (int.TryParse(strArray3[index5], out result))
              breathVoicePtnInfo.lstConditions.Add(result);
          }
          string str3 = data[index1][index4 + 1];
          char[] chArray1 = new char[1]{ ',' };
          foreach (string s in str3.Split(chArray1))
          {
            if (int.TryParse(s, out result))
              breathVoicePtnInfo.lstAnimeID.Add(result);
          }
          string str4 = data[index1][index4 + 2];
          char[] chArray2 = new char[1]{ ',' };
          foreach (string s in str4.Split(chArray2))
          {
            if (int.TryParse(s, out result))
              breathVoicePtnInfo.lstVoice.Add(result);
          }
          dicInfo[_mode][_kind][key1][key2].Add(breathVoicePtnInfo);
        }
        else
          break;
      }
    }
    return true;
  }

  private bool LoadShortBreathAddPtn(int _main, int _mode, int _kind)
  {
    for (int index1 = 0; index1 < 50; ++index1)
    {
      this.sbLoadFile.Clear();
      this.sbLoadFile.AppendFormat("HShortBreathPattern_{0:00}_{1:00}_{2:00}_{3:00}", (object) _main, (object) _mode, (object) _kind, (object) index1);
      string str1 = GlobalMethod.LoadAllListText(this.lstBreathAbnames, this.sbLoadFile.ToString());
      if (!str1.IsNullOrEmpty())
      {
        string[][] data;
        GlobalMethod.GetListString(str1, out data);
        int length = data.Length;
        int num1 = length == 0 ? 0 : data[0].Length;
        ValueDictionary<int, int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> dicInfo = this.shortBreathAddPtns[_main].dicInfo;
        for (int index2 = 0; index2 < length; ++index2)
        {
          if (!dicInfo.ContainsKey(_mode))
            dicInfo[_mode] = dicInfo.New<int, int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
          if (!dicInfo[_mode].ContainsKey(_kind))
            dicInfo[_mode][_kind] = dicInfo[_mode].New<int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
          int num2 = 0;
          string[] strArray1 = data[index2];
          int index3 = num2;
          int num3 = index3 + 1;
          int key1 = int.Parse(strArray1[index3]);
          if (!dicInfo[_mode][_kind].ContainsKey(key1))
            dicInfo[_mode][_kind][key1] = dicInfo[_mode][_kind].New<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
          string[] strArray2 = data[index2];
          int index4 = num3;
          int num4 = index4 + 1;
          int key2 = int.Parse(strArray2[index4]);
          if (!dicInfo[_mode][_kind][key1].ContainsKey(key2))
            dicInfo[_mode][_kind][key1][key2] = new List<HVoiceCtrl.BreathVoicePtnInfo>();
          for (int index5 = num4; index5 < num1; index5 += 3)
          {
            string str2 = data[index2][index5];
            if (!(str2 == string.Empty))
            {
              HVoiceCtrl.BreathVoicePtnInfo bvi = new HVoiceCtrl.BreathVoicePtnInfo();
              string[] strArray3 = str2.Split(',');
              int result = 0;
              for (int index6 = 0; index6 < strArray3.Length; ++index6)
              {
                if (int.TryParse(strArray3[index6], out result))
                  bvi.lstConditions.Add(result);
              }
              string str3 = data[index2][index5 + 1];
              char[] chArray1 = new char[1]{ ',' };
              foreach (string s in str3.Split(chArray1))
              {
                if (int.TryParse(s, out result))
                  bvi.lstAnimeID.Add(result);
              }
              string str4 = data[index2][index5 + 2];
              char[] chArray2 = new char[1]{ ',' };
              foreach (string s in str4.Split(chArray2))
              {
                if (int.TryParse(s, out result))
                  bvi.lstVoice.Add(result);
              }
              HVoiceCtrl.BreathVoicePtnInfo breathVoicePtnInfo = dicInfo[_mode][_kind][key1][key2].Find((Predicate<HVoiceCtrl.BreathVoicePtnInfo>) (f => f.lstConditions.SequenceEqual<int>((IEnumerable<int>) bvi.lstConditions) && f.lstAnimeID.SequenceEqual<int>((IEnumerable<int>) bvi.lstAnimeID)));
              if (breathVoicePtnInfo == null)
                dicInfo[_mode][_kind][key1][key2].Add(bvi);
              else
                breathVoicePtnInfo.lstVoice = new List<int>((IEnumerable<int>) bvi.lstVoice);
            }
            else
              break;
          }
        }
      }
    }
    this.LoadShortBreathPtnComposition(_main, _mode, _kind);
    return true;
  }

  private bool LoadShortBreathPtnComposition(int _main, int _mode, int _kind)
  {
    if (!this.shortBreathPtns[_main].dicInfo.ContainsKey(_mode) || !this.shortBreathPtns[_main].dicInfo[_mode].ContainsKey(_kind))
      return false;
    ValueDictionary<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> valueDictionary1 = this.shortBreathPtns[_main].dicInfo[_mode][_kind];
    if (!this.shortBreathAddPtns[_main].dicInfo.ContainsKey(_mode) || !this.shortBreathAddPtns[_main].dicInfo[_mode].ContainsKey(_kind))
      return false;
    ValueDictionary<int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> valueDictionary2 = this.shortBreathAddPtns[_main].dicInfo[_mode][_kind];
    foreach (KeyValuePair<int, ValueDictionary<int, List<HVoiceCtrl.BreathVoicePtnInfo>>> keyValuePair1 in (Dictionary<int, ValueDictionary<int, List<HVoiceCtrl.BreathVoicePtnInfo>>>) valueDictionary1)
    {
      if (valueDictionary2.ContainsKey(keyValuePair1.Key))
      {
        foreach (KeyValuePair<int, List<HVoiceCtrl.BreathVoicePtnInfo>> keyValuePair2 in (Dictionary<int, List<HVoiceCtrl.BreathVoicePtnInfo>>) keyValuePair1.Value)
        {
          if (valueDictionary2[keyValuePair1.Key].ContainsKey(keyValuePair2.Key))
            keyValuePair2.Value.AddRange((IEnumerable<HVoiceCtrl.BreathVoicePtnInfo>) valueDictionary2[keyValuePair1.Key][keyValuePair2.Key]);
        }
      }
    }
    return true;
  }

  [DebuggerHidden]
  private IEnumerator LoadStartVoicePtnList(
    int charNum,
    string _pathAssetFolder,
    bool merchant)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HVoiceCtrl.\u003CLoadStartVoicePtnList\u003Ec__Iterator5()
    {
      merchant = merchant,
      _pathAssetFolder = _pathAssetFolder,
      charNum = charNum,
      \u0024this = this
    };
  }

  [Serializable]
  public class FaceInfo
  {
    [RangeLabel("目の開き", 0.0f, 1f)]
    public float openEye = 10f;
    [RangeLabel("口の開き最大", 0.0f, 1f)]
    public float openMouthMax = 1f;
    [Label("視線角度")]
    public Vector3[] EyeRot = new Vector3[2];
    [Label("首角度")]
    public Vector3[] NeckRot = new Vector3[2];
    [Label("頭角度")]
    public Vector3[] HeadRot = new Vector3[2];
    [RangeLabel("口の開き最小", 0.0f, 1f)]
    public float openMouthMin;
    [Label("眉の形")]
    public int eyeBlow;
    [Label("目の形")]
    public int eye;
    [Label("口の形")]
    public int mouth;
    [RangeLabel("涙", 0.0f, 1f)]
    public float tear;
    [RangeLabel("頬赤", 0.0f, 1f)]
    public float cheek;
    [Label("ハイライト")]
    public bool highlight;
    [Label("瞬き")]
    public bool blink;
    [Label("首挙動")]
    public int behaviorNeckLine;
    [Label("目挙動")]
    public int behaviorEyeLine;
    [Label("首タゲ")]
    public int targetNeckLine;
    [Label("目タゲ")]
    public int targetEyeLine;
  }

  [Serializable]
  public class VoiceListInfo
  {
    [Label("ファイル名")]
    public string nameFile = string.Empty;
    [Label("アセットバンドルパス")]
    public string pathAsset = string.Empty;
    [Label("呼吸グループ(呼吸時のみ)")]
    public int group = -1;
    [Label("アタリにあたっていない")]
    public List<HVoiceCtrl.FaceInfo> lstNotHitFace = new List<HVoiceCtrl.FaceInfo>();
    [Label("アタリにあたっている")]
    public List<HVoiceCtrl.FaceInfo> lstHitFace = new List<HVoiceCtrl.FaceInfo>();
    [Label("上書き禁止フラグ")]
    public bool notOverWrite;
    [Label("喋った(セリフ時のみ)")]
    public bool isPlay;
    [Label("おしっこセリフ(呼吸時のみ)")]
    public bool urine;
  }

  [Serializable]
  public class BreathList
  {
    public Dictionary<int, HVoiceCtrl.VoiceListInfo> lstVoiceList = new Dictionary<int, HVoiceCtrl.VoiceListInfo>();
    [SerializeField]
    private List<HVoiceCtrl.BreathList.InspectorBreathList> debugList = new List<HVoiceCtrl.BreathList.InspectorBreathList>();

    public void DebugListSet()
    {
      foreach (KeyValuePair<int, HVoiceCtrl.VoiceListInfo> lstVoice in this.lstVoiceList)
        this.debugList.Add(new HVoiceCtrl.BreathList.InspectorBreathList()
        {
          key = lstVoice.Key,
          value = lstVoice.Value
        });
    }

    [Serializable]
    private struct InspectorBreathList
    {
      public int key;
      public HVoiceCtrl.VoiceListInfo value;
    }
  }

  [Serializable]
  public class VoiceList
  {
    public Dictionary<int, HVoiceCtrl.VoiceListInfo> dicdicVoiceList = new Dictionary<int, HVoiceCtrl.VoiceListInfo>();
    public int total;
  }

  [Serializable]
  public struct PlayVoiceinfo
  {
    public int mode;
    public int kind;
    public int voiceID;
  }

  [Serializable]
  public class ShortVoiceList
  {
    public Dictionary<int, HVoiceCtrl.VoiceListInfo> dicShortBreathLists = new Dictionary<int, HVoiceCtrl.VoiceListInfo>();
  }

  [Serializable]
  public class BreathVoicePtnInfo
  {
    public List<int> lstConditions = new List<int>();
    public List<int> lstVoice = new List<int>();
    public List<int> lstAnimeID = new List<int>();
  }

  [Serializable]
  public class BreathPtn
  {
    [Label("アニメーション名")]
    public string anim = string.Empty;
    [Label("表情変更時間最小")]
    public float timeChangeFaceMin = 5f;
    [Label("表情変更時間最題")]
    public float timeChangeFaceMax = 5f;
    public List<HVoiceCtrl.BreathVoicePtnInfo> lstInfo = new List<HVoiceCtrl.BreathVoicePtnInfo>();
    [Label("段階")]
    public int level;
    [Label("ループ中1回")]
    public bool onlyOne;
    [Label("ループ中1回がtrueのとき使用")]
    public bool isPlay;
    [Label("強制フラグ")]
    public bool force;
  }

  [Serializable]
  public class VoicePtnInfo
  {
    public List<int> lstAnimList = new List<int>();
    public List<int> lstPlayConditions = new List<int>();
    public int loadListmode = -1;
    public int loadListKind = -1;
    public List<int> lstVoice = new List<int>();
  }

  [Serializable]
  public class VoicePtn
  {
    [Label("アニメーション名")]
    public string anim = string.Empty;
    [Label("キャラの状態")]
    public int condition = -1;
    [Label("開始パターン")]
    public int startKind = -1;
    [Label("外部フラグ見る")]
    public bool LookFlag = true;
    public List<HVoiceCtrl.VoicePtnInfo> lstInfo = new List<HVoiceCtrl.VoicePtnInfo>();
    [Label("掛け合いか")]
    public int howTalk;
  }

  public struct CheckVoicePtn
  {
    [Label("アニメーション名")]
    public string anim;
    [Label("キャラの状態")]
    public int condition;
    [Label("開始パターン")]
    public int startKind;
    [Label("掛け合いか")]
    public int howTalk;
    [Label("外部フラグ見る")]
    public bool LookFlag;
  }

  [Serializable]
  public class StartVoicePtn
  {
    [Label("キャラの状態")]
    public int condition = -1;
    [Label("どの体位の開始か")]
    public int nTaii = -1;
    [Label("タイミング名")]
    public string anim = string.Empty;
    public List<HVoiceCtrl.VoicePtnInfo> lstInfo = new List<HVoiceCtrl.VoicePtnInfo>();
    [Label("襲う種類")]
    public int nForce;
    [Label("タイミング")]
    public int timing;
  }

  [Serializable]
  public class ShortBreathPtn
  {
    public ValueDictionary<int, int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>> dicInfo = new ValueDictionary<int, int, int, int, List<HVoiceCtrl.BreathVoicePtnInfo>>();
  }

  [Serializable]
  public class VoiceAnimationPlayInfo
  {
    [Label("再生した")]
    public bool[] isPlays = new bool[2];
    [Label("アニメーション名(ハッシュ値)")]
    public int animationHash;
  }

  [Serializable]
  public class VoiceAnimationPlay
  {
    public List<HVoiceCtrl.VoiceAnimationPlayInfo> lstPlayInfo = new List<HVoiceCtrl.VoiceAnimationPlayInfo>();
    [Label("モーションの再生回数")]
    public int Count;

    public void SetAllFlags(bool _play)
    {
      for (int index = 0; index < this.lstPlayInfo.Count; ++index)
      {
        this.lstPlayInfo[index].isPlays[0] = _play;
        this.lstPlayInfo[index].isPlays[1] = _play;
      }
    }

    public void AfterFinish()
    {
      for (int index = 0; index < this.lstPlayInfo.Count; ++index)
      {
        if (this.lstPlayInfo[index].isPlays[0] || this.lstPlayInfo[index].isPlays[1])
        {
          this.lstPlayInfo[index].isPlays[0] = false;
          this.lstPlayInfo[index].isPlays[1] = false;
        }
      }
      ++this.Count;
    }

    public HVoiceCtrl.VoiceAnimationPlayInfo GetAnimation(int _animHash)
    {
      for (int index = 0; index < this.lstPlayInfo.Count; ++index)
      {
        if (_animHash == this.lstPlayInfo[index].animationHash)
          return this.lstPlayInfo[index];
      }
      return (HVoiceCtrl.VoiceAnimationPlayInfo) null;
    }
  }

  public enum VoiceKind
  {
    breath,
    breathShort,
    voice,
    startVoice,
    none,
  }

  [Serializable]
  public class Voice
  {
    public HVoiceCtrl.VoiceKind state = HVoiceCtrl.VoiceKind.none;
    [Label("呼吸グループ")]
    public int breathGroup = -1;
    public HVoiceCtrl.FaceInfo Face = new HVoiceCtrl.FaceInfo();
    [Header("呼吸")]
    public HVoiceCtrl.VoiceListInfo breathInfo;
    [Label("呼吸リストの配列番号")]
    public int arrayBreath;
    [Label("呼吸アニメーションステート")]
    public string animBreath;
    [Label("速い？")]
    public bool speedStateFast;
    [Label("表情変化経過時間")]
    public float timeFaceDelta;
    [Label("表情変化時間")]
    public float timeFace;
    [Label("当たってる？")]
    public bool isGaugeHit;
    [Header("セリフ")]
    public HVoiceCtrl.VoiceListInfo voiceInfo;
    [Label("セリフリストの配列番号")]
    public int arrayVoice;
    [Label("セリフリストの番号")]
    public int VoiceListID;
    [Label("セリフリストのシート番号")]
    public int VoiceListSheetID;
    [Label("セリフアニメーションステート")]
    public string animVoice;
    [Header("短い喘ぎ")]
    public HVoiceCtrl.VoiceListInfo shortInfo;
    [Label("短い喘ぎの配列番号")]
    public int arrayShort;
    [Header("共通")]
    [Label("上書き禁止")]
    public bool notOverWrite;
  }
}
