using System.Collections.Generic;
using UnityEngine;

public class CutsceneAudioPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Arraste aqui o Audio Source que vai tocar os sons")]
    public AudioSource audioSource;

    [Tooltip("Lista de efeitos sonoros (Sound Effects)")]
    public List<AudioClip> soundEffects = new List<AudioClip>();

    [Header("Configurações Opcionais")]
    [Tooltip("Se marcado, o som vai parar antes de tocar outro")]
    public bool stopPreviousSound = true;

    /// <summary>
    /// Toca o som da lista pelo índice.
    /// Essa função pode ser chamada diretamente pelo Animator (Animation Event).
    /// </summary>
    /// <param name="index">Índice do som na lista</param>
    public void PlaySound(int index)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[CutsceneAudioPlayer] AudioSource não está atribuído!");
            return;
        }

        if (soundEffects == null || soundEffects.Count == 0)
        {
            Debug.LogWarning("[CutsceneAudioPlayer] Nenhum som foi adicionado na lista!");
            return;
        }

        if (index < 0 || index >= soundEffects.Count)
        {
            Debug.LogWarning($"[CutsceneAudioPlayer] Índice inválido: {index}. Total de sons: {soundEffects.Count}");
            return;
        }

        // Para o som anterior se necessário
        if (stopPreviousSound && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = soundEffects[index];
        audioSource.Play();
    }

    /// <summary>
    /// Toca um som aleatório da lista (útil para variações).
    /// </summary>
    public void PlayRandomSound()
    {
        if (soundEffects.Count > 0)
        {
            int randomIndex = Random.Range(0, soundEffects.Count);
            PlaySound(randomIndex);
        }
    }

    /// <summary>
    /// Para o som que está tocando atualmente.
    /// </summary>
    public void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
