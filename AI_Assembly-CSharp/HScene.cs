// Decompiled with JetBrains decompiler
// Type: HScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using AIChara;
using AIProject;
using AIProject.Animal;
using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.Scene;
using Cinemachine;
using ConfigScene;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEx;

public class HScene : BaseLoader
{
  public HitObjectCtrl[] ctrlHitObjectFemales = new HitObjectCtrl[2];
  public HitObjectCtrl[] ctrlHitObjectMales = new HitObjectCtrl[2];
  public HScene.LightInfo[] infoLight = new HScene.LightInfo[2]
  {
    new HScene.LightInfo(),
    new HScene.LightInfo()
  };
  private ChaControl[] chaFemales = new ChaControl[2];
  private ChaControl[] chaMales = new ChaControl[2];
  private Transform[] chaFemalesTrans = new Transform[2];
  private Transform[] chaMalesTrans = new Transform[2];
  private List<Tuple<int, int, MotionIK>> lstMotionIK = new List<Tuple<int, int, MotionIK>>();
  private int mode = -1;
  private int modeCtrl = -1;
  private List<ProcBase> lstProc = new List<ProcBase>();
  private DynamicBoneReferenceCtrl[] ctrlDynamics = new DynamicBoneReferenceCtrl[2];
  private List<string> abName = new List<string>();
  private List<HScene.AnimationListInfo>[] lstAnimInfo = new List<HScene.AnimationListInfo>[6]
  {
    new List<HScene.AnimationListInfo>(),
    new List<HScene.AnimationListInfo>(),
    new List<HScene.AnimationListInfo>(),
    new List<HScene.AnimationListInfo>(),
    new List<HScene.AnimationListInfo>(),
    new List<HScene.AnimationListInfo>()
  };
  private HScene.AnimationListInfo aInfo = new HScene.AnimationListInfo();
  public HScene.AnimationListInfo StartAnimInfo = new HScene.AnimationListInfo();
  private RuntimeAnimatorController[,] runtimeAnimatorControllers = new RuntimeAnimatorController[2, 3];
  private bool nullPlayer = true;
  private StringBuilder sbLoadFileName = new StringBuilder();
  private Dictionary<int, float> initStandNip = new Dictionary<int, float>();
  [Tooltip("Hメッシュの判定を机にするかの基準")]
  public List<HScene.DeskChairInfo> deskChairInfos = new List<HScene.DeskChairInfo>();
  private Dictionary<string, RuntimeAnimatorController> racEtcM = new Dictionary<string, RuntimeAnimatorController>();
  private Dictionary<string, RuntimeAnimatorController> racEtcF = new Dictionary<string, RuntimeAnimatorController>();
  private HashSet<string> hashUseAssetBundleAnimator = new HashSet<string>();
  private bool prevBeforeWait = true;
  public HSceneFlagCtrl ctrlFlag;
  public GameObject objMetaBallBase;
  public GameObject objGrondInstantiate;
  public H_Lookat_dan[] ctrlLookAts;
  public YureCtrl[] ctrlYures;
  public YureCtrlMale ctrlYureMale;
  public HLayerCtrl ctrlLayer;
  public HAutoCtrl ctrlAuto;
  public CollisionCtrl[] ctrlMaleCollisionCtrls;
  public CollisionCtrl[] ctrlFemaleCollisionCtrls;
  public HVoiceCtrl ctrlVoice;
  public HMotionEyeNeckFemale[] ctrlEyeNeckFemale;
  public HMotionEyeNeckLesPlayer hMotionEyeNeckLesP;
  public HMotionEyeNeckMale[] ctrlEyeNeckMale;
  public SiruPasteCtrl[] ctrlSiruPastes;
  public ParticleSystem AtariEffect;
  public ParticleSystem FeelHitEffect3D;
  public Vector3 FeelHitEffect3DOffSet;
  public HPointCtrl hPointCtrl;
  private HParticleCtrl ctrlParitcle;
  private HSceneSprite sprite;
  [SerializeField]
  private CrossFade fade;
  private MetaballCtrl ctrlMeta;
  private HItemCtrl ctrlItem;
  private FeelHit ctrlFeelHit;
  private bool isSyncFirstStep;
  private HSeCtrl ctrlSE;
  private GameObject objGrondCollision;
  private bool isTuyaOn;
  public bool chaFemaleLoaded;
  public Transform StartPos;
  private bool isSetStartPos;
  private HScene.StartMotion autoMotion;
  private HSceneManager hSceneManager;
  private ActorCameraControl Camera;
  private bool nowStart;
  public bool NowStateIsEnd;
  private bool nowChangeAnim;
  private Vector3 distanceToContoroler;
  private HSceneSpriteHitem HItemDrag;
  public Transform hitemPlace;
  public Transform hParticlePlace;
  public Transform hitobjPlace;
  private RuntimeAnimatorController[] racM;
  private RuntimeAnimatorController[] racF;
  private RuntimeAnimatorController[] HoushiRacM;
  private RuntimeAnimatorController[] HoushiRacF;
  private bool _bareYobai;
  private bool[] prevCharaEntry;
  private bool useLotion;
  private HScene.PackData packData;
  private RuntimeAnimatorController racBeforeWait;
  private StartWaitAnim startWait;
  private Dictionary<int, int> preBeforWaitState;

  public HParticleCtrl CtrlParticle
  {
    get
    {
      if (this.ctrlParitcle == null)
        this.ctrlParitcle = !Singleton<Resources>.IsInstance() ? (HParticleCtrl) null : Singleton<Resources>.Instance.HSceneTable.hParticle;
      return this.ctrlParitcle;
    }
  }

  public bool NowChangeAnim
  {
    get
    {
      return this.nowChangeAnim;
    }
  }

  private OpenData openData { get; } = new OpenData();

