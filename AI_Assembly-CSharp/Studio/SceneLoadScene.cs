// Decompiled with JetBrains decompiler
// Type: Studio.SceneLoadScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class SceneLoadScene : MonoBehaviour
  {
    public static int page;
    [SerializeField]
    private ThumbnailNode[] buttonThumbnail;
    [SerializeField]
    private Button buttonClose;
    [SerializeField]
    private Canvas canvasWork;
    [SerializeField]
    private CanvasGroup canvasGroupWork;
    [SerializeField]
    private RawImage imageThumbnail;
    [SerializeField]
    private Button buttonLoad;
    [SerializeField]
    private Sprite spriteLoad;
    [SerializeField]
    private Button buttonImport;
    [SerializeField]
    private Sprite spriteImport;
    [SerializeField]
    private Button buttonCancel;
    [SerializeField]
    private Button buttonDelete;
    [SerializeField]
    private Sprite spriteDelete;
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject prefabButton;
    private List<string> listPath;
    private int thumbnailNum;
    private Dictionary<int, StudioNode> dicPage;
    private int pageNum;
    private int select;

    public SceneLoadScene()
    {
      base.\u002Ector();
    }

    public void OnClickThumbnail(int _id)
    {
      ((Behaviour) this.canvasWork).set_enabled(true);
      this.canvasGroupWork.Enable(true, false);
      this.select = 12 * SceneLoadScene.page + _id;
      this.imageThumbnail.set_texture(this.buttonThumbnail[_id].texture);
    }

    private void OnClickClose()
    {
      Singleton<Scene>.Instance.UnLoad();
    }

    private void OnClickPage(int _page)
    {
      this.SetPage(_page);
    }

    private void OnClickLoad()
    {
      this.canvasGroupWork.Enable(false, false);
      this.StartCoroutine(this.LoadScene(this.listPath[this.select]));
    }

    [DebuggerHidden]
    private IEnumerator LoadScene(string _path)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SceneLoadScene.\u003CLoadScene\u003Ec__Iterator0()
      {
        _path = _path,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator NotificationLoadCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SceneLoadScene.\u003CNotificationLoadCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void OnClickImport()
    {
      Singleton<Studio.Studio>.Instance.ImportScene(this.listPath[this.select]);
      ((Behaviour) this.canvasWork).set_enabled(false);
      this.StartCoroutine("NotificationImportCoroutine");
    }

    [DebuggerHidden]
    private IEnumerator NotificationImportCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SceneLoadScene.\u003CNotificationImportCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void OnClickCancel()
    {
      ((Behaviour) this.canvasWork).set_enabled(false);
    }

    private void OnClickDelete()
    {
      CheckScene.sprite = this.spriteDelete;
      // ISSUE: method pointer
      CheckScene.unityActionYes = new UnityAction((object) this, __methodptr(OnSelectDeleteYes));
      // ISSUE: method pointer
      CheckScene.unityActionNo = new UnityAction((object) this, __methodptr(OnSelectDeleteNo));
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "StudioCheck",
        isAdd = true
      }, false);
    }

    private void OnSelectDeleteYes()
    {
      Singleton<Scene>.Instance.UnLoad();
      File.Delete(this.listPath[this.select]);
      ((Behaviour) this.canvasWork).set_enabled(false);
      this.InitInfo();
      this.SetPage(SceneLoadScene.page);
    }

    private void OnSelectDeleteNo()
    {
      Singleton<Scene>.Instance.UnLoad();
    }

    private void InitInfo()
    {
      for (int index = 0; index < this.transformRoot.get_childCount(); ++index)
        Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
      this.transformRoot.DetachChildren();
      List<KeyValuePair<DateTime, string>> list = ((IEnumerable<string>) System.IO.Directory.GetFiles(UserData.Create("studio/scene"), "*.png")).Select<string, KeyValuePair<DateTime, string>>((Func<string, KeyValuePair<DateTime, string>>) (s => new KeyValuePair<DateTime, string>(File.GetLastWriteTime(s), s))).ToList<KeyValuePair<DateTime, string>>();
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");
      list.Sort((Comparison<KeyValuePair<DateTime, string>>) ((a, b) => b.Key.CompareTo(a.Key)));
      Thread.CurrentThread.CurrentCulture = currentCulture;
      this.listPath = list.Select<KeyValuePair<DateTime, string>, string>((Func<KeyValuePair<DateTime, string>, string>) (v => v.Value)).ToList<string>();
      this.thumbnailNum = this.listPath.Count;
      this.pageNum = this.thumbnailNum / 12 + (this.thumbnailNum % 12 == 0 ? 0 : 1);
      this.dicPage.Clear();
      for (int key = 0; key < this.pageNum; ++key)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SceneLoadScene.\u003CInitInfo\u003Ec__AnonStorey3 infoCAnonStorey3 = new SceneLoadScene.\u003CInitInfo\u003Ec__AnonStorey3();
        // ISSUE: reference to a compiler-generated field
        infoCAnonStorey3.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.prefabButton);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        StudioNode component = (StudioNode) gameObject.GetComponent<StudioNode>();
        component.active = true;
        // ISSUE: reference to a compiler-generated field
        infoCAnonStorey3.page = key;
        // ISSUE: method pointer
        component.addOnClick = new UnityAction((object) infoCAnonStorey3, __methodptr(\u003C\u003Em__0));
        component.text = string.Format("{0}", (object) (key + 1));
        this.dicPage.Add(key, component);
      }
    }

    private void SetPage(int _page)
    {
      StudioNode studioNode = (StudioNode) null;
      if (this.dicPage.TryGetValue(SceneLoadScene.page, out studioNode))
        studioNode.select = false;
      _page = Mathf.Clamp(_page, 0, this.pageNum - 1);
      int num = 12 * _page;
      for (int index = 0; index < 12; ++index)
      {
        int n = num + index;
        if (!MathfEx.RangeEqualOn<int>(0, n, this.thumbnailNum - 1))
        {
          this.buttonThumbnail[index].interactable = false;
        }
        else
        {
          this.buttonThumbnail[index].texture = (Texture) PngAssist.LoadTexture(this.listPath[n]);
          this.buttonThumbnail[index].interactable = true;
        }
      }
      SceneLoadScene.page = _page;
      if (this.dicPage.TryGetValue(SceneLoadScene.page, out studioNode))
        studioNode.select = true;
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }

    private void Awake()
    {
      this.InitInfo();
      this.SetPage(SceneLoadScene.page);
      // ISSUE: method pointer
      ((UnityEvent) this.buttonClose.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickClose)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonLoad.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickLoad)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonImport.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickImport)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonCancel.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickCancel)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonDelete.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDelete)));
      ((Behaviour) this.canvasWork).set_enabled(false);
    }
  }
}
