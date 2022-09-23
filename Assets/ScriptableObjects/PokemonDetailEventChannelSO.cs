using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/PokemonDetail Event Channel")]
public class PokemonDetailEventChannelSO : ScriptableObject
{
    public UnityAction<DataManager.PokemonDetail> OnEventRaised;

    public void RaiseEvent(DataManager.PokemonDetail pokemonParam)
    {
        OnEventRaised?.Invoke(pokemonParam);
    }
}
