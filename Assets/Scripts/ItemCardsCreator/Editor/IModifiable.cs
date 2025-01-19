using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Publiczny interface po kturego zaimplementowaniu klasy o roznych typach 
 * moga znajdowac sie w tej samej kolekcji lecz wolane na nich moga byc tylko metody z tego interfacu.
 */

public interface IModifiable
{
    public void Save();
    public void Load();
}
