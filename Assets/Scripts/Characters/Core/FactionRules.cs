public static class FactionRules
{
    public static bool CanAttack(FactionType attacker, FactionType target)
    {
        if (attacker == target)
            return false;

        switch (attacker)
        {
            case FactionType.Player:
                return target == FactionType.Enemy;

            case FactionType.Enemy:
                return target == FactionType.Player || target == FactionType.Friendly;

            case FactionType.Friendly:
                return target == FactionType.Enemy;

            default:
                return false;
        }
    }
}