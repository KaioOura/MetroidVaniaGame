using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeEnum> upgradeEnums;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasUpgrade(UpgradeEnum upgrade)
    {
        if (upgrade == UpgradeEnum.None)
            return true;

        return upgradeEnums.Contains(upgrade);
    }
}

public enum UpgradeEnum
{
    None,
    SecondAttack,
    ThirdAttack,
    AttackJump,
    DoubleJump,
    Run,
    RunAttack
}
