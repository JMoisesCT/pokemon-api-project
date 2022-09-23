using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectablePokemon : MonoBehaviour
{
    [SerializeField] private string _name;

    [Header("UI")]
    [SerializeField] private RawImage _image;
    [SerializeField] private Text _textName;
    [SerializeField] private Text _textNumber;

    private DataManager.PokemonDetail _pokemonDetail;

    [Header("Listener Events")]
    [SerializeField] private PokemonDetailEventChannelSO _eventCreateAndSavePokemonDetail;

    private void Awake()
    {
        _eventCreateAndSavePokemonDetail.OnEventRaised += LoadAndSaveDetail;
    }

    private void OnDestroy()
    {
        _eventCreateAndSavePokemonDetail.OnEventRaised -= LoadAndSaveDetail;
    }

    public void Initialize(string pokemonName)
    {
        _name = pokemonName;
    }

    public void LoadAndSaveDetail(DataManager.PokemonDetail pokemonDetail)
    {
        if (pokemonDetail.name != _name)
        {
            return;
        }
        _pokemonDetail = pokemonDetail;

        _image.texture = _pokemonDetail.imageDefault;
        _textName.text = _name;
        _textNumber.text = $"#00{pokemonDetail.id}";
    }
}
