using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public Health Health{ get; set; }

    public void OnTakeHit(DamageInfos damageInfos);

}
