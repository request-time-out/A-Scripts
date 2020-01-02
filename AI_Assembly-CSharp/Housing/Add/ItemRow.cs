// Decompiled with JetBrains decompiler
// Type: Housing.Add.ItemRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using SuperScrollView;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Housing.Add
{
  public class ItemRow : LoopListViewItem2
  {
    [SerializeField]
    private ItemRow.CellInfo[] cellInfos;

    public void SetData(int _index, AddUICtrl.FileInfo _info, Action _action, bool _select)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ItemRow.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new ItemRow.\u003CSetData\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._action = _action;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0.cell = this.cellInfos.SafeGet<ItemRow.CellInfo>(_index);
      // ISSUE: reference to a compiler-generated field
      if (dataCAnonStorey0.cell == null)
        return;
      if (_info == null)
      {
        // ISSUE: reference to a compiler-generated field
        dataCAnonStorey0.cell.Active = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        dataCAnonStorey0.cell.Texture = CommonLib.LoadAsset<Texture2D>(_info.loadInfo.thumbnailPath.bundle, _info.loadInfo.thumbnailPath.file, false, _info.loadInfo.thumbnailPath.manifest);
        // ISSUE: reference to a compiler-generated field
        ((UnityEventBase) dataCAnonStorey0.cell.button.get_onClick()).RemoveAllListeners();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        ((UnityEvent) dataCAnonStorey0.cell.button.get_onClick()).AddListener(new UnityAction((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        // ISSUE: reference to a compiler-generated field
        dataCAnonStorey0.cell.Init();
        // ISSUE: reference to a compiler-generated field
        dataCAnonStorey0.cell.Select = _select;
        // ISSUE: reference to a compiler-generated field
        dataCAnonStorey0.cell.Unlock = _info.Unlock;
        // ISSUE: reference to a compiler-generated field
        dataCAnonStorey0.cell.Active = true;
      }
    }

    [Serializable]
    private class CellInfo
    {
      public GameObject gameObject;
      public RawImage rawImage;
      public Button button;
      public Image imageSelect;
      public Toggle toggleSelect;
      public Image imageLock;
      public Image imageCursor;
      private bool isInit;

      public bool Active
      {
        set
        {
          this.gameObject.SetActiveIfDifferent(value);
        }
      }

      public bool Select
      {
        get
        {
          return this.toggleSelect.get_isOn();
        }
        set
        {
          this.toggleSelect.set_isOn(value);
        }
      }

      public Texture2D Texture
      {
        set
        {
          this.rawImage.set_texture((Texture) value);
          ((Graphic) this.rawImage).set_color(!Object.op_Inequality((Object) value, (Object) null) ? Color.get_clear() : Color.get_white());
        }
      }

      public bool Unlock
      {
        set
        {
          ((Behaviour) this.imageLock).set_enabled(!value);
        }
      }

      public void Init()
      {
        if (this.isInit)
          return;
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this.button), (Component) this.imageCursor), (Action<M0>) (_ => ((Behaviour) this.imageCursor).set_enabled(true))), this.gameObject);
        DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>(Observable.TakeUntilDestroy<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this.button), (Component) this.imageCursor), (Action<M0>) (_ => ((Behaviour) this.imageCursor).set_enabled(false))), this.gameObject);
        this.isInit = true;
      }
    }
  }
}