  [DebuggerHidden]
  public IEnumerator InitCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HScene.\u003CInitCoroutine\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void Update()
  {
    if (this.ctrlFlag.BeforeHWait)
    {
      if (!this.hSceneManager.isForce)
      {
        this.ctrlVoice.HBeforeHouchiTime += Time.get_unscaledDeltaTime();
        if ((double) this.ctrlVoice.HBeforeHouchiTime >= (double) this.ctrlFlag.HBeforeHouchiTime)
          this.ctrlVoice.HBeforeProc(this.chaFemales);
      }
      this.ShortcutKey();
    }
    else
    {
      if (this.aInfo != this.ctrlFlag.nowAnimationInfo)
        this.aInfo = this.ctrlFlag.nowAnimationInfo;
      if (this.ctrlFlag.cameraCtrl.isConfigTargetTex != Manager.Config.ActData.Look)
        this.ctrlFlag.cameraCtrl.isConfigTargetTex = Manager.Config.ActData.Look;
      if ((double) this.ctrlFlag.rateNip < (double) this.ctrlFlag.feel_f)
        this.ctrlFlag.rateNip = this.ctrlFlag.feel_f;
      float rate = Mathf.Lerp(0.0f, this.ctrlFlag.rateNipMax, this.ctrlFlag.rateNip);
      bool gloss = Manager.Config.HData.Gloss;
      if ((double) this.ctrlFlag.rateTuya < (double) this.ctrlFlag.feel_f && gloss)
        this.ctrlFlag.rateTuya = this.ctrlFlag.feel_f;
      this.PlayerWet();
      if (!this.useLotion)
        this.useLotion = this.HItemDrag.Effect(7);
      if ((double) this.ctrlFlag.rateTuya < 1.0 && this.useLotion && gloss)
      {
        this.ctrlFlag.rateTuya = 1f;
        this.HItemDrag.SetUse(7, false);
      }
      float num = !this.isTuyaOn ? this.ctrlFlag.rateTuya : 1f;
      if (!gloss)
        num = 0.0f;
      for (int index = 0; index < this.chaFemales.Length && (!Object.op_Equality((Object) this.chaFemales[index], (Object) null) && !Object.op_Equality((Object) this.chaFemales[index].objTop, (Object) null)); ++index)
      {
        if (!Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) || !Object.op_Equality((Object) this.hSceneManager.Player.ChaControl, (Object) this.chaFemales[index]))
        {
          this.chaFemales[index].ChangeNipRate(rate);
          this.chaFemales[index].skinGlossRate = num;
        }
      }
      if (this.HItemDrag.Effect(3))
      {
        this.hSceneManager.Toilet = 70f;
        this.HItemDrag.SetUse(3, false);
      }
      if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.SceneEnd)
        this.EndProc();
      if (this.NowStateIsEnd)
        return;
      AnimatorStateInfo animatorStateInfo = this.chaFemales[0].getAnimatorStateInfo(0);
      this.ctrlVoice.Proc(animatorStateInfo, this.chaFemales);
      this.ctrlSiruPastes[0].Proc(animatorStateInfo);
      if (this.aInfo.nPromiscuity >= 1)
        this.ctrlSiruPastes[1].Proc(animatorStateInfo);
      if (this.mode != -1 && this.modeCtrl != -1 && ProcBase.endInit)
        this.lstProc[this.mode].Proc(this.modeCtrl, this.aInfo);
      bool isAutoAutoLeaveItToYou = false;
      if (this.ctrlFlag.click == HSceneFlagCtrl.ClickKind.LeaveItToYou)
      {
        this.ctrlFlag.initiative = this.ctrlFlag.initiative != 0 ? 0 : 1;
        this.ctrlFlag.isAutoActionChange = false;
        this.sprite.MainCategoryOfLeaveItToYou(this.ctrlFlag.initiative != 0);
        this.ctrlAuto.Reset();
        this.ctrlAuto.AutoAutoLeaveItToYouInit();
        if (this.ctrlFlag.initiative != 0)
        {
          this.GetAutoAnimation(false);
          if (this.ctrlFlag.selectAnimationListInfo == null)
            this.GetAutoAnimation(true);
          ++this.ctrlFlag.numLeadFemale;
        }
        else
        {
          this.ReturnToNormalFromTheAuto();
          this.AtariEffect.Stop();
          this.FeelHitEffect3D.Stop();
        }
      }
      if (this.ctrlFlag.isAutoActionChange && this.ctrlFlag.selectAnimationListInfo == null)
      {
        this.sprite.SetMotionListDraw(false, -1);
        this.GetAutoAnimation(false);
        if (this.ctrlFlag.selectAnimationListInfo == null)
        {
          this.GetAutoAnimation(true);
          if (this.ctrlFlag.selectAnimationListInfo == null)
            this.ctrlFlag.isAutoActionChange = false;
        }
        this.ctrlAuto.SetSpeed(this.ctrlFlag.speed);
      }
      if (this.ctrlFlag.selectAnimationListInfo != null && !this.nowChangeAnim)
      {
        if (this.IsIdle(this.chaFemales[0].animBody) && !this.ctrlFlag.isFaintness && (!this.ctrlFlag.pointMoveAnimChange && this.ctrlFlag.nowAnimationInfo != this.ctrlFlag.selectAnimationListInfo))
          this.ctrlFlag.voice.playStart = 4;
        this.nowChangeAnim = true;
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (System.Action<M0>) (_ => ObservableExtensions.Subscribe<Unit>(Observable.Finally<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.ChangeAnimation(this.ctrlFlag.selectAnimationListInfo, false, isAutoAutoLeaveItToYou, !this.ctrlFlag.pointMoveAnimChange)), false), (System.Action) (() =>
        {
          if (!this.nowChangeAnim)
          {
            this.ctrlFlag.selectAnimationListInfo = (HScene.AnimationListInfo) null;
            this.ctrlFlag.isAutoActionChange = false;
          }
          if (this.ctrlFlag.pointMoveAnimChange)
            this.ctrlFlag.pointMoveAnimChange = false;
          GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, true);
          this.sprite.ChangeStart = false;
        })))));
        if (this.hSceneManager.Player.CameraControl.Mode != CameraMode.H)
        {
          this.hSceneManager.Player.CameraControl.Mode = CameraMode.H;
          ((Component) this.ctrlFlag.HBeforeCamera).get_gameObject().SetActive(false);
        }
      }
      for (int _main = 0; _main < this.ctrlHitObjectFemales.Length && (this.aInfo.nPromiscuity >= 1 || _main <= 0); ++_main)
      {
        if (!Object.op_Equality((Object) this.chaFemales[_main], (Object) null) && !Object.op_Equality((Object) this.chaFemales[_main].objBodyBone, (Object) null))
        {
          this.ctrlHitObjectFemales[_main].Proc(this.chaFemales[_main].animBody);
          bool flag = this.hSceneManager.Player.ChaControl.sex == (byte) 1 && !this.ctrlFlag.bFutanari;
          if (_main == 0 || !flag)
            this.ctrlEyeNeckFemale[_main].Proc(animatorStateInfo, this.ctrlVoice.nowVoices[_main].Face, _main);
          else
            this.hMotionEyeNeckLesP.Proc(animatorStateInfo, _main);
        }
      }
      for (int index = 0; index < this.ctrlHitObjectMales.Length; ++index)
      {
        if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && !Object.op_Equality((Object) this.chaMales[index].objTop, (Object) null))
        {
          if (this.aInfo.nPromiscuity == 0 || index <= 0)
          {
            if (!Object.op_Equality((Object) this.chaMales[index].objBodyBone, (Object) null))
            {
              this.ctrlHitObjectMales[index].Proc(this.chaMales[index].animBody);
              this.ctrlEyeNeckMale[index].Proc(animatorStateInfo);
            }
          }
          else
            break;
        }
      }
      this.ctrlDynamics[0].Proc();
      if (this.aInfo.nPromiscuity >= 1)
        this.ctrlDynamics[1].Proc();
      this.ctrlSE.Proc(animatorStateInfo, this.chaFemales);
      this.sprite.GuidProc(animatorStateInfo);
      this.ShortcutKey();
      if (!GlobalMethod.IsCameraMoveFlag(this.ctrlFlag.cameraCtrl) && !UnityEngine.Input.GetMouseButton(0) && !UnityEngine.Input.GetMouseButton(1))
        GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, true);
      this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
    }
  }

  public void SetStartAnimationInfo(HSceneManager.HEvent hEvent, int mode = -1)
  {
    this.StartAnimInfo = (HScene.AnimationListInfo) null;
    List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> valueTupleList = new List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>>();
    List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> lstStartAnimInfo = Singleton<Resources>.Instance.HSceneTable.lstStartAnimInfo;
    if (!this.hSceneManager.IsHousingHEnter)
    {
      switch (hEvent)
      {
        case HSceneManager.HEvent.Normal:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == this.hSceneManager.height)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Yobai:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == this.hSceneManager.height)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id && this.lstAnimInfo[mode][index2].bSleep)
                    {
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id && this.lstAnimInfo[mode1][index2].bSleep)
                  {
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Bath:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 11)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Toilet1:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 13)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Toilet2:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 14)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.ShagmiBare:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == this.hSceneManager.height)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Back:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == null)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Kitchen:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 9)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Tachi:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 12)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Stairs:
        case HSceneManager.HEvent.StairsBare:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 10)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.GyakuYobai:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == this.hSceneManager.height)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.FromFemale:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == this.hSceneManager.height)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.MapBath:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 1)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.KabeanaBack:
        case HSceneManager.HEvent.KabeanaFront:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 15)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.Neonani:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == null)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
        case HSceneManager.HEvent.TsukueBare:
          for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
          {
            if ((HSceneManager.HEvent) lstStartAnimInfo[index1].Item1 == hEvent && lstStartAnimInfo[index1].Item2 == 4)
            {
              if (mode != -1)
              {
                if (((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode == mode)
                {
                  int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                  for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
                  {
                    if (id == this.lstAnimInfo[mode][index2].id)
                    {
                      if (this.hSceneManager.isForce)
                      {
                        if (this.lstAnimInfo[mode][index2].nIyaAction == 0)
                          continue;
                      }
                      else if (this.lstAnimInfo[mode][index2].nIyaAction == 2)
                        continue;
                      this.StartAnimInfo = this.lstAnimInfo[mode][index2];
                      break;
                    }
                  }
                }
                else
                  continue;
              }
              else
              {
                int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
                int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
                for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
                {
                  if (id == this.lstAnimInfo[mode1][index2].id)
                  {
                    if (this.hSceneManager.isForce)
                    {
                      if (this.lstAnimInfo[mode1][index2].nIyaAction == 0)
                        continue;
                    }
                    else if (this.lstAnimInfo[mode1][index2].nIyaAction == 2)
                      continue;
                    this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
                    break;
                  }
                }
              }
              if (this.StartAnimInfo != null)
                break;
            }
          }
          break;
      }
    }
    else
    {
      for (int index1 = 0; index1 < lstStartAnimInfo.Count; ++index1)
      {
        if (lstStartAnimInfo[index1].Item2 == this.hSceneManager.height)
        {
          int id = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).id;
          int mode1 = ((HScene.StartMotion) lstStartAnimInfo[index1].Item3).mode;
          for (int index2 = 0; index2 < this.lstAnimInfo[mode1].Count; ++index2)
          {
            if (id == this.lstAnimInfo[mode1][index2].id)
            {
              this.StartAnimInfo = this.lstAnimInfo[mode1][index2];
              break;
            }
          }
          if (this.StartAnimInfo != null)
            break;
        }
      }
    }
    if (this.StartAnimInfo == null)
    {
      int index1 = mode == -1 ? 0 : mode;
      for (int index2 = 0; index2 < this.lstAnimInfo[index1].Count; ++index2)
      {
        int index3 = index2;
        if (this.lstAnimInfo[index1][index3].nPositons.Contains(this.hSceneManager.height))
        {
          this.StartAnimInfo = this.lstAnimInfo[index1][index3];
          break;
        }
      }
      if (this.StartAnimInfo == null)
        this.StartAnimInfo = this.lstAnimInfo[index1][0];
    }
    this.ChangeStartAnimation(hEvent);
  }

  private void ChangeStartAnimation(HSceneManager.HEvent hEvent)
  {
    if (Singleton<Manager.Voice>.Instance.IsVoiceCheck(this.ctrlFlag.voice.voiceTrs[0], true))
      Singleton<Manager.Voice>.Instance.Stop(this.ctrlFlag.voice.voiceTrs[0]);
    bool flag = hEvent == HSceneManager.HEvent.Normal;
    this.fade.FadeStart(1f);
    ObservableExtensions.Subscribe<Unit>(Observable.Finally<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.StartAnim(this.StartAnimInfo, 0, hEvent)), false), (System.Action) (() =>
    {
      this.ctrlFlag.voice.playStart = 1;
      GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, true);
    })));
    if (Object.op_Equality((Object) this.hSceneManager.Player.CameraControl.HCamera, (Object) null))
      this.hSceneManager.Player.CameraControl.HCamera = (CinemachineVirtualCameraBase) this.ctrlFlag.cameraCtrl;
    if (flag)
    {
      this.hSceneManager.Player.CameraControl.Mode = CameraMode.H;
      ((Component) this.ctrlFlag.HBeforeCamera).get_gameObject().SetActive(false);
    }
    Singleton<HPointCtrl>.Instance.HEnterCategory = this.StartAnimInfo.nAnimListInfoID;
  }

  public void SetStartAnimationInfoM(HSceneManager.HEvent hEvent)
  {
    List<ValueTuple<HSceneManager.HEvent, int, HScene.StartMotion>> lstStartAnimInfoM = Singleton<Resources>.Instance.HSceneTable.lstStartAnimInfoM;
    this.StartAnimInfo = (HScene.AnimationListInfo) null;
    if (this.hSceneManager.MerchantLimit == 0)
      return;
    List<HScene.AnimationListInfo> animationListInfoList = new List<HScene.AnimationListInfo>();
    for (int index1 = 0; index1 < lstStartAnimInfoM.Count; ++index1)
    {
      int mode = ((HScene.StartMotion) lstStartAnimInfoM[index1].Item3).mode;
      int id = ((HScene.StartMotion) lstStartAnimInfoM[index1].Item3).id;
      if (this.hSceneManager.MerchantLimit >= 1 && lstStartAnimInfoM[index1].Item1 == null && lstStartAnimInfoM[index1].Item2 == this.hSceneManager.height)
      {
        if (this.hSceneManager.Player.ChaControl.sex == (byte) 1 && !this.hSceneManager.bFutanari)
        {
          if (mode != 4)
            continue;
        }
        else if (this.hSceneManager.MerchantLimit == 1 && mode != 1 || Object.op_Equality((Object) this.hSceneManager.Agent[1], (Object) null) && mode == 5 || mode == 4)
          continue;
        for (int index2 = 0; index2 < this.lstAnimInfo[mode].Count; ++index2)
        {
          if (id == this.lstAnimInfo[mode][index2].id)
            animationListInfoList.Add(this.lstAnimInfo[mode][index2]);
        }
      }
    }
    ShuffleRand shuffleRand = new ShuffleRand(-1);
    shuffleRand.Init(animationListInfoList.Count);
    int index = shuffleRand.Get();
    this.StartAnimInfo = animationListInfoList[index];
    ObservableExtensions.Subscribe<Unit>(Observable.Finally<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.StartAnim(this.StartAnimInfo, 1, HSceneManager.HEvent.Normal)), false), (System.Action) (() => GlobalMethod.setCameraMoveFlag(this.ctrlFlag.cameraCtrl, true))));
    Singleton<HPointCtrl>.Instance.HEnterCategory = this.StartAnimInfo.nAnimListInfoID;
  }

  [DebuggerHidden]
  private IEnumerator StartAnim(
    HScene.AnimationListInfo StartAnimInfo,
    int mode,
    HSceneManager.HEvent hEvent = HSceneManager.HEvent.Normal)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HScene.\u003CStartAnim\u003Ec__Iterator1()
    {
      mode = mode,
      hEvent = hEvent,
      StartAnimInfo = StartAnimInfo,
      \u0024this = this
    };
  }

  private void EndProc()
  {
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
    this.NowStateIsEnd = true;
    this.sprite.ReSetLight();
    this.Camera.EnableUpdateCustomLight();
    Singleton<Sound>.Instance.Listener = ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl).get_transform();
    Singleton<Resources>.Instance.HSceneTable.HMeshObjDic[Singleton<Manager.Map>.Instance.MapID].SetActive(true);
    this.hSceneManager.CameraMesh?.SetActive(true);
    this.EndAnimChange();
    this.hSceneManager.endStatus = !this.ctrlFlag.isFaintness ? (byte) 0 : (byte) 1;
    this.chaFemales[0].SetClothesStateAll((byte) 0);
    this.chaFemales[0].SetAccessoryStateAll(true);
    for (int index = 0; index < 5; ++index)
      this.chaFemales[0].SetSiruFlag((ChaFileDefine.SiruParts) index, (byte) 0);
    if (Object.op_Inequality((Object) this.ctrlEyeNeckFemale[0], (Object) null) && Object.op_Inequality((Object) this.chaFemales[0], (Object) null) && Object.op_Inequality((Object) this.chaFemales[0].objBodyBone, (Object) null))
      this.ctrlEyeNeckFemale[0].NowEndADV = true;
    if (Object.op_Inequality((Object) this.ctrlEyeNeckFemale[1], (Object) null) && Object.op_Inequality((Object) this.chaFemales[1], (Object) null) && Object.op_Inequality((Object) this.chaFemales[1].objBodyBone, (Object) null))
    {
      if (this.hSceneManager.Player.ChaControl.sex == (byte) 0)
        this.ctrlEyeNeckFemale[1].NowEndADV = true;
      else if (this.hSceneManager.Player.ChaControl.sex == (byte) 1)
      {
        if (this.ctrlFlag.bFutanari)
          this.ctrlEyeNeckFemale[1].NowEndADV = true;
        else
          this.hMotionEyeNeckLesP.NowEndADV = true;
      }
    }
    if (Object.op_Inequality((Object) this.ctrlEyeNeckMale[0], (Object) null) && Object.op_Inequality((Object) this.chaMales[0], (Object) null) && Object.op_Inequality((Object) this.chaMales[0].objBodyBone, (Object) null))
      this.ctrlEyeNeckMale[0].NowEndADV = true;
    if (Object.op_Inequality((Object) this.ctrlEyeNeckMale[1], (Object) null) && Object.op_Inequality((Object) this.chaMales[1], (Object) null) && Object.op_Inequality((Object) this.chaMales[1].objBodyBone, (Object) null))
      this.ctrlEyeNeckMale[1].NowEndADV = true;
    using (IEnumerator<AgentActor> enumerator = Singleton<Manager.Map>.Instance.AgentTable.get_Values().GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        AgentActor current = enumerator.Current;
        if (!Object.op_Equality((Object) current, (Object) null) && this.hSceneManager.ReturnActionTypes.ContainsKey(current))
          current.EnableEntity();
      }
    }
    MerchantActor merchant = Singleton<Manager.Map>.Instance.Merchant;
    if (this.hSceneManager.bMerchant)
    {
      if (!((Behaviour) merchant.ChaControl.neckLookCtrl).get_enabled())
        ((Behaviour) merchant.ChaControl.neckLookCtrl).set_enabled(true);
      if (!((Behaviour) merchant.ChaControl.eyeLookCtrl).get_enabled())
        ((Behaviour) merchant.ChaControl.eyeLookCtrl).set_enabled(true);
      merchant.ChaControl.ChangeLookNeckPtn(3, 1f);
      merchant.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      merchant.ChaControl.ChangeLookEyesPtn(0);
      merchant.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      Singleton<Game>.Instance.GetExpression(merchant.ID, "標準")?.Change(merchant.ChaControl);
      merchant.ChaControl.ChangeMouthOpenMin(merchant.ChaControl.fileStatus.mouthOpenMin);
    }
    AnimatorStateInfo animatorStateInfo = merchant.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
    int shortNameHash = ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash();
    merchant.AnimationMerchant.SetAnimatorController(merchant.ChaControl.animBody.get_runtimeAnimatorController());
    merchant.AnimationMerchant.MapIK.Calc(shortNameHash);
    merchant.EnableEntity();
    if (Object.op_Inequality((Object) this.hSceneManager.male, (Object) null))
    {
      if (!((Behaviour) this.hSceneManager.male.ChaControl.neckLookCtrl).get_enabled())
        ((Behaviour) this.hSceneManager.male.ChaControl.neckLookCtrl).set_enabled(true);
      if (!((Behaviour) this.hSceneManager.male.ChaControl.eyeLookCtrl).get_enabled())
        ((Behaviour) this.hSceneManager.male.ChaControl.eyeLookCtrl).set_enabled(true);
      this.hSceneManager.male.ChaControl.ChangeLookNeckPtn(3, 1f);
      this.hSceneManager.male.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      this.hSceneManager.male.ChaControl.ChangeLookEyesPtn(0);
      this.hSceneManager.male.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      Singleton<Game>.Instance.GetExpression(this.hSceneManager.male.ID, "標準")?.Change(this.hSceneManager.male.ChaControl);
      this.hSceneManager.male.ChaControl.ChangeMouthOpenMin(this.hSceneManager.male.ChaControl.fileStatus.mouthOpenMin);
    }
    if (this.hSceneManager.females != null)
    {
      for (int index = 0; index < this.hSceneManager.females.Length; ++index)
      {
        if (!Object.op_Equality((Object) this.hSceneManager.females[index], (Object) null))
        {
          if (!((Behaviour) this.hSceneManager.females[index].ChaControl.neckLookCtrl).get_enabled())
            ((Behaviour) this.hSceneManager.females[index].ChaControl.neckLookCtrl).set_enabled(true);
          if (!((Behaviour) this.hSceneManager.females[index].ChaControl.eyeLookCtrl).get_enabled())
            ((Behaviour) this.hSceneManager.females[index].ChaControl.eyeLookCtrl).set_enabled(true);
          this.hSceneManager.females[index].ChaControl.ChangeLookNeckPtn(3, 1f);
          this.hSceneManager.females[index].ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
          this.hSceneManager.females[index].ChaControl.ChangeLookEyesPtn(0);
          this.hSceneManager.females[index].ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
          int personality = 0;
          if (Object.op_Implicit((Object) ((Component) this.hSceneManager.females[index]).GetComponent<PlayerActor>()))
            personality = -100;
          else if (Object.op_Equality((Object) ((Component) this.hSceneManager.females[index]).GetComponent<MerchantActor>(), (Object) null))
            personality = this.hSceneManager.females[index].ChaControl.fileParam.personality;
          Singleton<Game>.Instance.GetExpression(personality, "標準")?.Change(this.hSceneManager.females[index].ChaControl);
          this.hSceneManager.females[index].ChaControl.ChangeMouthOpenMin(this.hSceneManager.females[index].ChaControl.fileStatus.mouthOpenMin);
        }
      }
    }
    for (int index = 0; index < this.chaMales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && !Object.op_Equality((Object) this.chaMales[index].objBody, (Object) null))
        this.chaMales[index].visibleSon = false;
    }
    this.ctrlFlag.cameraCtrl.Reset(0);
    ((Behaviour) this.Camera).set_enabled(true);
    this.Camera.EnabledInput = true;
    this.Camera.Mode = CameraMode.Normal;
    AnimalBase.CreateDisplay = true;
    AnimalManager instance = Singleton<AnimalManager>.Instance;
    int index1 = 0;
    for (int index2 = 0; index2 < instance.Animals.Count; ++index2)
    {
      instance.Animals[index1].BodyEnabled = true;
      ((Behaviour) instance.Animals[index1++]).set_enabled(true);
    }
    instance.SettingAnimalPointBehavior();
    this.ctrlFlag.selectAnimationListInfo = (HScene.AnimationListInfo) null;
    if (this.ctrlItem != null)
      this.ctrlItem.ReleaseItem();
    this.ctrlFlag.cameraCtrl.visibleForceVanish(true);
    this.ctrlMeta.Clear();
    this.hPointCtrl.MarkerObjDel();
    this.hPointCtrl.ExistSecondfemale = false;
    if (Object.op_Inequality((Object) this.ctrlFlag.nowHPoint.endPlayerPos, (Object) null))
    {
      this.hSceneManager.Player.ActivateNavMeshAgent();
      this.hSceneManager.Player.NavMeshAgent.Warp(this.ctrlFlag.nowHPoint.endPlayerPos.get_position());
      ((Component) this.hSceneManager.Player.ChaControl).get_transform().set_position(Vector3.get_zero());
      ((Component) this.hSceneManager.Player.ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.Player.NavMeshAgent).get_transform().get_position());
      this.hSceneManager.Player.Rotation = this.ctrlFlag.nowHPoint.endPlayerPos.get_rotation();
    }
    else
    {
      this.hSceneManager.Player.ActivateNavMeshAgent();
      this.hSceneManager.Player.NavMeshAgent.Warp(((Component) this.ctrlFlag.nowHPoint).get_transform().get_position());
      ((Component) this.hSceneManager.Player.ChaControl).get_transform().set_position(Vector3.get_zero());
      ((Component) this.hSceneManager.Player.ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.Player.NavMeshAgent).get_transform().get_position());
      this.hSceneManager.Player.Rotation = ((Component) this.ctrlFlag.nowHPoint).get_transform().get_rotation();
    }
    if (this.ctrlFlag.nowHPoint.endFemalePos != null)
    {
      if (!this.hSceneManager.bMerchant)
      {
        for (int index2 = 0; index2 < 2; ++index2)
        {
          if (!Object.op_Equality((Object) this.hSceneManager.females[index2], (Object) null) && !Object.op_Inequality((Object) ((Component) this.hSceneManager.females[index2]).GetComponent<PlayerActor>(), (Object) null))
          {
            if (index2 > 0 && this.ctrlFlag.nowHPoint.endFemalePos.Length <= 1)
            {
              this.hSceneManager.females[index2].ActivateNavMeshAgent();
              this.hSceneManager.females[index2].NavMeshAgent.Warp(((Component) this.ctrlFlag.nowHPoint).get_transform().get_position());
              ((Component) this.hSceneManager.females[index2].ChaControl).get_transform().set_position(Vector3.get_zero());
              ((Component) this.hSceneManager.females[index2].ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.females[index2].NavMeshAgent).get_transform().get_position());
              this.hSceneManager.females[index2].Rotation = ((Component) this.ctrlFlag.nowHPoint).get_transform().get_rotation();
            }
            else
            {
              if (Object.op_Inequality((Object) this.ctrlFlag.nowHPoint.endFemalePos[index2], (Object) null))
              {
                this.hSceneManager.females[index2].ActivateNavMeshAgent();
                this.hSceneManager.females[index2].NavMeshAgent.Warp(this.ctrlFlag.nowHPoint.endFemalePos[index2].get_position());
                ((Component) this.hSceneManager.females[index2].ChaControl).get_transform().set_position(Vector3.get_zero());
                ((Component) this.hSceneManager.females[index2].ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.females[index2].NavMeshAgent).get_transform().get_position());
                this.hSceneManager.females[index2].Rotation = this.ctrlFlag.nowHPoint.endFemalePos[index2].get_rotation();
              }
              else
              {
                this.hSceneManager.females[index2].ActivateNavMeshAgent();
                this.hSceneManager.females[index2].NavMeshAgent.Warp(((Component) this.ctrlFlag.nowHPoint).get_transform().get_position());
                ((Component) this.hSceneManager.females[index2].ChaControl).get_transform().set_position(Vector3.get_zero());
                ((Component) this.hSceneManager.females[index2].ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.females[index2].NavMeshAgent).get_transform().get_position());
                this.hSceneManager.females[index2].Rotation = ((Component) this.ctrlFlag.nowHPoint).get_transform().get_rotation();
              }
              ((Component) this.hSceneManager.females[index2].Locomotor).get_transform().LookAt(((Component) this.hSceneManager.Player.Locomotor).get_transform());
            }
          }
        }
      }
      else
      {
        if (Object.op_Inequality((Object) this.ctrlFlag.nowHPoint.endFemalePos[0], (Object) null))
        {
          this.hSceneManager.merchantActor.ActivateNavMeshAgent();
          this.hSceneManager.merchantActor.NavMeshAgent.Warp(this.ctrlFlag.nowHPoint.endFemalePos[0].get_position());
          ((Component) this.hSceneManager.merchantActor.ChaControl).get_transform().set_position(Vector3.get_zero());
          ((Component) this.hSceneManager.merchantActor.ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.merchantActor.NavMeshAgent).get_transform().get_position());
          this.hSceneManager.merchantActor.Rotation = this.ctrlFlag.nowHPoint.endFemalePos[0].get_rotation();
        }
        else
        {
          this.hSceneManager.merchantActor.ActivateNavMeshAgent();
          this.hSceneManager.merchantActor.NavMeshAgent.Warp(((Component) this.ctrlFlag.nowHPoint).get_transform().get_position());
          ((Component) this.hSceneManager.merchantActor.ChaControl).get_transform().set_position(Vector3.get_zero());
          ((Component) this.hSceneManager.merchantActor.ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.merchantActor.NavMeshAgent).get_transform().get_position());
          this.hSceneManager.merchantActor.Rotation = ((Component) this.ctrlFlag.nowHPoint).get_transform().get_rotation();
        }
        if (Object.op_Inequality((Object) this.hSceneManager.Agent[1], (Object) null))
        {
          if (Object.op_Inequality((Object) this.ctrlFlag.nowHPoint.endFemalePos[1], (Object) null) && this.ctrlFlag.nowHPoint.endFemalePos.Length > 1)
          {
            this.hSceneManager.Agent[1].ActivateNavMeshAgent();
            this.hSceneManager.Agent[1].NavMeshAgent.Warp(this.ctrlFlag.nowHPoint.endFemalePos[1].get_position());
            ((Component) this.hSceneManager.Agent[1].ChaControl).get_transform().set_position(Vector3.get_zero());
            ((Component) this.hSceneManager.Agent[1].ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.Agent[1].NavMeshAgent).get_transform().get_position());
            this.hSceneManager.Agent[1].Rotation = this.ctrlFlag.nowHPoint.endFemalePos[1].get_rotation();
          }
          else
          {
            this.hSceneManager.Agent[1].ActivateNavMeshAgent();
            this.hSceneManager.Agent[1].NavMeshAgent.Warp(((Component) this.ctrlFlag.nowHPoint).get_transform().get_position());
            ((Component) this.hSceneManager.Agent[1].ChaControl).get_transform().set_position(Vector3.get_zero());
            ((Component) this.hSceneManager.Agent[1].ChaControl.animBody).get_transform().set_position(((Component) this.hSceneManager.Agent[1].NavMeshAgent).get_transform().get_position());
            this.hSceneManager.Agent[1].Rotation = ((Component) this.ctrlFlag.nowHPoint).get_transform().get_rotation();
          }
        }
        ((Component) this.hSceneManager.merchantActor.Locomotor).get_transform().LookAt(((Component) this.hSceneManager.Player.Locomotor).get_transform());
        if (Object.op_Inequality((Object) this.hSceneManager.Agent[1], (Object) null))
          ((Component) this.hSceneManager.Agent[1].Locomotor).get_transform().LookAt(((Component) this.hSceneManager.Player.Locomotor).get_transform());
      }
    }
    this.hSceneManager.Player.ChaControl.visibleAll = false;
    if (!this.hSceneManager.bMerchant)
    {
      if (this.ctrlFlag.numOrgasmTotal < this.ctrlFlag.gotoFaintnessCount)
      {
        if (!this.hSceneManager.isForce)
        {
          this.ctrlFlag.AddParam(3, 0);
        }
        else
        {
          this.ctrlFlag.AddParam(5, 0);
          if (this.hSceneManager.HSkil.ContainsValue(9))
            this.ctrlFlag.AddSkileParam(9);
        }
      }
      if (this.ctrlFlag.numOutSide > 0)
      {
        this.ctrlFlag.AddParam(11, 1);
        if (this.hSceneManager.HSkil.ContainsValue(8))
          this.ctrlFlag.AddSkileParam(8);
      }
      if (this.ctrlFlag.numInside > 0)
      {
        this.ctrlFlag.AddParam(12, 1);
        if (this.hSceneManager.HSkil.ContainsValue(5))
          this.ctrlFlag.AddSkileParam(5);
      }
      if (this.ctrlFlag.numSameOrgasm > 0)
      {
        this.ctrlFlag.AddParam(13, 1);
        if (this.hSceneManager.HSkil.ContainsValue(2))
          this.ctrlFlag.AddSkileParam(2);
      }
      if (this.ctrlFlag.numAibu > 0)
        this.ctrlFlag.AddParam(0, 1);
      if (this.ctrlFlag.numHoushi > 0)
      {
        this.ctrlFlag.AddParam(1, 1);
        if (this.hSceneManager.HSkil.ContainsValue(4))
          this.ctrlFlag.AddSkileParam(4);
      }
      if (this.ctrlFlag.numSonyu > 0)
        this.ctrlFlag.AddParam(2, 1);
      if (this.ctrlFlag.numLes > 0)
      {
        this.ctrlFlag.AddParam(7, 1);
        if (this.hSceneManager.HSkil.ContainsValue(12))
          this.ctrlFlag.AddSkileParam(12);
      }
      if (this.ctrlFlag.numUrine > 0 && this.hSceneManager.HSkil.ContainsValue(14))
        this.ctrlFlag.AddSkileParam(14);
      if (this.ctrlFlag.numLeadFemale > 0)
      {
        this.ctrlFlag.AddParam(4, 1);
        if (this.hSceneManager.HSkil.ContainsValue(10))
          this.ctrlFlag.AddSkileParam(10);
      }
      if (this.ctrlFlag.isPainActionParam)
        this.ctrlFlag.AddParam(3, 1);
      if (this.hSceneManager.isForce)
        this.ctrlFlag.AddParam(5, 1);
      if (this.ctrlFlag.numFaintness > 0)
      {
        this.ctrlFlag.AddParam(14, 1);
        if (this.hSceneManager.HSkil.ContainsValue(20))
          this.ctrlFlag.AddSkileParam(20);
      }
      if (this.ctrlFlag.numOrgasmTotal <= 0 && this.hSceneManager.HSkil.ContainsValue(21))
        this.ctrlFlag.AddSkileParam(21);
      if (this.ctrlFlag.isNotCtrl)
        this.ctrlFlag.AddParam(16, 1);
      if (this.ctrlFlag.isFemaleNaked)
        this.ctrlFlag.AddParam(34, 1);
      foreach (KeyValuePair<int, int> changeParam in this.ctrlFlag.ChangeParams)
        this.hSceneManager.SetParamator(changeParam.Key, changeParam.Value);
    }
    this.hSceneManager.maleFinish = this.ctrlFlag.numInside + this.ctrlFlag.numOutSide + this.ctrlFlag.numDrink + this.ctrlFlag.numVomit;
    this.hSceneManager.femalePlayerFinish = this.ctrlFlag.numOrgasmFemalePlayer;
    this.hSceneManager.femaleFinish = this.ctrlFlag.numOrgasmTotal;
    this.hSceneManager.endStatus = !this.ctrlFlag.isFaintness ? (byte) 0 : (byte) 1;
    this.hSceneManager.isCtrl = !this.ctrlFlag.isNotCtrl;
    this.EndProcADV();
    this.hSceneManager.merchantActor = (MerchantActor) null;
  }

  private void LateUpdate()
  {
    this.LotionProc();
    if (this.ctrlFlag.BeforeHWait)
    {
      if (this.preBeforWaitState != null && this.preBeforWaitState.Count > 0)
      {
        AnimatorStateInfo animatorStateInfo;
        if (this.preBeforWaitState.ContainsKey(0))
        {
          animatorStateInfo = this.hSceneManager.Player.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
          int shortNameHash = ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash();
          if (this.preBeforWaitState[0] != shortNameHash)
          {
            this.hSceneManager.Player.Animation.MapIK.Calc(shortNameHash);
            this.preBeforWaitState[0] = shortNameHash;
          }
        }
        for (int index = 0; index < this.hSceneManager.Agent.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.hSceneManager.Agent[index], (Object) null) && !Object.op_Equality((Object) this.hSceneManager.Agent[index].ChaControl, (Object) null) && (!Object.op_Equality((Object) this.hSceneManager.Agent[index].ChaControl.animBody, (Object) null) && this.preBeforWaitState.ContainsKey(1 + index)))
          {
            animatorStateInfo = this.hSceneManager.Agent[index].ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
            int shortNameHash = ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash();
            if (this.preBeforWaitState[1 + index] != shortNameHash)
            {
              this.hSceneManager.Agent[index].Animation.MapIK.Calc(shortNameHash);
              this.preBeforWaitState[1 + index] = shortNameHash;
            }
          }
        }
      }
      this.prevBeforeWait = this.ctrlFlag.BeforeHWait;
    }
    else
    {
      if (this.prevBeforeWait && !this.ctrlFlag.BeforeHWait)
      {
        this.sprite.endFade = 0;
        this.prevBeforeWait = this.ctrlFlag.BeforeHWait;
      }
      if (!ProcBase.endInit)
        return;
      bool flag = Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null);
      bool activeSelf = this.hSceneManager.HSceneUISet.get_activeSelf();
      if (this.NowStateIsEnd)
      {
        if (flag || !activeSelf)
          return;
        this.hSceneManager.HSceneUISet.SetActive(false);
      }
      else
      {
        if (this.chaFemales[0].GetNowClothesType() == 3)
          this.ctrlFlag.isFemaleNaked = true;
        HSystem hdata = Manager.Config.HData;
        foreach (int clothesKind in new List<int>()
        {
          0,
          2,
          1,
          3,
          5,
          6
        })
        {
          if (this.hSceneManager.Player.ChaControl.IsClothesStateKind(clothesKind))
          {
            byte state = 0;
            if (!hdata.Cloth)
              state = (byte) 2;
            this.hSceneManager.Player.ChaControl.SetClothesState(clothesKind, state, true);
          }
        }
        this.hSceneManager.Player.ChaControl.SetAccessoryStateAll(hdata.Accessory);
        this.hSceneManager.Player.ChaControl.SetClothesState(7, !hdata.Shoes ? (byte) 2 : (byte) 0, true);
        this.ctrlFlag.semenType = Manager.Config.HData.Siru;
        if (Object.op_Inequality((Object) this.Camera, (Object) null))
          this.Camera.AmbientLight = Manager.Config.GraphicData.AmbientLight;
        this.ctrlFlag.cameraCtrl.ConfigVanish = Manager.Config.GraphicData.Shield;
        this.SyncAnimation();
      }
    }
  }

  private void LotionProc()
  {
    if (!this.useLotion)
      return;
    float num1 = 100f;
    for (int index = 0; index < this.hSceneManager.Agent.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.hSceneManager.Agent[index], (Object) null))
      {
        AgentData agentData = this.hSceneManager.Agent[index].AgentData;
        if (agentData != null)
        {
          agentData.Wetness = num1;
          float num2 = Mathf.InverseLerp(0.0f, 100f, agentData.Wetness);
          this.hSceneManager.Agent[index].ChaControl.wetRate = num2;
        }
      }
    }
    if (!this.hSceneManager.bMerchant || !Object.op_Inequality((Object) this.hSceneManager.merchantActor, (Object) null))
      return;
    MerchantData merchantData = this.hSceneManager.merchantActor.MerchantData;
    if (merchantData == null)
      return;
    merchantData.Wetness = num1;
    this.hSceneManager.merchantActor.ChaControl.wetRate = Mathf.InverseLerp(0.0f, 100f, merchantData.Wetness);
  }

  public void ConfigEnd()
  {
    this.ctrlFlag.click = HSceneFlagCtrl.ClickKind.None;
    this.NowStateIsEnd = true;
    this.sprite.ReSetLight();
    this.Camera.EnableUpdateCustomLight();
    Singleton<Sound>.Instance.Listener = ((Component) Singleton<Manager.Map>.Instance.Player.CameraControl).get_transform();
    Singleton<Resources>.Instance.HSceneTable.HMeshObjDic[Singleton<Manager.Map>.Instance.MapID].SetActive(true);
    this.hSceneManager.CameraMesh?.SetActive(true);
    this.EndAnimChange();
    this.hSceneManager.endStatus = !this.ctrlFlag.isFaintness ? (byte) 0 : (byte) 1;
    this.chaFemales[0].SetClothesStateAll((byte) 0);
    if (Object.op_Inequality((Object) this.ctrlEyeNeckFemale[0], (Object) null) && Object.op_Inequality((Object) this.chaFemales[0], (Object) null) && Object.op_Inequality((Object) this.chaFemales[0].objBodyBone, (Object) null))
      this.ctrlEyeNeckFemale[0].NowEndADV = true;
    if (Object.op_Inequality((Object) this.ctrlEyeNeckFemale[1], (Object) null) && Object.op_Inequality((Object) this.chaFemales[1], (Object) null) && Object.op_Inequality((Object) this.chaFemales[1].objBodyBone, (Object) null))
    {
      if (this.hSceneManager.Player.ChaControl.sex == (byte) 0)
        this.ctrlEyeNeckFemale[1].NowEndADV = true;
      else if (this.hSceneManager.Player.ChaControl.sex == (byte) 1)
      {
        if (this.ctrlFlag.bFutanari)
          this.ctrlEyeNeckFemale[1].NowEndADV = true;
        else
          this.hMotionEyeNeckLesP.NowEndADV = true;
      }
    }
    if (Object.op_Inequality((Object) this.ctrlEyeNeckMale[0], (Object) null) && Object.op_Inequality((Object) this.chaMales[0], (Object) null) && Object.op_Inequality((Object) this.chaMales[0].objBodyBone, (Object) null))
      this.ctrlEyeNeckMale[0].NowEndADV = true;
    if (Object.op_Inequality((Object) this.ctrlEyeNeckMale[1], (Object) null) && Object.op_Inequality((Object) this.chaMales[1], (Object) null) && Object.op_Inequality((Object) this.chaMales[1].objBodyBone, (Object) null))
      this.ctrlEyeNeckMale[1].NowEndADV = true;
    using (IEnumerator<AgentActor> enumerator = Singleton<Manager.Map>.Instance.AgentTable.get_Values().GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        AgentActor current = enumerator.Current;
        if (!Object.op_Equality((Object) current, (Object) null) && this.hSceneManager.ReturnActionTypes.ContainsKey(current))
          current.EnableEntity();
      }
    }
    MerchantActor merchant = Singleton<Manager.Map>.Instance.Merchant;
    if (this.hSceneManager.bMerchant)
    {
      if (!((Behaviour) merchant.ChaControl.neckLookCtrl).get_enabled())
        ((Behaviour) merchant.ChaControl.neckLookCtrl).set_enabled(true);
      if (!((Behaviour) merchant.ChaControl.eyeLookCtrl).get_enabled())
        ((Behaviour) merchant.ChaControl.eyeLookCtrl).set_enabled(true);
      merchant.ChaControl.ChangeLookNeckPtn(3, 1f);
      merchant.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      merchant.ChaControl.ChangeLookEyesPtn(0);
      merchant.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      Singleton<Game>.Instance.GetExpression(merchant.ID, "標準")?.Change(merchant.ChaControl);
      merchant.ChaControl.ChangeMouthOpenMin(merchant.ChaControl.fileStatus.mouthOpenMin);
    }
    AnimatorStateInfo animatorStateInfo = merchant.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
    int shortNameHash = ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash();
    merchant.AnimationMerchant.SetAnimatorController(merchant.ChaControl.animBody.get_runtimeAnimatorController());
    merchant.AnimationMerchant.MapIK.Calc(shortNameHash);
    merchant.EnableEntity();
    if (Object.op_Inequality((Object) this.hSceneManager.male, (Object) null))
    {
      if (!((Behaviour) this.hSceneManager.male.ChaControl.neckLookCtrl).get_enabled())
        ((Behaviour) this.hSceneManager.male.ChaControl.neckLookCtrl).set_enabled(true);
      if (!((Behaviour) this.hSceneManager.male.ChaControl.eyeLookCtrl).get_enabled())
        ((Behaviour) this.hSceneManager.male.ChaControl.eyeLookCtrl).set_enabled(true);
      this.hSceneManager.male.ChaControl.ChangeLookNeckPtn(3, 1f);
      this.hSceneManager.male.ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
      this.hSceneManager.male.ChaControl.ChangeLookEyesPtn(0);
      this.hSceneManager.male.ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
      Singleton<Game>.Instance.GetExpression(this.hSceneManager.male.ID, "標準")?.Change(this.hSceneManager.male.ChaControl);
      this.hSceneManager.male.ChaControl.ChangeMouthOpenMin(this.hSceneManager.male.ChaControl.fileStatus.mouthOpenMin);
    }
    if (this.hSceneManager.females != null)
    {
      for (int index = 0; index < this.hSceneManager.females.Length; ++index)
      {
        if (!Object.op_Equality((Object) this.hSceneManager.females[index], (Object) null))
        {
          if (!((Behaviour) this.hSceneManager.females[index].ChaControl.neckLookCtrl).get_enabled())
            ((Behaviour) this.hSceneManager.females[index].ChaControl.neckLookCtrl).set_enabled(true);
          if (!((Behaviour) this.hSceneManager.females[index].ChaControl.eyeLookCtrl).get_enabled())
            ((Behaviour) this.hSceneManager.females[index].ChaControl.eyeLookCtrl).set_enabled(true);
          this.hSceneManager.females[index].ChaControl.ChangeLookNeckPtn(3, 1f);
          this.hSceneManager.females[index].ChaControl.ChangeLookNeckTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 0.8f);
          this.hSceneManager.females[index].ChaControl.ChangeLookEyesPtn(0);
          this.hSceneManager.females[index].ChaControl.ChangeLookEyesTarget(0, (Transform) null, 0.5f, 0.0f, 1f, 2f);
          int personality = 0;
          if (Object.op_Implicit((Object) ((Component) this.hSceneManager.females[index]).GetComponent<PlayerActor>()))
            personality = -100;
          else if (Object.op_Equality((Object) ((Component) this.hSceneManager.females[index]).GetComponent<MerchantActor>(), (Object) null))
            personality = this.hSceneManager.females[index].ChaControl.fileParam.personality;
          Singleton<Game>.Instance.GetExpression(personality, "標準")?.Change(this.hSceneManager.females[index].ChaControl);
          this.hSceneManager.females[index].ChaControl.ChangeMouthOpenMin(this.hSceneManager.females[index].ChaControl.fileStatus.mouthOpenMin);
        }
      }
    }
    for (int index = 0; index < this.chaMales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && !Object.op_Equality((Object) this.chaMales[index].objBody, (Object) null))
        this.chaMales[index].visibleSon = false;
    }
    this.ctrlFlag.cameraCtrl.Reset(0);
    ((Behaviour) this.Camera).set_enabled(true);
    this.Camera.EnabledInput = true;
    this.Camera.Mode = CameraMode.Normal;
    AnimalBase.CreateDisplay = true;
    AnimalManager instance = Singleton<AnimalManager>.Instance;
    int index1 = 0;
    for (int index2 = 0; index2 < instance.Animals.Count; ++index2)
    {
      instance.Animals[index1].BodyEnabled = true;
      ((Behaviour) instance.Animals[index1++]).set_enabled(true);
    }
    instance.SettingAnimalPointBehavior();
    this.ctrlFlag.selectAnimationListInfo = (HScene.AnimationListInfo) null;
    if (this.ctrlItem != null)
      this.ctrlItem.ReleaseItem();
    this.ctrlFlag.cameraCtrl.visibleForceVanish(true);
    this.ctrlMeta.Clear();
    this.hPointCtrl.MarkerObjDel();
    this.hPointCtrl.ExistSecondfemale = false;
    this.CloseADV();
    this.hSceneManager.HSceneUISet.SetActive(false);
    this.hSceneManager.merchantActor = (MerchantActor) null;
  }

  private void OnValidate()
  {
  }

  private void OnDisable()
  {
    if (this.nowStart)
      return;
    if (Object.op_Implicit((Object) this.ctrlFlag) && Object.op_Implicit((Object) this.ctrlFlag.cameraCtrl))
    {
      this.ctrlFlag.cameraCtrl.visibleForceVanish(true);
      this.ctrlFlag.cameraCtrl.ResetVanish();
    }
    if (Singleton<Housing>.IsInstance())
      Singleton<Housing>.Instance.EndShield();
    this.hSceneManager.isForce = false;
    for (int index = 0; index < 2; ++index)
    {
      if (this.ctrlDynamics[index] != null)
        this.ctrlDynamics[index].InitDynamicBoneReferenceBone();
    }
    if (Object.op_Inequality((Object) this.ctrlVoice, (Object) null))
    {
      this.ctrlVoice.FaceReset(this.chaFemales[0]);
      if (Object.op_Inequality((Object) this.chaFemales[1], (Object) null))
        this.ctrlVoice.FaceReset(this.chaFemales[1]);
    }
    for (int index = 0; index < this.ctrlYures.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.ctrlYures[index], (Object) null))
        this.ctrlYures[index].ResetShape();
    }
    if (Object.op_Inequality((Object) this.ctrlYureMale, (Object) null))
      this.ctrlYureMale.ResetShape();
    if (this.ctrlSiruPastes != null)
    {
      foreach (SiruPasteCtrl ctrlSiruPaste in this.ctrlSiruPastes)
        ctrlSiruPaste.Release();
    }
    foreach (ChaControl chaFemale in this.chaFemales)
    {
      if (!Object.op_Equality((Object) chaFemale, (Object) null) && !Object.op_Equality((Object) chaFemale.objBody, (Object) null))
      {
        chaFemale.ChangeBustInert(false);
        chaFemale.playDynamicBoneBust(0, true);
        chaFemale.playDynamicBoneBust(1, true);
        chaFemale.fileStatus.skinTuyaRate = 0.0f;
        chaFemale.ChangeEyesOpenMax(1f);
        FBSCtrlMouth mouthCtrl = chaFemale.mouthCtrl;
        if (mouthCtrl != null)
          mouthCtrl.OpenMin = 0.0f;
        chaFemale.SetAccessoryStateAll(true);
        chaFemale.SetClothesStateAll((byte) 0);
        for (int index = 0; index < 5; ++index)
        {
          ChaFileDefine.SiruParts parts = (ChaFileDefine.SiruParts) index;
          chaFemale.SetSiruFlag(parts, (byte) 0);
        }
        chaFemale.DisableShapeMouth(false);
        for (int index = 0; index < 7; ++index)
        {
          int id = index;
          chaFemale.DisableShapeBodyID(2, id, false);
        }
        chaFemale.DisableShapeBodyID(2, 7, false);
        chaFemale.ReleaseHitObject();
      }
    }
    for (int key = 0; key < this.chaFemales.Length; ++key)
    {
      if (!Object.op_Equality((Object) this.chaFemales[key], (Object) null) && !Object.op_Equality((Object) this.chaFemales[key].objBody, (Object) null))
      {
        float rate = 0.0f;
        if (this.initStandNip.TryGetValue(key, out rate))
          this.chaFemales[key].ChangeNipRate(rate);
      }
    }
    for (int index = 0; index < this.chaMales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && !Object.op_Equality((Object) this.chaMales[index].objBody, (Object) null))
        this.chaMales[index].ReleaseHitObject();
    }
    this.initStandNip.Clear();
    if (this.CtrlParticle != null)
      this.CtrlParticle.RePlaceObject();
    for (int index = 0; index < this.ctrlHitObjectFemales.Length; ++index)
    {
      if (this.ctrlHitObjectFemales[index] != null)
        this.ctrlHitObjectFemales[index].ReleaseObject();
    }
    for (int index = 0; index < this.ctrlHitObjectMales.Length; ++index)
    {
      if (this.ctrlHitObjectMales[index] != null)
        this.ctrlHitObjectMales[index].ReleaseObject();
    }
    if (Object.op_Implicit((Object) this.objGrondCollision))
    {
      Object.Destroy((Object) this.objGrondCollision);
      this.objGrondCollision = (GameObject) null;
    }
    if (Singleton<Sound>.IsInstance())
    {
      Singleton<Sound>.Instance.Stop(Sound.Type.GameSE2D);
      Singleton<Sound>.Instance.Stop(Sound.Type.GameSE3D);
    }
    if (Singleton<Manager.Voice>.IsInstance())
      Singleton<Manager.Voice>.Instance.StopAll(true);
    this.ctrlVoice.HBeforeHouchiTime = 0.0f;
    this.hSceneManager.bMerchant = false;
    if (Singleton<GameCursor>.IsInstance())
      Singleton<GameCursor>.Instance.SetCursorLock(false);
    AssetBundleManager.UnloadAssetBundle(this.hSceneManager.strAssetSE, true, (string) null, false);
    for (int index = 0; index < this.ctrlFlag.voice.lstUseAsset.Count; ++index)
      AssetBundleManager.UnloadAssetBundle(this.ctrlFlag.voice.lstUseAsset[index], true, (string) null, false);
    if (Singleton<HSceneManager>.IsInstance())
    {
      foreach (string assetBundleName in this.hSceneManager.hashUseAssetBundle)
        AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
      this.hSceneManager.hashUseAssetBundle.Clear();
    }
    this.ctrlLayer.Release();
    this.isSetStartPos = false;
    this.mode = -1;
    this.modeCtrl = -1;
    this.nowStart = false;
    this.nullPlayer = true;
    this.NowStateIsEnd = false;
    this.nowChangeAnim = false;
    this.lstMotionIK.Clear();
    Singleton<HPointCtrl>.Instance.endHScene();
    this.lstProc.Clear();
    this.packData = (HScene.PackData) null;
    this.hSceneManager.choiceDisposable?.Dispose();
    this.hSceneManager.choiceDisposable = (IDisposable) null;
    this.hSceneManager.nInvitePtn = -1;
    HSceneManager.SleepStart = false;
    this.hSceneManager.Agent[0] = (AgentActor) null;
    this.hSceneManager.Agent[1] = (AgentActor) null;
    this.useLotion = false;
    if (Object.op_Inequality((Object) this.hSceneManager.Player, (Object) null) && this.hSceneManager.Player.ChaControl.sex == (byte) 1 && this.hSceneManager.bFutanari)
      this.hSceneManager.Player.ChaControl.ChangeBustInert(false);
    this.hSceneManager.numFemaleClothCustom = 0;
    this.hSceneManager.MerchantLimit = -1;
    this.hSceneManager.EndHScene();
    this.ctrlFlag.isJudgeSelect.Clear();
  }

  public void HParticleSetNull()
  {
    this.ctrlParitcle = (HParticleCtrl) null;
  }

  private void CreateListAnimationFileName()
  {
    this.lstAnimInfo = Singleton<Resources>.Instance.HSceneTable.lstAnimInfo;
  }

  private void SyncAnimation()
  {
    if (Object.op_Equality((Object) this.chaFemales[0].animBody, (Object) null))
      return;
    AnimatorStateInfo animatorStateInfo = this.chaFemales[0].getAnimatorStateInfo(0);
    List<int> lstSyncAnimLayer = this.ctrlFlag.lstSyncAnimLayers[0, 0];
    for (int index1 = 1; index1 < this.ctrlFlag.lstSyncAnimLayers.GetLength(0); ++index1)
    {
      for (int index2 = 1; index2 < this.ctrlFlag.lstSyncAnimLayers.GetLength(1); ++index2)
      {
        for (int index3 = 0; index3 < this.ctrlFlag.lstSyncAnimLayers[index1, index2].Count; ++index3)
        {
          if (!lstSyncAnimLayer.Contains(this.ctrlFlag.lstSyncAnimLayers[index1, index2][index3]))
            lstSyncAnimLayer.Add(this.ctrlFlag.lstSyncAnimLayers[index1, index2][index3]);
        }
      }
    }
    this.ctrlItem.ParentScaleReject();
    if (Object.op_Equality((Object) this.chaMales[0], (Object) null) || Object.op_Equality((Object) this.chaMales[0].animBody, (Object) null) || Object.op_Equality((Object) this.chaMales[0].objBodyBone, (Object) null))
    {
      if (!this.chaFemales[0].isBlend(0) && Object.op_Inequality((Object) this.ctrlItem.GetItem(), (Object) null))
      {
        this.ctrlItem.syncPlay(animatorStateInfo);
        this.ctrlItem.Update();
      }
      if (Object.op_Implicit((Object) this.chaFemales[1]))
        this.chaFemales[1].syncPlay(animatorStateInfo, 0);
      if (lstSyncAnimLayer.Count == 0)
        this.isSyncFirstStep = false;
      else if (!this.isSyncFirstStep)
      {
        this.isSyncFirstStep = true;
      }
      else
      {
        for (int index1 = 0; index1 < this.chaFemales.Length; ++index1)
        {
          if (!Object.op_Equality((Object) this.chaFemales[index1], (Object) null) && (!Object.op_Implicit((Object) this.chaFemales[index1]) || !Object.op_Equality((Object) this.chaFemales[index1].animBody, (Object) null)))
          {
            for (int index2 = 0; index2 < this.ctrlFlag.lstSyncAnimLayers[1, index1].Count; ++index2)
            {
              int _nLayer = this.ctrlFlag.lstSyncAnimLayers[1, index1][index2];
              if (this.chaFemales[index1].animBody.get_layerCount() > _nLayer)
                this.chaFemales[index1].syncPlay(animatorStateInfo, _nLayer);
            }
          }
        }
      }
    }
    else
    {
      if (Object.op_Inequality((Object) this.chaFemales[1], (Object) null) && Object.op_Inequality((Object) this.chaFemales[1].objTop, (Object) null))
        this.chaFemales[1].syncPlay(animatorStateInfo, 0);
      for (int index1 = 0; index1 < this.chaMales.Length; ++index1)
      {
        if (!Object.op_Equality((Object) this.chaMales[index1], (Object) null) && !Object.op_Equality((Object) this.chaMales[index1].objTop, (Object) null))
        {
          for (int index2 = 0; index2 < this.ctrlFlag.lstSyncAnimLayers[0, index1].Count; ++index2)
          {
            int num = this.ctrlFlag.lstSyncAnimLayers[0, index1][index2];
            if (this.chaMales[index1].animBody.get_layerCount() > num)
              this.chaMales[index2].syncPlay(animatorStateInfo, 0);
          }
        }
      }
      if (Object.op_Inequality((Object) this.ctrlItem.GetItem(), (Object) null))
      {
        this.ctrlItem.syncPlay(animatorStateInfo);
        this.ctrlItem.Update();
      }
      if (this.ctrlFlag.lstSyncAnimLayers[1, 0].Count == 0)
        this.isSyncFirstStep = false;
      else if (!this.isSyncFirstStep)
      {
        this.isSyncFirstStep = true;
      }
      else
      {
        bool flag1 = false;
        bool flag2 = false;
        for (int index1 = 0; index1 < this.chaFemales.Length; ++index1)
        {
          if (!Object.op_Equality((Object) this.chaFemales[index1], (Object) null) && (!Object.op_Implicit((Object) this.chaFemales[index1]) || !Object.op_Equality((Object) this.chaFemales[index1].animBody, (Object) null)))
          {
            for (int index2 = 0; index2 < this.ctrlFlag.lstSyncAnimLayers[1, index1].Count; ++index2)
            {
              int _nLayer = this.ctrlFlag.lstSyncAnimLayers[1, index1][index2];
              if (this.chaFemales[index1].animBody.get_layerCount() > _nLayer)
              {
                this.chaFemales[index1].syncPlay(animatorStateInfo, _nLayer);
                flag1 = true;
              }
            }
          }
        }
        for (int index1 = 0; index1 < this.chaMales.Length; ++index1)
        {
          if (!Object.op_Equality((Object) this.chaMales[index1], (Object) null) && (!Object.op_Implicit((Object) this.chaMales[index1]) || !Object.op_Equality((Object) this.chaMales[index1].animBody, (Object) null)))
          {
            for (int index2 = 0; index2 < this.ctrlFlag.lstSyncAnimLayers[0, index1].Count; ++index2)
            {
              int _nLayer = this.ctrlFlag.lstSyncAnimLayers[0, index1][index2];
              if (this.chaMales[index1].animBody.get_layerCount() > _nLayer)
              {
                this.chaMales[index1].syncPlay(animatorStateInfo, _nLayer);
                flag2 = true;
              }
            }
          }
        }
        if (flag1)
        {
          for (int index = 0; index < this.chaFemales.Length; ++index)
          {
            if (!Object.op_Equality((Object) this.chaFemales[index], (Object) null) && (!Object.op_Implicit((Object) this.chaFemales[index]) || !Object.op_Equality((Object) this.chaFemales[index].animBody, (Object) null)))
              this.chaFemales[index].animBody.Update(0.0f);
          }
        }
        if (flag2)
        {
          for (int index = 0; index < this.chaMales.Length; ++index)
          {
            if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && (!Object.op_Implicit((Object) this.chaMales[index]) || !Object.op_Equality((Object) this.chaMales[index].animBody, (Object) null)))
              this.chaMales[index].animBody.Update(0.0f);
          }
        }
        for (int index = 0; index < this.ctrlEyeNeckFemale.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.ctrlEyeNeckFemale[index], (Object) null))
          {
            bool flag3 = this.hSceneManager.Player.ChaControl.sex == (byte) 1 && !this.ctrlFlag.bFutanari;
            if (index == 0 || !flag3)
              this.ctrlEyeNeckFemale[index].EyeNeckCalc();
            else
              this.hMotionEyeNeckLesP.EyeNeckCalc();
          }
        }
        for (int index = 0; index < this.ctrlEyeNeckMale.Length; ++index)
        {
          if (!Object.op_Equality((Object) this.ctrlEyeNeckMale[index], (Object) null))
            this.ctrlEyeNeckMale[index].EyeNeckCalc();
        }
      }
    }
  }

  [DebuggerHidden]
  public IEnumerator ChangeAnimation(
    HScene.AnimationListInfo _info,
    bool _isForceResetCamera,
    bool _isForceLoopAction = false,
    bool _UseFade = true)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HScene.\u003CChangeAnimation\u003Ec__Iterator2()
    {
      _info = _info,
      _isForceLoopAction = _isForceLoopAction,
      _UseFade = _UseFade,
      _isForceResetCamera = _isForceResetCamera,
      \u0024this = this
    };
  }

  private bool IsIdle(Animator _anim)
  {
    if (Object.op_Equality((Object) _anim.get_runtimeAnimatorController(), (Object) null))
      return true;
    AnimatorStateInfo animatorStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    return ((AnimatorStateInfo) ref animatorStateInfo).IsName("Idle") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("WIdle") || (((AnimatorStateInfo) ref animatorStateInfo).IsName("SIdle") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("Insert")) || (((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Idle") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Insert"));
  }

  private bool IsAfterIdle(Animator _anim, int _mode)
  {
    if (Object.op_Equality((Object) _anim.get_runtimeAnimatorController(), (Object) null))
      return true;
    AnimatorStateInfo animatorStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
    switch (_mode)
    {
      case 0:
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_OUT_A") || (((AnimatorStateInfo) ref animatorStateInfo).IsName("Drink_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("Vomit_A")) || (((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_IN_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("OrgasmM_OUT_A") || (((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Orgasm_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Orgasm_OUT_A"))) || (((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Orgasm_IN_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("D_OrgasmM_OUT_A")))
          return true;
        break;
      case 1:
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_OUT_A") || (((AnimatorStateInfo) ref animatorStateInfo).IsName("Drink_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("Vomit_A")) || (((AnimatorStateInfo) ref animatorStateInfo).IsName("Orgasm_IN_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("OrgasmM_OUT_A")))
          return true;
        break;
      case 2:
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Orgasm_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Orgasm_OUT_A") || (((AnimatorStateInfo) ref animatorStateInfo).IsName("D_Orgasm_IN_A") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("D_OrgasmM_OUT_A")))
          return true;
        break;
    }
    return false;
  }

  public bool setCameraLoad(HScene.AnimationListInfo _info, bool _isForceResetCamera)
  {
    if (_isForceResetCamera || this.isSetStartPos && Manager.Config.HData.InitCamera)
      GlobalMethod.loadCamera(this.ctrlFlag.cameraCtrl, this.hSceneManager.strAssetCameraList, _info.nameCamera, false);
    else
      GlobalMethod.loadResetCamera(this.ctrlFlag.cameraCtrl, this.hSceneManager.strAssetCameraList, _info.nameCamera, false);
    return true;
  }

  private bool SetPosition(
    Transform _trans,
    Vector3 offsetpos,
    Vector3 offsetrot,
    bool _FadeStart = true)
  {
    if (Object.op_Equality((Object) _trans, (Object) null))
      return false;
    for (int index = 0; index < this.chaMales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && !Object.op_Equality((Object) this.chaMales[index].objBody, (Object) null))
      {
        this.chaMalesTrans[index].set_position(_trans.get_position());
        this.chaMalesTrans[index].set_rotation(_trans.get_rotation());
        Transform chaMalesTran = this.chaMalesTrans[index];
        chaMalesTran.set_localPosition(Vector3.op_Addition(chaMalesTran.get_localPosition(), Quaternion.op_Multiply(_trans.get_rotation(), offsetpos)));
        this.chaMalesTrans[index].set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(offsetrot), this.chaMalesTrans[index].get_localRotation()));
        ((Component) this.chaMales[index].animBody).get_transform().set_localPosition(Vector3.get_zero());
        ((Component) this.chaMales[index].animBody).get_transform().set_localRotation(Quaternion.get_identity());
        this.chaMales[index].SetPosition(Vector3.get_zero());
        this.chaMales[index].SetRotation(Vector3.get_zero());
        this.chaMales[index].resetDynamicBoneAll = true;
      }
    }
    if (Object.op_Inequality((Object) this.hSceneManager.male, (Object) null) && Object.op_Inequality((Object) this.chaMales[0], (Object) null) && Object.op_Inequality((Object) this.chaMales[0].objBody, (Object) null))
      this.hSceneManager.male.Position = this.chaMalesTrans[0].get_position();
    for (int index = 0; index < this.chaFemales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaFemales[index], (Object) null) && !Object.op_Equality((Object) this.chaFemales[index].objBody, (Object) null))
      {
        this.chaFemalesTrans[index].set_position(_trans.get_position());
        this.chaFemalesTrans[index].set_rotation(_trans.get_rotation());
        Transform chaFemalesTran = this.chaFemalesTrans[index];
        chaFemalesTran.set_localPosition(Vector3.op_Addition(chaFemalesTran.get_localPosition(), Quaternion.op_Multiply(_trans.get_rotation(), offsetpos)));
        this.chaFemalesTrans[index].set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(offsetrot), this.chaFemalesTrans[index].get_localRotation()));
        ((Component) this.chaFemales[index].animBody).get_transform().set_localPosition(Vector3.get_zero());
        ((Component) this.chaFemales[index].animBody).get_transform().set_localRotation(Quaternion.get_identity());
        this.chaFemales[index].SetPosition(Vector3.get_zero());
        this.chaFemales[index].SetRotation(Vector3.get_zero());
        this.chaFemales[index].ResetDynamicBoneAll(false);
        this.chaFemales[index].resetDynamicBoneAll = true;
        if (this.hSceneManager.females != null && Object.op_Inequality((Object) this.hSceneManager.females[index], (Object) null))
          this.hSceneManager.females[index].Position = this.chaFemalesTrans[index].get_position();
      }
    }
    ((Component) this.FeelHitEffect3D).get_transform().set_position(Vector3.op_Addition(this.chaFemales[0].objHeadBone.get_transform().get_position(), this.FeelHitEffect3DOffSet));
    if (_FadeStart)
      this.fade.FadeStart(2f);
    this.ctrlItem.setTransform(_trans);
    return true;
  }

  private bool SetPosition(
    Vector3 pos,
    Quaternion rot,
    Vector3 offsetpos,
    Vector3 offsetrot,
    bool _FadeStart = true)
  {
    for (int index = 0; index < this.chaMales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaMales[index], (Object) null) && !Object.op_Equality((Object) this.chaMales[index].objBody, (Object) null))
      {
        this.chaMalesTrans[index].set_position(pos);
        this.chaMalesTrans[index].set_rotation(rot);
        Transform chaMalesTran = this.chaMalesTrans[index];
        chaMalesTran.set_localPosition(Vector3.op_Addition(chaMalesTran.get_localPosition(), Quaternion.op_Multiply(rot, offsetpos)));
        this.chaMalesTrans[index].set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(offsetrot), this.chaMalesTrans[index].get_localRotation()));
        ((Component) this.chaMales[index].animBody).get_transform().set_localPosition(Vector3.get_zero());
        ((Component) this.chaMales[index].animBody).get_transform().set_localRotation(Quaternion.get_identity());
        this.chaMales[index].SetPosition(Vector3.get_zero());
        this.chaMales[index].SetRotation(Vector3.get_zero());
        this.chaMales[index].resetDynamicBoneAll = true;
      }
    }
    for (int index = 0; index < this.chaFemales.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.chaFemales[index], (Object) null) && !Object.op_Equality((Object) this.chaFemales[index].objBody, (Object) null))
      {
        this.chaFemalesTrans[index].set_position(pos);
        this.chaFemalesTrans[index].set_rotation(rot);
        Transform chaFemalesTran = this.chaFemalesTrans[index];
        chaFemalesTran.set_localPosition(Vector3.op_Addition(chaFemalesTran.get_localPosition(), Quaternion.op_Multiply(rot, offsetpos)));
        this.chaFemalesTrans[index].set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(offsetrot), this.chaFemalesTrans[index].get_localRotation()));
        ((Component) this.chaFemales[index].animBody).get_transform().set_localPosition(Vector3.get_zero());
        ((Component) this.chaFemales[index].animBody).get_transform().set_localRotation(Quaternion.get_identity());
        this.chaFemales[index].SetPosition(Vector3.get_zero());
        this.chaFemales[index].SetRotation(Vector3.get_zero());
        this.chaFemales[index].ResetDynamicBoneAll(false);
        this.chaFemales[index].resetDynamicBoneAll = true;
      }
    }
    ((Component) this.FeelHitEffect3D).get_transform().set_position(Vector3.op_Addition(this.chaFemales[0].objHeadBone.get_transform().get_position(), this.FeelHitEffect3DOffSet));
    if (_FadeStart)
      this.fade.FadeStart(-1f);
    HItemCtrl ctrlItem = this.ctrlItem;
    Vector3 position = this.chaFemalesTrans[0].get_position();
    Quaternion rotation = this.chaFemalesTrans[0].get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
    ctrlItem.setTransform(position, eulerAngles);
    this.objMetaBallBase.get_transform().set_rotation(this.chaFemalesTrans[0].get_rotation());
    return true;
  }

  public bool SetMovePositionPoint(Transform trans, Vector3 offsetpos, Vector3 offsetrot)
  {
    this.SetPosition(trans, offsetpos, offsetrot, false);
    GlobalMethod.setCameraBase(this.ctrlFlag.cameraCtrl, this.chaFemalesTrans[0]);
    GlobalMethod.loadCamera(this.ctrlFlag.cameraCtrl, this.hSceneManager.strAssetCameraList, this.ctrlFlag.nowAnimationInfo.nameCamera, false);
    return true;
  }

  private bool ShortcutKey()
  {
    if (this.sprite.isFade | this.sprite.GetFadeKindProc() == HSceneSprite.FadeKindProc.OutEnd | Object.op_Inequality((Object) Singleton<Game>.Instance.Config, (Object) null) | Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null) | Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null) | Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
      return true;
    GlobalMethod.CameraKeyCtrl(this.ctrlFlag.cameraCtrl, this.chaFemales);
    if (UnityEngine.Input.GetKeyDown((KeyCode) 284))
    {
      ConfigWindow.UnLoadAction = (System.Action) (() => {});
      ConfigWindow.TitleChangeAction = (System.Action) (() =>
      {
        this.ConfigEnd();
        ConfigWindow.UnLoadAction = (System.Action) null;
        Singleton<Game>.Instance.Dialog.TimeScale = 1f;
      });
      Singleton<Game>.Instance.LoadConfig();
      return true;
    }
    if (!UnityEngine.Input.GetKey((KeyCode) 304) && !UnityEngine.Input.GetKey((KeyCode) 303))
    {
      if (UnityEngine.Input.GetKeyDown((KeyCode) 49))
        Manager.Config.HData.EyeDir0 = !Manager.Config.HData.EyeDir0;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 50))
        Manager.Config.HData.NeckDir0 = !Manager.Config.HData.NeckDir0;
    }
    else
    {
      if (UnityEngine.Input.GetKeyDown((KeyCode) 49))
        Manager.Config.HData.EyeDir1 = !Manager.Config.HData.EyeDir1;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 50))
        Manager.Config.HData.NeckDir1 = !Manager.Config.HData.NeckDir1;
    }
    if (!UnityEngine.Input.GetKey((KeyCode) 304) && !UnityEngine.Input.GetKey((KeyCode) 303))
    {
      if (UnityEngine.Input.GetKeyDown((KeyCode) 51))
        Manager.Config.HData.Visible = !Manager.Config.HData.Visible;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 52))
        Manager.Config.HData.Son = !Manager.Config.HData.Son;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 53))
        Manager.Config.HData.Cloth = !Manager.Config.HData.Cloth;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 54))
        Manager.Config.HData.Accessory = !Manager.Config.HData.Accessory;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 55))
        Manager.Config.HData.Shoes = !Manager.Config.HData.Shoes;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 56))
        Manager.Config.GraphicData.SimpleBody = !Manager.Config.GraphicData.SimpleBody;
      if (UnityEngine.Input.GetKeyDown((KeyCode) 116))
        this.sprite.ChangeStateAllEquip();
    }
    if (UnityEngine.Input.GetKeyDown((KeyCode) 57))
      Manager.Config.HData.Siru = (Manager.Config.HData.Siru + 1) % 3;
    if (!UnityEngine.Input.GetKey((KeyCode) 304) && !UnityEngine.Input.GetKey((KeyCode) 303))
    {
      if (UnityEngine.Input.GetKeyDown((KeyCode) 48))
      {
        byte lv = !GlobalMethod.CheckFlagsArray(new bool[5]
        {
          this.chaFemales[0].GetSiruFlag(ChaFileDefine.SiruParts.SiruKao) != (byte) 0,
          this.chaFemales[0].GetSiruFlag(ChaFileDefine.SiruParts.SiruFrontTop) != (byte) 0,
          this.chaFemales[0].GetSiruFlag(ChaFileDefine.SiruParts.SiruFrontBot) != (byte) 0,
          this.chaFemales[0].GetSiruFlag(ChaFileDefine.SiruParts.SiruBackTop) != (byte) 0,
          this.chaFemales[0].GetSiruFlag(ChaFileDefine.SiruParts.SiruBackBot) != (byte) 0
        }, 1) ? (byte) 2 : (byte) 0;
        if (Object.op_Implicit((Object) this.chaFemales[0]) && this.chaFemales[0].visibleAll && Object.op_Inequality((Object) this.chaFemales[0].objBody, (Object) null))
        {
          for (ChaFileDefine.SiruParts parts = ChaFileDefine.SiruParts.SiruKao; parts < (ChaFileDefine.SiruParts.SiruFrontTop | ChaFileDefine.SiruParts.SiruBackBot); ++parts)
            this.chaFemales[0].SetSiruFlag(parts, lv);
        }
      }
    }
    else if (Object.op_Implicit((Object) this.chaFemales[1]) && this.chaFemales[1].visibleAll && (Object.op_Inequality((Object) this.chaFemales[1].objBody, (Object) null) && UnityEngine.Input.GetKeyDown((KeyCode) 48)))
    {
      byte lv = !GlobalMethod.CheckFlagsArray(new bool[5]
      {
        this.chaFemales[1].GetSiruFlag(ChaFileDefine.SiruParts.SiruKao) != (byte) 0,
        this.chaFemales[1].GetSiruFlag(ChaFileDefine.SiruParts.SiruFrontTop) != (byte) 0,
        this.chaFemales[1].GetSiruFlag(ChaFileDefine.SiruParts.SiruFrontBot) != (byte) 0,
        this.chaFemales[1].GetSiruFlag(ChaFileDefine.SiruParts.SiruBackTop) != (byte) 0,
        this.chaFemales[1].GetSiruFlag(ChaFileDefine.SiruParts.SiruBackBot) != (byte) 0
      }, 1) ? (byte) 2 : (byte) 0;
      for (ChaFileDefine.SiruParts parts = ChaFileDefine.SiruParts.SiruKao; parts < (ChaFileDefine.SiruParts.SiruFrontTop | ChaFileDefine.SiruParts.SiruBackBot); ++parts)
        this.chaFemales[1].SetSiruFlag(parts, lv);
    }
    if (UnityEngine.Input.GetKeyDown((KeyCode) 97))
      Manager.Config.HData.FeelingGauge = !Manager.Config.HData.FeelingGauge;
    if (UnityEngine.Input.GetKeyDown((KeyCode) 115))
      Manager.Config.HData.MenuIcon = !Manager.Config.HData.MenuIcon;
    if (UnityEngine.Input.GetKeyDown((KeyCode) 100))
      Manager.Config.HData.FinishButton = !Manager.Config.HData.FinishButton;
    if (UnityEngine.Input.GetKeyDown((KeyCode) 102))
      Manager.Config.HData.InitCamera = !Manager.Config.HData.InitCamera;
    if (UnityEngine.Input.GetKeyDown((KeyCode) 122))
      Manager.Config.ActData.Look = !Manager.Config.ActData.Look;
    if (UnityEngine.Input.GetKeyDown((KeyCode) 120))
      this.isTuyaOn = !this.isTuyaOn;
    if (UnityEngine.Input.GetKeyDown((KeyCode) 99))
      Manager.Config.HData.ActionGuide = !Manager.Config.HData.ActionGuide;
    return true;
  }

  private bool GetAutoAnimation(bool _isFirst)
  {
    this.autoMotion = this.ctrlAuto.GetAnimation(this.lstAnimInfo, this.ctrlFlag.initiative, _isFirst);
    if (this.autoMotion == null || this.lstAnimInfo.Length <= this.autoMotion.mode || this.autoMotion.mode == -1)
      return false;
    foreach (HScene.AnimationListInfo animationListInfo in this.lstAnimInfo[this.autoMotion.mode])
    {
      if (animationListInfo.id == this.autoMotion.id)
      {
        this.ctrlFlag.selectAnimationListInfo = animationListInfo;
        break;
      }
    }
    return true;
  }

  private int GetAnimationListModeFromSelectInfo(HScene.AnimationListInfo _info)
  {
    if (_info == null)
      return -1;
    int num = -1;
    for (int index1 = 0; index1 < this.lstAnimInfo.Length; ++index1)
    {
      for (int index2 = 0; index2 < this.lstAnimInfo[index1].Count; ++index2)
      {
        if (this.lstAnimInfo[index1][index2] == _info)
        {
          num = index1;
          break;
        }
      }
    }
    return num;
  }

  private bool SetStartVoice()
  {
    if (this.hSceneManager.EventKind != HSceneManager.HEvent.Normal)
      return false;
    this.ctrlFlag.voice.playStart = 0;
    return true;
  }

  private void SetClothStateStartMotion(int _cha, HScene.AnimationListInfo info)
  {
    if (this.chaFemales.Length <= _cha)
      return;
    byte femaleUpperCloth = (byte) info.nFemaleUpperCloths[_cha];
    byte femaleLowerCloth = (byte) info.nFemaleLowerCloths[_cha];
    GlobalMethod.SetAllClothState(this.chaFemales[_cha], true, (int) femaleUpperCloth, false);
    GlobalMethod.SetAllClothState(this.chaFemales[_cha], false, (int) femaleLowerCloth, false);
  }

  private bool ReturnToNormalFromTheAuto()
  {
    int modeFromSelectInfo = this.GetAnimationListModeFromSelectInfo(this.ctrlFlag.nowAnimationInfo);
    if (modeFromSelectInfo != -1)
    {
      foreach (HScene.AnimationListInfo animationListInfo in this.lstAnimInfo[modeFromSelectInfo])
      {
        if (animationListInfo.id == this.ctrlFlag.nowAnimationInfo.nBackInitiativeID)
        {
          this.ctrlFlag.selectAnimationListInfo = animationListInfo;
          break;
        }
      }
    }
    return true;
  }

  private void EndProcADV()
  {
    if (!this.hSceneManager.bMerchant)
    {
      switch (this.hSceneManager.EventKind)
      {
        case HSceneManager.HEvent.Yobai:
          Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
          this.hSceneManager.HSceneUISet.SetActive(false);
          if (new ShuffleRand(100).Get() < this.ctrlFlag.YobaiBareRate)
          {
            this._bareYobai = true;
            this.openData.FindLoad("11", this.packData.charaID, this.packData.adv_category);
            this.packData.onComplete = (System.Action) (() => this.CloseADV());
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
            break;
          }
          this._bareYobai = false;
          int personality = 0;
          if (Object.op_Implicit((Object) ((Component) this.hSceneManager.females[0]).GetComponent<PlayerActor>()))
            personality = -100;
          else if (Object.op_Equality((Object) ((Component) this.hSceneManager.females[0]).GetComponent<MerchantActor>(), (Object) null))
            personality = this.hSceneManager.females[0].ChaControl.fileParam.personality;
          Singleton<Game>.Instance.GetExpression(personality, "標準（目閉じ）")?.Change(this.hSceneManager.females[0].ChaControl);
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (System.Action<M0>) (_ => this.CloseADV()));
          break;
        case HSceneManager.HEvent.Bath:
        case HSceneManager.HEvent.Toilet1:
        case HSceneManager.HEvent.Toilet2:
        case HSceneManager.HEvent.ShagmiBare:
        case HSceneManager.HEvent.Back:
        case HSceneManager.HEvent.Kitchen:
        case HSceneManager.HEvent.Tachi:
        case HSceneManager.HEvent.Stairs:
        case HSceneManager.HEvent.StairsBare:
        case HSceneManager.HEvent.MapBath:
        case HSceneManager.HEvent.KabeanaBack:
        case HSceneManager.HEvent.KabeanaFront:
        case HSceneManager.HEvent.Neonani:
        case HSceneManager.HEvent.TsukueBare:
          if (!this.ctrlFlag.isFaintness)
            this.openData.FindLoad("9", this.packData.charaID, this.packData.adv_category);
          else
            this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
          this.packData.onComplete = (System.Action) (() => this.CloseADV());
          Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
          Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          break;
        case HSceneManager.HEvent.GyakuYobai:
          if (!HSceneManager.SleepStart)
          {
            if (!this.ctrlFlag.isFaintness)
              this.openData.FindLoad("7", this.packData.charaID, this.packData.adv_category);
            else
              this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
          }
          else if (!this.ctrlFlag.isFaintness)
            this.openData.FindLoad("13", this.packData.charaID, this.packData.adv_category);
          else
            this.openData.FindLoad("14", this.packData.charaID, this.packData.adv_category);
          this.packData.onComplete = (System.Action) (() => this.CloseADV());
          Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
          Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          break;
        default:
          if (this.hSceneManager.EventKind != HSceneManager.HEvent.FromFemale || this.hSceneManager.nInvitePtn != -1)
          {
            if (Object.op_Equality((Object) this.hSceneManager.females[1], (Object) null) || Object.op_Inequality((Object) ((Component) this.hSceneManager.females[1]).GetComponent<PlayerActor>(), (Object) null))
            {
              bool flag1 = this.hSceneManager.femalePlayerFinish == 0 && this.hSceneManager.maleFinish == 0 && this.ctrlFlag.numOrgasmTotal == 0;
              bool flag2 = this.ctrlFlag.isJudgeSelect.Contains(HSceneFlagCtrl.JudgeSelect.Constraint);
              bool flag3 = this.ctrlFlag.isJudgeSelect.Contains(HSceneFlagCtrl.JudgeSelect.Pain);
              if ((flag2 || flag3) && (this.hSceneManager.isForce || this.hSceneManager.GetFlaverSkillLevel(2) < 170))
              {
                if (flag3)
                {
                  if (!this.ctrlFlag.isFaintness)
                  {
                    this.openData.FindLoad("5", this.packData.charaID, this.packData.adv_category);
                    this.packData.isPainAction = this.ctrlFlag.isPainAction;
                  }
                  else
                    this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
                  this.packData.ConditionMode = 2;
                  this.packData.onComplete = (System.Action) (() => this.CloseADV());
                  Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.Agent[0]);
                  ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
                  ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
                  ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
                  ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
                  Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
                  break;
                }
                if (!flag2)
                  break;
                if (!this.ctrlFlag.isFaintness)
                {
                  this.openData.FindLoad("12", this.packData.charaID, this.packData.adv_category);
                  this.packData.isConstraintAction = this.ctrlFlag.isConstraintAction;
                }
                else
                  this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
                this.packData.ConditionMode = 2;
                this.packData.onComplete = (System.Action) (() => this.CloseADV());
                Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.Agent[0]);
                ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
                ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
                ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
                ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
                Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
                break;
              }
              if (!this.ctrlFlag.isNotCtrl && !flag1)
              {
                if (this.hSceneManager.femalePlayerFinish > 0 || this.hSceneManager.maleFinish > 0)
                {
                  if (this.ctrlFlag.numOrgasmTotal == 0)
                  {
                    if (!this.ctrlFlag.isFaintness)
                      this.openData.FindLoad("3", this.packData.charaID, this.packData.adv_category);
                    else
                      this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
                    this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
                    this.packData.onComplete = (System.Action) (() => this.CloseADV());
                    Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.Agent[0]);
                    ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
                    ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
                    ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
                    ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
                    Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
                    break;
                  }
                  if (!this.ctrlFlag.isFaintness)
                  {
                    this.openData.FindLoad("0", this.packData.charaID, this.packData.adv_category);
                    this.packData.numOrgasmFemale = this.ctrlFlag.numOrgasmTotal;
                    this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
                    this.packData.onComplete = (System.Action) (() => this.CloseADV());
                    Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.Agent[0]);
                    ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
                    ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
                    ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
                    ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
                    Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
                    break;
                  }
                  this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
                  this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
                  this.packData.onComplete = (System.Action) (() => this.CloseADV());
                  Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.Agent[0]);
                  ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
                  ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
                  ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
                  ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
                  Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
                  break;
                }
                if (!this.ctrlFlag.isFaintness)
                  this.openData.FindLoad("2", this.packData.charaID, this.packData.adv_category);
                else
                  this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
                this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
                this.packData.onComplete = (System.Action) (() => this.CloseADV());
                Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.Agent[0]);
                ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
                ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
                ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
                ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
                Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
                break;
              }
              this.openData.FindLoad("4", this.packData.charaID, this.packData.adv_category);
              this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
              this.packData.onComplete = (System.Action) (() => this.CloseADV());
              Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
              ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
              ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
              ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
              ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
              Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
              break;
            }
            if (!Object.op_Inequality((Object) this.hSceneManager.females[1], (Object) null) || !Object.op_Equality((Object) ((Component) this.hSceneManager.females[1]).GetComponent<PlayerActor>(), (Object) null))
              break;
            if (!this.ctrlFlag.isFaintness)
              this.openData.FindLoad("10", this.packData.charaID, this.packData.adv_category);
            else
              this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
            this.packData.onComplete = (System.Action) (() => this.CloseADV());
            Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
            ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
            ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
            ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
            ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
            break;
          }
          if (this.hSceneManager.EventKind == HSceneManager.HEvent.FromFemale || this.hSceneManager.nInvitePtn == 0)
          {
            if (!this.ctrlFlag.isFaintness)
              this.openData.FindLoad("6", this.packData.charaID, this.packData.adv_category);
            else
              this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
            this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
            this.packData.onComplete = (System.Action) (() => this.CloseADV());
            Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
            ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
            ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
            ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
            ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
            Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
            break;
          }
          if (this.hSceneManager.nInvitePtn != 1)
            break;
          if (!this.ctrlFlag.isFaintness)
            this.openData.FindLoad("8", this.packData.charaID, this.packData.adv_category);
          else
            this.openData.FindLoad("1", this.packData.charaID, this.packData.adv_category);
          this.packData.ConditionMode = !this.hSceneManager.isForce ? (this.hSceneManager.Agent[0].ChaControl.fileGameInfo.phase >= 2 ? 1 : 0) : 2;
          this.packData.onComplete = (System.Action) (() => this.CloseADV());
          Manager.ADV.ChangeADVCamera((Actor) ((Component) this.hSceneManager.Agent[0]).GetComponent<AgentActor>());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localPosition(Vector3.get_zero());
          ((Component) this.hSceneManager.females[0].ChaControl).get_transform().set_localRotation(Quaternion.get_identity());
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_position(this.hSceneManager.females[0].Position);
          ((Component) this.hSceneManager.females[0].ChaControl.animBody).get_transform().set_rotation(this.hSceneManager.females[0].Rotation);
          Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
          break;
      }
    }
    else
    {
      this.openData.FindLoad("0", this.packData.charaID, this.packData.adv_category);
      this.packData.onComplete = (System.Action) (() => this.CloseADV());
      Manager.ADV.ChangeADVCamera((Actor) this.hSceneManager.merchantActor);
      this.packData.JumpTag = "OUT";
      this.packData.isWeaknessH = this.ctrlFlag.isFaintness;
      Singleton<MapUIContainer>.Instance.OpenADV(this.openData, (IPack) this.packData);
    }
  }

  private void CloseADV()
  {
    if (this.packData != null)
      this.packData.Release();
    int num = Manager.Config.GraphicData.MaxCharaNum - 1;
    if (!MapScene.EqualsSequence(this.prevCharaEntry, Manager.Config.GraphicData.CharasEntry))
      Singleton<Manager.Map>.Instance.ApplyConfig((System.Action) (() => this.CloseADVProc()), (System.Action) null);
    else
      this.CloseADVProc();
  }

  private void CloseADVProc()
  {
    for (int index = 0; index < this.hSceneManager.females.Length; ++index)
      this.hSceneManager.females[index] = (Actor) null;
    this.hSceneManager.male = (Actor) null;
    MerchantActor merchant = Singleton<Manager.Map>.Instance.Merchant;
    if (this.hSceneManager.Player.CameraControl.Mode != CameraMode.Normal)
      this.hSceneManager.Player.CameraControl.Mode = CameraMode.Normal;
    AnimatorStateInfo animatorStateInfo1 = this.hSceneManager.Player.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
    int shortNameHash1 = ((AnimatorStateInfo) ref animatorStateInfo1).get_shortNameHash();
    this.hSceneManager.Player.ChaControl.SetAccessoryStateAll(true);
    this.hSceneManager.Player.ChaControl.SetClothesStateAll((byte) 0);
    ((Behaviour) this.hSceneManager.Player.Animation).set_enabled(true);
    this.hSceneManager.Player.Animation.SetAnimatorController(this.hSceneManager.Player.ChaControl.animBody.get_runtimeAnimatorController());
    this.hSceneManager.Player.Animation.MapIK.Calc(shortNameHash1);
    ((Behaviour) this.hSceneManager.Player.Controller).set_enabled(true);
    ((Behaviour) this.hSceneManager.Player.CameraControl).set_enabled(true);
    if (!this.hSceneManager.Player.NavMeshAgent.get_isOnNavMesh())
      this.hSceneManager.Player.NavMeshWarp(this.hSceneManager.Player.Position, this.hSceneManager.Player.Rotation, 3, 200f);
    ((Component) this.hSceneManager.Player.ChaControl).get_transform().set_position(Vector3.get_zero());
    ((Component) this.hSceneManager.Player.ChaControl).get_transform().set_rotation(Quaternion.get_identity());
    if (Object.op_Inequality((Object) this.hSceneManager.Player.CurrentPoint, (Object) null))
      this.hSceneManager.Player.CurrentPoint.RemoveBookingUser((Actor) this.hSceneManager.Player);
    this.hSceneManager.Player.ReleaseCurrentPoint();
    if (!this.hSceneManager.bMerchant)
    {
      for (int index = 0; index < this.hSceneManager.Agent.Length; ++index)
      {
        if (!Object.op_Equality((Object) this.hSceneManager.Agent[index], (Object) null))
        {
          ((Component) this.hSceneManager.Agent[index].ChaControl).get_transform().set_position(Vector3.get_zero());
          ((Component) this.hSceneManager.Agent[index].ChaControl).get_transform().set_rotation(Quaternion.get_identity());
          this.hSceneManager.Agent[index].ChaControl.ChangeNowCoordinate(true, true);
        }
      }
    }
    else
    {
      ((Component) merchant.ChaControl).get_transform().set_position(Vector3.get_zero());
      ((Component) merchant.ChaControl).get_transform().set_rotation(Quaternion.get_identity());
    }
    if (!this.hSceneManager.Player.ChaControl.visibleAll)
      this.hSceneManager.Player.ChaControl.visibleAll = true;
    this.hSceneManager.Player.Controller.ChangeState("Normal");
    if (this.hSceneManager.Player.ChaControl.sex == (byte) 1 && !this.hSceneManager.bFutanari)
      this.hSceneManager.Player.ChaControl.ChangeNowCoordinate(true, true);
    if (Object.op_Inequality((Object) this.hSceneManager.Player.Partner, (Object) null))
    {
      AgentActor component = (AgentActor) ((Component) this.hSceneManager.Player.Partner).GetComponent<AgentActor>();
      component.DeactivatePairing(0);
      component.ActivateHoldingHands(0, false);
    }
    int num = 0;
    using (IEnumerator<AgentActor> enumerator = Singleton<Manager.Map>.Instance.AgentTable.get_Values().GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        AgentActor current = enumerator.Current;
        if (!Object.op_Equality((Object) current, (Object) null) && !this.hSceneManager.ReturnActionTypes.ContainsKey(current))
        {
          animatorStateInfo1 = current.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
          int shortNameHash2 = ((AnimatorStateInfo) ref animatorStateInfo1).get_shortNameHash();
          ((Behaviour) current.AnimationAgent).set_enabled(true);
          current.AnimationAgent.StopAllAnimCoroutine();
          current.AnimationAgent.EndIgnoreExpression();
          current.AnimationAgent.ResetDefaultAnimatorController();
          current.AnimationAgent.SetAnimatorController(current.ChaControl.animBody.get_runtimeAnimatorController());
          current.AnimationAgent.MapIK.Calc(shortNameHash2);
          int desireKey = Desire.GetDesireKey(Desire.Type.H);
          current.SetDesire(desireKey, 0.0f);
          this.EmptyImmoral(current);
          ((Behaviour) current.Controller).set_enabled(true);
          current.AnimationAgent.EnableItems();
          current.EnableBehavior();
          ((Behaviour) current.LocomotorAgent).set_enabled(true);
          current.ActivateNavMeshAgent();
          if (!current.NavMeshAgent.get_isOnNavMesh())
          {
            Vector3 position = current.Position;
            current.NavMeshWarp(position, current.Rotation, num++, 200f);
          }
          current.ChaControl.visibleAll = true;
          current.ResetActionFlag();
          if (this.hSceneManager.EventKind == HSceneManager.HEvent.Yobai)
          {
            if (this._bareYobai)
              current.ChangeBehavior(Desire.ActionType.Normal);
            else
              current.RecoverAction();
          }
          else if (this.hSceneManager.endStatus == (byte) 1 && !this.ctrlFlag.nowHPoint._nPlace.Exists<int, ValueTuple<int, int>>((Predicate<KeyValuePair<int, ValueTuple<int, int>>>) (x => x.Value.Item1 == 11 || x.Value.Item1 == 13 || x.Value.Item1 == 14)))
            current.StartWeakness();
          else
            current.ChangeBehavior(Desire.ActionType.Normal);
        }
      }
    }
    if (this.hSceneManager.bMerchant)
    {
      AnimatorStateInfo animatorStateInfo2 = merchant.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
      int shortNameHash2 = ((AnimatorStateInfo) ref animatorStateInfo2).get_shortNameHash();
      ((Behaviour) merchant.AnimationMerchant).set_enabled(true);
      merchant.AnimationMerchant.EndIgnoreExpression();
      merchant.AnimationMerchant.SetAnimatorController(merchant.ChaControl.animBody.get_runtimeAnimatorController());
      merchant.AnimationMerchant.MapIK.Calc(shortNameHash2);
      ((Behaviour) merchant.Controller).set_enabled(true);
      ((Behaviour) merchant.LocomotorMerchant).set_enabled(true);
      merchant.EnableBehavior();
      merchant.ChangeBehavior(merchant.LastNormalMode);
      merchant.ChaControl.ChangeNowCoordinate(true, true);
    }
    ((Behaviour) this.hSceneManager.Player).set_enabled(true);
    ((Behaviour) this.hSceneManager.Player.HandsHolder).set_enabled(this.hSceneManager.handsIK);
    this.hSceneManager.Player.SetActiveOnEquipedItem(true);
    foreach (AgentActor agentActor in this.hSceneManager.Agent)
    {
      if (!Object.op_Equality((Object) agentActor, (Object) null))
        agentActor.SetActiveOnEquipedItem(true);
    }
    if (this.hSceneManager.HSceneSet.get_activeSelf())
      this.hSceneManager.HSceneSet.SetActive(false);
    Singleton<Manager.ADV>.Instance.Captions.EndADV((System.Action) null);
    Singleton<MapUIContainer>.Instance.MinimapUI.MiniMap.SetActive(true);
    Singleton<MapUIContainer>.Instance.MinimapUI.MiniMapIcon.SetActive(true);
    Singleton<Manager.Map>.Instance.SetActiveMapEffect(true);
    MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (System.Action<M0>) (_ => MapUIContainer.SetVisibleHUD(true)));
  }

  private RuntimeAnimatorController MixRuntimeControler(
    RuntimeAnimatorController src,
    RuntimeAnimatorController over1,
    RuntimeAnimatorController over2)
  {
    if (Object.op_Equality((Object) src, (Object) null) || Object.op_Equality((Object) over1, (Object) null) || Object.op_Equality((Object) over2, (Object) null))
      return (RuntimeAnimatorController) null;
    AnimatorOverrideController overrideController = new AnimatorOverrideController(src);
    List<AnimationClip> animationClipList = new List<AnimationClip>();
    animationClipList.AddRange((IEnumerable<AnimationClip>) ((RuntimeAnimatorController) new AnimatorOverrideController(over1)).get_animationClips());
    animationClipList.AddRange((IEnumerable<AnimationClip>) ((RuntimeAnimatorController) new AnimatorOverrideController(over2)).get_animationClips());
    using (List<AnimationClip>.Enumerator enumerator = animationClipList.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        AnimationClip current = enumerator.Current;
        overrideController.set_Item(((Object) current).get_name(), current);
      }
    }
    ((Object) overrideController).set_name(((Object) over1).get_name());
    return (RuntimeAnimatorController) overrideController;
  }

  private void EndAnimChange()
  {
    List<HScene.EndMotion> lstEndAnimInfo = Singleton<Resources>.Instance.HSceneTable.lstEndAnimInfo;
    HScene.AnimationListInfo nowAnimationInfo = this.ctrlFlag.nowAnimationInfo;
    int index = !Object.op_Equality((Object) ((Component) this.hSceneManager.females[0]).GetComponent<MerchantActor>(), (Object) null) ? 5 : this.chaFemales[0].fileParam.personality;
    AnimatorStateInfo animatorStateInfo1;
    if (!this.hSceneManager.bMerchant)
    {
      this.chaFemales[0].LoadAnimation(lstEndAnimInfo[index].abNameH[0], lstEndAnimInfo[index].assetNameH[0], string.Empty);
      this.chaFemales[0].setPlay(lstEndAnimInfo[index].stateNameH[0], 0);
      AnimatorStateInfo animatorStateInfo2 = this.chaFemales[0].animBody.GetCurrentAnimatorStateInfo(0);
      ((ActorAnimation) ((Component) this.chaFemales[0].animBody).GetComponent<ActorAnimationAgent>()).MapIK.Calc(((AnimatorStateInfo) ref animatorStateInfo2).get_shortNameHash());
    }
    else
    {
      this.chaFemales[0].LoadAnimation(lstEndAnimInfo[index].abNameM, lstEndAnimInfo[index].assetNameM, string.Empty);
      this.chaFemales[0].setPlay(lstEndAnimInfo[index].stateNameM, 0);
      animatorStateInfo1 = this.chaFemales[0].animBody.GetCurrentAnimatorStateInfo(0);
      ((ActorAnimation) ((Component) this.chaFemales[0].animBody).GetComponent<ActorAnimationMerchant>()).MapIK.Calc(((AnimatorStateInfo) ref animatorStateInfo1).get_shortNameHash());
    }
    if (Object.op_Inequality((Object) this.chaFemales[1], (Object) null) && Object.op_Inequality((Object) this.chaFemales[1].objTop, (Object) null) && Object.op_Inequality((Object) this.chaFemales[1], (Object) this.hSceneManager.Player.ChaControl))
    {
      this.chaFemales[1].LoadAnimation(lstEndAnimInfo[index].abNameH[1], lstEndAnimInfo[index].assetNameH[1], string.Empty);
      this.chaFemales[1].setPlay(lstEndAnimInfo[index].stateNameH[1], 0);
      animatorStateInfo1 = this.chaFemales[1].animBody.GetCurrentAnimatorStateInfo(0);
      ((ActorAnimation) ((Component) this.chaFemales[1].animBody).GetComponent<ActorAnimationAgent>()).MapIK.Calc(((AnimatorStateInfo) ref animatorStateInfo1).get_shortNameHash());
    }
    this.hSceneManager.Player.ChaControl.LoadAnimation(lstEndAnimInfo[index].abNameP[(int) this.hSceneManager.Player.ChaControl.sex], lstEndAnimInfo[index].assetNameP[(int) this.hSceneManager.Player.ChaControl.sex], string.Empty);
    this.hSceneManager.Player.ChaControl.setPlay(lstEndAnimInfo[index].stateNameP[(int) this.hSceneManager.Player.ChaControl.sex], 0);
    animatorStateInfo1 = this.hSceneManager.Player.ChaControl.animBody.GetCurrentAnimatorStateInfo(0);
    ((ActorAnimation) ((Component) this.hSceneManager.Player.ChaControl.animBody).GetComponent<ActorAnimationPlayer>()).MapIK.Calc(((AnimatorStateInfo) ref animatorStateInfo1).get_shortNameHash());
    this.fade.FadeStart(1f);
  }

  [DebuggerHidden]
  private IEnumerator BeforeWait()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HScene.\u003CBeforeWait\u003Ec__Iterator3()
    {
      \u0024this = this
    };
  }

  public void LoadMoveOffset(string file, out Vector3 pos, out Vector3 rot)
  {
    string[][] data;
    GlobalMethod.GetListString(GlobalMethod.LoadAllListText(this.hSceneManager.strAssetMoveOffsetListFolder, file, (List<string>) null), out data);
    pos = Vector3.get_zero();
    rot = Vector3.get_zero();
    int length = data.GetLength(0);
    for (int index1 = 0; index1 < length; ++index1)
    {
      int num1 = 0;
      ref Vector3 local1 = ref pos;
      string[] strArray1 = data[index1];
      int index2 = num1;
      int num2 = index2 + 1;
      double num3 = (double) float.Parse(strArray1[index2]);
      local1.x = (__Null) num3;
      ref Vector3 local2 = ref pos;
      string[] strArray2 = data[index1];
      int index3 = num2;
      int num4 = index3 + 1;
      double num5 = (double) float.Parse(strArray2[index3]);
      local2.y = (__Null) num5;
      ref Vector3 local3 = ref pos;
      string[] strArray3 = data[index1];
      int index4 = num4;
      int num6 = index4 + 1;
      double num7 = (double) float.Parse(strArray3[index4]);
      local3.z = (__Null) num7;
      ref Vector3 local4 = ref rot;
      string[] strArray4 = data[index1];
      int index5 = num6;
      int num8 = index5 + 1;
      double num9 = (double) float.Parse(strArray4[index5]);
      local4.x = (__Null) num9;
      ref Vector3 local5 = ref rot;
      string[] strArray5 = data[index1];
      int index6 = num8;
      int num10 = index6 + 1;
      double num11 = (double) float.Parse(strArray5[index6]);
      local5.y = (__Null) num11;
      ref Vector3 local6 = ref rot;
      string[] strArray6 = data[index1];
      int index7 = num10;
      int num12 = index7 + 1;
      double num13 = (double) float.Parse(strArray6[index7]);
      local6.z = (__Null) num13;
    }
  }

  private void EmptyImmoral(AgentActor agent)
  {
    agent.SetDefaultImmoral();
  }

  public ChaControl[] GetFemales()
  {
    return this.chaFemales;
  }

  private void HousingVanishSet()
  {
    if (!Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.PointAgent, (Object) null))
      return;
    BasePoint[] basePoints = Singleton<Manager.Map>.Instance.PointAgent.BasePoints;
    if (basePoints == null)
      return;
    int mapId = Singleton<Manager.Map>.Instance.MapID;
    int index1 = -1;
    Dictionary<int, Dictionary<int, List<int>>> housingAreaGroup = Singleton<Resources>.Instance.Map.VanishHousingAreaGroup;
    Dictionary<int, List<int>> dictionary;
    if (housingAreaGroup == null || !housingAreaGroup.TryGetValue(mapId, out dictionary))
      return;
    foreach (KeyValuePair<int, List<int>> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.Contains(this.hSceneManager.Player.AreaID))
      {
        index1 = keyValuePair.Key;
        break;
      }
    }
    if (index1 < 0)
      return;
    for (int index2 = 0; index2 < basePoints.Length; ++index2)
    {
      if (!Object.op_Equality((Object) basePoints[index2].OwnerArea, (Object) null) && dictionary[index1].Contains(basePoints[index2].OwnerArea.AreaID) && basePoints[index2].ID >= 0)
      {
        Singleton<Housing>.Instance.StartShield(basePoints[index2].ID);
        break;
      }
    }
  }

  [DebuggerHidden]
  private IEnumerator PutHmesh()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new HScene.\u003CPutHmesh\u003Ec__Iterator4()
    {
      \u0024this = this
    };
  }

  private void PutHmesh(
    ref Vector3 allNearestVertex,
    ref float allMiniDistanceSqr,
    HMeshData hMeshData,
    int areaid,
    ref string tag)
  {
    Collider[] colliderArray;
    if (Object.op_Equality((Object) hMeshData, (Object) null) || !hMeshData.dicColliders.TryGetValue(areaid, out colliderArray))
      colliderArray = (Collider[]) null;
    Vector3 position = this.hSceneManager.females[0].Position;
    StringBuilder toRelease = StringBuilderPool.Get();
    if (colliderArray != null)
    {
      foreach (Collider collider in colliderArray)
      {
        toRelease.Clear();
        toRelease.Append(((Component) collider).get_gameObject().get_tag());
        if (!(toRelease.ToString() == "Untagged"))
        {
          if (!this.hSceneManager.bMerchant)
          {
            if (this.hSceneManager.EventKind == HSceneManager.HEvent.FromFemale)
            {
              if ((Singleton<Manager.Map>.Instance.Player.ChaControl.sex == (byte) 0 || Singleton<Manager.Map>.Instance.Player.ChaControl.sex == (byte) 1 && Singleton<HSceneManager>.Instance.bFutanari) && (toRelease.ToString() != "standfloor" && toRelease.ToString() != "bed"))
                continue;
            }
            else if (this.hSceneManager.EventKind == HSceneManager.HEvent.Yobai && toRelease.ToString() != "standfloor" && (toRelease.ToString() != "bed" && toRelease.ToString() != "sofabed"))
              continue;
            if (Singleton<Manager.Map>.Instance.Player.ChaControl.sex == (byte) 1 && !Singleton<HSceneManager>.Instance.bFutanari && !HSceneManager.HmeshLesTag.ContainsKey(toRelease.ToString()) || this.hSceneManager.EventKind != HSceneManager.HEvent.GyakuYobai && HSceneManager.SleepStart && (toRelease.ToString() != "standfloor" && toRelease.ToString() != "bed"))
              continue;
          }
          else if ("sofa" == toRelease.ToString() || !HSceneManager.HmeshTag.ContainsKey(toRelease.ToString()) || HSceneManager.HmeshTag[toRelease.ToString()] > 4)
            continue;
          Mesh sharedMesh = ((MeshCollider) ((Component) collider).GetComponent<MeshCollider>()).get_sharedMesh();
          float num = float.PositiveInfinity;
          Vector3 vector3_1 = Vector3.get_zero();
          foreach (Vector3 vertex in sharedMesh.get_vertices())
          {
            if (vertex.y <= (!this.hSceneManager.bMerchant ? this.hSceneManager.Agent[0].ChaControl.objHeadBone.get_transform().get_position().y : this.hSceneManager.merchantActor.ChaControl.objHeadBone.get_transform().get_position().y))
            {
              Vector3 vector3_2;
              ((Vector3) ref vector3_2).\u002Ector((float) vertex.x, (float) vertex.y, (float) vertex.z);
              if (Singleton<Manager.Map>.Instance.MapID == 0 && (areaid == 11 || areaid == 12 || areaid == 13))
              {
                Transform transform = ((Component) collider).get_transform();
                vector3_2.x = vector3_2.x * transform.get_localScale().x;
                vector3_2.y = vector3_2.y * transform.get_localScale().y;
                vector3_2.z = vector3_2.z * transform.get_localScale().z;
                vector3_2 = Quaternion.op_Multiply(transform.get_rotation(), vector3_2);
                vector3_2 = Vector3.op_Addition(vector3_2, transform.get_position());
              }
              Vector3 vector3_3 = Vector3.op_Subtraction(position, vector3_2);
              float sqrMagnitude = ((Vector3) ref vector3_3).get_sqrMagnitude();
              if ((double) sqrMagnitude < (double) num)
              {
                num = sqrMagnitude;
                vector3_1 = vector3_2;
              }
            }
          }
          if ((double) num < (double) allMiniDistanceSqr)
          {
            allNearestVertex = vector3_1;
            tag = ((Component) collider).get_gameObject().get_tag();
            allMiniDistanceSqr = num;
          }
        }
      }
    }
    StringBuilderPool.Release(toRelease);
  }

  public ProcBase GetProcBase()
  {
    return this.mode == -1 || this.lstProc.Count < this.mode ? (ProcBase) null : this.lstProc[this.mode];
  }

  private void PlayerWet()
  {
    if (Object.op_Equality((Object) this.hSceneManager.Player, (Object) null))
      return;
    PlayerData playerData = this.hSceneManager.Player.PlayerData;
    StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
    EnvironmentSimulator simulator = Singleton<Manager.Map>.Instance.Simulator;
    if (simulator.EnabledTimeProgression)
    {
      Weather weather = simulator.Weather;
      if (this.hSceneManager.Player.AreaType == MapArea.AreaType.Indoor)
      {
        playerData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
      }
      else
      {
        switch (weather)
        {
          case Weather.Rain:
            playerData.Wetness += statusProfile.WetRateInRain * Time.get_deltaTime();
            break;
          case Weather.Storm:
            playerData.Wetness += statusProfile.WetRateInStorm * Time.get_deltaTime();
            break;
          default:
            playerData.Wetness += statusProfile.DrySpeed * Time.get_deltaTime();
            break;
        }
        playerData.Wetness = Mathf.Clamp(playerData.Wetness, 0.0f, 100f);
      }
    }
    if (!Object.op_Inequality((Object) this.hSceneManager.Player.ChaControl, (Object) null))
      return;
    this.hSceneManager.Player.ChaControl.wetRate = Mathf.InverseLerp(0.0f, 100f, playerData.Wetness);
  }

  public ChaControl[] GetMales()
  {
    return this.chaMales;
  }

  [Serializable]
  public class LightInfo
  {
    [Tooltip("初期のライトの回転")]
    public Quaternion initRot = Quaternion.get_identity();
    [Tooltip("初期の強さ")]
    [Space]
    public float initIntensity = 1f;
    [Tooltip("強さの最大値")]
    [Range(0.0f, 2f)]
    public float maxIntensity = 2f;
    [Tooltip("初期の色")]
    public Color initColor = Color.get_white();
    [Tooltip("回すキャラライトオブジェクト")]
    public GameObject objCharaLight;
    [Tooltip("キャラライト")]
    public Light light;
    [Tooltip("強さの最小値")]
    [Range(0.0f, 2f)]
    public float minIntensity;
  }

  [Serializable]
  public class AnimationListInfo
  {
    public int id = -1;
    public string nameAnimation = string.Empty;
    public string assetpathBaseM = string.Empty;
    public string assetBaseM = string.Empty;
    public string assetpathMale = string.Empty;
    public string fileMale = string.Empty;
    public string assetpathBaseF = string.Empty;
    public string assetBaseF = string.Empty;
    public string assetpathBaseF2 = string.Empty;
    public string assetBaseF2 = string.Empty;
    public string assetpathFemale2 = string.Empty;
    public string fileFemale2 = string.Empty;
    public ValueTuple<int, int> ActionCtrl = new ValueTuple<int, int>(-1, -1);
    public List<int> nPositons = new List<int>();
    public List<string> lstOffset = new List<string>();
    public int nIyaAction = 1;
    public int nFemaleProclivity = -1;
    public int nBackInitiativeID = -1;
    public List<int> lstSystem = new List<int>();
    public int nMaleSon = -1;
    public int[] nFemaleUpperCloths = new int[2]{ -1, -1 };
    public int[] nFemaleLowerCloths = new int[2]{ -1, -1 };
    public string fileSiruPasteSecond = string.Empty;
    public int nShortBreahtPlay = 1;
    public HashSet<int> hasVoiceCategory = new HashSet<int>();
    public int nPromiscuity = -1;
    public int nAnimListInfoID = -1;
    public int nBreathID = -1;
    public bool isMaleHitObject;
    public string fileMotionNeckMale;
    public string assetpathFemale;
    public string fileFemale;
    public bool isFemaleHitObject;
    public string fileMotionNeckFemale;
    public string fileMotionNeckFemalePlayer;
    public bool isFemaleHitObject2;
    public string fileMotionNeckFemale2;
    public bool isNeedItem;
    public int nDownPtn;
    public int nFaintnessLimit;
    public int nPhase;
    public int nHentai;
    public bool bSleep;
    public int nInitiativeFemale;
    public int nFeelHit;
    public string nameCamera;
    public string fileSiruPaste;
    public string fileSe;
    public bool bMerchantMotion;
  }

  public class StartMotion
  {
    public int mode;
    public int id;

    public StartMotion(int _mode, int _id)
    {
      this.mode = _mode;
      this.id = _id;
    }
  }

  public class EndMotion
  {
    public string[] abNameP = new string[2];
    public string[] abNameH = new string[2];
    public string abNameM = string.Empty;
    public string[] assetNameP = new string[2];
    public string[] assetNameH = new string[2];
    public string assetNameM = string.Empty;
    public string[] stateNameP = new string[2];
    public string[] stateNameH = new string[2];
    public string stateNameM = string.Empty;
    public int height;

    public EndMotion(int _height, string[] _abName, string[] _assetName, string[] _stateName)
    {
      this.height = _height;
      for (int index1 = 0; index1 < _abName.Length; ++index1)
      {
        int index2 = index1;
        if (index2 < 2)
        {
          this.abNameP[index2] = _abName[index2];
          this.assetNameP[index2] = _assetName[index2];
          this.stateNameP[index2] = _stateName[index2];
        }
        else if (index2 < 4)
        {
          this.abNameH[index2 - 2] = _abName[index2];
          this.assetNameH[index2 - 2] = _assetName[index2];
          this.stateNameH[index2 - 2] = _stateName[index2];
        }
        else
        {
          this.abNameM = _abName[index2];
          this.assetNameM = _assetName[index2];
          this.stateNameM = _stateName[index2];
        }
      }
    }
  }

  [Serializable]
  public struct DeskChairInfo
  {
    public int eventID;
    public int poseID;
  }

  private class PackData : CharaPackData
  {
    public PackData(int charaID, int adv_category)
    {
      this.charaID = charaID;
      this.adv_category = adv_category;
    }

    public int charaID { get; }

    public int adv_category { get; }

    public string JumpTag { get; set; } = string.Empty;

    public bool isWeaknessH { get; set; }

    public int numOrgasmFemale { get; set; }

    public int ConditionMode { get; set; }

    public bool isPainAction { get; set; }

    public bool isConstraintAction { get; set; }

    public override List<Program.Transfer> Create()
    {
      List<Program.Transfer> transferList = base.Create();
      transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isWeaknessH", string.Format("{0}", (object) this.isWeaknessH)));
      transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "string", "JumpTag", this.JumpTag));
      transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "int", "numOrgasmFemale", string.Format("{0}", (object) this.numOrgasmFemale)));
      transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "int", "ConditionMode", string.Format("{0}", (object) this.ConditionMode)));
      transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isPainAction", string.Format("{0}", (object) this.isPainAction)));
      transferList.Add(Program.Transfer.Create(true, ADV.Command.VAR, "bool", "isConstraintAction", string.Format("{0}", (object) this.isConstraintAction)));
      return transferList;
    }

    public override void Receive(TextScenario scenario)
    {
      base.Receive(scenario);
    }
  }
}
