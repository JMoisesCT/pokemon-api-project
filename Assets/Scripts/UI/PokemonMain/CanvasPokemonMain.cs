using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPokemonMain : MonoBehaviour
{
    [SerializeField] private GameObject _prefabPokemon;
    [SerializeField] private RectTransform _transformContent;

    [Header("Listener Events")]
    [SerializeField] private StringEventChannelSO _eventCreateSelectablePokemon;

    // Start is called before the first frame update
    private void Start()
    {
        _eventCreateSelectablePokemon.OnEventRaised += CreateSelectablePokemon;
    }

    private void OnDestroy()
    {
        _eventCreateSelectablePokemon.OnEventRaised -= CreateSelectablePokemon;
    }

    private void CreateSelectablePokemon(string pokemonName)
    {
        SelectablePokemon pokemon = Instantiate(_prefabPokemon, _transformContent)
            .GetComponent<SelectablePokemon>();
        pokemon.Initialize(pokemonName);
    }

}
