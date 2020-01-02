// Decompiled with JetBrains decompiler
// Type: ProcBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class ProcBase
{
  protected float[] timeChangeMotions = new float[2];
  protected float[] timeChangeMotionDeltaTimes = new float[2];
  protected ShuffleRand[] randVoicePlays = new ShuffleRand[2];
  protected List<Tuple<int, int, MotionIK>> lstMotionIK = new List<Tuple<int, int, MotionIK>>();
  protected BoolReactiveProperty isAtariHit = new BoolReactiveProperty(false);
  protected int CatID = -1;
  protected StringBuilder sbWarning = new StringBuilder();
  protected HSceneFlagCtrl ctrlFlag;
  protected ChaControl[] chaFemales;
  protected ChaControl[] chaMales;
  protected CrossFade fade;
  protected MetaballCtrl ctrlMeta;
  protected HSceneSprite sprite;
  protected HItemCtrl item;
  protected FeelHit feelHit;
  protected HAutoCtrl auto;
  protected HVoiceCtrl voice;
  protected HParticleCtrl particle;
  protected HSeCtrl se;
  protected bool isHeight1Parameter;
  protected ParticleSystem AtariEffect;
  protected ParticleSystem FeelHitEffect3D;
  public static bool endInit;
  private bool isAtariHitOld;
  protected static HSceneManager hSceneManager;
  protected HSceneSpriteHitem Hitem;
  protected AnimatorStateInfo FemaleAi;

  public ProcBase(DeliveryMember _delivery)
  {
    this.ctrlFlag = _delivery.ctrlFlag;
    this.chaMales = _delivery.chaMales;
    this.chaFemales = _delivery.chaFemales;
    this.fade = _delivery.fade;
    this.ctrlMeta = _delivery.ctrlMeta;
    this.sprite = _delivery.sprite;
    this.item = _delivery.item;
    this.feelHit = _delivery.feelHit;
    this.auto = _delivery.auto;
    this.voice = _delivery.voice;
    this.particle = _delivery.particle;
    this.se = _delivery.se;
    this.lstMotionIK = _delivery.lstMotionIK;
    this.AtariEffect = _delivery.AtariEffect;
    this.FeelHitEffect3D = _delivery.FeelHitEffect3D;
    this.Hitem = (HSceneSpriteHitem) this.sprite.objHItem.GetComponent<HSceneSpriteHitem>();
    if (Object.op_Equality((Object) ProcBase.hSceneManager, (Object) null))
      ProcBase.hSceneManager = Singleton<HSceneManager>.Instance;
    for (int index = 0; index < 2; ++index)
    {
      this.randVoicePlays[index] = new ShuffleRand(-1);
      this.randVoicePlays[index].Init(index != 0 ? 2 : 3);
    }
    ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) this.isAtariHit, (Func<M0, bool>) (x => this.isAtariHitOld != x && this.CatID != 1)), (Action<M0>) (x =>
    {
      if (this.CatID == 7 && Singleton<HSceneFlagCtrl>.Instance.nowAnimationInfo.ActionCtrl.Item2 == 1 || Singleton<HSceneFlagCtrl>.Instance.nowAnimationInfo.ActionCtrl.Item2 == 2)
        return;
      this.isAtariHitOld = x;
      if (x)
      {
        this.AtariEffect.Play();
        if (!Singleton<HSceneManager>.Instance.isParticle)
          return;
        this.FeelHitEffect3D.Play();
      }
      else
      {
        this.AtariEffect.Stop();
        this.FeelHitEffect3D.Stop();
      }
    }));
  }

  public virtual bool Init(int _modeCtrl)
  {
    ProcBase.endInit = true;
    this.ctrlFlag.lstSyncAnimLayers[0, 0].Clear();
    this.ctrlFlag.lstSyncAnimLayers[0, 1].Clear();
    this.ctrlFlag.lstSyncAnimLayers[1, 0].Clear();
    this.ctrlFlag.lstSyncAnimLayers[1, 1].Clear();
    Singleton<GameCursor>.Instance.setCursor(GameCursor.CursorKind.None, Vector2.get_zero());
    return true;
  }

  public virtual bool SetStartMotion(
    bool _isIdle,
    int _modeCtrl,
    HScene.AnimationListInfo _infoAnimList)
  {
    return true;
  }

  public virtual bool Proc(int _modeCtrl, HScene.AnimationListInfo _infoAnimList)
  {
    return true;
  }

  public virtual void setAnimationParamater()
  {
  }

  public virtual bool ResetMotionSpeed()
  {
    this.auto.SetSpeed(this.ctrlFlag.speed);
    return true;
  }

  protected struct animParm
  {
    public float[] heights;
    public float breast;
    public float[] m;
    public float speed;
  }
}
