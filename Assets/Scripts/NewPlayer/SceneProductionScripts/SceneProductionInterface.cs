using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneProductionInterface
{
    public string sceneName;
    public string background;
    public CameraInterface camera;
    

    public EntityInterface[] entities;
    public WaypointInterface[] waypoints;

    public bool findEntity(string name, int type){

        for (int i = 0; i < entities.Length; i++){
            if (entities[i].name.Equals(name) && entities[i].type == type)
                return true;
        }

        return false;
    }

    public int findEntityIndex(string name, int type)
    {

        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].name.Equals(name) && entities[i].type == type)
                return i;
        }

        return -1;
    }

}

[System.Serializable]
public class EntityInterface
{
    public string name;
    public int type;
    public int renderLayer;
    public bool flip;
    public int idleState;
    public Vector2 start;
    public Vector2 scale;
    public string stateLayer;
}

[System.Serializable]
public class CameraInterface
{
    public Vector3 startPoint;
    public float size; //Camera size
    public string actor;

    public Vector2 min;

    public Vector2 max;
}

[System.Serializable]
public class WaypointInterface
{
    public string name;
    public Vector2 position;
    public Vector2 scale;
}
