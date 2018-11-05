
using System.Collections.Generic;

public class EnemyTypeSC
{
    public int PositionIndex;

    public int TypeIndex;

    public int ColorIndex;
}

public class SaveJson
{
    public List<EnemyTypeSC> enemyTypeSCs;

    public int shutnum;

    public int goalnum;

    public bool musicon;
}