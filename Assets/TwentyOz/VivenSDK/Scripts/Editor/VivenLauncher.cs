using System.Collections;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
using TwentyOz.VivenSDK.Scripts.Core.Common;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Device.Application;
using Debug = UnityEngine.Debug;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    public static class VivenLauncher
    {
        /// <summary>
        /// Viven 실행파일 경로
        /// </summary>
        private static string _vivenPath;

        /// <summary>
        /// 로그인 여부
        /// </summary>
        private static bool _isLogin;

        /// <summary>
        /// 로그인 여부
        /// </summary>
        public static bool IsLogin { get; set; }

        /// <summary>
        /// 사용자이게 로그인 시키기
        /// </summary>
        public static void Login()
        {
            // 새로운 윈도우를 열어서 로그인을 하도록 사용자 유도를 함.
            VivenLoginWindow.ShowWindow();
        }

        /// <summary>
        /// 사용자 로그아웃 시키기
        /// </summary>
        public static void Logout()
        {
            var request = VivenAPI.Logout(VivenDomain.CurrentDomain);
            request.SetRequestHeader("Authorization", "Bearer " + EditorPrefs.GetString("user-token"));
            request.SendWebRequest().completed += operation =>
            {
                EditorPrefs.DeleteKey("user-token");
                IsLogin = false;
            };
        }

        /// <summary>
        /// Viven Addressable Server에 V-Map을 업로드 합니다. 
        /// </summary>
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
            var bundlePath  = Application.dataPath.Replace("/Assets", "") + "/ServerData/" + SceneManager.GetActiveScene().name + "/" + buildTarget;
            var allFiles    = System.IO.Directory.GetFiles(bundlePath);
            foreach (var file in allFiles)
            {
                Debug.Log(file);
                var fileName = file.Split("/").Last();
                var fileData = System.IO.File.ReadAllBytes(file);
                Upload(fileName, fileData);
            }
        }

        public static void Upload(string fileName, byte[] fileData)
        {
            var form = new WWWForm();
            form.AddField("Folder-Name", SceneManager.GetActiveScene().name);
            form.AddBinaryData("addressable", fileData, fileName);

            var www = VivenAPI.Upload(VivenDomain.CurrentDomain, form);
            www.SendWebRequest();
        }
    }
}