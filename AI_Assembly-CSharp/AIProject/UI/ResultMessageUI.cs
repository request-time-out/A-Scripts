// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ResultMessageUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI.Popup;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  public class ResultMessageUI : MonoBehaviour
  {
    [SerializeField]
    private GameObject messagePrefab;
    private List<ResultMessageElement> openMessageList;
    private List<ResultMessageElement> closeMessageList;
    [SerializeField]
    [LabelText("白")]
    private OneColor white;
    [SerializeField]
    [LabelText("明緑")]
    private OneColor lightGreen;
    [SerializeField]
    [LabelText("緑")]
    private OneColor green;
    [SerializeField]
    [LabelText("黄")]
    private OneColor yellow;
    [SerializeField]
    [LabelText("青")]
    private OneColor blue;
    [SerializeField]
    [LabelText("シアン")]
    private OneColor cyan;
    [SerializeField]
    [LabelText("赤")]
    private OneColor red;
    [SerializeField]
    [LabelText("暗赤")]
    private OneColor darkRed;
    [SerializeField]
    [LabelText("黒")]
    private OneColor black;
    [SerializeField]
    [LabelText("暗黒")]
    private OneColor darkBlack;
    private CompositeDisposable showMessageDisposable;

    public ResultMessageUI()
    {
      base.\u002Ector();
    }

    public OneColor White
    {
      get
      {
        return this.white;
      }
    }

    public OneColor LightGreen
    {
      get
      {
        return this.lightGreen;
      }
    }

    public OneColor Green
    {
      get
      {
        return this.green;
      }
    }

    public OneColor Yellow
    {
      get
      {
        return this.yellow;
      }
    }

    public OneColor Blue
    {
      get
      {
        return this.blue;
      }
    }

    public OneColor Cyan
    {
      get
      {
        return this.cyan;
      }
    }

    public OneColor Red
    {
      get
      {
        return this.red;
      }
    }

    public OneColor DarkRed
    {
      get
      {
        return this.darkRed;
      }
    }

    public OneColor Black
    {
      get
      {
        return this.black;
      }
    }

    public OneColor DarkBlack
    {
      get
      {
        return this.darkBlack;
      }
    }

    private void Awake()
    {
      this.openMessageList = ListPool<ResultMessageElement>.Get();
      this.closeMessageList = ListPool<ResultMessageElement>.Get();
    }

    private void OnDestroy()
    {
      ListPool<ResultMessageElement>.Release(this.openMessageList);
      ListPool<ResultMessageElement>.Release(this.closeMessageList);
      this.openMessageList = (List<ResultMessageElement>) null;
      this.closeMessageList = (List<ResultMessageElement>) null;
    }

    private void CloseAction(ResultMessageElement _child)
    {
      if (this.openMessageList == null || this.closeMessageList == null)
        return;
      if (this.openMessageList.Contains(_child))
        this.openMessageList.Remove(_child);
      if (!this.closeMessageList.Contains(_child))
        this.closeMessageList.Add(_child);
      if (!((Component) _child).get_gameObject().get_activeSelf())
        return;
      ((Component) _child).get_gameObject().SetActive(false);
    }

    public void ShowCancel()
    {
      if (this.openMessageList.IsNullOrEmpty<ResultMessageElement>())
        return;
      foreach (ResultMessageElement openMessage in this.openMessageList)
      {
        if (Object.op_Inequality((Object) openMessage, (Object) null))
        {
          openMessage.Dispose();
          openMessage.CanvasAlpha = 0.0f;
          if (((Component) openMessage).get_gameObject().get_activeSelf())
            ((Component) openMessage).get_gameObject().SetActive(false);
          if (!this.closeMessageList.Contains(openMessage))
            this.closeMessageList.Add(openMessage);
        }
      }
      this.openMessageList.Clear();
    }

    public void ShowMessage(string _mes)
    {
      if (_mes.IsNullOrEmpty())
        return;
      if (!this.openMessageList.IsNullOrEmpty<ResultMessageElement>())
      {
        ResultMessageElement openMessage = this.openMessageList[this.openMessageList.Count - 1];
        if (openMessage.Message == _mes)
        {
          if (openMessage.PlayingFadeIn)
            return;
          if (openMessage.PlayingDisplay)
          {
            openMessage.StartDisplay();
            return;
          }
        }
      }
      ResultMessageElement resultMessageElement = this.closeMessageList.FirstOrDefault<ResultMessageElement>();
      if (Object.op_Equality((Object) resultMessageElement, (Object) null))
      {
        resultMessageElement = (ResultMessageElement) ((GameObject) Object.Instantiate<GameObject>((M0) this.messagePrefab, ((Component) this).get_transform(), true)).GetComponent<ResultMessageElement>();
        if (Object.op_Equality((Object) resultMessageElement, (Object) null))
          return;
        resultMessageElement.Root = this;
        resultMessageElement.EndAction = new Action<ResultMessageElement>(this.CloseAction);
      }
      else
        this.closeMessageList.RemoveAt(0);
      foreach (ResultMessageElement openMessage in this.openMessageList)
        openMessage.CloseMessage();
      this.openMessageList.Add(resultMessageElement);
      ((Component) resultMessageElement).get_transform().SetAsLastSibling();
      if (!((Component) resultMessageElement).get_gameObject().get_activeSelf())
        ((Component) resultMessageElement).get_gameObject().SetActive(true);
      resultMessageElement.ShowMessage(_mes);
    }
  }
}
