namespace CF
{
    /// <summary>
    /// <c>InjuryRegenListMode</c> is used to dictate whether a 
    /// <c>ThingComp</c> or <c>HediffComp</c> that uses 
    /// <c>ConfigurableRegenUtility</c> should automatically treat
    /// <c>HediffDef</c>s with the <c>Hediff_Injury</c> class as whitelisted or
    /// blacklisted, or if they should be treated normally (<c>None</c>).
    /// </summary>
    public enum InjuryRegenListMode
    {
        /// <summary>
        /// Injuries should be treated as any other <c>Hediff</c>.
        /// </summary>
        None,
        /// <summary>
        /// Injuries should never be accepted, unless explicitly whitelisted.
        /// </summary>
        Blacklist,
        /// <summary>
        /// Injuries should always be accepted, unless explicitly blacklisted.
        /// </summary>
        Whitelist,
    }
}
