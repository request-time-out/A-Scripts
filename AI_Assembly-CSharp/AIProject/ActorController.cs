// Decompiled with JetBrains decompiler
// Type: AIProject.ActorController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public abstract class ActorController : MonoBehaviour
  {
    protected IState _state;
    [SerializeField]
    protected Actor _actor;

    protected ActorController()
    {
      base.\u002Ector();
    }

    public IState State
    {
      get
      {
        return this._state;
      }
    }

    public Actor Actor
    {
      get
      {
        return this._actor;
      }
    }

    public bool FaceLightActive
    {
      get
      {
        return Object.op_Inequality((Object) this.FaceLight, (Object) null) && ((Behaviour) this.FaceLight).get_enabled();
      }
      set
      {
        if (!Object.op_Inequality((Object) this.FaceLight, (Object) null) || ((Behaviour) this.FaceLight).get_enabled() == value)
          return;
        ((Behaviour) this.FaceLight).set_enabled(value);
      }
    }

    public Light FaceLight { get; protected set; }

    public abstract void StartBehavior();

    protected virtual void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryFixedUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnFixedUpdate()));
    }

    private void OnFixedUpdate()
    {
      if (Singleton<Scene>.Instance.IsNowLoadingFade)
        return;
      this.SubFixedUpdate();
    }

    protected abstract void SubFixedUpdate();

    public void OnAnimatorStateEnter(AnimatorStateInfo info)
    {
      if (this._state == null)
        return;
      this._state.OnAnimatorStateEnter(this, info);
    }

    public void OnAnimatorStateExit(AnimatorStateInfo info)
    {
      if (this._state == null)
        return;
      this._state.OnAnimatorStateExit(this, info);
    }

    public abstract void ChangeState(string stateName);

    public void InitializeFaceLight(GameObject root)
    {
      if (Object.op_Inequality((Object) this.FaceLight, (Object) null))
        Object.Destroy((Object) ((Component) this.FaceLight).get_gameObject());
      if (!Singleton<Resources>.IsInstance() || Object.op_Equality((Object) root, (Object) null))
        return;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      string fadeLightParentName = locomotionProfile.FadeLightParentName;
      if (fadeLightParentName.IsNullOrEmpty())
        return;
      GameObject loop = root.get_transform().FindLoop(fadeLightParentName);
      if (Object.op_Equality((Object) loop, (Object) null))
        return;
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      GameObject gameObject1 = AssetUtility.LoadAsset<GameObject>(definePack.ABPaths.ActorPrefab, "FaceLight", definePack.ABManifests.Default);
      if (Object.op_Equality((Object) gameObject1, (Object) null))
        return;
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1, loop.get_transform(), false);
      if (Object.op_Equality((Object) gameObject2, (Object) null))
        return;
      this.FaceLight = (Light) gameObject2.GetComponent<Light>();
      if (Object.op_Equality((Object) this.FaceLight, (Object) null))
      {
        Object.Destroy((Object) gameObject2);
      }
      else
      {
        ((Component) this.FaceLight).get_transform().set_localRotation(Quaternion.get_identity());
        ((Component) this.FaceLight).get_transform().set_localPosition(locomotionProfile.FaceLightOffset);
        if (!((Behaviour) this.FaceLight).get_enabled())
          return;
        ((Behaviour) this.FaceLight).set_enabled(false);
      }
    }
  }
}
