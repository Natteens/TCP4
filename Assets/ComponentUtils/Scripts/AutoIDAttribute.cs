using System;
using System.Reflection;
using UnityEngine;

namespace ComponentUtils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoIDAttribute : PropertyAttribute
    {
        public IdGroup Group { get; private set; }

        public AutoIDAttribute(IdGroup group)
        {
            Group = group;
        }

        public AutoIDAttribute()
        {
            Group = IdGroup.None; 
        }

        public void GenerateIdIfNeeded(object target)
        {
            var idField = target.GetType().GetField("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (idField != null)
            {
                var idInstance = idField.GetValue(target) as ID;

                if (idInstance == null || idInstance.Id == 0 || ID.IsIdInUse(idInstance.Id, idInstance.Group))
                {
                    ID newId = ID.CreateNewID(Group);
                    idField.SetValue(target, newId);
                }
            }
        }
    }
}
