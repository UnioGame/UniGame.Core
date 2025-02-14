namespace UniModules.Runtime.Network
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Cysharp.Threading.Tasks;
    using global::UniCore.Runtime.ProfilerTools;
    using UnityEngine;
    using UnityEngine.Networking;

    [Serializable]
    public class WebRequestBuilder
    {
        public const string AuthorizationHeader = "Authorization";
        public const string ContentTypeHeader = "Content-Type";
        public const string ContentTypeJson = "application/json";
        public const string ContentTypeBinary = "application/octet-stream";
        public const string BearerValue = "Bearer {0}";
        public const string VersionParameter = "v";
        
        public static readonly Dictionary<string,string> EmptyData = new();
        public static readonly Sprite EmptySprite = Sprite
            .Create(new Texture2D(8, 8), new Rect(0, 0, 8, 8), Vector2.zero);
        
        public string userToken = string.Empty;
        public bool addVersion = true;

        private Vector2 _spritePivot = new(0.5f, 0.5f);

        public string GenerateSignUpUrl(string uuid, string rewardCode)
        {
            if(string.IsNullOrEmpty(rewardCode)) throw new ArgumentNullException("rewardCode");
            // # timestamp + "bastion" + "booster_ads" (тип Бустера) + user.uuid
            // # Такая строка должна получится для первой волны:
            //             1722874756449bastionbooster_ads4910b5d0-6508-4d8a-823c-2bc6d4df58bd
            // # Хешируем ее в md5 и получаем, например:
            //                 c0d64f977d7c987069114f73081e5325
            // # Используем эту строку как подпись
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            var data = timestamp + "bastion" + rewardCode + uuid;
            var md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var bytes = System.Text.Encoding.UTF8.GetBytes(data);
            var hash = md5Provider.ComputeHash(bytes);
            var signUpUrl = BitConverter.ToString(hash).Replace("-", "").ToLower();
            GameLog.Log(data, Color.cyan);
            GameLog.Log(signUpUrl, Color.cyan);
            return signUpUrl;
        }
                
        public async UniTask<WebServerResult> GetAsync(string url,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
        {
            var webRequest = BuildGetRequest(url, parameters, headers);
            return await SendRequestAsync(webRequest);
        }
        
        public async UniTask<WebServerResult> PostAsync(string url, WWWForm form)
        {
            var post = BuildPostRequest(url,form);
            return await SendRequestAsync(post);
        }
        
        public async UniTask<WebServerResult> PostAsync(string url, 
            Dictionary<string,string> headers = null, 
            byte[] data = null)
        {
            var post = BuildPostRequest(url,headers:headers,bytes:data,contentType:ContentTypeBinary);
            return await SendRequestAsync(post);
        }
        
        public async UniTask<WebServerResult> PostAsync(string url, 
            string data = null,
            Dictionary<string,string> headers = null)
        {
            var post = BuildPostRequest(url,data,headers);
            return await SendRequestAsync(post);
        }
        
        public string SetParameters(string url, Dictionary<string, string> parameters = null)
        {
            if (parameters is not { Count: > 0 }) return url;
            
            var uriBuilder = new UriBuilder(url);
            var query = uriBuilder.Query.ParseQueryString();
            
            if (addVersion)
            {
                query[VersionParameter] = Application.version;
            }
            
            foreach (var pair in parameters)
            {
                query[pair.Key] = pair.Value;
            }

            uriBuilder = uriBuilder.SetUrlQueryParameters(query);
            return uriBuilder.ToString();
        }
        
        public UnityWebRequest SetHeaders(UnityWebRequest request, Dictionary<string, string> headers = null)
        {
            request = SetBearerToken(request);
            
            if (headers is not { Count: > 0 }) return request;
            
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key,header.Value);
            }

            return request;
        }

        public UnityWebRequest SetBearerToken(UnityWebRequest request)
        {
            if(!string.IsNullOrEmpty(userToken))
                request.SetRequestHeader(AuthorizationHeader,string.Format(BearerValue,userToken));
            return request;
        }
        
        public UnityWebRequest BuildGetRequest(
            string url,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
        {
            SetParameters(url, parameters);
            
            
            var webRequest = UnityWebRequest.Get(url);
            webRequest = SetHeaders(webRequest, headers);

#if UNITY_EDITOR
            GameLog.Log("[WeRequest]: Get | " + webRequest.url, Color.cyan);
#endif
            
            return webRequest;
        }
        
        public UnityWebRequest BuildPostRequest(string url, WWWForm form)
        {
            return BuildPostRequest(url, headers: form.headers, bytes: form.data,contentType:ContentTypeJson);
        }
        
        public UnityWebRequest BuildPostRequest(
            string url,
            string json = null,
            Dictionary<string,string> headers = null)
        {
            json ??= string.Empty;
            
            var data = string.IsNullOrEmpty(json)
                ? Array.Empty<byte>()
                : System.Text.Encoding.UTF8.GetBytes(json);

            var postData = new PostData()
            {
                data = data,
                contentType = ContentTypeJson,
                headers = headers,
                url = url,
            };
            
            return BuildPostRequest(postData);
        }
        
        public UnityWebRequest BuildPostRequest(
            string url,
            Dictionary<string,string> form = null,
            Dictionary<string,string> parameters = null,
            Dictionary<string,string> headers = null,
            byte[] bytes = null,
            string contentType = ContentTypeJson)
        {
            var postData = new PostData()
            {
                url = url,
                data = bytes == null || bytes.Length <=0 
                    ? Array.Empty<byte>() : bytes,
                form = form,
                headers = headers,
                parameters = parameters,
                contentType = contentType,
            };

            return BuildPostRequest(postData);
        }
        
        public UnityWebRequest BuildPostRequest(PostData postData)
        {
            var url = SetParameters(postData.url, postData.parameters);
            
            var form = postData.form ?? EmptyData;
            var webRequest = UnityWebRequest.Post(url,form);
            webRequest = SetHeaders(webRequest, postData.headers);
            
            var bytes = postData.data;
            if (bytes is { Length: > 0 })
            {
                webRequest.uploadHandler = new UploadHandlerRaw(bytes);
                webRequest.SetRequestHeader(ContentTypeHeader, postData.contentType);
            }
            
#if UNITY_EDITOR
            GameLog.Log($"[WeRequest]: Post | {webRequest.url}", Color.cyan);
#endif
            
            return webRequest;
        }
        
        
        public async UniTask<WebServerTexture2DResult> GetTextureAsync(string url,Dictionary<string,string> parameters = null)
        {
            url = SetParameters(url, parameters);
            
            var request = UnityWebRequestTexture.GetTexture(url);
            
            SetHeaders(request, null);
            
            var requestResult = await SendRequestAsync(request);
            
            var result = new WebServerTexture2DResult()
            {
                error = requestResult.error,
                success = requestResult.success,
                texture = null,
            };

            if (!requestResult.success) return result;
            
            result.texture = DownloadHandlerTexture.GetContent(request);
            return result;
        }

        public async UniTask<WebServerSpriteResult> GetSpriteAsync(string url,Dictionary<string,string> parameters = null)
        {
            var texture2DResult = await GetTextureAsync(url,parameters);
            
            var result = new WebServerSpriteResult()
            {
                error = texture2DResult.error,
                success = texture2DResult.success,
                sprite = EmptySprite,
            };
            
            if (!texture2DResult.success) return result;

            var texture = texture2DResult.texture;
            result.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), _spritePivot);
            return result;
        }

        public void SetToken(string token)
        {
            userToken = token;
        }
        
        public string GetServerUrl(string serverUrl,string path)
        {
            return serverUrl.MergeUrl(path);
        }

        private async UniTask<WebServerResult> SendRequestAsync(UnityWebRequest request)
        {
            try
            {
                var asyncOperation = request.SendWebRequest();
                request = await asyncOperation.ToUniTask();
            }
            catch (Exception e)
            {
                GameLog.LogError("error on web request: " + e.Message);
                
                return new WebServerResult
                {
                    success = false,
                    data = null,
                    error = e.Message,
                };
            }
            
            var isSuccessful = request.result == UnityWebRequest.Result.Success;
            
            return new WebServerResult
            {
                success = isSuccessful,
                data = request.downloadHandler.text,
                error = request.error,
            };
        }
        

        // https://t.me/ТВОЙБОТ?start=refПОЛЬЗОВАТЕЛЬUUID
        // (https://t.me/share/url?url=https://t.me/%D0%A2%D0%92%D0%9E%D0%99%D0%91%D0%9E%D0%A2?start=ref%D0%9F%D0%9E%D0%9B%D0%AC%D0%97%D0%9E%D0%92%D0%90%D0%A2%D0%95%D0%9B%D0%ACUUID)
        //public string GetInviteUrl(string uuid) => $"https://t.me/{_settings.bot}?start=ref{uuid}";
    }
    
    
}