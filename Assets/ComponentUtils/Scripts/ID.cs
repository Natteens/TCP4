using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ComponentUtils
{
    [System.Serializable]
    public class ID : ISerializationCallbackReceiver
    {
        private static Dictionary<IdGroup, HashSet<int>> usedIdsByGroup = new Dictionary<IdGroup, HashSet<int>>();
        private static Dictionary<IdGroup, int> nextIdByGroup = new Dictionary<IdGroup, int>();
        private static Dictionary<int, List<ScriptableObject>> instancesById = new Dictionary<int, List<ScriptableObject>>();

        [SerializeField] public IdGroup group;
        [SerializeField] public int id;

        private int originalId;
        private IdGroup originalGroup;

        public IdGroup Group
        {
            get => group;
            set
            {
                if (group != value)
                {
                    ReleaseId();
                    group = value;
                    GenerateNewId();
                }
            }
        }

        public int Id
        {
            get => id;
            set
            {
                if (value <= 0 || IsIdInUse(value,Group))
                {
                    Debug.LogWarning("IDs não podem ser negativos ou zero. Mantendo o ID atual.");
                    return;
                }
                AssignManualId(value);
            }
        }

        public ID(IdGroup group)
        {
            this.group = group;
            GenerateNewId();
            originalId = id;
            originalGroup = group;
        }

        public void GenerateNewId()
        {
            ReleaseId();

            if (!usedIdsByGroup.ContainsKey(group))
            {
                usedIdsByGroup[group] = new HashSet<int>();
                nextIdByGroup[group] = 1;
            }

            int newId = 1;
            while (usedIdsByGroup[group].Contains(newId))
            {
                newId++;
            }

            id = newId;
            usedIdsByGroup[group].Add(id);
            instancesById[id] = new List<ScriptableObject>();
            nextIdByGroup[group] = newId + 1;
            Debug.Log($"Novo ID gerado: {id} para o grupo {group}");
        }

        public void ReleaseId()
        {
            if (id > 0 && usedIdsByGroup.ContainsKey(group))
            {
                usedIdsByGroup[group].Remove(id);
                instancesById.Remove(id);
                Debug.Log($"ID {id} do grupo {group} liberado.");
            }
        }

        public static ID CreateNewID(IdGroup group)
        {
            return new ID(group);
        }

        public static bool IsIdInUse(int checkId, IdGroup group) => usedIdsByGroup.ContainsKey(group) && usedIdsByGroup[group].Contains(checkId);

        public static int CountInstances(int checkId)
        {
            return instancesById.ContainsKey(checkId) ? instancesById[checkId].Count : 0;
        }

        public void AssignManualId(int newId)
        {
            if (newId <= 0)
            {
                Debug.LogError($"O ID {newId} é inválido (<= 0) no grupo {group}. Atribuição de ID falhou.");
                return;
            }

            if (IsIdInUse(newId, group))
            {
                Debug.LogWarning($"O ID {newId} já está em uso no grupo {group}. Revertendo para o ID anterior: {id}");
                return; 
            }

            ReleaseId(); 
            id = newId;
            usedIdsByGroup[group].Add(id);
            instancesById[id] = new List<ScriptableObject>();
            Debug.Log($"ID atualizado para {id} no grupo {group}.");
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            if (!usedIdsByGroup.ContainsKey(group))
            {
                usedIdsByGroup[group] = new HashSet<int>();
            }
            if (!usedIdsByGroup[group].Contains(id))
            {
                usedIdsByGroup[group].Add(id);
                instancesById[id] = new List<ScriptableObject>();
                nextIdByGroup[group] = Math.Max(nextIdByGroup.ContainsKey(group) ? nextIdByGroup[group] : 1, id + 1);
            }
            originalId = id;
            originalGroup = group;
        }
        
        public void ChangeGroup(IdGroup newGroup)
        {
            if (group != newGroup)
            {
                ReleaseId();
                group = newGroup;
                GenerateNewId();
                originalGroup = newGroup;
                originalId = id;
            }
        }

        public void ResetToOriginal()
        {
            if (group != originalGroup || id != originalId)
            {
                ReleaseId();
                group = originalGroup;
                id = originalId;
                if (!usedIdsByGroup.ContainsKey(group))
                {
                    usedIdsByGroup[group] = new HashSet<int>();
                }
                usedIdsByGroup[group].Add(id);
            }
        }

        public static void ClearAllIds()
        {
            usedIdsByGroup.Clear();
            nextIdByGroup.Clear();
            instancesById.Clear();
        }

        public static void ClearGroupIds(IdGroup group)
        {
            if (usedIdsByGroup.ContainsKey(group))
            {
                usedIdsByGroup[group].Clear();
                nextIdByGroup[group] = 1;
                instancesById.Clear();
                Debug.Log($"Todos os IDs do grupo {group} foram limpos.");
            }
        }

        public static void LoadExistingIds<T>() where T : ScriptableObject
        {
            usedIdsByGroup.Clear();
            nextIdByGroup.Clear();
            instancesById.Clear();

            T[] allEntities = Resources.LoadAll<T>("");

            foreach (var entity in allEntities)
            {
                var fields = entity.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<AutoIDAttribute>() != null)
                    {
                        if (field.GetValue(entity) is ID idInstance && idInstance != null)
                        {
                            if (!usedIdsByGroup.ContainsKey(idInstance.group))
                            {
                                usedIdsByGroup[idInstance.group] = new HashSet<int>();
                                nextIdByGroup[idInstance.group] = 1;
                            }

                            usedIdsByGroup[idInstance.group].Add(idInstance.Id);
                            instancesById[idInstance.Id].Add(entity);
                            nextIdByGroup[idInstance.group] = Mathf.Max(nextIdByGroup[idInstance.group], idInstance.Id + 1);
                        }
                    }
                }
            }

            CleanUnusedIds<T>();
        }

        public static void CleanUnusedIds<T>() where T : ScriptableObject
        {
            // Realiza a limpeza de IDs não usados, atualiza `nextIdByGroup`
            Dictionary<IdGroup, HashSet<int>> validIdsByGroup = new Dictionary<IdGroup, HashSet<int>>();

            T[] allEntities = Resources.LoadAll<T>("");

            foreach (var entity in allEntities)
            {
                var fields = entity.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<AutoIDAttribute>() != null)
                    {
                        if (field.GetValue(entity) is ID idInstance && idInstance != null)
                        {
                            if (!validIdsByGroup.ContainsKey(idInstance.group))
                            {
                                validIdsByGroup[idInstance.group] = new HashSet<int>();
                            }

                            validIdsByGroup[idInstance.group].Add(idInstance.Id);
                        }
                    }
                }
            }

            foreach (var group in usedIdsByGroup.Keys)
            {
                if (!validIdsByGroup.ContainsKey(group))
                {
                    validIdsByGroup[group] = new HashSet<int>();
                }

                usedIdsByGroup[group].RemoveWhere(id => !validIdsByGroup[group].Contains(id));

                if (usedIdsByGroup[group].Count > 0)
                {
                    nextIdByGroup[group] = usedIdsByGroup[group].Max() + 1;
                }
                else
                {
                    nextIdByGroup[group] = 1;
                }
            }

            Debug.Log("IDs órfãos removidos.");
        }

        public static List<IdGroup> GetAllGroups()
        {
            return new List<IdGroup>(usedIdsByGroup.Keys);
        }

        public static int GetIdCountForGroup(IdGroup group)
        {
            return usedIdsByGroup.ContainsKey(group) ? usedIdsByGroup[group].Count : 0;
        }
    }

}