using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class TrivialManager : MonoBehaviour
{
    public Preguntas preguntas;
    public int numPregunta;
    public int numPreguntasTotales = 10;

    public Text preguntaText;
    public Text resultadoText;

    public Button opcion1Button;
    public Button opcion2Button;
    public Button opcion3Button;
    public Button opcion4Button;

    private Pregunta preguntaActual;
    private List<Button> botonesOpciones;

    // Start is called before the first frame update
    void Start()
    {
        botonesOpciones = new List<Button> { opcion1Button, opcion2Button, opcion3Button, opcion4Button };
        StartCoroutine(GetRequest("https://opentdb.com/api.php?amount=10"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                preguntas = JsonUtility.FromJson<Preguntas>(webRequest.downloadHandler.text);
                numPregunta = 0;
                MostrarPregunta();
            }
            else
            {
                Debug.Log("Error en la solicitud: " + webRequest.error);
            }
        }
    }

    private void MostrarPregunta()
    {
        preguntaActual = preguntas.results[numPregunta];
        preguntaText.text = preguntaActual.question;

        // Barajar las opciones
        List<string> opciones = new List<string>(preguntaActual.incorrect_answers);
        opciones.Add(preguntaActual.correct_answer);
        opciones.Shuffle();

        // Mostrar las opciones en los botones
        for (int i = 0; i < opciones.Count; i++)
        {
            botonesOpciones[i].GetComponentInChildren<Text>().text = opciones[i];
        }

        resultadoText.text = "";
    }

    public void ComprobarRespuesta(Button opcionSeleccionada)
    {
        if (opcionSeleccionada.GetComponentInChildren<Text>().text == preguntaActual.correct_answer)
        {
            resultadoText.text = "¡Correcto!";
        }
        else
        {
            resultadoText.text = "Incorrecto. La respuesta correcta es: " + preguntaActual.correct_answer;
        }
    }

    public void SiguientePregunta()
    {
        numPregunta++;
        if (numPregunta >= numPreguntasTotales)
        {
            // Fin del juego
            Debug.Log("Fin del juego");
        }
        else
        {
            MostrarPregunta();
        }
    }
}

public static class ListExtensions
{
    // Método de extensión para barajar una lista
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
