using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class TrocaCaminhos : MonoBehaviour
{
    private CinemachineDollyCart cart;

    public CinemachineSmoothPath caminhoInicial;
    public CinemachineSmoothPath[] caminhos;

    private void Awake()
    {
        cart = GetComponent<CinemachineDollyCart>();
        ResetarCaminho();
    }

    private void ResetarCaminho()
    {
        StopAllCoroutines(); // Para evitar múltiplas corrotinas simultâneas
        cart.m_Path = caminhoInicial;
        cart.m_Position = 0; // Reinicia a posição no caminho inicial
        StartCoroutine(MudarCaminhos());
    }

    private IEnumerator MudarCaminhos()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 6));

            if (caminhos.Length > 0)
            {
                var novoCaminho = caminhos[Random.Range(0, caminhos.Length)];
                cart.m_Path = novoCaminho;
                cart.m_Position = 0; // Reinicia a posição após a troca de caminho
            }
        }
    }
}
