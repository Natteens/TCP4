using UnityEngine;
using UnityEngine.Splines;
using System.Collections; // Adicionei este namespace para Coroutine

public class TrocaCaminhos : MonoBehaviour
{
    private Unity.Cinemachine.CinemachineSplineCart cart;
    public SplineContainer caminhoInicial;
    [SerializeField] private SplineContainer[] caminhos;
    private SplineContainer ultimoCaminho;
    
    private IEnumerator trocarCaminhosCoroutine; // Mudei para IEnumerator

    private void Awake()
    {
        cart = GetComponent<Unity.Cinemachine.CinemachineSplineCart>();
        
        // Verificação crítica de segurança
        if (cart == null) 
        {
            Debug.LogError("CinemachineSplineCart não encontrado!");
            enabled = false;
            return;
        }
        
        ResetarCaminho();
    }

    private void ResetarCaminho()
    {
        // Para a corrotina de forma segura
        if (trocarCaminhosCoroutine != null)
        {
            StopCoroutine(trocarCaminhosCoroutine);
        }

        cart.Spline = caminhoInicial;
        cart.SplinePosition = 0;

        // Inicia a corrotina corretamente
        trocarCaminhosCoroutine = RotinaTrocaCaminhos();
        StartCoroutine(trocarCaminhosCoroutine);
    }

    private IEnumerator RotinaTrocaCaminhos()
    {
        // Intervalo inicial aleatório
        yield return new WaitForSeconds(Random.Range(4f, 6f));

        while (true)
        {
            MudarCaminho();
            
            // Adicionei um break de segurança
            if (caminhos.Length == 0) yield break;

            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
    }

    private void MudarCaminho()
    {
        if (caminhos.Length == 0 || caminhos == null)
        {
            Debug.LogWarning("Array de caminhos vazio!");
            return;
        }

        SplineContainer novoCaminho;
        int tentativas = 0;
        int maxTentativas = 10; // Prevenção contra loop infinito

        do
        {
            novoCaminho = caminhos[Random.Range(0, caminhos.Length)];
            tentativas++;
            
            // Quebra se encontrar muitas repetições
            if (tentativas >= maxTentativas)
            {
                Debug.LogWarning("Possível loop infinito detectado!");
                break;
            }

        } while (novoCaminho == ultimoCaminho && caminhos.Length > 1);

        cart.Spline = novoCaminho;
        cart.SplinePosition = 0;
        ultimoCaminho = novoCaminho;
    }

    private void OnDisable()
    {
        // Para todas as corrotinas de forma mais eficaz
        StopAllCoroutines();
    }
}