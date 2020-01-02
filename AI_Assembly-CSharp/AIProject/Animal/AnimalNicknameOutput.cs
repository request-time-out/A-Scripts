// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalNicknameOutput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.Animal
{
  public class AnimalNicknameOutput : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private float _showDistance;
    [SerializeField]
    private float _hideDistance;
    [SerializeField]
    private float _showAngle;
    [SerializeField]
    private float _hideAngle;
    [SerializeField]
    private float _fadeTime;
    [SerializeField]
    private float _elmRefreshInterval;
    [SerializeField]
    private GameObject _prefab;
    private List<AnimalNicknameUI> _uiPool;
    private List<INicknameObject> _elmList;
    private List<INicknameObject> _showList;
    private List<INicknameObject> _hideList;
    private Dictionary<int, ValueTuple<INicknameObject, AnimalNicknameUI>> _showTable;
    private PlayerActor _player;
    private Camera _camera;

    public AnimalNicknameOutput()
    {
      base.\u002Ector();
    }

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    private void Awake()
    {
      ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => Object.op_Equality((Object) Manager.Map.GetCameraComponent(), (Object) null))), 1), (Action<M0>) (_ =>
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return;
        this._player = Manager.Map.GetPlayer();
        this._camera = Manager.Map.GetCameraComponent(this._player);
        if (!Object.op_Inequality((Object) this._player, (Object) null) || !Object.op_Inequality((Object) this._camera, (Object) null))
          return;
        ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Component) this._player), (Component) this._camera), (Action<M0>) (__ => this.OnUpdate()));
        ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Interval(TimeSpan.FromSeconds((double) this._elmRefreshInterval)), (Component) this), (Component) this._player), (Component) this._camera), (Action<M0>) (__ =>
        {
          this._elmList.RemoveAll((Predicate<INicknameObject>) (x => x == null));
          this._showList.RemoveAll((Predicate<INicknameObject>) (x => x == null));
          this._hideList.RemoveAll((Predicate<INicknameObject>) (x => x == null));
          List<int> toRelease = ListPool<int>.Get();
          using (Dictionary<int, ValueTuple<INicknameObject, AnimalNicknameUI>>.Enumerator enumerator = this._showTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, ValueTuple<INicknameObject, AnimalNicknameUI>> current = enumerator.Current;
              ValueTuple<INicknameObject, AnimalNicknameUI> valueTuple = current.Value;
              if (valueTuple.Item1 == null)
              {
                this.ReturnUI((AnimalNicknameUI) valueTuple.Item2);
                toRelease.Add(current.Key);
              }
            }
          }
          foreach (int num in toRelease)
            toRelease.Remove(num);
          ListPool<int>.Release(toRelease);
        }));
      }));
    }

    private void OnUpdate()
    {
      List<INicknameObject> toRelease1 = ListPool<INicknameObject>.Get();
      List<INicknameObject> toRelease2 = ListPool<INicknameObject>.Get();
      for (int index = 0; index < this._hideList.Count; ++index)
      {
        INicknameObject hide = this._hideList[index];
        if (hide == null)
        {
          this._hideList.RemoveAt(index);
          --index;
        }
        else if (this.ShowState(hide))
        {
          toRelease2.Add(hide);
          this._hideList.RemoveAt(index);
          --index;
        }
      }
      for (int index = 0; index < this._showList.Count; ++index)
      {
        INicknameObject show = this._showList[index];
        if (show == null)
        {
          this._showList.RemoveAt(index);
          --index;
        }
        else if (this.HideState(show))
        {
          toRelease1.Add(show);
          this._showList.RemoveAt(index);
          --index;
        }
      }
      for (int index = 0; index < toRelease2.Count; ++index)
      {
        INicknameObject elm = toRelease2[index];
        AnimalNicknameUI ui = this.GetUI(elm);
        this._showTable[elm.InstanceID] = new ValueTuple<INicknameObject, AnimalNicknameUI>(elm, ui);
      }
      if (!((IReadOnlyList<INicknameObject>) toRelease2).IsNullOrEmpty<INicknameObject>())
        this._showList.AddRange((IEnumerable<INicknameObject>) toRelease2);
      for (int index = 0; index < toRelease1.Count; ++index)
      {
        int instanceId = toRelease1[index].InstanceID;
        ValueTuple<INicknameObject, AnimalNicknameUI> valueTuple;
        if (this._showTable.TryGetValue(instanceId, out valueTuple))
        {
          this.ReturnUI((AnimalNicknameUI) valueTuple.Item2);
          this._showTable.Remove(instanceId);
        }
      }
      if (!((IReadOnlyList<INicknameObject>) toRelease1).IsNullOrEmpty<INicknameObject>())
        this._hideList.AddRange((IEnumerable<INicknameObject>) toRelease1);
      ListPool<INicknameObject>.Release(toRelease1);
      ListPool<INicknameObject>.Release(toRelease2);
    }

    public void AddElement(INicknameObject addElm)
    {
      if (addElm == null || this._elmList.Contains(addElm))
        return;
      this._elmList.Add(addElm);
      this._hideList.Add(addElm);
      addElm.ChangeNickNameEvent = (Action) (() => this.Rename(addElm));
    }

    public void RemoveElement(INicknameObject remelm)
    {
      if (remelm == null)
        return;
      this._showList.Remove(remelm);
      this._hideList.Remove(remelm);
      if (this._showTable.ContainsKey(remelm.InstanceID))
      {
        this.ReturnUI((AnimalNicknameUI) this._showTable[remelm.InstanceID].Item2);
        this._showTable.Remove(remelm.InstanceID);
      }
      this._elmList.Remove(remelm);
      remelm.ChangeNickNameEvent = (Action) null;
    }

    public bool ShowState(INicknameObject elm)
    {
      if (elm == null)
        return false;
      Transform nicknameRoot = elm.NicknameRoot;
      if (Object.op_Equality((Object) nicknameRoot, (Object) null) || !elm.NicknameEnabled)
        return false;
      Vector3 vector3 = Vector3.op_Subtraction(nicknameRoot.get_position(), this._player.Position);
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() > (double) this._showDistance * (double) this._showDistance)
        return false;
      Transform transform = ((Component) this._camera).get_transform();
      return (double) Vector3.Angle(transform.get_forward(), Vector3.op_Subtraction(nicknameRoot.get_position(), transform.get_position())) * 2.0 <= (double) this._showAngle;
    }

    public bool HideState(INicknameObject elm)
    {
      if (elm == null)
        return true;
      Transform nicknameRoot = elm.NicknameRoot;
      if (Object.op_Equality((Object) nicknameRoot, (Object) null) || !elm.NicknameEnabled)
        return true;
      double num = (double) this._hideDistance * (double) this._hideDistance;
      Vector3 vector3 = Vector3.op_Subtraction(nicknameRoot.get_position(), this._player.Position);
      double sqrMagnitude = (double) ((Vector3) ref vector3).get_sqrMagnitude();
      if (num < sqrMagnitude)
        return true;
      Transform transform = ((Component) this._camera).get_transform();
      return (double) this._hideAngle < (double) Vector3.Angle(transform.get_forward(), Vector3.op_Subtraction(nicknameRoot.get_position(), transform.get_position())) * 2.0;
    }

    private void Rename(INicknameObject elm)
    {
      ValueTuple<INicknameObject, AnimalNicknameUI> valueTuple;
      if (elm == null || !this._showTable.TryGetValue(elm.InstanceID, out valueTuple))
        return;
      AnimalNicknameUI animalNicknameUi = (AnimalNicknameUI) valueTuple.Item2;
      if (!Object.op_Inequality((Object) animalNicknameUi, (Object) null))
        return;
      animalNicknameUi.Nickname = elm.Nickname;
    }

    private AnimalNicknameUI GetUI(INicknameObject elm)
    {
      AnimalNicknameUI animalNicknameUi = this._uiPool.PopFront<AnimalNicknameUI>();
      if (Object.op_Equality((Object) animalNicknameUi, (Object) null))
        animalNicknameUi = (AnimalNicknameUI) ((GameObject) Object.Instantiate<GameObject>((M0) this._prefab, ((Component) this).get_transform())).GetComponent<AnimalNicknameUI>();
      animalNicknameUi.Nickname = elm.Nickname;
      animalNicknameUi.TargetObject = elm.NicknameRoot;
      animalNicknameUi.TargetCamera = this._camera;
      animalNicknameUi.PlayFadeIn(this._fadeTime);
      return animalNicknameUi;
    }

    private void ReturnUI(AnimalNicknameUI ui)
    {
      if (Object.op_Equality((Object) ui, (Object) null) || this._uiPool.Contains(ui))
        return;
      ui.PlayFadeOut(this._fadeTime);
      this._uiPool.Add(ui);
    }
  }
}
