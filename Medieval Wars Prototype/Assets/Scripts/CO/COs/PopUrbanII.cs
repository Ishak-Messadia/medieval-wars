public class PopUrbanII : CO
{


    void Start()
    {
        BarLevelMustHaveToActivateCoPower = GetCoPowerBarLimit();
    }


    //!!!!!! PASSIVE POWER


    public override void ActivateDailyPower()
    {
        foreach (Unit unit in playerOwner.unitList)
        {
            switch (unit.unitName)
            {
                case UnitUtil.UnitName.CARRACK:
                case UnitUtil.UnitName.FIRESHIP:
                // case UnitUtil.UnitName.RAMSHIP:
                case UnitUtil.UnitName.TSHIP:
                    unit.moveRange++;
                    break;
            }
        }

        SetAttackAndDefenseBoostsForAllUnits();
    }


    public void SetAttackAndDefenseBoostsForAllUnits()
    {
        foreach (Unit unit in playerOwner.unitList)
        {
            SetAttackAndDefenseBoostsForOneUnit(unit);
        }
    }

    public void SetAttackAndDefenseBoostsForOneUnit(Unit unit)
    {
        switch (unit.unitName)
        {
            // naval units
            case UnitUtil.UnitName.CARRACK:
            case UnitUtil.UnitName.FIRESHIP:
            // case UnitUtil.UnitName.RAMSHIP:
            case UnitUtil.UnitName.TSHIP:
                unit.SetAttackAndDefenseBoosts(1.20f, 1.20f);
                break;
        }
    }





    //!!!!!!! SUPER POWER 


    public override void ActivateSuperPower()
    {
        if (CanActivateSuperPower == false) return;
        numberOfTimeThatTheSuperPowerHasBeenUsed++;
        isSuperPowerActivated = true;
        foreach (Unit unit in playerOwner.unitList)
        {
            switch (unit.unitName)
            {
                case UnitUtil.UnitName.CAVALRY:
                case UnitUtil.UnitName.RCAVALRY:
                case UnitUtil.UnitName.FIRESHIP:
                    // case UnitUtil.UnitName.RAMSHIP:
                    if (unit.hasMoved == true)
                    {
                        unit.hasMoved = false;
                        unit.unitView.ResetHighlightedUnit();
                        unit.SetSpecialAttackAndDefenseBoostsInSuperPower(0.80f, 0.70f);
                    }
                    break;
            }
        }
        BarLevelMustHaveToActivateCoPower = GetCoPowerBarLimit();
        CanActivateSuperPower = false;

    }


    public override void DeactivateSuperPower()
    {
        isSuperPowerActivated = false;
        foreach (Unit unit in playerOwner.unitList)
        {
            switch (unit.unitName)
            {
                case UnitUtil.UnitName.CAVALRY:
                case UnitUtil.UnitName.RCAVALRY:
                case UnitUtil.UnitName.FIRESHIP:
                    // case UnitUtil.UnitName.RAMSHIP:
                    unit.ResetSpecialAttackAndDefenseBoostsInSuperPower();
                    break;
            }
        }
    }


    //!!!!!!!!!!!!!!!!!!!!!!!!!


    public override int GetTerrainDefenceStart(Terrain terrain)
    {
        // daily power ONLY 
        switch (terrain.terrainName)
        {
            case TerrainsUtils.TerrainName.DOCK:
            case TerrainsUtils.TerrainName.SEA:
            case TerrainsUtils.TerrainName.SHOAL:
            case TerrainsUtils.TerrainName.RIVER:
            case TerrainsUtils.TerrainName.REEF:
                return TerrainsUtils.defenceStars[terrain.TerrainIndex] + 2;

            default:
                return TerrainsUtils.defenceStars[terrain.TerrainIndex];
        }
    }



}
