using System;
using System.Collections.Generic;
using UnityEngine;

namespace ComponentUtils
{
    public class ServiceLocator
    {
        // Dicionário que armazena serviços por tipo, específico para uma instância
        private Dictionary<Type, object> services = new Dictionary<Type, object>();

        // Registra um novo serviço para a instância
        public void RegisterService<T>(T service)
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
             //   Debug.Log($"ServiceLocator: Serviço do tipo {type} já registrado. Substituindo pelo novo.");
                services[type] = service;  // Substitui o serviço existente
            }
            else
            {
                services.Add(type, service);
              //  Debug.Log($"ServiceLocator: Serviço do tipo {type} registrado com sucesso.");
            }
        }

        // Remove um serviço para a instância
        public void UnregisterService<T>()
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                services.Remove(type);
            }
        }

        // Retorna um serviço registrado para a instância
        public T GetService<T>()
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                return (T)services[type];
            }
            else
            {
                throw new Exception($"Serviço do tipo {type} não está registrado para esta entidade.");
            }
        }

        // Limpa todos os serviços registrados para a instância
        public void ClearAllServices()
        {
            services.Clear();
        }
    }
}
