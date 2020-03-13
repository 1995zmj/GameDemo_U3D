﻿using UnityEngine;
#if UNITY_EDITOR
partial class GameLevel {
    public bool HasMissingLevelObjects {
        get {
            if (levelObjects != null) {
                for (int i = 0; i < levelObjects.Length; i++) {
                    if (levelObjects[i] == null) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
    
    // 处理空的引用
    public void RemoveMissingLevelObjects () {
        if (Application.isPlaying) {
            Debug.LogError("Do not invoke in play mode!");
            return;
        }
        int holes = 0;
        for (int i = 0; i < levelObjects.Length - holes; i++) {
            if (levelObjects[i] == null) {
                holes += 1;
                System.Array.Copy(
                    levelObjects, i + 1, levelObjects, i,
                    levelObjects.Length - i - 1 - - holes
                );
                i -= 1;
            }
        }
        System.Array.Resize(ref levelObjects, levelObjects.Length - holes);
    }
    
    public bool HasLevelObject (GameLevelObject o) {
        if (levelObjects != null) {
            for (int i = 0; i < levelObjects.Length; i++) {
                if (levelObjects[i] == o) {
                    return true;
                }
            }
        }
        return false;
    }
    
    public void RegisterLevelObject (GameLevelObject o) {
        if (Application.isPlaying) {
            Debug.LogError("Do not invoke in play mode!");
            return;
        }
        
        if (HasLevelObject(o)) {
            return;
        }
		
        if (levelObjects == null) {
            levelObjects = new GameLevelObject[] { o };
        }
        else {
            System.Array.Resize(ref levelObjects, levelObjects.Length + 1);
            levelObjects[levelObjects.Length - 1] = o;
        }
    }
}
#endif