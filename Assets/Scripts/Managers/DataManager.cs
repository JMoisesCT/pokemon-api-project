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
        public int id;
        public Texture imageDefault;
    }

    [Serializable]
    public class PokemonDetailDB
    {
        public string name;
        public int id;
        public Sprites sprites;
    }

    [Serializable]
    public class Sprites
    {
        public string front_default;
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
    [SerializeField] private PokemonDetailEventChannelSO _eventSavePokemonDetail;

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
                string jsonPokemonList = request.downloadHandler.text;
                PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(jsonPokemonList);
                for (int i = 0; i < pokemonList.results.Length; ++i)
                {
                    _eventCreateSelectablePokemon.RaiseEvent(pokemonList.results[i].name);
                    StartCoroutine(LoadPokemonDetail(pokemonList.results[i].url));
                }
                Debug.Log($"jsonPokemon {jsonPokemonList}");
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

    private IEnumerator LoadPokemonDetail(string urlPokemon)
    {
        var request = new UnityWebRequest(urlPokemon, "GET");
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
                PokemonDetailDB pokemonDetailFromDatabase = JsonUtility.FromJson<PokemonDetailDB>(jsonPokemon);
                PokemonDetail pokemonDetail = new PokemonDetail();
                pokemonDetail.name = pokemonDetailFromDatabase.name;
                pokemonDetail.id = pokemonDetailFromDatabase.id;
                StartCoroutine(LoadTexture(pokemonDetailFromDatabase.sprites.front_default, pokemonDetail));
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

    IEnumerator LoadTexture(string url, PokemonDetail pokemonDetail)
    {
        UnityWebRequest requestTexture = UnityWebRequestTexture.GetTexture(url);
        yield return requestTexture.SendWebRequest();

        if (requestTexture.result == UnityWebRequest.Result.ConnectionError ||
            requestTexture.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(requestTexture.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)requestTexture.downloadHandler).texture;
            pokemonDetail.imageDefault = myTexture;
            // After the texture is saved, we can send the pokemon detail class to the scroll list.
            _eventSavePokemonDetail.RaiseEvent(pokemonDetail);
        }
    }
}
