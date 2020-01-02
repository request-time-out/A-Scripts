// Decompiled with JetBrains decompiler
// Type: AIProject.InitializeScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AIProject
{
  public class InitializeScene : BaseLoader
  {
    private int _width = 1280;
    private int _height = 720;
    private int _quality = 2;
    [SerializeField]
    [SceneEnum]
    private int _loadLevel = 1;
    [SerializeField]
    private Color _fadeColor = Color.get_black();
    private XElement _xml;
    private bool _full;
    private const string SetupFilePath = "UserData/setup.xml";

    private void Start()
    {
      if (SystemInfo.get_graphicsShaderLevel() < 30)
        MonoBehaviour.print((object) "shaders Non support");
      if (File.Exists("UserData/setup.xml"))
      {
        try
        {
          this._xml = XElement.Load("UserData/setup.xml");
          if (this._xml != null)
          {
            foreach (XElement element in this._xml.Elements())
            {
              switch (element.Name.ToString())
              {
                case "Width":
                  this._width = int.Parse(element.Value);
                  continue;
                case "Height":
                  this._height = int.Parse(element.Value);
                  continue;
                case "FullScreen":
                  this._full = bool.Parse(element.Value);
                  continue;
                case "Quality":
                  this._quality = int.Parse(element.Value);
                  continue;
                default:
                  continue;
              }
            }
            Screen.SetResolution(this._width, this._height, this._full);
            switch (this._quality)
            {
              case 0:
                QualitySettings.SetQualityLevel(0);
                break;
              case 1:
                QualitySettings.SetQualityLevel(2);
                break;
              case 2:
                QualitySettings.SetQualityLevel(4);
                break;
            }
          }
        }
        catch (XmlException ex)
        {
          Debug.LogException((Exception) ex, (Object) ((Component) this).get_gameObject());
          File.Delete("UserData/setup.xml");
        }
      }
      Singleton<Scene>.Instance.SetFadeColor(this._fadeColor);
      string pathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(this._loadLevel);
      int num1 = pathByBuildIndex.LastIndexOf("/");
      int num2 = pathByBuildIndex.LastIndexOf(".");
      string str = pathByBuildIndex.Substring(num1 + 1, num2 - num1 - 1);
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = str,
        isFade = true
      }, false);
      Debug.Log((object) ("First Scene: " + str));
    }
  }
}
