// Decompiled with JetBrains decompiler
// Type: SuperScrollView.MenuSceneScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SuperScrollView
{
  internal class MenuSceneScript : MonoBehaviour
  {
    public Transform mButtonPanelTf;
    private SceneNameInfo[] mSceneNameArray;

    public MenuSceneScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.CreateFpsDisplyObj();
      int childCount = this.mButtonPanelTf.get_childCount();
      int length = this.mSceneNameArray.Length;
      for (int index = 0; index < childCount; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MenuSceneScript.\u003CStart\u003Ec__AnonStorey0 startCAnonStorey0 = new MenuSceneScript.\u003CStart\u003Ec__AnonStorey0();
        if (index >= length)
        {
          ((Component) this.mButtonPanelTf.GetChild(index)).get_gameObject().SetActive(false);
        }
        else
        {
          ((Component) this.mButtonPanelTf.GetChild(index)).get_gameObject().SetActive(true);
          // ISSUE: reference to a compiler-generated field
          startCAnonStorey0.info = this.mSceneNameArray[index];
          Button component = (Button) ((Component) this.mButtonPanelTf.GetChild(index)).GetComponent<Button>();
          // ISSUE: method pointer
          ((UnityEvent) component.get_onClick()).AddListener(new UnityAction((object) startCAnonStorey0, __methodptr(\u003C\u003Em__0)));
          // ISSUE: reference to a compiler-generated field
          ((Text) ((Component) ((Component) component).get_transform().Find("Text")).GetComponent<Text>()).set_text(startCAnonStorey0.info.mName);
        }
      }
    }

    private void CreateFpsDisplyObj()
    {
      if (Object.op_Inequality((Object) Object.FindObjectOfType<FPSDisplay>(), (Object) null))
        return;
      GameObject gameObject = new GameObject();
      ((Object) gameObject).set_name("FPSDisplay");
      gameObject.AddComponent<FPSDisplay>();
      Object.DontDestroyOnLoad((Object) gameObject);
    }
  }
}
