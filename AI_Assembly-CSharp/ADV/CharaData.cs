// Decompiled with JetBrains decompiler
// Type: ADV.CharaData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Illusion.Game.Elements.EasyLoader;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ADV
{
  public class CharaData
  {
    private MotionOverride _motionOverride = new MotionOverride();
    private ChaControl _chaCtrl;
    private IKMotion _ikMotion;
    private TextScenario scenario;

    public CharaData(TextScenario.ParamData data, TextScenario scenario)
    {
      this.data = data;
      this.scenario = scenario;
      this.itemDic = new Dictionary<int, CharaData.CharaItem>();
      this.isADVCreateChara = true;
      this.initialized = true;
    }

    public CharaData(
      TextScenario.ParamData data,
      TextScenario scenario,
      CharaData.MotionReserver motionReserver)
    {
      this.data = data;
      this.scenario = scenario;
      this.motionReserver = motionReserver;
      this._chaCtrl = data.chaCtrl;
      this.isADVCreateChara = Object.op_Equality((Object) this._chaCtrl, (Object) null);
      if (!Object.op_Inequality((Object) this._chaCtrl, (Object) null))
        return;
      this.Initialize();
      this.backup = new CharaData.Backup(data);
    }

    public Dictionary<int, CharaData.CharaItem> itemDic { get; private set; }

    public bool initialized { get; private set; }

    private bool isADVCreateChara { get; set; }

    private GameObject dataBaseRoot { get; set; }

    public Transform root
    {
      get
      {
        return Object.op_Equality((Object) this.chaCtrl, (Object) null) ? (Transform) null : ((Component) this.chaCtrl).get_transform();
      }
    }

    public int voiceNo
    {
      get
      {
        return this.data.voiceNo;
      }
    }

    public float voicePitch
    {
      get
      {
        return this.data.voicePitch;
      }
    }

    public Transform voiceTrans
    {
      get
      {
        if (Object.op_Equality((Object) this.chaCtrl, (Object) null) || !this.chaCtrl.loadEnd)
          return (Transform) null;
        Transform trfHeadParent = this.chaCtrl.cmpBoneBody.targetEtc.trfHeadParent;
        return Object.op_Equality((Object) trfHeadParent, (Object) null) ? (Transform) null : ((Component) trfHeadParent).get_transform();
      }
    }

    public TextScenario.ParamData data { get; private set; }

    public ChaControl chaCtrl
    {
      get
      {
        return this.GetCacheObject<ChaControl>(ref this._chaCtrl, (Func<ChaControl>) (() => this.data.chaCtrl));
      }
    }

    public Transform transform
    {
      get
      {
        if (Object.op_Inequality((Object) this.data.transform, (Object) null))
          return this.data.transform;
        return Object.op_Equality((Object) this.chaCtrl, (Object) null) ? (Transform) null : ((Component) this.chaCtrl).get_transform();
      }
    }

    private CharaData.MotionReserver motionReserver { get; set; }

    public IKMotion ikMotion
    {
      get
      {
        return this.GetCache<IKMotion>(ref this._ikMotion, (Func<IKMotion>) (() =>
        {
          IKMotion ikMotion = new IKMotion();
          ikMotion.Create(this.chaCtrl, (MotionIK) null, (MotionIK[]) Array.Empty<MotionIK>());
          return ikMotion;
        }));
      }
    }

    public YureMotion yureMotion
    {
      get
      {
        return this._yureMotion;
      }
    }

    private YureMotion _yureMotion { get; set; }

    public MotionOverride motionOverride
    {
      get
      {
        return this._motionOverride;
      }
    }

    public CharaData.Backup backup { get; set; }

    public void Initialize()
    {
      this.itemDic = new Dictionary<int, CharaData.CharaItem>();
      if (this.motionReserver != null)
      {
        if (this.motionReserver.ikMotion != null)
          this._ikMotion = this.motionReserver.ikMotion;
        if (this.motionReserver.yureMotion != null)
          this._yureMotion = this.motionReserver.yureMotion;
        if (this.motionReserver.motionOverride != null)
          this._motionOverride = this.motionReserver.motionOverride;
      }
      if (this._yureMotion == null)
      {
        this._yureMotion = new YureMotion();
        this._yureMotion.Create(this.chaCtrl, (YureCtrlEx) null);
      }
      if (this.isADVCreateChara)
      {
        this.scenario.commandController.LoadingCharaList.Remove(this);
        this.chaCtrl.SetActiveTop(true);
        MotionIK motionIK = new MotionIK(this.chaCtrl, false, (MotionIKData) null);
        if (this._ikMotion == null)
        {
          this._ikMotion = new IKMotion();
          this._ikMotion.Create(this.chaCtrl, motionIK, (MotionIK[]) Array.Empty<MotionIK>());
        }
      }
      this.initialized = true;
    }

    public void MotionPlay(ADV.Commands.Base.Motion.Data motion, bool isCrossFade)
    {
      if (isCrossFade)
        this.scenario.CrossFadeStart();
      if (!motion.pair.HasValue)
      {
        if (this._motionOverride.Setting(this.chaCtrl.animBody, motion.assetBundleName, motion.assetName, motion.overrideAssetBundleName, motion.overrideAssetName, motion.stateName, true))
        {
          Info.Anime.Play play = this.scenario.info.anime.play;
          this._motionOverride.isCrossFade = play.isCrossFade;
          this._motionOverride.layers = motion.layerNo;
          this._motionOverride.transitionDuration = play.transitionDuration;
          this._motionOverride.normalizedTime = play.normalizedTime;
          this._motionOverride.Play(this.chaCtrl.animBody);
        }
        IKMotion ikMotion = this.ikMotion;
        bool enabled = ((Behaviour) this.data.actor.Animation).get_enabled();
        ikMotion.use = !enabled;
        ikMotion.motionIK.enabled = enabled;
        ikMotion.Setting(this.chaCtrl, motion.ikAssetBundleName, motion.ikAssetName, motion.stateName, false);
        this._yureMotion.Setting(this.chaCtrl, motion.shakeAssetBundleName, motion.shakeAssetName, motion.stateName, false);
      }
      else
      {
        int postureId = motion.pair.Value.postureID;
        int poseId = motion.pair.Value.poseID;
        Actor actor = this.data.actor;
        actor.ActionID = postureId;
        actor.PoseID = poseId;
        PlayState autoPlayState = this.GetAutoPlayState();
        if (autoPlayState == null)
          return;
        actor.Animation.StopAllAnimCoroutine();
        ActorAnimInfo actorAnimInfo = actor.Animation.LoadActionState(postureId, poseId, autoPlayState);
        actor.Animation.PlayInAnimation(actorAnimInfo.inEnableBlend, actorAnimInfo.inBlendSec, autoPlayState.MainStateInfo.FadeOutTime, actorAnimInfo.layer);
      }
    }

    public PlayState GetAutoPlayState()
    {
      if (Object.op_Equality((Object) this.data.actor, (Object) null))
        return (PlayState) null;
      Resources.AnimationTables animation = Singleton<Resources>.Instance.Animation;
      PlayState playState;
      if (Object.op_Inequality((Object) this.data.agentActor, (Object) null))
        playState = this.GetPlayState(animation.AgentActionAnimTable);
      else if (Object.op_Inequality((Object) this.data.playerActor, (Object) null))
      {
        PlayerActor playerActor = this.data.playerActor;
        playState = this.GetPlayState(animation.PlayerActionAnimTable[(int) playerActor.PlayerData.Sex]);
      }
      else
        playState = (this.GetPlayState(animation.MerchantOnlyActionAnimStateTable) ?? this.GetPlayState(animation.MerchantCommonActionAnimStateTable)) ?? this.GetPlayState(animation.AgentActionAnimTable);
      return playState;
    }

    public PlayState GetPlayState(
      Dictionary<int, Dictionary<int, PlayState>> stateTable)
    {
      Actor actor = this.data.actor;
      return Object.op_Equality((Object) actor, (Object) null) ? (PlayState) null : CharaData.GetPlayState((IReadOnlyDictionary<int, Dictionary<int, PlayState>>) stateTable, actor.ActionID, actor.PoseID);
    }

    private static PlayState GetPlayState(
      IReadOnlyDictionary<int, Dictionary<int, PlayState>> stateTable,
      int postureID,
      int poseID)
    {
      Dictionary<int, PlayState> dictionary;
      PlayState playState;
      return stateTable.TryGetValue(postureID, ref dictionary) && dictionary.TryGetValue(poseID, out playState) ? playState : (PlayState) null;
    }

    public void Release()
    {
      if (this.initialized)
      {
        foreach (KeyValuePair<int, CharaData.CharaItem> keyValuePair in this.itemDic)
          keyValuePair.Value.Delete();
      }
      if (this.backup == null)
        return;
      this.backup.Repair();
    }

    public class MotionReserver
    {
      public IKMotion ikMotion { get; set; }

      public YureMotion yureMotion { get; set; }

      public MotionOverride motionOverride { get; set; }
    }

    public class Backup
    {
      private bool isRepair;

      public Backup(TextScenario.ParamData data)
      {
        this.navMeshAgent = data.actor.NavMeshAgent;
        this.isNavmesh = ((Behaviour) this.navMeshAgent).get_enabled();
        this.transform = ((Component) data.actor.Animation.Character).get_transform();
        this.position = this.transform.get_localPosition();
        this.rotation = this.transform.get_localRotation();
      }

      public Transform transform { get; }

      private Vector3 position { get; }

      private Quaternion rotation { get; }

      private bool isNavmesh { get; }

      private NavMeshAgent navMeshAgent { get; }

      public void Set()
      {
        this.isRepair = true;
        ((Behaviour) this.navMeshAgent).set_enabled(false);
      }

      public void Repair()
      {
        if (!this.isRepair)
          return;
        if (Object.op_Inequality((Object) this.transform, (Object) null))
        {
          this.transform.set_localPosition(this.position);
          this.transform.set_localRotation(this.rotation);
        }
        if (!Object.op_Inequality((Object) this.navMeshAgent, (Object) null))
          return;
        ((Behaviour) this.navMeshAgent).set_enabled(this.isNavmesh);
      }
    }

    public class CharaItem
    {
      private Illusion.Game.Elements.EasyLoader.Motion motion;

      public CharaItem()
      {
      }

      public CharaItem(GameObject item)
      {
        this.item = item;
      }

      public GameObject item { get; private set; }

      public void Delete()
      {
        if (!Object.op_Inequality((Object) this.item, (Object) null))
          return;
        Object.Destroy((Object) this.item);
        this.item = (GameObject) null;
      }

      public void LoadObject(
        string bundle,
        string asset,
        Transform root,
        bool worldPositionStays = false,
        string manifest = null)
      {
        this.Delete();
        GameObject asset1 = AssetBundleManager.LoadAsset(bundle, asset, typeof (GameObject), manifest).GetAsset<GameObject>();
        this.item = (GameObject) Object.Instantiate<GameObject>((M0) asset1);
        AssetBundleManager.UnloadAssetBundle(bundle, false, manifest, false);
        ((Object) this.item).set_name(((Object) asset1).get_name());
        this.item.get_transform().SetParent(root, worldPositionStays);
      }

      public void LoadAnimator(string bundle, string asset, string state)
      {
        Animator orAddComponent = this.item.GetOrAddComponent<Animator>();
        if (this.motion == null)
          this.motion = new Illusion.Game.Elements.EasyLoader.Motion(bundle, asset, state);
        if (!this.motion.Setting(orAddComponent, bundle, asset, state, true))
          return;
        this.motion.Play(orAddComponent);
      }
    }
  }
}
