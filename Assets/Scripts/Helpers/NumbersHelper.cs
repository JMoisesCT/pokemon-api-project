using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumbersHelper
{
    public static string GetPokemonFormat(this int id)
    {
        string zerosBefore = (id < 10) ? ("00") : ((id < 100) ? ("0") : (""));
        return $"#{zerosBefore}{id}";
    }
}
