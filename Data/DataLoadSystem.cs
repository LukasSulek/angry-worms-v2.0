using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public enum SceneType
{
    MainScene,
    GameScene
}

public class DataLoadSystem : SingletonPersistent<DataLoadSystem>
{
    public List<LevelData> LevelDatas = new List<LevelData>();
    public List<VegetableData> VegetableDatas = new List<VegetableData>();
    public List<WormData> WormDatas = new List<WormData>();


    public async void LoadScene(SceneType scene, string dataKey = null)
    {
        LevelData data = await LoadDataT<LevelData>(dataKey);

        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Single);
        var sceneLoadTask = new TaskCompletionSource<bool>();

        loadSceneOperation.completed += (op) =>
        {
            sceneLoadTask.SetResult(true);
        };

        await sceneLoadTask.Task;

        if(GameManager.Instance != null) GameManager.Instance.LoadLevelData(data);
    }


    public async Task<IList<T>> LoadDatasT<T>(string label)
    {
        var tcs = new TaskCompletionSource<IList<T>>();

        AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);

        handle.Completed += opHandle =>
        {
            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                tcs.SetResult(opHandle.Result);
            }
            else
            {
                tcs.SetException(new Exception($"Failed to load assets with label {label}"));
            }
            Addressables.Release(handle);
        };

        return await tcs.Task;
    }
    public async Task<T> LoadDataT<T>(string dataKey)
    {
        var tcs = new TaskCompletionSource<T>();
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(dataKey);

        handle.Completed += opHandle => 
        {
            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
                tcs.SetResult(opHandle.Result);
            }
            else
            {
                tcs.SetException(new Exception($"Failed to load asset with key {dataKey}"));
            }
            Addressables.Release(handle);
        };

        return await tcs.Task;
    }
}

