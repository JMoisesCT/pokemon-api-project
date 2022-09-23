using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    // For Pokemon API.
    [Serializable]
    public class PokemonDetail
    {
        public string name;
        public string urlDefault;
        public string urlShiny;
        public Texture imageDefault;
        public Texture imageShiny;
    }

    [Serializable]
    public class Sprites
    {
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class PokemonResult
    {
        public string name;
        public string url;
    }

    [Serializable]
    public class PokemonList
    {
        public string count;
        public string next;
        public string previous;
        public PokemonResult[] results;
    }
    //--- End of Json Classes ---

    const string POKEMON_API = "https://pokeapi.co/api/v2/pokemon";
    const string POKEMON_LIST_URL_1 = "?limit=";
    const string POKEMON_LIST_URL_2 = "&offset=0";

    [SerializeField] private int _maxPokemon;
    [Header("Sender Events")]
    [SerializeField] private StringEventChannelSO _eventCreateSelectablePokemon;

    private Dictionary<string, string> _dictionaryPokemons;

    private void Awake()
    {
        _dictionaryPokemons = new Dictionary<string, string>();
    }

    private void Start()
    {
        string listUrl = $"{POKEMON_LIST_URL_1}{_maxPokemon}{POKEMON_LIST_URL_2}";
        StartCoroutine(LoadPokemonList(listUrl));
    }

    private IEnumerator LoadPokemonList(string listUrl)
    {
        var request = new UnityWebRequest(POKEMON_API + listUrl, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error Pokemon API: " + request.error);
        }
        else
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonPokemon = request.downloadHandler.text;
                PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(jsonPokemon);
                for (int i = 0; i < pokemonList.results.Length; ++i)
                {
                    _eventCreateSelectablePokemon.RaiseEvent(pokemonList.results[i].name);
                    _dictionaryPokemons.Add(pokemonList.results[i].name, pokemonList.results[i].url);
                }
                Debug.Log($"jsonPokemon {jsonPokemon}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Unathourized Pokemon API");
            }
            else
            {
                Debug.Log($"Error Pokemon API: { request.error}");
            }
        }
    }
}
