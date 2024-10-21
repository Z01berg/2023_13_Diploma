using UnityEngine;

/**
 * Publiczna clasa TimerData ma za zadanie {get; set} 4 rzeczy:
 * - Value (znaczenie zegarka)
 * - Tag (Tag z enuma "boss"; "player")
 * - HP (znaczenie HP)
 * - EnemyID (identyfikator przeciwnika)
 */

public class TimerData
{
    public int Value { get; set; }
    public string Tag { get; set; }
    public GameObject HP { get; set; }
    public int EnemyId { get; set;}
}