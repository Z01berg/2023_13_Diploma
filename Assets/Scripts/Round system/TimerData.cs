using UnityEngine;

/**
 * Publiczna clasa TimerData ma za zadanie {get; set} 3 rzeczy:
 * - Value (znaczenie zegarka)
 * - Tag (Tag z enuma "boss"; "player")
 * - HP (znaczenie HP)
 */

public class TimerData
{
    public int Value { get; set; }
    public string Tag { get; set; }
    public GameObject HP { get; set; }
}