using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "New Achievement Data", menuName = "Achievements/Achievement Data")]
public class AchievementData : ScriptableObject
{
    [Serializable]
    public struct Achievement
    {
        public string achievementID;       // ID da conquista (Steam, etc.)
        public string displayName;        // Nome exibido ao jogador
        public string description;         // Descrição da conquista
        public Sprite icon;               // Ícone da conquista
        public bool unlocked;             // Indica se a conquista está desbloqueada
        public int currentProgress;       // Progresso atual (se a conquista tiver progresso)
        public int requiredProgress;      // Progresso necessário para desbloquear (se aplicável)
    }

    public List<Achievement> achievements = new List<Achievement>(); // Lista de conquistas
}
