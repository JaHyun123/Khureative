using System;
using System.IO;
using System.IO.Compression;
using TwentyOz.VivenSDK.Scripts.Core.Common;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.String;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    public class BuildSettingWindow : EditorWindow
    {
        private string _cttId;
        private string _cttBinVal;

        public static void ShowWindow()
        {
            var window = GetWindow<BuildSettingWindow>();
            window.titleContent = new GUIContent("Build Setting");
            window.Show();
        }

        private void OnGUI()
        {
            CreateGUI();
        }

        private void CreateGUI()
        {
            GUILayout.Label("빌드 설정", EditorStyles.boldLabel);
            _cttId = EditorGUILayout.TextField("콘텐츠 ID", _cttId);
            _cttBinVal = EditorGUILayout.TextField("콘텐츠 버전", _cttBinVal);
            if (GUILayout.Button("빌드하기"))
            {
                BuildRoutine(VivenToolbarExtension.GetUserInfo());
                Close();
            }
        }

        private void BuildRoutine(UserInfo userInfo)
        {
            // 현재 열려있는 Scene 가져오기
            var currentScenePath = SceneManager.GetActiveScene().path;


            // Addressable Asset 설정 가져오기
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            //RemoteCatalog를 True로 해야함.
            settings.BuildRemoteCatalog = true;

            var serverDataPath = $"ServerData/{userInfo.mbrId}/{SceneManager.GetActiveScene().name}/[BuildTarget]";
            var loadDataPath = $"{VivenDomain.CDN.GetDomainCDN()}/attach/ctt/map/data/{_cttId}/{_cttBinVal}/";
            settings.profileSettings.SetValue(settings.activeProfileId, "Remote.BuildPath", serverDataPath);
            settings.profileSettings.SetValue(settings.activeProfileId, "Remote.LoadPath", loadDataPath);

            // Catalog의 Player Version Override를 "vmap"으로 변경
            settings.OverridePlayerVersion = _cttBinVal;

            // Addressable Asset Group (Default)의 Build & Local Path를 Remote로 변경
            // Build & LocalPath를 Remote로 변경
            foreach (var group in settings.groups)
            foreach (var schema in group.Schemas)
            {
                if (schema is not BundledAssetGroupSchema buildRemoteCatalogSchema) continue;

                buildRemoteCatalogSchema.BuildPath.SetVariableByName(settings, "Remote.BuildPath");
                buildRemoteCatalogSchema.LoadPath.SetVariableByName(settings, "Remote.LoadPath");
            }

            // 기존 Default Group에 있는 Scene 제거
            foreach (var entry in settings.DefaultGroup.entries)
                if (entry.guid != AssetDatabase.AssetPathToGUID(currentScenePath))
                    settings.RemoveAssetEntry(entry.guid);

            // Scene을 Addressable Group에 추가
            var defaultGroup = settings.DefaultGroup;
            var sceneEntry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(currentScenePath), defaultGroup);
            // Test에서는 vmap이지만 실제 빌드에서는 해당 Scene의 BinId로 해야함.
            sceneEntry.address = _cttId + _cttBinVal;

            // 변경 사항 저장
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            // 자동으로 서버에 만들어진 Addressable Group에 Scene을 추가하고 업로드를 진행합니다.
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings
                .ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();

            // cttId, cttBinId를 포함하는 manifest.json 파일을 생성함.
            var manifestPath = Application.dataPath.Replace("/Assets", "") + "/ServerData/" + userInfo.mbrId + "/" +
                               SceneManager.GetActiveScene().name + "/" +
                               EditorUserBuildSettings.activeBuildTarget + "/manifest.json";

            var manifest = new VivenSDKVMapManifestData();
            manifest.cttDataType = "Addressable";
            manifest.binval = _cttBinVal;
            manifest.cttId = _cttId;

            var json = JsonUtility.ToJson(manifest);
            File.WriteAllText(manifestPath, json);

            // Hash 파일을 생성함.
            var hashPath = Application.dataPath.Replace("/Assets", "") + "/ServerData/" + userInfo.mbrId + "/" +
                           SceneManager.GetActiveScene().name + "/" +
                           EditorUserBuildSettings.activeBuildTarget + "/manifest.hash";
            File.WriteAllText(hashPath, hashPath.GetHashCode().ToString());

            BuildResultWindow.ShowWindow();


            // Upload to VIVEN Addressable Server
            ArchiveAllFiles(userInfo);
            // UploadVMap();
        }

        private static void ArchiveAllFiles(UserInfo userInfo)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var scenePath = Application.dataPath.Replace("/Assets", "") + "/ServerData/" + userInfo.mbrId + "/" +
                            SceneManager.GetActiveScene().name;
            var bundlePath = scenePath + "/" + buildTarget;

            // archive files to zip
            var zipTargetDirectory = Application.dataPath.Replace("/Assets", "") + "/ServerData/" + userInfo.mbrId;
            var zipPath = zipTargetDirectory + "/" + $"{SceneManager.GetActiveScene().name}.vmap";
            if (File.Exists(zipPath)) File.Delete(zipPath);

            ZipFile.CreateFromDirectory(bundlePath, zipPath);

            // Delete Directory of build target
            Directory.Delete(scenePath, true);

            // Ask user to vmap file to move another directory
            var movePath =
                EditorUtility.SaveFilePanel("Save V-Map", "", $"{SceneManager.GetActiveScene().name}.vmap", "vmap");
            if (IsNullOrEmpty(movePath)) return;

            //파일이 존재하면 삭제
            if (File.Exists(movePath))
                File.Delete(movePath);
            File.Move(zipPath, movePath);
        }
    }
}