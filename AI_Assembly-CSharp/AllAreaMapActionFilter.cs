// Decompiled with JetBrains decompiler
// Type: AllAreaMapActionFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AllAreaMapActionFilter : MonoBehaviour
{
  [SerializeField]
  [Tooltip("エロありか")]
  private bool CanH;
  [SerializeField]
  private Button ActionCategoryShow;
  [SerializeField]
  private Toggle[] _ActionToggles;
  [SerializeField]
  private Toggle HToggle;
  [SerializeField]
  private AllAreaMapActionFilter.IconIDInfo[] IconIDs;
  [SerializeField]
  private AllAreaMapActionFilter.OpenPos[] OpenPositions;
  [SerializeField]
  private float ClosePos;
  [SerializeField]
  private MapActionCategoryUI mapAction;
  private bool ActionOpen;
  private bool ActionMoving;
  private float ActionMovingTime;
  [SerializeField]
  private float ActionMovingEndTime;
  [SerializeField]
  private RectTransform actionWindowRect;
  private AllAreaMapUI AllAreaMapUI;
  private MiniMapControler miniMapcontroler;

  public AllAreaMapActionFilter()
  {
    base.\u002Ector();
  }

  public Toggle[] ActionToggles
  {
    get
    {
      return this._ActionToggles;
    }
  }

  public void Init(MiniMapControler miniMapCtrl, AllAreaMapUI _allAreaMapUI)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AllAreaMapActionFilter.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new AllAreaMapActionFilter.\u003CInit\u003Ec__AnonStorey0();
    // ISSUE: reference to a compiler-generated field
    initCAnonStorey0.miniMapCtrl = miniMapCtrl;
    // ISSUE: reference to a compiler-generated field
    initCAnonStorey0.\u0024this = this;
    this.CanH = Game.isAdd01;
    ((Component) this.HToggle).get_gameObject().SetActive(this.CanH);
    // ISSUE: reference to a compiler-generated field
    this.miniMapcontroler = initCAnonStorey0.miniMapCtrl;
    this.AllAreaMapUI = _allAreaMapUI;
    ((UnityEventBase) this.ActionCategoryShow.get_onClick()).RemoveAllListeners();
    // ISSUE: method pointer
    ((UnityEvent) this.ActionCategoryShow.get_onClick()).AddListener(new UnityAction((object) initCAnonStorey0, __methodptr(\u003C\u003Em__0)));
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) new Func<long, bool>(initCAnonStorey0.\u003C\u003Em__1)), (Action<M0>) new Action<long>(initCAnonStorey0.\u003C\u003Em__2));
    for (int index = 0; index < this._ActionToggles.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AllAreaMapActionFilter.\u003CInit\u003Ec__AnonStorey1 initCAnonStorey1 = new AllAreaMapActionFilter.\u003CInit\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey1.\u003C\u003Ef__ref\u00240 = initCAnonStorey0;
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey1.id = index;
      // ISSUE: reference to a compiler-generated field
      Image component = (Image) ((Component) this._ActionToggles[initCAnonStorey1.id]).GetComponent<Image>();
      // ISSUE: reference to a compiler-generated field
      if (initCAnonStorey1.id == 0)
      {
        Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Category, 0, component, true);
        // ISSUE: reference to a compiler-generated field
        ((UnityEventBase) this._ActionToggles[initCAnonStorey1.id].onValueChanged).RemoveAllListeners();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent<bool>) this._ActionToggles[initCAnonStorey1.id].onValueChanged).AddListener(new UnityAction<bool>((object) initCAnonStorey1, __methodptr(\u003C\u003Em__0)));
      }
      else
      {
        int id = -1;
        // ISSUE: reference to a compiler-generated field
        if (this.IDContain(initCAnonStorey1.id, ref id))
        {
          if (this.IconIDs[id].Category == 0)
            Resources.ItemIconTables.SetIcon(Resources.ItemIconTables.IconCategory.Category, this.IconIDs[id].IconID, component, true);
          else if (this.IconIDs[id].Category == 1)
          {
            Sprite sprite = component.get_sprite();
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(this.IconIDs[id].IconID, out sprite);
          }
        }
        // ISSUE: reference to a compiler-generated field
        ((UnityEventBase) this._ActionToggles[initCAnonStorey1.id].onValueChanged).RemoveAllListeners();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent<bool>) this._ActionToggles[initCAnonStorey1.id].onValueChanged).AddListener(new UnityAction<bool>((object) initCAnonStorey1, __methodptr(\u003C\u003Em__1)));
      }
    }
  }

  public void ChangeActionWindowState()
  {
    Vector2 anchoredPosition = this.actionWindowRect.get_anchoredPosition();
    int index = !this.CanH ? 1 : 0;
    float num1;
    float num2;
    if (this.ActionOpen)
    {
      num1 = this.ClosePos;
      num2 = this.OpenPositions[index].pos[!Object.op_Inequality((Object) this.AllAreaMapUI, (Object) null) ? 1 : (!this.AllAreaMapUI.GameClear ? 1 : 0)];
    }
    else
    {
      num1 = this.OpenPositions[index].pos[!Object.op_Inequality((Object) this.AllAreaMapUI, (Object) null) ? 1 : (!this.AllAreaMapUI.GameClear ? 1 : 0)];
      num2 = this.ClosePos;
    }
    this.ActionMovingTime += Time.get_unscaledDeltaTime();
    float num3 = this.ActionMovingTime / this.ActionMovingEndTime;
    if ((double) num3 >= 1.0)
    {
      num3 = 1f;
      this.ActionMoving = false;
    }
    anchoredPosition.x = (__Null) (double) Mathf.Lerp(num1, num2, num3);
    this.actionWindowRect.set_anchoredPosition(anchoredPosition);
  }

  public void Refresh()
  {
    Vector2 anchoredPosition = this.actionWindowRect.get_anchoredPosition();
    int index1 = !this.CanH ? 1 : 0;
    anchoredPosition.x = !this.ActionOpen ? (__Null) (double) this.ClosePos : (__Null) (double) this.OpenPositions[index1].pos[!Object.op_Inequality((Object) this.AllAreaMapUI, (Object) null) ? 1 : (!this.AllAreaMapUI.GameClear ? 1 : 0)];
    this.actionWindowRect.set_anchoredPosition(anchoredPosition);
    AllAreaCameraControler component = (AllAreaCameraControler) this.miniMapcontroler.AllAreaMap.GetComponent<AllAreaCameraControler>();
    component.ChangeActionFilterAllTgl(this._ActionToggles[0].get_isOn());
    for (int index2 = 1; index2 < this._ActionToggles.Length; ++index2)
      component.ChangeactionFilter((MapUIActionCategory) index2, this._ActionToggles[index2].get_isOn());
  }

  private bool IDContain(int check, ref int id)
  {
    for (int index = 0; index < this.IconIDs.Length; ++index)
    {
      if (this.IconIDs[index].ToggleID == check)
      {
        id = index;
        return true;
      }
    }
    return false;
  }

  [Serializable]
  public struct OpenPos
  {
    public float[] pos;
  }

  [Serializable]
  public struct IconIDInfo
  {
    public int ToggleID;
    public int Category;
    public int IconID;
  }
}
