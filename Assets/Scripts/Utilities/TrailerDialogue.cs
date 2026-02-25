using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrailerDialogue : MonoBehaviour
{
    public List<List<string>> dialogues = new List<List<string>>();
    public List<string> section1 = new List<string>();
    public List<string> section2 = new List<string>();
    public List<string> section3 = new List<string>();

    public TextMeshProUGUI myText;

    public float speedMultiplier;
    public float phrasePause;
    // Função principal (Coroutine)
    public IEnumerator TypeDialogue(TextMeshProUGUI textUI, List<List<string>> dialogueLists, float letterDelay = 0.05f, float listInterval = 2f)
    {
        /*
        textUI.text = ""; // Limpa inicial

        foreach (List<string> currentList in dialogueLists)
        {
            foreach (string phrase in currentList)
            {
                textUI.text = ""; // <<< NOVO: limpa antes de cada nova frase (evita acumular)

                // Typing letra por letra
                foreach (char letter in phrase.ToCharArray())
                {
                    textUI.text += letter;
                    yield return new WaitForSeconds(letterDelay);
                }

                // Pausa após frase
                yield return new WaitForSeconds(0.5f);
            }

            // Intervalo entre listas
            if (dialogueLists.IndexOf(currentList) < dialogueLists.Count - 1)
            {
                yield return new WaitForSeconds(listInterval);
            }
        }
        */
        yield return new WaitForSeconds(3f);
        textUI.text = ""; // Limpa inicial

        float adjustedLetterDelay = letterDelay / speedMultiplier; // <<< NOVO: ajusta velocidade (multiplier >1 = mais rápido)

        foreach (List<string> currentList in dialogueLists)
        {
            foreach (string phrase in currentList)
            {
                textUI.text = ""; // Limpa antes de cada frase

                // Typing letra por letra
                foreach (char letter in phrase.ToCharArray())
                {
                    textUI.text += letter;
                    yield return new WaitForSeconds(adjustedLetterDelay); // <<< usa delay ajustado
                }

                // Pausa após frase completa (nova variável)
                yield return new WaitForSeconds(phrasePause); // <<< NOVO: pausa após linha
            }

            // Intervalo entre listas
            if (dialogueLists.IndexOf(currentList) < dialogueLists.Count - 1)
            {
                yield return new WaitForSeconds(listInterval);
            }
        }
        // Fim (opcional: limpa ou sinaliza)
        // textUI.text = ""; ou yield return null;
    }
    IEnumerator WaitingToStart()
    {
        yield return new WaitForSeconds(10f);
    }

    // Exemplo de uso (chama em Start ou botão)
    private void Start()
    {
        /*
        // Exemplo de listas
        List<List<string>> dialogues = new List<List<string>>
        {
            new List<string> { "Olá, aventureiro!", "Bem-vindo ao mundo RPG." },
            new List<string> { "Escolha sua arma.", "Boa sorte na jornada!" }
        };*/
        dialogues.Add(section1);
        dialogues.Add(section2);
        dialogues.Add(section3);

        //TextMeshProUGUI myText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        //StartCoroutine(WaitingToStart());
        StartCoroutine(TypeDialogue(myText, dialogues, 0.05f, 2f));
    }
}
