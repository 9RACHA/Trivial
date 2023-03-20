using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GameManager : MonoBehaviour {

    public Preguntas preguntas;
    public int numPregunta;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10"));
    }

    IEnumerator GetRequest(string uri) {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success){
                Debug.LogError("Error al llamar a la API: " + webRequest.error);
            }
            else {
                preguntas = JsonUtility.FromJson<Preguntas>(webRequest.downloadHandler.text);
                Debug.Log("Número de preguntas: " + preguntas.results.Count);
                Debug.Log("Categoría: " + preguntas.results[0].category);
                Debug.Log("Tipo: " + preguntas.results[0].type);
                Debug.Log("Dificultad: " + preguntas.results[0].difficulty);
                Debug.Log("Pregunta: " + preguntas.results[0].question);
                Debug.Log("Respuesta correcta: " + preguntas.results[0].correct_answer);
                if (preguntas.results.Count > 0)
{
    Pregunta primeraPregunta = preguntas.results[0];
    Debug.Log("Respuestas incorrectas:");

    foreach (string respuesta in primeraPregunta.incorrect_answers)
    {
        Debug.Log(respuesta);
    }
}
else
{
    Debug.Log("No se encontraron preguntas.");
}

            }
        }
    }
}
