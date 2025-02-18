using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


    public class TrocaCaminhos : MonoBehaviour
    {
        private CinemachineDollyCart cart;

        public CinemachineSmoothPath caminhoInicial;



        [SerializeField] private CinemachineSmoothPath[] caminhos; // Agora aparece no Inspector

        private void Awake()
        {
            cart = GetComponent<CinemachineDollyCart>();
            ResetarCaminho();
        }

        private void ResetarCaminho()
        {
            StopAllCoroutines();
            cart.m_Path = caminhoInicial;
            cart.m_Position = 0;
            StartCoroutine(MudarCaminhos());
        }

        private IEnumerator MudarCaminhos()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(4, 6));

                if (caminhos.Length > 0)
                {
                    var novoCaminho = caminhos[UnityEngine.Random.Range(0, caminhos.Length)];
                    cart.m_Path = novoCaminho;
                    cart.m_Position = 0;
                }
            }
        }
    }



