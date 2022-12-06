#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
using BepInEx.Logging;
#endif

using System.Reflection;
using Semver;
using UnityEngine;
using KitchenLib.Utils;
using System.Runtime.CompilerServices;
using HarmonyLib;
using KitchenLib.Registry;
using static MelonLoader.MelonLogger;
using KitchenLib.Customs;

namespace KitchenLib
{
	public abstract class BaseMod : LoaderMod
	{
		public string ModID = "";
		public string ModName = "";
		public string ModVersion = "";
		public string CompatibleVersions = "";

		public static KitchenVersion version;
		public static SemVersion semVersion;

		public static BaseMod instance;
		
#if BEPINEX || WORKSHOP
		public HarmonyLib.Harmony harmonyInstance;
#endif
		public BaseMod(string modID, string modName, string author, string modVersion, string compatibleVersions, Assembly assembly) : base()
		{
			instance = this;
			ModID = modID;
			ModName = modName;
			ModVersion = modVersion;
			CompatibleVersions = compatibleVersions;

			if (!Debug.isDebugBuild)
				version = new KitchenVersion(Application.version);
			else
				version = new KitchenVersion("");
			
#if BEPINEX || WORKSHOP
			harmonyInstance = new HarmonyLib.Harmony(modID);
			harmonyInstance.PatchAll(assembly);
#endif

			semVersion = new SemVersion(version.Major, version.Minor, version.Patch);
			ModRegistery.Register(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Log(string message)
		{
#if BEPINEX
			Logger.Log(LogLevel.Info, message);
#endif
#if MELONLOADER
			MelonLogger.Msg(message);
#endif
#if WORKSHOP
			Debug.Log(message);
#endif
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string message)
		{
#if BEPINEX
			Logger.Log(LogLevel.Error, message);
#endif
#if MELONLOADER
			MelonLogger.Error(message);
#endif
#if WORKSHOP
			Debug.LogError(message);
#endif
		}
		
		protected abstract void OnFrameUpdate();
		protected abstract void OnInitialise();

#if BEPINEX
		void Update()
		{
			OnFrameUpdate();
		}

		void Awake()
		{
			OnInitialise();
		}
#endif

#if MELONLOADER
		public override void OnUpdate()
		{
			OnFrameUpdate();
		}

		public override void OnInitializeMelon()
		{
			OnInitialise();
		}
#endif

#if WORKSHOP
		protected override void OnUpdate()
		{
			OnFrameUpdate();
		}

		protected override void Initialise()
		{
			OnInitialise();
		}
#endif

		public T AddGameDataObject<T>() where T : CustomGameDataObject, new()
		{
			T gdo = new T();
			gdo.ModName = ModName;
			return CustomGDO.RegisterGameDataObject(gdo);
		}

		public T AddSubProcess<T>() where T : CustomSubProcess, new()
		{
			T subProcess = new T();
			return CustomSubProcess.RegisterSubProcess(subProcess);
		}

		public T AddPreference<T>(string modID, string key, string name) where T : BasePreference, new()
		{
			T preference = new T();
			return PreferenceUtils.Register<T>(modID, key, name);
		}
	}
}