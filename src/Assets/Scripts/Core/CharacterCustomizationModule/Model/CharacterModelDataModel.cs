using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Core.MVC;
using Pixiv.VroidSdk;
using UniGLTF;
using UnityEngine;
using UniVRM10;

namespace Core.CharacterCustomizationModule.Model
{
    public class CharacterModelDataModel: ModelBase
    {
        public List<string> ModelId;
        public string ChatRoomSelectModelId;
        public string ChatBotSelectModelId = "FoxGirl.vrm_";
        private static SynchronizationContext s_context;
        private const string ModelPath = "/Model/";
        
        public void LoadModelList()
        {
            ModelId.Clear();
            if (Directory.Exists(Application.persistentDataPath + ModelPath))
            {
                string[] files = Directory.GetFiles(Application.persistentDataPath + ModelPath);
                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    ModelId.Add(fileName);
                    Debug.Log(fileName);
                }
            }
        }
        
        public void CreateCharacter(string modelId, Action<GameObject> onVrmModelLoaded)
        {
            LoadModelFromPath(modelId, onVrmModelLoaded);
        }
        
        public void CreateChatBotCharacter(Action<GameObject> onVrmModelLoaded)
        {
            LoadModelFromPath(ChatBotSelectModelId, onVrmModelLoaded);
        }

        private static void LoadModelFromPath(string modelName, Action<GameObject> onLoad = null)
        {
            var path = Application.persistentDataPath + ModelPath + modelName;
            byte[] bytes = File.ReadAllBytes(path);
            LoadVrm(bytes,
                (vrm) =>
                {
                    Debug.Log("Loaded");
                    onLoad?.Invoke(vrm);
                }, (error) =>
                {
                    Debug.LogError("Load fail");
                });
        }

        private static void LoadVrm(byte[] characterBinary, Action<GameObject> onVrmModelLoaded, 
            Action<ModelLoadFailException> onFailed,
            IMaterialDescriptorGenerator materialGenerator = null)
        {
            materialGenerator ??= new UrpVrm10MaterialDescriptorGenerator();
            Vrm10.LoadBytesAsync(characterBinary, canLoadVrm0X: true, showMeshes: true, materialGenerator: materialGenerator).ContinueWith(result =>
            {
                s_context.Post(_ =>
                {
                    if (result.Exception != null)
                    {
                        var exception = result.Exception.Flatten().InnerException;
                        onFailed?.Invoke(new ModelLoadFailException(exception.Message));
                    }
                    else
                    {
                        onVrmModelLoaded?.Invoke(result.Result.gameObject);
                    }
                }, null);
            });
        }
        
        protected override void OnInit()
        {
            s_context = SynchronizationContext.Current;
            ModelId = new();
            if (!Directory.Exists(Application.persistentDataPath + ModelPath))
            {
                Directory.CreateDirectory(Application.persistentDataPath + ModelPath);
            }
            LoadModelList();
        }
    }
}