using System;
using TwentyOz.VivenSDK.Scripts.Core.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace TwentyOz.VivenSDK.Scripts.Editor
{
    public class VivenLoginWindow : EditorWindow
    {
        private string _id       = "";
        private string _password = "";
        private LoginDomain _domain;
        
        public static void ShowWindow()
        {
            var window = GetWindow<VivenLoginWindow>();
            // window size height
            window.minSize      = new Vector2(300, 50);
            window.maxSize      = new Vector2(300, 100);
            window.titleContent = new GUIContent("Login");
            window.Show();
        }

        private void OnGUI()
        {
            CreateGUI();
        }

        private void CreateGUI()
        {
            // Login UI를 구현
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Viven Login");
            
            _id       = EditorGUILayout.TextField("ID", _id);
            _password = EditorGUILayout.PasswordField("Password", _password);
            var options = Enum.GetNames(typeof(LoginDomain));
            _domain = (LoginDomain)EditorGUILayout.Popup("Domain", (int)_domain, options);

            if (_domain != VivenDomain.CurrentDomain)
            {
                VivenDomain.SetDomain(_domain);
                var token = EditorPrefs.GetString("user-token");
                if (!string.IsNullOrEmpty(token)) EditorPrefs.DeleteKey("user-token");
            }
                
            if (GUILayout.Button("Login"))
            {
                if (_domain == LoginDomain.None)
                {
                    EditorUtility.DisplayDialog("도메인 오류", "도메인 설정을 해주세요.", "OK");
                    EditorGUILayout.EndVertical();
                    // refresh gui
                    Repaint();
                    return; // 밑에 실행 안되게 하기
                }
                
                var formData = new WWWForm();
                formData.AddField("loginId", _id);
                formData.AddField("pw", _password);
                var request = VivenAPI.GetLoginToken(VivenDomain.CurrentDomain, formData);
                request.SendWebRequest().completed += operation =>
                {
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        EditorUtility.DisplayDialog("로그인에 실패하였습니다.", request.downloadHandler.text, "OK");
                        VivenLauncher.IsLogin = false;
                        EditorGUILayout.EndVertical();
                        // refresh gui
                        Repaint();
                        return; // 밑에 실행 안되게 하기
                    }

                    var tokenData = JsonUtility.FromJson<LoginToken>(request.downloadHandler.text);
                    EditorPrefs.SetString("user-token", tokenData.token);

                    VivenLauncher.IsLogin = true;
                };

                Close();
            }

            EditorGUILayout.EndVertical();

            // refresh gui
            Repaint();
        }
    }
}