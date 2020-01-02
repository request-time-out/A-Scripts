// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PlantUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class PlantUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _farmland;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private Button _allGetButton;

    public event Action<PlantIcon> IconChanged;

    public UnityEvent OnSubmitRemove { get; private set; } = new UnityEvent();

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    public PlantIcon currentIcon
    {
      get
      {
        return this._plantIcons[this._currentIndex];
      }
    }

    public int currentIndex
    {
      get
      {
        return this._currentIndex;
      }
    }

    public PlantIcon[] plantIcons
    {
      get
      {
        return this._plantIcons;
      }
    }

    public IObservable<Unit> AllGet
    {
      get
      {
        return UnityUIComponentExtensions.OnClickAsObservable(this._allGetButton);
      }
    }

    private PlantIcon[] _plantIcons { get; set; }

    private int _currentIndex { get; set; } = -1;

    private List<AIProject.SaveData.Environment.PlantInfo> _plantList { get; set; }

    private CompositeDisposable disposable { get; } = new CompositeDisposable();

    public void SetPlantItem(StuffItem item)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID);
      if (stuffItemInfo == null)
      {
        Debug.LogError((object) string.Format("Item not Find\n[{0}:{1}][{2}:{3}]", (object) "CategoryID", (object) item.CategoryID, (object) "ID", (object) item.ID), (Object) this);
      }
      else
      {
        AIProject.SaveData.Environment.PlantInfo plantInfo = Singleton<Resources>.Instance.GameInfo.GetPlantInfo(stuffItemInfo.nameHash);
        this.currentIcon.info = plantInfo;
        this._plantList[this._currentIndex] = plantInfo;
      }
    }

    public int GetEmptySum()
    {
      return this._plantList.Count<AIProject.SaveData.Environment.PlantInfo>((Func<AIProject.SaveData.Environment.PlantInfo, bool>) (x => x == null));
    }

    public void SetPlantItemForAll(StuffItem item, int count)
    {
      StuffItemInfo stuffItemInfo = Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID);
      if (stuffItemInfo == null)
      {
        Debug.LogError((object) string.Format("Item not Find\n[{0}:{1}][{2}:{3}]", (object) "CategoryID", (object) item.CategoryID, (object) "ID", (object) item.ID), (Object) this);
      }
      else
      {
        int num = 0;
        for (int index = 0; index < this._plantList.Count; ++index)
        {
          if (this._plantList[index] == null)
          {
            if (num++ >= count)
              break;
            AIProject.SaveData.Environment.PlantInfo plantInfo = Singleton<Resources>.Instance.GameInfo.GetPlantInfo(stuffItemInfo.nameHash);
            this._plantIcons[index].info = plantInfo;
            this._plantList[index] = plantInfo;
          }
        }
      }
    }

    public void Open(List<AIProject.SaveData.Environment.PlantInfo> plantList)
    {
      this._plantList = plantList;
      int count = plantList.Count;
      for (int index = 0; index < this._plantIcons.Length; ++index)
      {
        bool flag = index < count;
        this._plantIcons[index].visible = flag;
        this._plantIcons[index].info = !flag ? (AIProject.SaveData.Environment.PlantInfo) null : plantList[index];
      }
      foreach (PlantIcon plantIcon in this._plantIcons)
        plantIcon.toggle.set_isOn(false);
      this._plantIcons[0].toggle.set_isOn(true);
      ((Behaviour) this._cursor).set_enabled(false);
    }

    public void Refresh()
    {
      if (this.IconChanged == null)
        return;
      this.IconChanged(this.currentIcon);
    }

    [DebuggerHidden]
    private IEnumerator BindingUI()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlantUI.\u003CBindingUI\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      this._canvasGroup.set_alpha(1f);
      ((MonoBehaviour) this).StartCoroutine(this.BindingUI());
      this.IsActiveControl = true;
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__1)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.SquareX
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand3);
      ActionIDDownCommand actionIdDownCommand4 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand4);
      base.Start();
    }

    private void OnDestroy()
    {
      this.disposable.Clear();
    }

    private void OnInputSubmit()
    {
      this.OnSubmitRemove?.Invoke();
    }

    private void OnInputCancel()
    {
      this.OnCancel?.Invoke();
    }
  }
}
