﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour, ILoopParticleContainer
{
    public UIMonsterHpBar hpBar;
    public int monsterIndex;
    public SpineCharacterCtrl spineCharactor;
    public Transform effectFrontPoint, effectBackPoint;
    public RectTransform spinePoint;
    public Canvas canvas;
    /// <summary> 儲存掛在身上的 Loop Particle 特效。 key: passiveId,  value:ParticleObj </summary>
    private Dictionary<int, GameObject> loopParticleObj = new Dictionary<int, GameObject>();

    public void Active(bool torf)
    {
        gameObject.SetActive(torf);
        hpBar.Active(torf);
    }

    public async UniTask MonsterRemove()
    {
        spineCharactor.spine.color = Color.white;
        await spineCharactor.spine.DOFade(0, 0.5f).AsyncWaitForCompletion().AsUniTask();
        Active(false);
    }

    public async UniTask MonsterShow()
    {
        Active(true);
        spineCharactor.spine.color = new Color(1, 1, 1, 0);
        await spineCharactor.spine.DOFade(1, 0.5f).AsyncWaitForCompletion().AsUniTask();
    }

    public bool UpdateLoopParticle(int passiveId, GameObject particle)
    {
        var updateSuccess = false;
        if (passiveId > 0)
        {
            if (particle != null && !loopParticleObj.ContainsKey(passiveId))
            {
                loopParticleObj.Add(passiveId, particle);
                updateSuccess = true;
            }
            else if (particle == null && loopParticleObj.ContainsKey(passiveId))
            {
                var removeParticle = loopParticleObj[passiveId];
                removeParticle.SetActive(false);
                GameObject.DestroyImmediate(removeParticle);
                loopParticleObj.Remove(passiveId);
                updateSuccess = true;
            }
        }

        return updateSuccess;
    }

    public void ResetLoopParticle()
    {
        foreach (var particle in loopParticleObj.Values)
        {
            if (particle == null) continue;
            particle.gameObject.SetActive(false);
            GameObject.DestroyImmediate(particle.gameObject);
        }
        loopParticleObj.Clear();
    }

    public void Clear()
    {
        if (spineCharactor != null) Destroy(spineCharactor.gameObject);
        monsterIndex = -1;
        hpBar.Clear();
        ResetLoopParticle();
        Active(false);
    }

    
}
