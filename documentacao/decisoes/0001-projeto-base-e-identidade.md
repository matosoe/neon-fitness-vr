# ADR 0001 — Projeto-base e identidade provisória

- Status: aceita
- Data: 2026-09-19
- Responsáveis: equipe Neon Fitness VR

## Contexto

O MVP precisa iniciar em uma versão fixa do Unity, com renderização adequada ao Android/Quest e uma identidade válida para builds de desenvolvimento.

## Decisão

Usar Unity `6000.3.24f1`, URP `17.3.0`, empresa/produto `Neon Fitness VR`, namespace raiz `NeonFitnessVR` e package ID Android provisório `com.neonfitnessvr.prototype`.

## Consequências

O package ID deve ser revisto antes de publicação. Pacotes XR ainda não foram adicionados; suas versões serão decididas após liberar o gate de hardware e inspecionar a compatibilidade instalada.
