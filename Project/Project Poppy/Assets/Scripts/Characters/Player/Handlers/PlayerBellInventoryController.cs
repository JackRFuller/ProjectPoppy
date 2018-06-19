using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBellInventoryController : Entity
{
    [Header(("Bell Objects"))]
    [SerializeField] private Bell[] bells;
    private Dictionary<int, Soundwave> loadedBells;

    private Soundwave equippedBell;
    private bool hasEquippedBell;

    public void RingBell(int bellIndex)
    {
        if(!bells[bellIndex].hasUnlocked)
            return;

        if (!hasEquippedBell)
        {
            EquipBell(bellIndex);
        }
        else
        {
            equippedBell.GrowSoundWave();
        }
    }

    public void StopRingingBell()
    {
        equippedBell.StopSoundwave();
        hasEquippedBell = false;
    }

    private void EquipBell(int bellIndex)
    {
        if(loadedBells == null)
            loadedBells = new Dictionary<int, Soundwave>();

        if (loadedBells.ContainsKey(bellIndex))
        {
            equippedBell = loadedBells[bellIndex];
        }
        else
        {
            GameObject soundWave = Instantiate(bells[bellIndex].bellSoundwave);
            equippedBell = soundWave.GetComponent<Soundwave>();
            loadedBells.Add(bellIndex, equippedBell);
        }

        equippedBell.ResetSoundwave(transform.position);
        hasEquippedBell = true;
    }

    [System.Serializable]
    public class Bell
    {
        public string bellName;
        public bool hasUnlocked;
        public GameObject bellSoundwave;
    }
}
