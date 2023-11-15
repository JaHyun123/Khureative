using System;
using UnityEngine;
using static System.String;

namespace TwentyOz.VivenSDK.Scripts.Core.Common
{
    /// <summary>
    /// Viven Domain 정보
    /// </summary>
    public static class VivenDomain
    {
        /// <summary>
        /// 현재 로그인 Domain
        /// </summary>
        public static LoginDomain CurrentDomain { get; private set; }
        
        /// <summary>
        /// API Domain
        /// </summary>
        public static class API
        {
            private static readonly string GlobalAPI = Empty;
            private static readonly string XSpaceAPI = "https://api.play.xspace.viven.app";
            private static readonly string DevAPI = "https://api.play.dev.viven.app";

            public static string GetDomainAPI(LoginDomain loginDomain)
            {
                return loginDomain switch
                {
                    LoginDomain.None => Empty,
                    LoginDomain.Global => Empty, //TODO 현재 글로벌 도메인 없음
                    LoginDomain.XSpace => XSpaceAPI,
                    LoginDomain.Dev => DevAPI,
                    _ => throw new ArgumentOutOfRangeException(nameof(loginDomain), loginDomain, null)
                };
            }
            
            public static string GetDomainAPI()
            {
                return GetDomainAPI(CurrentDomain);
            }
            
        }

        public static class WebURL
        {
            private static readonly string GlobalWebUrl = Empty;
            private static readonly string XSpaceWebUrl = "play.xspace.viven.app";
            private static readonly string DevWebUrl = "play.dev.viven.app";

            public static string GetDomainWebURL(LoginDomain loginDomain)
            {
                return loginDomain switch
                {
                    LoginDomain.None => Empty,
                    LoginDomain.Global => Empty, //TODO 현재 글로벌 도메인 없음
                    LoginDomain.XSpace => XSpaceWebUrl,
                    LoginDomain.Dev => DevWebUrl,
                    _ => throw new ArgumentOutOfRangeException(nameof(loginDomain), loginDomain, null)
                };
            }
            
            public static string GetDomainWebURL()
            {
                return GetDomainWebURL(CurrentDomain);
            }
        }
        
        public static class CDN
        {
            private static readonly string GlobalCDN = Empty;
            private static readonly string XSpaceCDN = "https://cdn.play.xspace.viven.app";
            private static readonly string DevCDN = "https://cdn.play.dev.viven.app";

            public static string GetDomainCDN(LoginDomain loginDomain)
            {
                return loginDomain switch
                {
                    LoginDomain.None => Empty,
                    LoginDomain.Global => Empty, //TODO 현재 글로벌 도메인 없음
                    LoginDomain.XSpace => XSpaceCDN,
                    LoginDomain.Dev => DevCDN,
                    _ => throw new ArgumentOutOfRangeException(nameof(loginDomain), loginDomain, null)
                };
            }
            
            public static string GetDomainCDN()
            {
                return GetDomainCDN(CurrentDomain);
            }
        }

        /// <summary>
        /// Domain을 변경해줌.
        /// </summary>
        /// <param name="domain"></param>
        public static void SetDomain(LoginDomain domain)
        {
            CurrentDomain = domain;
        }

        public static class DTS
        {
            private static readonly string GlobalDTS = Empty;
            private static readonly string XSpaceDTS = "ZHRzOi8veHNwYWNlLnZpdmVuLmFwcDo5OTk3";
            private static readonly string DevDTS = "ZHRzOi8vMTI1LjEzMC4xMjUuOTY6MjAwOTc";

            public static string GetDomainDTS(LoginDomain loginDomain)
            {
                return loginDomain switch
                {
                    LoginDomain.None => Empty,
                    LoginDomain.Global => Empty, //TODO 현재 글로벌 도메인 없음
                    LoginDomain.XSpace => XSpaceDTS,
                    LoginDomain.Dev => DevDTS,
                    _ => throw new ArgumentOutOfRangeException(nameof(loginDomain), loginDomain, null)
                };
            }
            
            public static string GetDomainDTS()
            {
                return GetDomainDTS(CurrentDomain);
            }
        }
        
        
        
    }
}