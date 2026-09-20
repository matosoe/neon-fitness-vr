# Neon Fitness VR

Protótipo fitness em realidade virtual para Meta Quest, em desenvolvimento com Unity 6 e URP.

## Estado

O projeto-base foi criado com Unity `6000.3.24f1` e URP `17.3.0`. Pareamento do headset, configuração XR e validação de APK permanecem condicionados aos gates do plano.

## Fundação XR

- Alvo ativo: Android (fallback para quando o perfil Meta Quest não está disponível no Editor).
- XR Plug-in Management: `4.5.3`.
- Unity OpenXR Plugin: `1.16.1`.
- OpenXR está atribuído para Standalone e Android.
- Meta XR Core SDK e Meta XR Interaction SDK ainda não foram instalados; serão obtidos pela conta Unity/Asset Store antes de configurar os recursos específicos da Meta.

## Pré-requisitos

- Unity `6000.3.24f1` com Android Build Support, SDK/NDK e OpenJDK
- Unity Hub e Unity CLI
- Meta Quest Developer Hub e driver Oculus ADB
- Git LFS
- Headset Meta Quest em Developer Mode para as etapas de hardware

Consulte o [plano de implementação](documentacao/planos/plano-implementacao-codex-vscode.md) e a [especificação do MVP](documentacao/projeto_neon_fitness_vr_mvp.md).
