// Decompiled with JetBrains decompiler
// Type: Studio.RouteControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class RouteControl : MonoBehaviour
  {
    [SerializeField]
    private Transform nodeRoot;
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private Sprite[] spritePlay;
    [Space]
    [SerializeField]
    private Button buttonAll;
    [SerializeField]
    private Button buttonReAll;
    [SerializeField]
    private Button buttonStopAll;
    [Space]
    [SerializeField]
    private MPRouteCtrl mpRouteCtrl;
    [SerializeField]
    private MPRoutePointCtrl mpRoutePointCtrl;
    private BoolReactiveProperty _visible;
    private List<ObjectInfo> listInfo;
    private Dictionary<ObjectInfo, RouteNode> dicNode;

    public RouteControl()
    {
      base.\u002Ector();
    }

    public bool visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this._visible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._visible).set_Value(value);
      }
    }

    public void Init()
    {
      int childCount = this.nodeRoot.get_childCount();
      for (int index = 0; index < childCount; ++index)
        Object.Destroy((Object) ((Component) this.nodeRoot.GetChild(index)).get_gameObject());
      this.nodeRoot.DetachChildren();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode.Clear();
      this.listInfo = ObjectInfoAssist.Find(4);
      for (int index = 0; index < this.listInfo.Count; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RouteControl.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new RouteControl.\u003CInit\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        initCAnonStorey0.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
        if (!gameObject.get_activeSelf())
          gameObject.SetActive(true);
        gameObject.get_transform().SetParent(this.nodeRoot, false);
        RouteNode component = (RouteNode) gameObject.GetComponent<RouteNode>();
        OIRouteInfo oiRouteInfo = this.listInfo[index] as OIRouteInfo;
        component.spritePlay = this.spritePlay;
        component.text = oiRouteInfo.name;
        // ISSUE: reference to a compiler-generated field
        initCAnonStorey0.no = index;
        // ISSUE: method pointer
        ((UnityEvent) component.buttonSelect.get_onClick()).AddListener(new UnityAction((object) initCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent) component.buttonPlay.get_onClick()).AddListener(new UnityAction((object) initCAnonStorey0, __methodptr(\u003C\u003Em__1)));
        OCIRoute ctrlInfo = Studio.Studio.GetCtrlInfo(this.listInfo[index].dicKey) as OCIRoute;
        component.state = !ctrlInfo.isEnd ? (!ctrlInfo.isPlay ? RouteNode.State.Stop : RouteNode.State.Play) : RouteNode.State.End;
        this.dicNode.Add(this.listInfo[index], component);
      }
    }

    public void ReflectOption()
    {
      if (this.listInfo.IsNullOrEmpty<ObjectInfo>())
        this.listInfo = ObjectInfoAssist.Find(4);
      foreach (OCIRoute ociRoute in this.listInfo.Select<ObjectInfo, OCIRoute>((Func<ObjectInfo, OCIRoute>) (i => Studio.Studio.GetCtrlInfo(i.dicKey) as OCIRoute)))
        ociRoute.UpdateLine();
    }

    public void SetState(ObjectInfo _info, RouteNode.State _state)
    {
      if (this.dicNode == null)
        return;
      RouteNode routeNode = (RouteNode) null;
      if (!this.dicNode.TryGetValue(_info, out routeNode))
        return;
      routeNode.state = _state;
    }

    private void OnSelect(int _idx)
    {
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.SelectSingle(Studio.Studio.GetCtrlInfo(this.listInfo[_idx].dicKey).treeNodeObject, false);
    }

    private void OnPlay(int _idx)
    {
      OCIRoute ctrlInfo = Studio.Studio.GetCtrlInfo(this.listInfo[_idx].dicKey) as OCIRoute;
      if (ctrlInfo.isPlay)
      {
        ctrlInfo.Stop(true);
        this.dicNode[this.listInfo[_idx]].state = RouteNode.State.Stop;
      }
      else if (ctrlInfo.Play())
        this.dicNode[this.listInfo[_idx]].state = RouteNode.State.Play;
      this.mpRouteCtrl.UpdateInteractable(ctrlInfo);
      this.mpRoutePointCtrl.UpdateInteractable(ctrlInfo);
    }

    private void OnClickALL()
    {
      foreach (ObjectInfo index in this.listInfo)
      {
        OCIRoute ctrlInfo = Studio.Studio.GetCtrlInfo(index.dicKey) as OCIRoute;
        if (!ctrlInfo.isPlay && ctrlInfo.Play())
        {
          this.dicNode[index].state = RouteNode.State.Play;
          this.mpRouteCtrl.UpdateInteractable(ctrlInfo);
          this.mpRoutePointCtrl.UpdateInteractable(ctrlInfo);
        }
      }
    }

    private void OnClickReAll()
    {
      foreach (ObjectInfo index in this.listInfo)
      {
        OCIRoute ctrlInfo = Studio.Studio.GetCtrlInfo(index.dicKey) as OCIRoute;
        if (ctrlInfo.Play())
        {
          this.dicNode[index].state = RouteNode.State.Play;
          this.mpRouteCtrl.UpdateInteractable(ctrlInfo);
          this.mpRoutePointCtrl.UpdateInteractable(ctrlInfo);
        }
      }
    }

    private void OnClickStopAll()
    {
      foreach (ObjectInfo index in this.listInfo)
      {
        OCIRoute ctrlInfo = Studio.Studio.GetCtrlInfo(index.dicKey) as OCIRoute;
        ctrlInfo.Stop(true);
        this.dicNode[index].state = RouteNode.State.Stop;
        this.mpRouteCtrl.UpdateInteractable(ctrlInfo);
        this.mpRoutePointCtrl.UpdateInteractable(ctrlInfo);
      }
    }

    private void Awake()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.buttonAll.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickALL)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonReAll.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickReAll)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonStopAll.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickStopAll)));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visible, (Action<M0>) (_b =>
      {
        if (_b)
          this.Init();
        ((Component) this).get_gameObject().SetActive(_b);
      }));
      this.dicNode = new Dictionary<ObjectInfo, RouteNode>();
      ((Component) this).get_gameObject().SetActive(false);
    }
  }
}
