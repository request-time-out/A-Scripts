// Decompiled with JetBrains decompiler
// Type: CTS.CTSDemoController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CTS
{
  public class CTSDemoController : MonoBehaviour
  {
    [Header("Target")]
    public GameObject m_target;
    [Header("Walk Controller")]
    public CTSWalk m_walkController;
    private CharacterController m_characterController;
    [Header("Fly Controller")]
    public CTSFly m_flyController;
    [Header("Look Controller")]
    public CTSLook m_lookController;
    [Header("Profiles")]
    public CTSProfile m_unityProfile;
    public CTSProfile m_liteProfile;
    public CTSProfile m_basicProfile;
    public CTSProfile m_advancedProfile;
    public CTSProfile m_tesselatedProfile;
    [Header("UX Text")]
    public Text m_mode;
    public Text m_readme;
    public Text m_instructions;
    [Header("Post FX")]
    public ScriptableObject m_postFX;
    private Component m_postProcessingComponent;

    public CTSDemoController()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (Object.op_Equality((Object) this.m_target, (Object) null))
        this.m_target = ((Component) Camera.get_main()).get_gameObject();
      try
      {
        if (Object.op_Inequality((Object) this.m_postFX, (Object) null))
        {
          Camera camera = Camera.get_main();
          if (Object.op_Equality((Object) camera, (Object) null))
            camera = (Camera) Object.FindObjectOfType<Camera>();
          if (Object.op_Inequality((Object) camera, (Object) null))
          {
            Type type = CTSDemoController.GetType("UnityEngine.PostProcessing.PostProcessingBehaviour");
            if (type != (Type) null)
            {
              GameObject gameObject = ((Component) camera).get_gameObject();
              this.m_postProcessingComponent = gameObject.GetComponent(type);
              if (Object.op_Equality((Object) this.m_postProcessingComponent, (Object) null))
                this.m_postProcessingComponent = gameObject.AddComponent(type);
              if (Object.op_Inequality((Object) this.m_postProcessingComponent, (Object) null))
              {
                FieldInfo field = type.GetField("profile", BindingFlags.Instance | BindingFlags.Public);
                if (field != (FieldInfo) null)
                  field.SetValue((object) this.m_postProcessingComponent, (object) this.m_postFX);
                ((Behaviour) this.m_postProcessingComponent).set_enabled(false);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) "Failed to set up post fx.");
      }
      if (Object.op_Equality((Object) this.m_flyController, (Object) null))
        this.m_flyController = (CTSFly) this.m_target.GetComponent<CTSFly>();
      if (Object.op_Equality((Object) this.m_flyController, (Object) null))
        this.m_flyController = (CTSFly) this.m_target.AddComponent<CTSFly>();
      ((Behaviour) this.m_flyController).set_enabled(false);
      if (Object.op_Equality((Object) this.m_characterController, (Object) null))
        this.m_characterController = (CharacterController) this.m_target.GetComponent<CharacterController>();
      if (Object.op_Equality((Object) this.m_characterController, (Object) null))
      {
        this.m_characterController = (CharacterController) this.m_target.AddComponent<CharacterController>();
        this.m_characterController.set_height(4f);
      }
      ((Collider) this.m_characterController).set_enabled(false);
      if (Object.op_Equality((Object) this.m_walkController, (Object) null))
        this.m_walkController = (CTSWalk) this.m_target.GetComponent<CTSWalk>();
      if (Object.op_Equality((Object) this.m_walkController, (Object) null))
      {
        this.m_walkController = (CTSWalk) this.m_target.AddComponent<CTSWalk>();
        this.m_walkController.m_controller = this.m_characterController;
      }
      ((Behaviour) this.m_walkController).set_enabled(false);
      if (Object.op_Equality((Object) this.m_lookController, (Object) null))
        this.m_lookController = (CTSLook) this.m_target.GetComponent<CTSLook>();
      if (Object.op_Equality((Object) this.m_lookController, (Object) null))
      {
        this.m_lookController = (CTSLook) this.m_target.AddComponent<CTSLook>();
        this.m_lookController._playerRootT = this.m_target.get_transform();
        this.m_lookController._cameraT = this.m_target.get_transform();
      }
      ((Behaviour) this.m_lookController).set_enabled(false);
      if (Object.op_Inequality((Object) this.m_instructions, (Object) null))
      {
        string str = string.Empty;
        if (Object.op_Inequality((Object) this.m_unityProfile, (Object) null))
          str += "Controls: 1. Unity";
        if (Object.op_Inequality((Object) this.m_liteProfile, (Object) null))
          str = str.Length <= 0 ? "Controls: 2. Lite" : str + ", 2. Lite";
        if (Object.op_Inequality((Object) this.m_basicProfile, (Object) null))
          str = str.Length <= 0 ? "Controls: 3. Basic" : str + ", 3. Basic";
        if (Object.op_Inequality((Object) this.m_advancedProfile, (Object) null))
          str = str.Length <= 0 ? "Controls: 4. Advanced" : str + ", 4. Advanced";
        if (Object.op_Inequality((Object) this.m_tesselatedProfile, (Object) null))
          str = str.Length <= 0 ? "Controls: 5. Tesselated" : str + ", 5. Tesselated";
        if (Object.op_Inequality((Object) this.m_flyController, (Object) null))
          str = str.Length <= 0 ? "Controls: 6. Fly" : str + ", 6. Fly";
        if (Object.op_Inequality((Object) this.m_walkController, (Object) null))
          str = str.Length <= 0 ? "Controls: 7. Walk" : str + ", 7. Walk";
        if (Object.op_Inequality((Object) this.m_postProcessingComponent, (Object) null))
          str = str.Length <= 0 ? "Controls: P. Post FX" : str + ", P. Post FX";
        this.m_instructions.set_text(str.Length <= 0 ? "Controls: ESC. Exit." : str + ", ESC. Exit.");
      }
      this.SelectBasic();
      if (Object.op_Inequality((Object) this.m_flyController, (Object) null))
        ((Behaviour) this.m_flyController).set_enabled(false);
      if (Object.op_Inequality((Object) this.m_walkController, (Object) null))
        ((Behaviour) this.m_walkController).set_enabled(false);
      if (Object.op_Inequality((Object) this.m_characterController, (Object) null))
        ((Collider) this.m_characterController).set_enabled(false);
      if (!Object.op_Inequality((Object) this.m_lookController, (Object) null))
        return;
      ((Behaviour) this.m_lookController).set_enabled(false);
    }

    public void SelectUnity()
    {
      if (!Object.op_Inequality((Object) this.m_unityProfile, (Object) null))
        return;
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_unityProfile);
      if (!Object.op_Inequality((Object) this.m_mode, (Object) null))
        return;
      this.m_mode.set_text("Unity");
    }

    public void SelectLite()
    {
      if (!Object.op_Inequality((Object) this.m_liteProfile, (Object) null))
        return;
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_liteProfile);
      if (!Object.op_Inequality((Object) this.m_mode, (Object) null))
        return;
      this.m_mode.set_text("Lite");
    }

    public void SelectBasic()
    {
      if (!Object.op_Inequality((Object) this.m_basicProfile, (Object) null))
        return;
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_basicProfile);
      if (!Object.op_Inequality((Object) this.m_mode, (Object) null))
        return;
      this.m_mode.set_text("Basic");
    }

    public void SelectAdvanced()
    {
      if (!Object.op_Inequality((Object) this.m_advancedProfile, (Object) null))
        return;
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_advancedProfile);
      if (!Object.op_Inequality((Object) this.m_mode, (Object) null))
        return;
      this.m_mode.set_text("Advanced");
    }

    public void SelectTesselated()
    {
      if (!Object.op_Inequality((Object) this.m_tesselatedProfile, (Object) null))
        return;
      CTSSingleton<CTSTerrainManager>.Instance.BroadcastProfileSelect(this.m_tesselatedProfile);
      if (!Object.op_Inequality((Object) this.m_mode, (Object) null))
        return;
      this.m_mode.set_text("Tesselated");
    }

    public void Fly()
    {
      if (!Object.op_Inequality((Object) this.m_flyController, (Object) null) || ((Behaviour) this.m_flyController).get_isActiveAndEnabled())
        return;
      if (Object.op_Inequality((Object) this.m_characterController, (Object) null))
        ((Collider) this.m_characterController).set_enabled(false);
      if (Object.op_Inequality((Object) this.m_walkController, (Object) null) && ((Behaviour) this.m_walkController).get_isActiveAndEnabled())
        ((Behaviour) this.m_walkController).set_enabled(false);
      if (Object.op_Inequality((Object) this.m_lookController, (Object) null))
        ((Behaviour) this.m_lookController).set_enabled(true);
      ((Behaviour) this.m_flyController).set_enabled(true);
    }

    public void Walk()
    {
      if (!Object.op_Inequality((Object) this.m_walkController, (Object) null) || ((Behaviour) this.m_walkController).get_isActiveAndEnabled())
        return;
      if (Object.op_Inequality((Object) this.m_flyController, (Object) null) && ((Behaviour) this.m_flyController).get_isActiveAndEnabled())
        ((Behaviour) this.m_flyController).set_enabled(false);
      if (Object.op_Inequality((Object) this.m_characterController, (Object) null))
        ((Collider) this.m_characterController).set_enabled(true);
      if (Object.op_Inequality((Object) this.m_lookController, (Object) null))
        ((Behaviour) this.m_lookController).set_enabled(true);
      ((Behaviour) this.m_walkController).set_enabled(true);
    }

    public void PostFX()
    {
      if (!Object.op_Inequality((Object) this.m_postProcessingComponent, (Object) null))
        return;
      if (((Behaviour) this.m_postProcessingComponent).get_isActiveAndEnabled())
        ((Behaviour) this.m_postProcessingComponent).set_enabled(false);
      else
        ((Behaviour) this.m_postProcessingComponent).set_enabled(true);
    }

    private void Update()
    {
      if (Input.GetKeyDown((KeyCode) 49))
        this.SelectUnity();
      else if (Input.GetKeyDown((KeyCode) 50))
        this.SelectLite();
      else if (Input.GetKeyDown((KeyCode) 51))
        this.SelectBasic();
      else if (Input.GetKeyDown((KeyCode) 52))
        this.SelectAdvanced();
      else if (Input.GetKeyDown((KeyCode) 53))
        this.SelectTesselated();
      else if (Input.GetKeyDown((KeyCode) 54))
        this.Fly();
      else if (Input.GetKeyDown((KeyCode) 55))
        this.Walk();
      else if (Input.GetKeyDown((KeyCode) 112))
      {
        this.PostFX();
      }
      else
      {
        if (!Input.GetKeyDown((KeyCode) 27))
          return;
        Application.Quit();
      }
    }

    public static Type GetType(string TypeName)
    {
      Type type1 = Type.GetType(TypeName);
      if (type1 != (Type) null)
        return type1;
      if (TypeName.Contains("."))
      {
        string assemblyString = TypeName.Substring(0, TypeName.IndexOf('.'));
        try
        {
          Assembly assembly = Assembly.Load(assemblyString);
          if (assembly == (Assembly) null)
            return (Type) null;
          Type type2 = assembly.GetType(TypeName);
          if (type2 != (Type) null)
            return type2;
        }
        catch (Exception ex)
        {
        }
      }
      Assembly callingAssembly = Assembly.GetCallingAssembly();
      if (callingAssembly != (Assembly) null)
      {
        Type type2 = callingAssembly.GetType(TypeName);
        if (type2 != (Type) null)
          return type2;
      }
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      for (int index = 0; index < assemblies.GetLength(0); ++index)
      {
        Type type2 = assemblies[index].GetType(TypeName);
        if (type2 != (Type) null)
          return type2;
      }
      foreach (AssemblyName referencedAssembly in callingAssembly.GetReferencedAssemblies())
      {
        Assembly assembly = Assembly.Load(referencedAssembly);
        if (assembly != (Assembly) null)
        {
          Type type2 = assembly.GetType(TypeName);
          if (type2 != (Type) null)
            return type2;
        }
      }
      return (Type) null;
    }
  }
}
