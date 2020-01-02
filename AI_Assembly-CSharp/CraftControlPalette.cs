// Decompiled with JetBrains decompiler
// Type: CraftControlPalette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftControlPalette : MonoBehaviour
{
  [SerializeField]
  private GameObject[] paletteCorums;
  [SerializeField]
  private GameObject endPanel;
  [SerializeField]
  private Button craftEnd;
  [SerializeField]
  private Button cancel;
  [SerializeField]
  private Text floorCnt;
  [SerializeField]
  private Button floorUp;
  [SerializeField]
  private Button floorDown;
  [SerializeField]
  private CraftControler craftControler;
  [SerializeField]
  private CraftCamera Cam;
  private IntReactiveProperty nTargetFloorCnt;
  private int nPrevTargetFloorCnt;

  public CraftControlPalette()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Button) this.paletteCorums[0]?.GetComponent<Button>())?.get_onClick()).AddListener(new UnityAction((object) this.craftControler, __methodptr(Undo)));
    // ISSUE: method pointer
    ((UnityEvent) ((Button) this.paletteCorums[1]?.GetComponent<Button>())?.get_onClick()).AddListener(new UnityAction((object) this.craftControler, __methodptr(Redo)));
    if (Object.op_Inequality((Object) this.paletteCorums[4], (Object) null))
    {
      // ISSUE: method pointer
      ((UnityEvent<bool>) ((Toggle) this.paletteCorums[4].GetComponent<Toggle>())?.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__0)));
    }
    // ISSUE: method pointer
    ((UnityEvent) ((Button) this.paletteCorums[6]?.GetComponent<Button>())?.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(SetEndPanel)));
    ObservableExtensions.Subscribe<int>(Observable.Where<int>((IObservable<M0>) this.nTargetFloorCnt, (Func<M0, bool>) (x => x != this.nPrevTargetFloorCnt)), (Action<M0>) (x => this.ChangeFloorCntTex()));
    if (this.floorUp != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.floorUp.get_onClick()).AddListener(new UnityAction((object) this.craftControler, __methodptr(OpelateFloorUp)));
    }
    if (this.floorUp != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.floorUp.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(ChangeFloorCntTex)));
    }
    if (this.floorDown != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.floorDown.get_onClick()).AddListener(new UnityAction((object) this.craftControler, __methodptr(OpelateFloorUp)));
    }
    if (this.floorDown != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.floorDown.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(ChangeFloorCntTex)));
    }
    if (this.craftEnd != null)
    {
      // ISSUE: method pointer
      ((UnityEvent) this.craftEnd.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
    }
    if (this.cancel == null)
      return;
    // ISSUE: method pointer
    ((UnityEvent) this.cancel.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
  }

  private void ChangeFloorCntTex()
  {
    this.nPrevTargetFloorCnt = ((ReactiveProperty<int>) this.nTargetFloorCnt).get_Value();
    this.floorCnt.set_text(string.Format("{0}", (object) (((ReactiveProperty<int>) this.nTargetFloorCnt).get_Value() + 1)));
  }

  private void ChangeCamLock(bool x)
  {
    this.Cam.bLock = x;
  }

  private void SetEndPanel()
  {
    if (this.endPanel.get_activeSelf())
      return;
    this.endPanel.SetActive(true);
  }

  private void Update()
  {
    ((ReactiveProperty<int>) this.nTargetFloorCnt).set_Value(Singleton<CraftCommandListBaseObject>.Instance.nTargetFloorCnt);
  }
}
