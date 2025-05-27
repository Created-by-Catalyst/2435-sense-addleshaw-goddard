using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SimpleBarricade : Obstacle
{
    public bool _spawnOnMultipleLanes = true;
    public bool impassable = false;
    protected int k_MinObstacleCount = 1;
    protected const int k_MaxObstacleCount = 3;
    protected const int k_LeftMostLaneIndex = -1;
    protected const int k_RightMostLaneIndex = 1;
    
    public override IEnumerator Spawn(TrackSegment segment, float t)
    {
        //the tutorial very firts barricade need to be center and alone, so player can swipe safely in bother direction to avoid it
        bool isTutorialFirst = TrackManager.instance.isTutorial && TrackManager.instance.firstObstacle && segment == segment.manager.currentSegment;

        if (isTutorialFirst)
            TrackManager.instance.firstObstacle = false;

        int maxcount;
        if (!_spawnOnMultipleLanes)
            maxcount = 1;
        else
        {

            if (impassable)
            {
                maxcount = 2;
            }
            else
            {
                k_MinObstacleCount = 2;
                maxcount = k_MaxObstacleCount;
            }
        }


        int count = isTutorialFirst ? 1 : Random.Range(k_MinObstacleCount, maxcount + 1);
        int startLane = isTutorialFirst ? 0 : Random.Range(k_LeftMostLaneIndex, k_RightMostLaneIndex + 1);

        Vector3 position;
        Quaternion rotation;
        segment.GetPointAt(t, out position, out rotation);


       
        for(int i = 0; i < count; ++i)
        {
            int lane = startLane + i;
            lane = lane > k_RightMostLaneIndex ? k_LeftMostLaneIndex : lane;

            AsyncOperationHandle op = Addressables.InstantiateAsync(gameObject.name, position, rotation);
            yield return op;
            if (op.Result == null || !(op.Result is GameObject))
            {
                Debug.LogWarning(string.Format("Unable to load obstacle {0}.", gameObject.name));
                yield break;
            }
            GameObject obj = op.Result as GameObject;

            if (obj == null)
                Debug.Log(gameObject.name);
            else
            {
                obj.transform.position += obj.transform.right * lane * segment.manager.laneOffset;

                obj.transform.SetParent(segment.objectRoot, true);

                //TODO : remove that hack related to #issue7
                Vector3 oldPos = obj.transform.position;
                obj.transform.position += Vector3.back;
                obj.transform.position = oldPos;
            }
        }
    }
}
