using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using TwentyOz.VivenSDK.Scripts.Core.Common;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    [InitializeOnLoad]
    public class VivenToolbarExtension
    {
        private static UserInfo _userInfo;

        private static string _cttId;
        private static string _cttBinId;

        static VivenToolbarExtension()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarLeftGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarRightGUI);
        }

        private static bool IsLogin =>
            EditorPrefs.HasKey("user-token") && VivenDomain.CurrentDomain != LoginDomain.None;

        private static void OnToolbarLeftGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("Test on VIVEN", "VIVEN Play")))
            {
                // 일단, 비벤에서 열어서 테스트 하는 기능을 구현할 수가 없음.
                // Local Temp Directory에 V-Map을 만들어서 실행하는 방법을 생각해보자.

                // Clear all files in Tmp Directory
                var tmpPath = $"{Path.GetTempPath()}/VivenSDK/";
                if (Directory.Exists(tmpPath)) Directory.Delete(tmpPath, true); // Delete Directory
                Directory.CreateDirectory(tmpPath);                             // Create Directory

                if (!ValidateCanBuildMap()) return; // Validation Check

                var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
                
                //RemoteCatalog를 True로 해야함.
                settings.BuildRemoteCatalog = true;

                // 기존 Default Group에 있는 Scene 제거
                foreach (var entry in settings.DefaultGroup.entries)
                    if (entry.guid != AssetDatabase.AssetPathToGUID(SceneManager.GetActiveScene().path))
                        settings.RemoveAssetEntry(entry.guid);

                // Scene을 Addressable Group에 추가
                var defaultGroup = settings.DefaultGroup;
                settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(SceneManager.GetActiveScene().path), defaultGroup);

                settings.profileSettings.SetValue(settings.activeProfileId, "Remote.BuildPath", tmpPath);
                settings.profileSettings.SetValue(settings.activeProfileId, "Remote.LoadPath", tmpPath);
                // Addressable Asset Group (Default)의 Build & Local Path를 Remote로 변경
                
                settings.OverridePlayerVersion = "0.1";
                
                // Build & LocalPath를 Remote로 변경
                foreach (var group in settings.groups)
                foreach (var schema in group.Schemas)
                {
                    if (schema is not BundledAssetGroupSchema buildRemoteCatalogSchema) continue;

                    buildRemoteCatalogSchema.BuildPath.SetVariableByName(settings, "Remote.BuildPath");
                    buildRemoteCatalogSchema.LoadPath.SetVariableByName(settings, "Remote.LoadPath");
                }
                
                // Default group의 첫번째 Asset의 Path를 "vmap"으로 변경
                settings.DefaultGroup.entries.First().address = "vmap";

                // 변경 사항 저장
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();

                // 자동으로 서버에 만들어진 Addressable Group에 Scene을 추가하고 업로드를 진행합니다.
                AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
                AddressableAssetSettings.BuildPlayerContent();

                // Process를 돌린다.
                PlayViven();
            }
        }

        private static void PlayViven()
        {
            const string registryPath = @"HKEY_CLASSES_ROOT\viven\shell\open\command";
            var          val          = Registry.GetValue(registryPath, "", "").ToString();
            var vivenPath = val.Remove(val.Length - 5, 5).Replace("\"", "");
            var processInfo = new ProcessStartInfo
            {
                Arguments       = $"viven://{VivenDomain.WebURL.GetDomainWebURL()}?d={VivenDomain.DTS.GetDomainDTS()}&t={EditorPrefs.GetString("user-token")}&s=true",
                UseShellExecute = true,
                FileName        = vivenPath,
            };
            Debug.Log(vivenPath);
            Process.Start(processInfo);
        }

        private static void OnToolbarRightGUI()
        {
            if (GUILayout.Button(new GUIContent("Build V-Map", "Build V-Map")))
            {
                if (!ValidateCanBuildMap())
                    return;
                
                BuildSettingWindow.ShowWindow();
            }

            GUILayout.FlexibleSpace();
            
            // Check logged in
            if (IsLogin && Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Web에서 User Profile을 가져온다.
                var request = VivenAPI.GetUserProfile(VivenDomain.CurrentDomain,EditorPrefs.GetString("user-token"));

                request.SendWebRequest().completed += _ =>
                {
                    _userInfo = JsonUtility.FromJson<UserInfo>(request.downloadHandler.text);
                    EditorApplication.RepaintHierarchyWindow();
                };
            }

            if (IsLogin) GUILayout.Label(_userInfo.nickname);

            // Login 버튼 (Check logged in by EditorPrefs)
            if (GUILayout.Button(IsLogin ? "Logout" : "Login"))
            {
                if (IsLogin)
                {
                    _userInfo = default;
                    VivenLauncher.Logout();
                }
                else
                {
                    VivenLauncher.Login();
                }
            }
        }

        private static bool ValidateCanBuildMap()
        {
            // Check internet connection
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("인터넷 연결이 필요합니다.");
                EditorUtility.DisplayDialog("Error", "인터넷 연결이 필요합니다.", "OK");
                return false;
            }

            if (!IsLogin)
            {
                Debug.LogError("로그인이 필요합니다.");
                EditorUtility.DisplayDialog("Error", "로그인이 필요합니다.", "OK");
                return false;
            }

            // Scene에 Camera가 있는지 확인
            if (Object.FindAnyObjectByType<Camera>(FindObjectsInactive.Include))
            {
                Debug.LogError("Scene에 Camera가 있을 수 없습니다.");
                EditorUtility.DisplayDialog("Error", "Scene에 Camera가 있을 수 없습니다.", "OK");
                return false;
            }

            if (!Object.FindAnyObjectByType<VivenMapEnvironment>())
            {
                Debug.LogError("Scene에 VivenMapEnvironment가 없습니다.");
                EditorUtility.DisplayDialog("Error", "Scene에 VivenMapEnvironment가 없습니다.", "OK");
                return false;
            }

            return true;
        }

        public static void UploadVMap()
        {
            // 자동으로 서버에 만들어진 Addressable Group에 Scene을 추가하고 업로드를 진행합니다.
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();

            // 변경 사항 저장
            EditorUtility.SetDirty(AddressableAssetSettingsDefaultObject.Settings);
            AssetDatabase.SaveAssets();

            // 위에서 빌드하여 만들어진 Addressable Asset bundle을 서버에 업로드 합니다.
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var bundlePath  = Application.dataPath.Replace("/Assets", "") + "/ServerData/" + _userInfo.mbrId + "/" + SceneManager.GetActiveScene().name + "/" + buildTarget;
            var allFiles    = Directory.GetFiles(bundlePath);
            foreach (var file in allFiles)
            {
                Debug.Log(file);
                var fileName = file.Split("/").Last();
                var fileData = File.ReadAllBytes(file);
                Upload(fileName, fileData);
            }
        }

        private static void Upload(string fileName, byte[] fileData)
        {
            var form = new WWWForm();
            form.AddField("User-Name", _userInfo.mbrId);
            form.AddField("Folder-Name", SceneManager.GetActiveScene().name);
            form.AddBinaryData("addressable", fileData, fileName);

            var www = VivenAPI.Upload(VivenDomain.CurrentDomain, form);
            www.SendWebRequest();
        }

        public static UserInfo GetUserInfo() => _userInfo;
    }
}